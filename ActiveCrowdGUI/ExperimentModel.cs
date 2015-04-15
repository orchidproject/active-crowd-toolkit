using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//CrowdsourcingProject namespace
using CrowdsourcingModels;
using System.ComponentModel; 

namespace AcriveCrowdGUI
{
   
    /// <summary>
    /// The class for the selected model in the experiment. 
    /// </summary>
   public class ExperimentModel
    {

        //Settings of the experiment model
       
        /// <summary>
        /// Selected taskSelectionMethod
        /// </summary>
        public TaskSelectionMethod taskSelectionMethod
        {
            get;
            private set;
        }

        
        /// <summary>
        /// Selected RunType 
        /// </summary>
        public RunType runType
        {
            get;
            private set;
        }
       
        /// <summary>
        /// The  labelling starting point of the model after exploration
        /// </summary>
        public int labelStartingPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Number of LabellingRound 
        /// </summary>
        public int numberOfLabellingRound      
        {
            get;
            set;
        }

        /// <summary>
        /// the mab constant 
        /// </summary>
        public double mabConstant
        {
            get;
            private set;
        }

        /// <summary>
        /// boolean indicates if this ExperimentModel is completed, as to switch to the next ExperimentModel
        /// </summary>
        public Boolean isSimulationComplete
        {
            get;
            private set;
        }

        /// <summary>
        /// The list of accuracies of the model
        /// </summary>
        public List<double> accuracyOfTheExperimentModel
        {
            get;
            private set;
        }

        //ActiveLearning BindingList of the result
        public BindingList<ActiveLearningResult> resultsBindingList;
   
        /// <summary>
        /// The worker selection method used in the ExperimentModel
        /// </summary>
        public WorkerSelectionMethod WorkerSelectionMethod
        {
            get;
            private set;
        }

        public ExperimentModel(RunType runType, int numberOfLabellingRound, int labelStartingPoint)
        {
            //set current TaskSelectionMethod to default of the TaskSelectionMethod enum
            this.taskSelectionMethod = default(TaskSelectionMethod);
            this.runType = runType;
            this.labelStartingPoint = labelStartingPoint;
            this.numberOfLabellingRound = numberOfLabellingRound;
            accuracyOfTheExperimentModel = new List<double>();

            isSimulationComplete = false;

            //initial the BindingList
            resultsBindingList = new BindingList<ActiveLearningResult>();
        }

        /// <summary>
        /// Constructor for non-EntropyMABTask Selection Method
        /// </summary>
        /// <param name="taskSelectionMethod"></param>
        /// <param name="runType"></param>
        /// <param name="numberOfLabellingRound"></param>
        /// <param name="labelStartingPoint"></param>
        public ExperimentModel(TaskSelectionMethod taskSelectionMethod, RunType runType, int numberOfLabellingRound, int labelStartingPoint)
            : this(runType, numberOfLabellingRound, labelStartingPoint)
        {
            this.taskSelectionMethod = taskSelectionMethod;
           
        }

        /// <summary>
        /// Constructor for EntropyMABTask selection method
        /// </summary>
        /// <param name="taskSelectionMethod"></param>
        /// <param name="runType"></param>
        /// <param name="numberOfLabellingRound"></param>
        /// <param name="labelStartingPoint"></param>
        /// <param name="mabconstant"></param>
        public ExperimentModel(TaskSelectionMethod taskSelectionMethod, RunType runType, int numberOfLabellingRound, int labelStartingPoint, double mabConstant)
            : this(taskSelectionMethod, runType, numberOfLabellingRound, labelStartingPoint)
        {
            this.mabConstant = mabConstant;

        }

        public Object getWorkerList() 
        {
            return resultsBindingList.Select(m => m.WorkerId).Distinct();
        }

        /// <summary>
        /// Return the Name of this experiment item, used for labelling and indexing for items
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        {
            string currentExperimentItemName = "";
            ExperimentSetting currentExperimentSetting = MainPage.mainPageForm.currentExperimentSetting;
            //Only display the runType name of this experiment model,  when the experiment is Batch Running
            if (currentExperimentSetting.experimentType == ExperimentType.BatchRunning)
            {
                return GetRunTypeString();
            }
            
            //Display both runType and taskSelectionMethod of the current experiment model, for other types of experiments
            currentExperimentItemName = GetRunTypeString() + ":" + Enum.GetName(typeof(TaskSelectionMethod), taskSelectionMethod);

            //if the taskSelectionMethod is EntropyMABTask, add the MAB value to the model name
            if (taskSelectionMethod == TaskSelectionMethod.EntropyMABTask)
            {
                currentExperimentItemName += mabConstant;
            }

            return currentExperimentItemName;
        }

        /// <summary>
        /// Return the runType of this experimentModel in string format
        /// </summary>
        /// <returns></returns>
        public string GetRunTypeString()
        {
            return Enum.GetName(typeof(RunType), runType);
        }
    } //End Class
} //End Namespace
