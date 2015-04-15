using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using MicrosoftResearch.Infer.Distributions;
using CrowdsourcingModels;

namespace AcriveCrowdGUI
{
    /// <summary>
    /// The ExperimentSetting Class for setting of a specific dataset, running with different experimentModels
    /// </summary>
    public class ExperimentSetting
    {
        //number Of Labelling Round of the experiment
        public int numberOfLabellingRound;

        //Initial Community Count of the experiment 
        public int communityCount = 3;

        //Dataset used in the experiment
        public Dataset currentDataset
        {
            get;
            private set;
        }

        //List of ExperimentModels used in the experiment
        public List<ExperimentModel> experimentModels;

        //Results of all ExperimentModels
        public List<Results> results;

        /// <summary>
        /// Accuracy array for all ExperimentModels from the experiment
        /// </summary>
        public List<double>[] accuracyArrayOfAllExperimentModels
        {
            get;
            private set;
        }
   
        /// <summary>
        /// Initial starting label point after exploration phase
        /// </summary>
        public int initialStartingLabel
        {
            get;
            private set;
        }

        /// <summary>
        /// Object to store the current state, for passing to the caller
        /// </summary>
        public class CurrentParallelState
        {
            public Boolean isRunningComplete;
            public ExperimentModel currentExperimentModel;
            public ActiveLearningResult currentActiveLearningResult;
            public int currentExperimentModelIndex;
            public Boolean isCurrentModelCompleted;

        }

        /// <summary>
        /// The number of Iteration of the experiment (ie. For ActiveLearning would be 1)
        /// </summary>
        public int numberOfIterations
        {
            get;
            private set;
        }

        /// <summary>
        /// The experiment type of current experiment, ie ActiveLearning, BatchRunning
        /// </summary>
        public ExperimentType experimentType
        {
            get;
            set;
        }
       
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="initialNumberOfLabelsPerTask"></param>
        /// <param name="currentDataset"></param>
        /// <param name="currentExperimentType"></param>
        /// <param name="numberOfIterations"></param>
        public ExperimentSetting(int initialNumberOfLabelsPerTask, Dataset currentDataset, ExperimentType currentExperimentType, int numberOfIterations = 1)
        {

            this.numberOfLabellingRound = initialNumberOfLabelsPerTask;
            experimentModels = new List<ExperimentModel>();
            this.currentDataset = currentDataset;
            accuracyArrayOfAllExperimentModels = null;
            SetInitialStartingLabel();
            this.numberOfIterations = numberOfIterations;
            this.experimentType = currentExperimentType;

        }

        /// <summary>
        /// Add ExperimentModel into the Experiment
        /// </summary>
        /// <param name="expModel"></param>
        public void AddExperimentModel(ExperimentModel expModel) {
            experimentModels.Add(expModel);
        }

        /// <summary>
        /// Get the total number of ExperimentModels in this ExperimentSetting
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfExperiemntModels() 
        {
            return experimentModels.Count; 
        }

        /// <summary>
        /// Get the ExperimentModel
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public ExperimentModel GetExperimentModel(int itemIndex)
        {
            return experimentModels[itemIndex];
        }

        /// <summary>
        /// Get the index of the experimentModel
        /// </summary>
        /// <param name="experItem"></param>
        /// <returns>the index of the experimentModel in current ExperimentSetting</returns>
        public int GetExperimenModelIndex(ExperimentModel experItem)
        {
            return experimentModels.IndexOf(experItem);
        }

        /// <summary>
        /// Get initial starting label for plotting graphs and the progress pane
        /// </summary>
        /// <returns></returns>
        public void SetInitialStartingLabel() 
        {
            initialStartingLabel = currentDataset.totalNumberOfTasks + 1;
        }

        /// <summary>
        /// Get Total Number Of Labelling Rows
        /// </summary>
        /// <returns></returns>
        public int GetTotalNumberOfLabellingRows()
        {
            return currentDataset.totalNumberOfLabellingRows;
        }

        /// <summary>
        /// Background Thread for running the active learning experiment
        /// <param name="worker"></param>
        /// <param name="e"></param>
        public void RunParallelActiveLearning(
            System.ComponentModel.BackgroundWorker worker,
            System.ComponentModel.DoWorkEventArgs e)
        {

            //Create a state of the Thread
            CurrentParallelState currentState = new CurrentParallelState();
        
            //Set setting in the experimentSetting Class
            int totalNumberOfModels = GetNumberOfExperiemntModels();
            //Clear previous results
            ActiveLearning.resetParallelAccuracyList(totalNumberOfModels);

            //obtain the accuracy list reference
            accuracyArrayOfAllExperimentModels = ActiveLearning.accuracyArray;
       
            //The RunTypes that have Worker Confusion Matrices
            RunType[] runTypesHaveWorkerMatrices = { RunType.DawidSkene, RunType.BCC, RunType.CBCC };

            //Set the models selected in the setting pane
            string[] currentModelNames = new string[totalNumberOfModels];
            RunType[] currentRunTypes = new RunType[totalNumberOfModels];
            TaskSelectionMethod[] currentTaskSelectionMethods = new TaskSelectionMethod[totalNumberOfModels];
            WorkerSelectionMethod[] currentWorkerSelectionMethods = new WorkerSelectionMethod[totalNumberOfModels];
            BCC[] currentBCCModels = new BCC[totalNumberOfModels];

            //for each ExperimentModel, set runTypeArray, taskSelectionMethodArray, workerSelectionMethodArray...
            for (int i = 0; i < totalNumberOfModels; i++)
            {
                ExperimentModel currentExperimentModel = GetExperimentModel(i);
                RunType currentRunType = currentExperimentModel.runType;
                currentRunTypes[i] = currentRunType;

                //set the task selection method
                currentTaskSelectionMethods[i] = currentExperimentModel.taskSelectionMethod;  

                //Add into worker selection method array if the runType can have worker selection 
                if (runTypesHaveWorkerMatrices.Contains(currentRunType))
                {
                    currentWorkerSelectionMethods[i] = currentExperimentModel.WorkerSelectionMethod;

                    //Add corresponding model
                    //if the RunType is BCC, add into BCC model array
                    if (currentRunType == RunType.BCC)
                    {
                        currentBCCModels[i] = new BCC();
                    }//CBCC Model
                    else if(currentRunType == RunType.CBCC)
                    {
                        CBCC currentBCCmodel = new CBCC();
                        currentBCCModels[i] = currentBCCmodel;
                    }
                } //end if the runType has worker confusion matrices
            } //end for

            currentModelNames = currentModelNames.Select((s, i) => CrowdsourcingModels.Program.GetModelName(currentDataset.GetDataSetNameWithoutExtension(), currentRunTypes[i])).ToArray();

            //run RunParallelActiveLearning in the ActiveLearning  
            ActiveLearning.RunParallelActiveLearning(currentDataset.LoadData(), currentModelNames, currentRunTypes, 
                currentBCCModels, currentTaskSelectionMethods, currentWorkerSelectionMethods, 
                communityCount, numberOfLabellingRound);

            currentState.isRunningComplete = true;
            Debug.WriteLine("RunParallelActiveLearning Complete");
            //isSimulationComplete = true;
            //worker.ReportProgress(0, currentState);

        }//end function RunParallelActiveLearning

        /// <summary>
        /// RunBatchRunning experiment in background thread 
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        public void RunBatchRunningExperiment(
            System.ComponentModel.BackgroundWorker worker,
            System.ComponentModel.DoWorkEventArgs e)
        {
            CurrentParallelState currentState;
            //Get the number of the total experiment Items
            int totalNumberOfModels = GetNumberOfExperiemntModels();

            //A List of Results array ofreport all experimentItems 
            results = new List<Results>();
        
            currentState = new CurrentParallelState();

            //Running the current experimentSetting lists and runGold accordinglyb
            foreach (ExperimentModel currentExpItem in experimentModels) 
            {
                //currentState.currentExperimentModel = currentExpItem;
                if (MainPage.mainPageForm.isExperimentComplete)
                {
                    return;
                }

                currentState = new CurrentParallelState();
                currentState.currentExperimentModelIndex = GetExperimenModelIndex(currentExpItem);
                currentState.isCurrentModelCompleted = false;
                //Pass the started currentIndex to the mainpage, such that this currentExpItem is started
                worker.ReportProgress(0, currentState);
                
                //Create a BCC/CBCC model of the Batch Running Experiment
                BCC currentModel = null;
                if( currentExpItem.runType == RunType.BCC)
                {
                    currentModel = new BCC();
                }
                else if(currentExpItem.runType == RunType.CBCC)
                {
                    currentModel = new CBCC();
                    ((CBCC)currentModel).SetCommunityCount(MainPage.mainPageForm.currentExperimentSetting.communityCount);
                }

                //When the experiment is not running
                while (!MainPage.mainPageForm.isExperimentRunning ) 
                {
                }

                if (MainPage.mainPageForm.isExperimentComplete) 
                {
                    return;
                }
                results.Add(CrowdsourcingModels.Program.RunGold(currentDataset.datasetPath, currentExpItem.runType, currentModel, MainPage.mainPageForm.currentExperimentSetting.communityCount));

                //When the experiment is not running
                while (!MainPage.mainPageForm.isExperimentRunning)
                {
                }


                if (MainPage.mainPageForm.isExperimentComplete)
                {
                    return;
                }
                //add the results into the List<Results[]>
                //convert the lists into a single array of results (using LINQ)
                //notify the mainPage UI while it is completed
                currentState.isCurrentModelCompleted = true;
                worker.ReportProgress(0, currentState);
           
            } // For each experimentItem

            //The Batch Running is completed
            currentState.isRunningComplete = true;
     
        }

        /// <summary>
        /// Get the accuracy of a labelling point
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <param name="currentLabel"></param>
        /// <returns></returns>
        public double getAccuracy(int indexOfExperimentModel, int currentLabel) 
        {
            int requiredLabelIndex = currentLabel - experimentModels[indexOfExperimentModel].labelStartingPoint;

            //return error when the currentLabel is larger than totalNumberOfLabels
            if (currentLabel > currentDataset.totalNumberOfLabellingRows)
            {
                return -1;
            }

            //check if the currentLabelIndex result is simulated
            Debug.WriteLine("//check if the currentLabelIndex result is simulated");
            while (requiredLabelIndex >= accuracyArrayOfAllExperimentModels[indexOfExperimentModel].Count)
            {
                //do nothing and wait 
       
            }
            double requiredAccuracy = accuracyArrayOfAllExperimentModels[indexOfExperimentModel][requiredLabelIndex];


            Debug.WriteLine("//check if the current accuracy has loaded into the accuracyList");
            //wait until the current accuracy has loaded into the accuracyList
            while (requiredAccuracy == 0)
            {
       
                requiredAccuracy = accuracyArrayOfAllExperimentModels[indexOfExperimentModel][requiredLabelIndex];
            }

            Debug.WriteLine("//return Accuracy");
            return requiredAccuracy;
        }

        /// <summary>
        /// Check if the experimentItem should start at this labelling point
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <param name="currentLabellingRound"></param>
        /// <returns></returns>
        public Boolean isExperimentItemStarted(int indexOfExperimentItem, int currentLabellingRound)
        {
            return currentLabellingRound >= experimentModels[indexOfExperimentItem].labelStartingPoint;
        }

        /// <summary>
        /// Get the ActiveLearningResult of the required labelling round
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <param name="currentLabellingRound"></param>
        /// <returns></returns>
        public ActiveLearningResult GetActiveLearningResult(int indexOfExperimentItem, int currentLabellingRound)
        {
            int requiredLabelIndex = currentLabellingRound - experimentModels[indexOfExperimentItem].labelStartingPoint;

            while (requiredLabelIndex >= ActiveLearning.taskValueListArray[indexOfExperimentItem].Count)
            {
                //do nothing and wait 
            }

            ActiveLearningResult tempResultRow = ActiveLearning.taskValueListArray[indexOfExperimentItem][requiredLabelIndex];
          //  experimentModels[indexOfExperimentItem].AddWorkerValueRow(tempResultRow);
            
            return tempResultRow;
        }


        /// <summary>
        /// Get the List of ActiveLearningResult of the given index of ExperimentModel
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <param name="currentLabellingRound"></param>
        /// <returns></returns>
        public List<ActiveLearningResult> GetActiveLearningResult(int indexOfExperimentModel)
        {
            List<ActiveLearningResult> currentActiveLearningResultList = ActiveLearning.taskValueListArray[indexOfExperimentModel];
            while (currentActiveLearningResultList == null)
            {
                //wait until the list has results
                currentActiveLearningResultList = ActiveLearning.taskValueListArray[indexOfExperimentModel];
            }

            return currentActiveLearningResultList;
        }

        /// <summary>
        /// Check if the worker exists in the ExperimentModel
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public Boolean ContainWorkerId(int indexOfExperimentModel, String workerId )
        {
            List<ActiveLearningResult> currentResultList = ActiveLearning.taskValueListArray[indexOfExperimentModel];
            currentResultList = currentResultList.GetRange(0, currentResultList.Count - 1);
         
            return currentResultList.Any(o => o.WorkerId == workerId);
        }
       
        /// <summary>
        /// Check if the task Id is valid in the experimentModel
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <param name="currentTaskId"></param>
        /// <returns></returns>
        public Boolean ContainTaskId(int indexOfExperimentModel, String currentTaskId)
        {
            List<ActiveLearningResult> currentResultList = ActiveLearning.taskValueListArray[indexOfExperimentModel];
            currentResultList = currentResultList.GetRange(0,currentResultList.Count-1);
            Boolean isExist = currentResultList.Any(o => o.TaskId == currentTaskId);
            return isExist;
        }

        /// <summary>
        /// Get the number of community in the experiment
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <returns></returns>
        public int GetCommunityCount(int indexOfExperimentModel)
        {

            Results currentExperimentResults = GetExperimentModelResult(indexOfExperimentModel);
            return currentExperimentResults.CommunityCount;
        }

        /// <summary>
        /// Get the worker results of the experiment model
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public double[,] getWorkerResults(int indexOfExperimentModel, String workerId)
        {
            Results currentExperimentResults = GetExperimentModelResult(indexOfExperimentModel);
            double[,] printableConfusionMatrix = currentExperimentResults.GetConfusionMatrices(workerId);
            return printableConfusionMatrix;

        }

        /// <summary>
        /// Get the string of the confusion matrix
        /// </summary>
        /// <param name="printableConfusionMatrix"></param>
        /// <returns></returns>
        public String getConfusionMatrixString(double[,] printableConfusionMatrix)
        {
            
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            int labelCount = printableConfusionMatrix.GetLength(0);
          
            //Print the header
            for (int j = 0; j < labelCount; j++)
                writer.Write("     {0}        ", j+1);
            writer.WriteLine();

            //Print the values
            for (int i = 0; i < labelCount; i++)
            {
                writer.Write((i+1) + " ");
                for (int j = 0; j < labelCount; j++)
                    //writer.Write(",{0:0.0000}", printableConfusionMatrix[i, j]);
                    writer.Write(String.Format("{0:0.0000}    ", printableConfusionMatrix[i, j]));
                writer.WriteLine();
            }

            return writer.ToString();
        }

        /// <summary>
        /// Get the double array of the community results
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <param name="communityIndex"></param>
        /// <returns></returns>
        public double[,] getCommunityResults(int indexOfExperimentItem, int communityIndex)
        {
            Results currentExperimentResults = GetExperimentModelResult(indexOfExperimentItem);

            double[,] printableConfusionMatrix = currentExperimentResults.GetConfusionMatrices(communityIndex: communityIndex);
            return printableConfusionMatrix;

        }
        
        /// <summary>
        /// Return the task true label 
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Discrete GetTaskTrueLabel(int indexOfExperimentItem, string taskId)
        {
            Results currentExperimentResults = GetExperimentModelResult(indexOfExperimentItem);
            return currentExperimentResults.getTaskTrueLabel(taskId);
        }

        /// <summary>
        /// Get the Result Class of the experiment model
        /// </summary>
        /// <param name="indexOfExperimentModel"></param>
        /// <returns></returns>
        private Results GetExperimentModelResult(int indexOfExperimentModel)
        {
            Results currentExperimentResults = null;

            //Retreive the Result object according to the experiment type
            //For Active Learning
            if (experimentType == ExperimentType.ActiveLearning)
            {
                currentExperimentResults = ActiveLearning.results[indexOfExperimentModel];
            }
            else //for other experiment types, eg. Batch Running Experiment
            {
                currentExperimentResults = results[indexOfExperimentModel];
            }

            return currentExperimentResults;
        }
    } //end ExperimentSetting
} //end namespace
