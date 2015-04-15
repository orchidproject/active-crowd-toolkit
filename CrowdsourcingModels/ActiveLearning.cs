using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Factors;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetAnotherLabel;
//
namespace CrowdsourcingModels
{
    /// <summary>
    /// Class of active learning functions
    /// </summary>
    public class ActiveLearning
    {
        /// <summary>
        /// Full list of simulated data from every task and worker
        /// It is used for initialising the BCC and the CBCC model.
        /// </summary>
        IList<Datum> PredictionData;

        /// <summary>
        /// Flag to indicate whether the model instance is CBCC (true) or BCC (false).
        /// </summary>
        bool IsCommunityModel;

        /// <summary>
        /// Model instance.
        /// </summary>
        BCC bcc;

        /// <summary>
        ///  Result instance for active learning.
        /// </summary>
        Results ActiveLearningResults;

        /// <summary>
        /// Result instance for batch training.
        /// </summary>
        Results BatchResults;

        /// <summary>
        /// Indicate whether the experiment is completed or not, used for the GUI
        /// </summary>
        public static Boolean isExperimentCompleted = false;


        /// <summary>
        /// Static accuracy list used for the gui
        /// </summary>
        public static List<double> accuracy;

        /// <summary>
        /// Static array of accuracy lists used for the gui
        /// </summary>
        public static List<double>[] accuracyArray;

        /// <summary>
        /// List of utility values for the tasks selected at each round 
        /// </summary>
        public static List<ActiveLearningResult> taskValueList;

        /// <summary>
        /// Array of lists of utility values for the tasks selected at each round in parallel mode
        /// </summary>
        public static List<ActiveLearningResult>[] taskValueListArray;

        /// <summary>
        /// List of Results for showing the confusion matrices of specific worker
        /// </summary>
        public static Results[] results;
        /// <summary>
        /// Constructs an active learning instance with a specified data set and model instance.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="model">The model instance.</param>
        /// <param name="results">The results instance.</param>
        /// <param name="numCommunities">The number of communities (only for CBCC).</param>
        public ActiveLearning(IList<Datum> data, BCC model, Results results, int numCommunities)
        {
            this.bcc = model;
            CBCC communityModel = model as CBCC;
            IsCommunityModel = (communityModel != null);

            ActiveLearningResults = results;
            BatchResults = results;
            isExperimentCompleted = false;


            /// Builds the full matrix of data from every task and worker
            PredictionData = new List<Datum>();

        }

        /// <summary>
        /// Updates the active learning results object.
        /// </summary>
        /// <param name="results">The new results</param>
        public void UpdateActiveLearningResults(Results results)
        {
            ActiveLearningResults = results;
        }

        /// <summary>
        /// Computes the entropy on the true label posterior distribution of the active learning results.
        /// </summary>
        /// <returns>A dictionary keyed by the TaskId and the value is the true label entropy.</returns>
        public Dictionary<string, ActiveLearningResult> EntropyTrueLabelPosterior()
        {
            return BatchResults.TrueLabel.ToDictionary(kvp => kvp.Key, kvp => new ActiveLearningResult
            {
                TaskId = kvp.Key,
                TotalTaskValue = kvp.Value == null ? double.MaxValue : -kvp.Value.GetAverageLog(kvp.Value)
            });
        }

        /// <summary>
        /// Runs the standard active learning procedure on a model instance and an input data set.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="modelName">The model name.</param>
        /// <param name="runType">The model run type.</param>
        /// <param name="model">The model instance.</param>
        /// <param name="taskSelectionMethod">The method for selecting tasks (Random / Entropy).</param>
        /// <param name="workerSelectionMethod">The method for selecting workers (only Random is implemented).</param>
        /// <param name="resultsDir">The directory to save the log files.</param>
        /// <param name="communityCount">The number of communities (only for CBCC).</param>
        /// <param name="initialNumLabelsPerTask">The initial number of exploratory labels that are randomly selected for each task.</param>
        public static void RunActiveLearning(
            IList<Datum> data,
            string modelName,
            RunType runType,
            BCC model,
            TaskSelectionMethod taskSelectionMethod,
            WorkerSelectionMethod workerSelectionMethod,
            string resultsDir,
            int communityCount = -1,
            int initialNumLabelsPerTask = 1,
            double lipschitzConstant = 1,
            int numIncremData = 1,
            bool startWithRandomData = false)
        {
            //Count elapsed time
            Stopwatch stopWatchTotal = new Stopwatch();
            stopWatchTotal.Start();
            int totalLabels = data.Count();

            // Dictionary keyed by task Id, with randomly order labelings
            var groupedRandomisedData =
                data.GroupBy(d => d.TaskId).
                Select(g =>
                {
                    var arr = g.ToArray();
                    int cnt = arr.Length;
                    var perm = Rand.Perm(cnt);
                    return new
                    {
                        key = g.Key,
                        arr = g.Select((t, i) => arr[perm[i]]).ToArray()
                    };
                }).ToDictionary(a => a.key, a => a.arr);

            // Dictionary keyed by task Id, with label counts
            Dictionary<string, int> totalCounts = groupedRandomisedData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Length);
            
            // Keyed by task, value is a HashSet containing all the remaining workers with a label - workers are removed after adding a new datum 
            Dictionary<string, HashSet<string>> remainingWorkersPerTask = groupedRandomisedData.ToDictionary(kvp => kvp.Key, kvp => new HashSet<string>(kvp.Value.Select(dat => dat.WorkerId)));
            int numTaskIds = totalCounts.Count();

            int totalInstances = initialNumLabelsPerTask > 0 ? data.Count - initialNumLabelsPerTask * numTaskIds : data.Count - numIncremData;

            //throw an exception if the totalInstances is less than or equals to zero
            if (totalInstances <= 0)
            {
                throw new System.Exception("The variable 'totalInstances' should be greater than zero");
            }
            
            string[] WorkerIds = data.Select(d => d.WorkerId).Distinct().ToArray();

            //only creat accuracy list when it's null (for GUI Use)
            if (accuracy == null)
            { 
                accuracy = new List<double>();
            }
            
            List<double> nlpd = new List<double>();
            List<double> avgRecall = new List<double>();
            //List<ActiveLearningResult> taskValueList = new List<ActiveLearningResult>();
            taskValueList = new List<ActiveLearningResult>();
            int index = 0;

            Console.WriteLine("Active Learning: {0}", modelName);
            Console.WriteLine("\t\tAcc\tAvgRec");

            // Get initial data
            Results results = new Results();
            List<Datum> subData = null;
            Dictionary<string, int> currentCounts = null;
            if (startWithRandomData) // Start with random sets of labels for each task
            {
                currentCounts = groupedRandomisedData.ToDictionary(kvp => kvp.Key, kvp => 0);
                Dictionary<string, ActiveLearningResult> TaskValueInit = data.GroupBy(d => d.TaskId).ToDictionary(a => a.Key, a => new ActiveLearningResult
                {
                    TotalTaskValue = Rand.Double()
                });
                subData = GetNextData(groupedRandomisedData, TaskValueInit, currentCounts, totalCounts, numIncremData, TaskSelectionMethod.RandomTask);
            }
            else // Start with set of labels with uniform size for each task
            {
                currentCounts = groupedRandomisedData.ToDictionary(kvp => kvp.Key, kvp => initialNumLabelsPerTask);
                subData = GetSubdata(groupedRandomisedData, currentCounts, remainingWorkersPerTask);

            }

            // For BCCWords
            List<string> VocabularyOnSubData = null;
            ResultsWords resultsWords = null;
            BCCWords bccWordsModel = null;
            if(runType == RunType.BCCWords)
            {
                VocabularyOnSubData = ResultsWords.BuildVocabularyOnSubdata((List<Datum>)data);
                resultsWords = new ResultsWords(data, VocabularyOnSubData);
                bccWordsModel = model as BCCWords;
                results = resultsWords;
            }

            var s = remainingWorkersPerTask.Select(w => w.Value.Count).Sum();
            List<Datum> nextData = null;
            ActiveLearning activeLearning = null;
            MultiArmedBandit mab = null;
            int numberOfObservedData = 300;
            isExperimentCompleted = false;
            //for (int iter = 0; iter < 200; iter++)
            for (int iter = 0; ; iter++) //run all the data
            {
                bool calculateAccuracy = true;
                ////bool doSnapShot = iter % 100 == 0; // Frequency of snapshots
                bool doSnapShot = true;
                if (subData != null || nextData != null)
                {
                    switch (runType)
                    {
                        case RunType.VoteDistribution:
                            results.RunMajorityVote(subData, data, calculateAccuracy, true);
                            break;
                        case RunType.MajorityVote:
                            results.RunMajorityVote(subData, data, calculateAccuracy, false);
                            break;
                        case RunType.DawidSkene:
                            results.RunDawidSkene(subData, data, calculateAccuracy);
                            break;
                        case RunType.BCCWords:
                            resultsWords.RunBCCWords("BCCwords", subData, data, bccWordsModel, Results.RunMode.ClearResults, true, false);
                            break;
                        default: // Run BCC models
                            results.RunBCC(modelName, subData, data, model, Results.RunMode.ClearResults, calculateAccuracy, communityCount, false);
                            break;
                    }
                } //end for running all the data

                if (activeLearning == null)
                {
                    activeLearning = new ActiveLearning(data, model, results, communityCount);
                    mab = new MultiArmedBandit(currentCounts, lipschitzConstant);
                }
                else
                {
                    activeLearning.UpdateActiveLearningResults(results);
                }


                // Select next task
                Dictionary<string, ActiveLearningResult> TaskValue = null;
                List<Tuple<string, string, ActiveLearningResult>> LabelValue = null;
                switch (taskSelectionMethod)  //R2
                {
                    case TaskSelectionMethod.EntropyTask:
                        TaskValue = activeLearning.EntropyTrueLabelPosterior();
                        break;

                    case TaskSelectionMethod.RandomTask:
                        TaskValue = data.GroupBy(d => d.TaskId).ToDictionary(a => a.Key, a => new ActiveLearningResult
                        {
                            TotalTaskValue = Rand.Double()
                        });
                        break;

                    case TaskSelectionMethod.UniformTask:
                        //add task value according to the count left
                        TaskValue = currentCounts.OrderBy(kvp => kvp.Value).ToDictionary(a => a.Key, a => new ActiveLearningResult
                        {
                            TotalTaskValue = 1
                        });
                        break;

                    //The Entropy MAB Task selection method
                    case TaskSelectionMethod.EntropyMABTask:
                        //get the entropy value
                        TaskValue = activeLearning.EntropyTrueLabelPosterior();

                        mab.AddUCB(TaskValue, currentCounts, iter + numberOfObservedData);
                        break;

                    default: // Entropy task selection
                        TaskValue = activeLearning.EntropyTrueLabelPosterior();
                        break;
                }

                // Don't call this if UniformTask
                nextData = GetNextData(groupedRandomisedData, TaskValue, currentCounts, totalCounts, numIncremData, taskSelectionMethod);

                if (nextData == null || nextData.Count == 0)
                    break;

                index += nextData.Count;
                subData.AddRange(nextData);

                // Logs
                if (calculateAccuracy)
                {
                    accuracy.Add(results.Accuracy);
                    nlpd.Add(results.NegativeLogProb);
                    avgRecall.Add(results.AvgRecall);

                    if (TaskValue == null) 
                    {
                        var sortedLabelValue = LabelValue.OrderByDescending(kvp => kvp.Item3.TotalTaskValue).ToArray();
                        //var sortedLabelValue = currentCounts.OrderByDescending(kvp => kvp.Value).ToArray();
                        taskValueList.Add(sortedLabelValue.First().Item3);
                    }
                    else 
                    {

                        //Adding WorkerId into taskValueList 
                        ActiveLearningResult nextTaskValueItem = TaskValue[nextData.First().TaskId];
                        nextTaskValueItem.WorkerId = nextData.First().WorkerId;

                        //add taskID
                        nextTaskValueItem.TaskId = nextData.First().TaskId;

                        taskValueList.Add(nextTaskValueItem);
                    }

                    if (doSnapShot)
                    {
                        Console.WriteLine("{0} of {1}:\t{2:0.000}\t{3:0.0000}", index, totalInstances, accuracy.Last(), avgRecall.Last());
                        //DoSnapshot(accuracy, nlpd, avgRecall, taskValueList, results, modelName, "interim", resultsDir, initialNumLabelsPerTask, lipschitzConstant);
                    }
                }//end if logs


            }//end for all data

            isExperimentCompleted = true;

            stopWatchTotal.Stop();
            DoSnapshot(accuracy, nlpd, avgRecall, taskValueList, results, modelName, "final", resultsDir, initialNumLabelsPerTask, lipschitzConstant);
            Console.WriteLine("Elapsed time: {0}\n", stopWatchTotal.Elapsed);
        }

        /// <summary>
        /// Runs the standard active learning procedure in parallel on an array of model instances and an input data set.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="modelName">The model name.</param>
        /// <param name="runType">The model run type.</param>
        /// <param name="model">The model instance.</param>
        /// <param name="taskSelectionMethod">The method for selecting tasks (Random / Entropy).</param>
        /// <param name="workerSelectionMethod">The method for selecting workers (only Random is implemented).</param>
        /// <param name="resultsDir">The directory to save the log files.</param>
        /// <param name="communityCount">The number of communities (only for CBCC).</param>
        /// <param name="initialNumLabelsPerTask">The initial number of exploratory labels that are randomly selected for each task.</param>
        public static void RunParallelActiveLearning(IList<Datum> data, string[] modelName, RunType[] runType, BCC[] model, TaskSelectionMethod[] taskSelectionMethod, WorkerSelectionMethod[] workerSelectionMethod, int communityCount = -1, int initialNumLabelsPerTask = 1, double lipschitzConstant = 1)
        {

            int numModels = runType.Length;
            Stopwatch stopWatch = new Stopwatch();
            int totalLabels = data.Count();

            // Dictionary keyed by task Id, with randomly order labelings
            var groupedRandomisedData =
                data.GroupBy(d => d.TaskId).
                Select(g =>
                {
                    var arr = g.ToArray();
                    int cnt = arr.Length;
                    var perm = Rand.Perm(cnt);
                    return new
                    {
                        key = g.Key,
                        arr = g.Select((t, i) => arr[perm[i]]).ToArray()
                    };
                }).ToDictionary(a => a.key, a => a.arr);

            // Dictionary keyed by task Id, with label counts
            Dictionary<string, int> totalCounts = groupedRandomisedData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Length);
            Dictionary<string, int> currentCounts = groupedRandomisedData.ToDictionary(kvp => kvp.Key, kvp => initialNumLabelsPerTask);

            // Keyed by task, value is a HashSet containing all the remaining workers with a label - workers are removed after adding a new datum 
            Dictionary<string, HashSet<string>> remainingWorkersPerTask = groupedRandomisedData.ToDictionary(kvp => kvp.Key, kvp => new HashSet<string>(kvp.Value.Select(dat => dat.WorkerId)));
            int numTaskIds = totalCounts.Count();
            int totalInstances = data.Count - initialNumLabelsPerTask * numTaskIds;

            //throw an exception if the totalInstances is less than or equals to zero
            if (totalInstances <= 0)
            {
                throw new System.Exception("The variable 'totalInstances' should be greater than zero");
            }

            //only creates accuracy list when it's null (for GUI Use)
            if (accuracyArray == null)
            {
                accuracyArray = Util.ArrayInit<List<double>>(numModels, i => new List<double>());

            }

            List<double>[] nlpdArray = Util.ArrayInit(numModels, i => new List<double>());
            List<double>[] avgRecallArray = Util.ArrayInit(numModels, i => new List<double>());
            taskValueListArray = Util.ArrayInit(numModels, i => new List<ActiveLearningResult>());
            int[] indexArray = new int[numModels];

            Console.WriteLine("Parallel Active Learning");
            Console.WriteLine("\tModel\tAcc\tAvgRec");

            // Get initial data
            //Results[] results = Util.ArrayInit<Results>(numModels, i => new Results());
            //make the results variable be global for GUi
            results = Util.ArrayInit<Results>(numModels, i => new Results());
            List<Datum> subData = GetSubdata(groupedRandomisedData, currentCounts, remainingWorkersPerTask);
            List<Datum>[] subDataArray = Util.ArrayInit<List<Datum>>(numModels, i => new List<Datum>(subData));
            List<Datum>[] nextData = new List<Datum>[numModels];
            int numIncremData = 1; //number of Increment Data 
            ActiveLearning[] activeLearning = new ActiveLearning[numModels];
            MultiArmedBandit[] mab = new MultiArmedBandit[numModels];
            int numberOfObservedData = 300;
            isExperimentCompleted = false;
        
            /// Main loop
            /// 
            for (int iter = 0; ; iter++)
            {
                bool calculateAccuracy = true;
                ////bool doSnapShot = iter % 100 == 0; // Frequency of snapshots
                bool doSnapShot = true;
                
                //stop Active Learning if the user requests to stop
                if (isExperimentCompleted) {
                    return;
                
                }

                ///
                /// Run through all the models
                ///
                for (int indexModel = 0; indexModel < numModels; indexModel++ )
                {
                    if (subDataArray[indexModel] != null || nextData[indexModel] != null)
                    {
                        switch (runType[indexModel])
                        {
                            case RunType.VoteDistribution:
                                results[indexModel].RunMajorityVote(subDataArray[indexModel], data, calculateAccuracy, true);
                                break;
                            case RunType.MajorityVote:
                                results[indexModel].RunMajorityVote(subDataArray[indexModel], data, calculateAccuracy, false);
                                break;
                            case RunType.DawidSkene:
                                results[indexModel].RunDawidSkene(subDataArray[indexModel], data, calculateAccuracy);
                                break;
                            default: // Run BCC models
                                results[indexModel].RunBCC(modelName[indexModel], subDataArray[indexModel], data, model[indexModel], Results.RunMode.ClearResults, calculateAccuracy, communityCount, false);
                                break;
                        }
                    } //end for running all the data

                    if (activeLearning[indexModel] == null)
                    {
                        activeLearning[indexModel] = new ActiveLearning(data, model[indexModel], results[indexModel], communityCount);
                        mab[indexModel] = new MultiArmedBandit(currentCounts, lipschitzConstant);
                    }
                    else
                    {
                        activeLearning[indexModel].UpdateActiveLearningResults(results[indexModel]);
                    }


                    // Select next task
                    Dictionary<string, ActiveLearningResult>[] TaskValue = new Dictionary<string, ActiveLearningResult>[numModels];
                    List<Tuple<string, string, ActiveLearningResult>>[] LabelValue = new List<Tuple<string, string, ActiveLearningResult>>[numModels];
                    switch (taskSelectionMethod[indexModel]) 
                    {
                        case TaskSelectionMethod.EntropyTask:
                            TaskValue[indexModel] = activeLearning[indexModel].EntropyTrueLabelPosterior();
                            break;

                        case TaskSelectionMethod.RandomTask:
                            TaskValue[indexModel] = data.GroupBy(d => d.TaskId).ToDictionary(a => a.Key, a => new ActiveLearningResult
                            {
                                TotalTaskValue = Rand.Double()
                            });
                            break;

                        case TaskSelectionMethod.UniformTask:
                            //add task value according to the count left
                            TaskValue[indexModel] = currentCounts.OrderBy(kvp => kvp.Value).ToDictionary(a => a.Key, a => new ActiveLearningResult
                            {
                                TotalTaskValue = 1
                            });
                            break;

                        //The Entropy MAB Task selection method
                        case TaskSelectionMethod.EntropyMABTask:
                            //get the entropy value
                            TaskValue[indexModel] = activeLearning[indexModel].EntropyTrueLabelPosterior();
                            mab[indexModel].AddUCB(TaskValue[indexModel], currentCounts, iter + numberOfObservedData);
                            break;

                        default: // Entropy task selection
                            TaskValue[indexModel] = activeLearning[indexModel].EntropyTrueLabelPosterior();
                            break;
                    }

                    // Don't call this if UniformTask
                    nextData[indexModel] = GetNextData(groupedRandomisedData, TaskValue[indexModel], currentCounts, totalCounts, numIncremData, taskSelectionMethod[indexModel]);

                    if (nextData == null || nextData[indexModel].Count == 0)
                        break;

                    indexArray[indexModel] += nextData[indexModel].Count;
                    subDataArray[indexModel].AddRange(nextData[indexModel]);

                    // Logs
                    if (calculateAccuracy)
                    {
                        accuracyArray[indexModel].Add(results[indexModel].Accuracy);
                        avgRecallArray[indexModel].Add(results[indexModel].AvgRecall);
                        nlpdArray[indexModel].Add(results[indexModel].NegativeLogProb);

                        if (TaskValue[indexModel] == null)
                        {
                            var sortedLabelValue = LabelValue[indexModel].OrderByDescending(kvp => kvp.Item3.TotalTaskValue).ToArray();
                            //var sortedLabelValue = currentCounts.OrderByDescending(kvp => kvp.Value).ToArray();
                            taskValueListArray[indexModel].Add(sortedLabelValue.First().Item3);
                        }
                        else
                        {

                            //Adding WorkerId into taskValueListArray
                            ActiveLearningResult nextTaskValueItem = TaskValue[indexModel][nextData[indexModel].First().TaskId];
                            nextTaskValueItem.WorkerId = nextData[indexModel].First().WorkerId;
                            nextTaskValueItem.TaskId = nextData[indexModel].First().TaskId;
                            taskValueListArray[indexModel].Add(nextTaskValueItem);
                        }

                        if (doSnapShot)
                        {
                            Console.WriteLine("{0} of {1}:\t{2}\t{3:0.000}\t{4:0.0000}", indexArray[indexModel], totalInstances, modelName[indexModel], accuracyArray[indexModel].Last(), avgRecallArray[indexModel].Last());
                        }

                   
                    }
                }//end of models
            }//end for all data
        }

        /// <summary>
        /// Saves the results of the inference and the model's parameters on csv files.
        /// </summary>
        /// <param name="accuracy">The list of accuracies evaluated on the gold labels at each active learning round.</param>
        /// <param name="nlpd">The list of NLPD scores evaluated on the gold labels at each active learning round.</param>
        /// <param name="avgRecall">The list of average recalls evaluated on the gold labels at each active learning round.</param>
        /// <param name="taskValue">The list of utilities of the task selected at each active learning round.</param>
        /// <param name="results">The result instance.</param>
        /// <param name="modelName">The model name.</param>
        /// <param name="suffix">The suffix of the csv files.</param>
        /// <param name="resultsDir">The directory to store the csv files.</param>
        public static void DoSnapshot(List<double> accuracy, List<double> nlpd, List<double> avgRecall, List<ActiveLearningResult> taskValue, Results results, string modelName, string suffix, string resultsDir, int projectInitialNumLabelsPerTask, double lipschitzConstant = -1)
        {

            String new_graph_csv_file_name = "";
            //Add the LipschitzConstant variable as the csv name
            //if (modelName.Contains("MABTask"))
            //{
            //    int digitToMultiply = 10;
            //    modelName += "_LipschitzConstant_" + (lipschitzConstant * digitToMultiply) + "dividedBy" + digitToMultiply;
            //}
                new_graph_csv_file_name = String.Format("{2}{0}_graph_{1}_InitialNumberOfLabels_{3}.csv", modelName, suffix, resultsDir, projectInitialNumLabelsPerTask);

            using (StreamWriter writer = new StreamWriter(new_graph_csv_file_name))
            {
                var accArr = accuracy.ToArray();
                var nlpdArr = nlpd.ToArray();
                var avgRec = avgRecall.ToArray();
                for (int i = 0; i < accArr.Length; i++)
                {
                    /// <summary>
                    /// Enable one of the lines below to get the accuracy printed if the format that you want.
                    /// </summary>

                    writer.WriteLine("{0:0.0000}", accArr[i]); // Only accuracy
                    //writer.WriteLine("{0:0.0000},{1:0.0000}", accArr[i], avgRec[i]); // Accuracy and average recall
                    //writer.WriteLine("{0:0.0000},{1:0.0000}", accArr[i], nlpdArr[i]); // Accuracy and negative log probability density
                }
            }

            using (StreamWriter writer = new StreamWriter(String.Format("{2}{0}_parameters_{1}_InitialNumberOfLabels_{3}.csv", modelName, suffix, resultsDir, projectInitialNumLabelsPerTask)))
            {
                results.WriteResults(writer, true, true, true);
            }

            using (StreamWriter writer = new StreamWriter(String.Format("{2}{0}_taskValue_{1}_InitialNumberOfLabels_{3}.csv", modelName, suffix, resultsDir, projectInitialNumLabelsPerTask)))
            {
                for (int i = 0; i < taskValue.Count; i++)
                {
                    //write taskId, WorkerId, TaskValue into the csv file
                    writer.WriteLine(String.Format("{0},{1},{2:0.000},{3:0.000},{4:0.000}", taskValue[i].TaskId, taskValue[i].WorkerId, taskValue[i].TotalTaskValue, taskValue[i].TaskValue, taskValue[i].UcbValue));
                }
            }
        }

        /// <summary>
        /// Returns a list of sub-data selected sequentially from the input data list.
        /// </summary>
        /// <param name="groupedRandomisedData">The randomised data.</param>
        /// <param name="currentCounts">The current data count per task.</param>
        /// <param name="workersPerTask">The dictionary keyed by taskId and the value is an hashset of workerId who have remaining labels for the tasks.</param>
        /// <returns>The list of sub-data.</returns>
        /// 

        public static List<Datum> GetSubdata(Dictionary<string, Datum[]> groupedRandomisedData, Dictionary<string, int> currentCounts, Dictionary<string, HashSet<string>> workersPerTask)
        {
            var data = groupedRandomisedData.Select(g => g.Value.Take(currentCounts[g.Key])).SelectMany(d => d).ToList();
            foreach (Datum d in data)
            {
                workersPerTask[d.TaskId].Remove(d.WorkerId);
            }
            return data;
        }

        /// <summary>
        /// Return the list of sub-data for the task with the highest utility.
        /// </summary>
        /// <param name="groupedRandomisedData">The randomised data.</param>
        /// <param name="taskValue">The dictionary keyed by taskId and the value is an active learning result instance.</param>
        /// <param name="currentCounts">The current data count per task.</param>
        /// <param name="totalCounts">The total data count for all the tasks.</param>
        /// <param name="numIncremData">The number of data to be selected.</param>
        /// <returns>The list of sub-data.</returns>
        public static List<Datum> GetNextData(
            Dictionary<string, Datum[]> groupedRandomisedData,
            Dictionary<string, ActiveLearningResult> taskValue,// Don't need it for uniform exploration
            Dictionary<string, int> currentCounts,
            Dictionary<string, int> totalCounts,
            int numIncremData,
            TaskSelectionMethod taskSelectionMethod)
        {
            List<Datum> data = new List<Datum>();

                var sortedTaskValues = taskValue.OrderByDescending(kvp => kvp.Value.TotalTaskValue).ToArray();
                var sortedCounts = currentCounts.OrderByDescending(kvp => kvp.Value).ToArray();

                int numAdded = 0;
                for (; ; ) //outer for
                {
                    bool noMoreData = currentCounts.All(kvp => kvp.Value >= totalCounts[kvp.Key]);
                    if (noMoreData)
                        break;

                    for (int i = 0; i < sortedTaskValues.Length; i++)//inner for
                    {
                        var task = sortedTaskValues[i].Key;
                        int index = currentCounts[task];
                        if (index >= totalCounts[task])
                            continue;
                        data.Add(groupedRandomisedData[task][index]);
                        currentCounts[task] = index + 1;
                        if (++numAdded >= numIncremData)
                            return data;
                }// end outer for 
            }//end if task selection method is not uniform


            return data;
        }//


        public static Boolean IsExperimentCompleted()
        {
            return isExperimentCompleted;
        }

        public static List<double> getAccuracyList() {
            return accuracy;
        }

        public static void resetAccuracyList()
        {
            accuracy = new List<double>();
        }

        public static void resetParallelAccuracyList(int numModels)
        {
            accuracyArray = Util.ArrayInit<List<double>>(numModels, i => new List<double>());
        }

    } // end class ActiveLearning

   

} // end of Namespace