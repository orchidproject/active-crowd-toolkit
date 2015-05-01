using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
        /// List of Results for showing the confusion matrices of specific worker, made global variable for GUI
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
        public Dictionary<string, ActiveLearningResult> EntropyTrueLabel()
        {
            return BatchResults.TrueLabel.ToDictionary(kvp => kvp.Key, kvp => new ActiveLearningResult
            {
                TaskId = kvp.Key,
                TaskValue = kvp.Value == null ? double.MaxValue : -kvp.Value.GetAverageLog(kvp.Value)
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
            int numIncremData = 1)
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
            
            List<double> avgRecall = new List<double>();
            //List<ActiveLearningResult> taskValueList = new List<ActiveLearningResult>();
            taskValueList = new List<ActiveLearningResult>();
            int index = 0;

            Console.WriteLine("Active Learning: {0}", modelName);
            Console.WriteLine("\t\t\t\t\t\tAcc\tAvgRec");

            // Get initial data
            Results results = new Results();
            Dictionary<string, int> currentCounts = groupedRandomisedData.ToDictionary(kvp => kvp.Key, kvp => initialNumLabelsPerTask);
            List<Datum> subData = GetSubdata(groupedRandomisedData, currentCounts, remainingWorkersPerTask);

            var s = remainingWorkersPerTask.Select(w => w.Value.Count).Sum();
            List<Datum> nextData = null;
            ActiveLearning activeLearning = null;
            isExperimentCompleted = false;
            for (int iter = 0; ; iter++) //run until data run out
            {
                bool calculateAccuracy = true;
                bool doSnapShot = iter % 1 == 0;
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
                        default: // Run BCC models
                            results.RunBCC(modelName, subData, data, model, RunMode.ClearResults, calculateAccuracy, communityCount, false);
                            break;
                    }
                }

                if (activeLearning == null)
                {
                    activeLearning = new ActiveLearning(data, model, results, communityCount);
                }
                else
                {
                    activeLearning.UpdateActiveLearningResults(results);
                }

                ///
                /// We create a list of task utilities
                /// 

                // TaskValue: Dictionary keyed by task, the value is an active learning result.
                Dictionary<string, ActiveLearningResult> TaskUtility = null;
                switch (taskSelectionMethod)
                {
                    case TaskSelectionMethod.EntropyTask:
                        TaskUtility = activeLearning.EntropyTrueLabel();
                        break;

                    case TaskSelectionMethod.RandomTask:
                        TaskUtility = data.GroupBy(d => d.TaskId).ToDictionary(a => a.Key, a => new ActiveLearningResult
                        {
                            TaskValue = Rand.Double()

                        });
                        break;

                    case TaskSelectionMethod.UniformTask:
                        // Reproduce uniform task selection by picking the task with the lowest number of current labels. That is, minus the current count.
                        TaskUtility = currentCounts.OrderBy(kvp => kvp.Value).ToDictionary(a => a.Key, a => new ActiveLearningResult
                        {
                            TaskId = a.Key,
                            TaskValue = -a.Value
                        });
                        break;

                    default:
                        TaskUtility = activeLearning.EntropyTrueLabel();
                        break;
                }

                ///
                /// We create a list of worker utilities
                ///
                Dictionary<string, double> WorkerAccuracy = null;

                // Best worker selection is only allowed for methods that infer worker confusion matrices.
                if (results.WorkerConfusionMatrix == null)
                    workerSelectionMethod = WorkerSelectionMethod.RandomWorker;

                switch (workerSelectionMethod)
                {
                    case WorkerSelectionMethod.BestWorker:
                        // Assign worker accuracies to the maximum value on the diagonal of the confusion matrix (conservative approach).
                        // Alternative ways are also possible.
                        WorkerAccuracy = results.WorkerConfusionMatrixMean.ToDictionary(
                                kvp => kvp.Key,
                                kvp => Results.GetConfusionMatrixDiagonal(kvp.Value).Max());
                        break;
                    case WorkerSelectionMethod.RandomWorker:
                        // Assign worker accuracies to random values
                        WorkerAccuracy = results.FullMapping.WorkerIdToIndex.ToDictionary(kvp => kvp.Key, kvp => Rand.Double());
                        break;
                    default:
                        throw new ApplicationException("No worker selection method selected");
                }

                ///
                /// Create a list of tuples <TaskId, WorkerId, ActiveLearningResult>
                ///
                List<Tuple<string, string, ActiveLearningResult>> LabelValue = new List<Tuple<string,string,ActiveLearningResult>>();
                foreach (var kvp in TaskUtility)
                {
                    foreach (var workerId in remainingWorkersPerTask[kvp.Key])
                    {
                        var labelValue = new ActiveLearningResult
                        {
                            WorkerId = workerId,
                            TaskId = kvp.Key,
                            TaskValue = kvp.Value.TaskValue,
                            WorkerValue = WorkerAccuracy[workerId]
                        };
                        LabelValue.Add(Tuple.Create(labelValue.TaskId, labelValue.WorkerId, labelValue));
                    }
                }
                
                // Increment tha active set with new data
                nextData = GetNextData(groupedRandomisedData, LabelValue, currentCounts, totalCounts, remainingWorkersPerTask, numIncremData);

                if (nextData == null || nextData.Count == 0)
                    break;

                index += nextData.Count;
                subData.AddRange(nextData);

                // Logs
                if (calculateAccuracy)
                {
                    accuracy.Add(results.Accuracy);
                    avgRecall.Add(results.AvgRecall);

                    if (TaskUtility == null) 
                    {
                        var sortedLabelValue = LabelValue.OrderByDescending(kvp => kvp.Item3.TaskValue).ToArray();
                        taskValueList.Add(sortedLabelValue.First().Item3);
                    }
                    else 
                    {

                        //Adding WorkerId into taskValueList 
                        ActiveLearningResult nextTaskValueItem = TaskUtility[nextData.First().TaskId];
                        nextTaskValueItem.WorkerId = nextData.First().WorkerId;

                        //add taskID
                        nextTaskValueItem.TaskId = nextData.First().TaskId;

                        taskValueList.Add(nextTaskValueItem);
                    }

                    if (doSnapShot)
                    {
                        Console.WriteLine("{0} (label {1} of {2}):\t{3:0.000}\t{4:0.0000}", modelName, index, totalInstances, accuracy.Last(), avgRecall.Last());
                        //DoSnapshot(accuracy, nlpd, avgRecall, taskValueList, results, modelName, "interim", resultsDir, initialNumLabelsPerTask);
                    }
                }//end if logs
            }//end for all data

            isExperimentCompleted = true;

            stopWatchTotal.Stop();
            DoSnapshot(accuracy, avgRecall, taskValueList, results, modelName, "final", resultsDir, initialNumLabelsPerTask);
            ResetAccuracyList();
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
        public static void RunParallelActiveLearning(IList<Datum> data,
            string[] modelName,
            RunType[] runType,
            BCC[] model,
            TaskSelectionMethod[] taskSelectionMethod,
            WorkerSelectionMethod[] workerSelectionMethod,
            int communityCount = -1,
            int initialNumLabelsPerTask = 1,
            int numIncremData = 1)
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

            List<double>[] avgRecallArray = Util.ArrayInit(numModels, i => new List<double>());
            taskValueListArray = Util.ArrayInit(numModels, i => new List<ActiveLearningResult>());
            int[] indexArray = new int[numModels];

            Debug.WriteLine("Parallel Active Learning");
            Debug.WriteLine("\tModel\tAcc\tAvgRec");

            // Get initial data
            //make the results variable be global for GUi
            results = Util.ArrayInit<Results>(numModels, i => new Results());
            List<Datum> subData = GetSubdata(groupedRandomisedData, currentCounts, remainingWorkersPerTask);
            List<Datum>[] subDataArray = Util.ArrayInit<List<Datum>>(numModels, i => new List<Datum>(subData));
            List<Datum>[] nextData = new List<Datum>[numModels];
            ActiveLearning[] activeLearning = new ActiveLearning[numModels];
            isExperimentCompleted = false;
        
            /// Main loop
            /// 
            for (int iter = 0; ; iter++)
            {
                bool calculateAccuracy = true;
                bool doSnapShot = iter % 100 == 0; // Frequency of snapshots
                
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
                                results[indexModel].RunBCC(modelName[indexModel], subDataArray[indexModel], data, model[indexModel], RunMode.ClearResults, calculateAccuracy, communityCount, false);
                                break;
                        }
                    } //end for running all the data

                    if (activeLearning[indexModel] == null)
                    {
                        activeLearning[indexModel] = new ActiveLearning(data, model[indexModel], results[indexModel], communityCount);
                    }
                    else
                    {
                        activeLearning[indexModel].UpdateActiveLearningResults(results[indexModel]);
                    }


                    // Select next task
                    Dictionary<string, ActiveLearningResult> TaskUtility = new Dictionary<string, ActiveLearningResult>();
                    switch (taskSelectionMethod[indexModel]) 
                    {
                        case TaskSelectionMethod.EntropyTask:
                            TaskUtility = activeLearning[indexModel].EntropyTrueLabel();
                            break;

                        case TaskSelectionMethod.RandomTask:
                            TaskUtility = data.GroupBy(d => d.TaskId).ToDictionary(a => a.Key, a => new ActiveLearningResult
                            {
                                TaskValue = Rand.Double()
                            });
                            break;

                        case TaskSelectionMethod.UniformTask:
                            //add task value according to the count left
                            TaskUtility = currentCounts.OrderBy(kvp => kvp.Value).ToDictionary(a => a.Key, a => new ActiveLearningResult
                            {
                                TaskValue = 1
                            });
                            break;

                        default: // Entropy task selection
                            TaskUtility = activeLearning[indexModel].EntropyTrueLabel();
                            break;
                    }

                    ///
                    /// We create a list of worker utilities
                    ///
                    Dictionary<string, double> WorkerAccuracy = null;

                    // Best worker selection is only allowed for methods that infer worker confusion matrices.
                    if (results[indexModel].WorkerConfusionMatrix == null)
                        workerSelectionMethod[indexModel] = WorkerSelectionMethod.RandomWorker;

                    switch (workerSelectionMethod[indexModel])
                    {
                        case WorkerSelectionMethod.BestWorker:
                            // Assign worker accuracies to the maximum value on the diagonal of the confusion matrix (conservative approach).
                            // Alternative ways are also possible.
                            WorkerAccuracy = results[indexModel].WorkerConfusionMatrixMean.ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => Results.GetConfusionMatrixDiagonal(kvp.Value).Max());
                            break;
                        case WorkerSelectionMethod.RandomWorker:
                            // Assign worker accuracies to random values
                            WorkerAccuracy = results[indexModel].FullMapping.WorkerIdToIndex.ToDictionary(kvp => kvp.Key, kvp => Rand.Double());
                            break;
                        default:
                            throw new ApplicationException("No worker selection method selected");
                    }

                    ///
                    /// Create a list of tuples <TaskId, WorkerId, ActiveLearningResult>
                    ///
                    List<Tuple<string, string, ActiveLearningResult>> LabelValue = new List<Tuple<string, string, ActiveLearningResult>>();
                    foreach (var kvp in TaskUtility)
                    {
                        foreach (var workerId in remainingWorkersPerTask[kvp.Key])
                        {
                            var labelValue = new ActiveLearningResult
                            {
                                WorkerId = workerId,
                                TaskId = kvp.Key,
                                TaskValue = kvp.Value.TaskValue,
                                WorkerValue = WorkerAccuracy[workerId]
                            };
                            LabelValue.Add(Tuple.Create(labelValue.TaskId, labelValue.WorkerId, labelValue));
                        }
                    }

                    // Increment tha active set with new data
                    nextData[indexModel] = GetNextData(groupedRandomisedData, LabelValue, currentCounts, totalCounts, remainingWorkersPerTask, numIncremData);

                    if (nextData[indexModel] == null || nextData[indexModel].Count == 0)
                        break;


                    indexArray[indexModel] += nextData[indexModel].Count;
                    subDataArray[indexModel].AddRange(nextData[indexModel]);

                    // Logs
                    if (calculateAccuracy)
                    {
                        accuracyArray[indexModel].Add(results[indexModel].Accuracy);
                        avgRecallArray[indexModel].Add(results[indexModel].AvgRecall);

                        if (TaskUtility == null)
                        {
                            var sortedLabelValue = LabelValue.OrderByDescending(kvp => kvp.Item3.TaskValue).ToArray();
                            taskValueListArray[indexModel].Add(sortedLabelValue.First().Item3);
                        }
                        else
                        {

                            //Adding WorkerId into taskValueListArray
                            ActiveLearningResult nextTaskValueItem = TaskUtility[nextData[indexModel].First().TaskId];
                            nextTaskValueItem.WorkerId = nextData[indexModel].First().WorkerId;
                            nextTaskValueItem.TaskId = nextData[indexModel].First().TaskId;
                            taskValueListArray[indexModel].Add(nextTaskValueItem);
                        }

                        if (doSnapShot)
                        {
                            Debug.WriteLine("{0} of {1}:\t{2}\t{3:0.000}\t{4:0.0000}", indexArray[indexModel], totalInstances, modelName[indexModel], accuracyArray[indexModel].Last(), avgRecallArray[indexModel].Last());
                        }
                    }
                }//end of models
            }//end for all data
        }

        /// <summary>
        /// Saves the results of the inference and the model's parameters on csv files.
        /// </summary>
        /// <param name="accuracy">The list of accuracies evaluated on the gold labels at each active learning round.</param>
        /// <param name="avgRecall">The list of average recalls evaluated on the gold labels at each active learning round.</param>
        /// <param name="taskValue">The list of utilities of the task selected at each active learning round.</param>
        /// <param name="results">The result instance.</param>
        /// <param name="modelName">The model name.</param>
        /// <param name="suffix">The suffix of the csv files.</param>
        /// <param name="resultsDir">The directory to store the csv files.</param>
        public static void DoSnapshot(List<double> accuracy, List<double> avgRecall, List<ActiveLearningResult> taskValue, Results results, string modelName, string suffix, string resultsDir, int projectInitialNumLabelsPerTask, double lipschitzConstant = -1)
        {
            suffix = suffix == "final" ? "" : suffix;
            String new_graph_csv_file_name = String.Format("{2}{0}__graph_{1}_InitialLabels_{3}.csv", modelName, suffix, resultsDir, projectInitialNumLabelsPerTask);

            using (StreamWriter writer = new StreamWriter(new_graph_csv_file_name))
            {
                var accArr = accuracy.ToArray();
                var avgRec = avgRecall.ToArray();
                writer.WriteLine("Accuracy,AvgRecall");
                for (int i = 0; i < accArr.Length; i++)
                {
                    /// <summary>
                    /// Edit this print line to get the accuracy printed if the format that you want.
                    /// </summary>
                    writer.WriteLine("{0:0.0000},{1:0.0000}", accArr[i], avgRec[i]); // Accuracy and average recall
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

        public static List<Datum> GetNextData(
            Dictionary<string, Datum[]> groupedRandomisedData,
            List<Tuple<string, string, ActiveLearningResult>> labelValue,
            Dictionary<string, int> currentCounts,
            Dictionary<string, int> totalCounts,
            Dictionary<string, HashSet<string>> workersPerTask,
            int numIncremData)
        {
            List<Datum> data = new List<Datum>();

            // Randomlise ordering in the list of label values to aid with random worker selection
            var perm = Rand.Perm(labelValue.Count);
            var randomisedLabelValue = labelValue.Select((l, i) => labelValue[perm[i]]).ToArray();

            // Sort label values in descending order
            var sortedLabelValue = randomisedLabelValue.OrderByDescending(kvp => kvp.Item3.TaskValue).ToArray();

            // Cap num. requested labels with num. available labels
            if (numIncremData > sortedLabelValue.Length)
                numIncremData = sortedLabelValue.Length;

            int numAdded = 0;
            for (; ; )
            {
                // Check if there is any data left
                bool noMoreData = currentCounts.All(kvp => kvp.Value >= totalCounts[kvp.Key]);
                if (noMoreData)
                    break;

                // Pick the task with the highest value and then pick the worker with the highest value
                for (int i = 0; i < sortedLabelValue.Length; i++)
                {
                    var task = sortedLabelValue[i].Item1;
                    var sortedTaskLabels = sortedLabelValue.Where(t => t.Item1.Equals(task)).OrderByDescending(vkp => vkp.Item3.WorkerValue).ToArray();
                    var taskLabels = groupedRandomisedData[task].GroupBy(d => d.WorkerId).ToDictionary(a => a.Key, a => a.First());
                    for (int j = 0; j < sortedTaskLabels.Length; j++)
                    {
                        //Dictionary keyed by worker. Value is the worker's label for task
                        var worker = sortedTaskLabels[j].Item2;
                        if (taskLabels.ContainsKey(worker))
                        {
                            int indexTask = currentCounts[task];
                            if (!workersPerTask[task].Contains(worker))
                                continue;
                            data.Add(taskLabels[worker]);
                            currentCounts[task] = indexTask + 1;
                            workersPerTask[task].Remove(worker);
                            if (++numAdded >= numIncremData)
                                return data;
                        }
                    }
                }
                Console.WriteLine("Warning: No labels were found, return a random one");
                break;
                //data.Add(GetRandomDatum(groupedRandomisedData, currentCounts, workersPerTask));

                if (++numAdded >= numIncremData)
                    return data;
            }
            return data;
        }


        public static Datum GetRandomDatum(
                Dictionary<string, Datum[]> groupedRandomisedData,
                Dictionary<string, int> currentCounts,
                Dictionary<string, HashSet<string>> workersPerTask)
        {
            foreach (string task in currentCounts.Keys)
            {
                foreach (string worker in workersPerTask[task])
                {
                    int indexTask = currentCounts[task];
                    var taskLabels = groupedRandomisedData[task].GroupBy(d => d.WorkerId).ToDictionary(a => a.Key, a => a.First());
                    var datum = taskLabels[worker];
                    currentCounts[task] = indexTask + 1;
                    workersPerTask[task].Remove(worker);
                    return datum;
                }
            }
            return null;
        }

        public static void ResetAccuracyList()
        {
            accuracy = new List<double>();
        }

        public static void ResetParallelAccuracyList(int numModels)
        {
            accuracyArray = Util.ArrayInit<List<double>>(numModels, i => new List<double>());
        }
    }
}