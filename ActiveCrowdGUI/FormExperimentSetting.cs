using ComponentFactory.Krypton.Toolkit;
using CrowdsourcingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AcriveCrowdGUI
{
    
    
    /// <summary>
    /// Windows form for setting experiment
    /// </summary>
    public partial class FormExperimentSetting : KryptonForm
    {
        //Set current instances
        public static FormExperimentSetting currentFormInstance = null;

        //temporary array for storing the TaskSelectionMethod Options of each RunType 
        int[] currentStartingLabelPoints;
        List<ExperimentModel> currentListOfExperimentModels;
        
        //Index of the delete button in the model gridview   
        int deleteButtonIndex ;
        ExperimentType currentExperimentType;
        int numberOfIterations = 1;


        public FormExperimentSetting(ExperimentType currentExperimentType)
        {
            InitializeComponent();
            this.currentExperimentType = currentExperimentType;
        
            dropDownListOfWorkerSelectionMethod.Items.AddRange(Enum.GetNames(typeof(WorkerSelectionMethod)));

            //initial the tempTaskSelectionMethodsArray
            //tempTaskSelectionMethodsArray = new int[checkedListBoxForRunTypeModels.Items.Count, checkedListBoxForTaskSelection.Items.Count];

            //set current FormActiveLearningSetting instance pointer
            currentFormInstance = this;
            //Obtain all available datasets
            comboBoxForSelectingDataset.Items.AddRange(GlobalVariables.getAllDatasetNames());
            comboBoxForSelectingDataset.SelectedIndex = 0;

            dropDownListOfWorkerSelectionMethod.SelectedIndex = 0;
            //Run Default Experiment Settings
            RunDefaultExperimentSettings();
            SetComboBoxListOfRunTypesValue();
          
            currentListOfExperimentModels = new List<ExperimentModel>();

            //change the visibility of the buttons according to the current experiment type
            switch (currentExperimentType)
            {
                case ExperimentType.BatchRunning:
                   
                    btnRunExperiment.Visible = false; 
                    buttonRunBatchRunning.Visible = true;
                    //disable task selection method and worker selection method
                
                    labelTaskSelectionMethod.Visible = false;
                    comboBoxListOfTaskSelectionMethods.Visible = false;
                    labelWorkerSelectionMethod.Visible = false;
                    dropDownListOfWorkerSelectionMethod.Visible = false;
                    //change the outerbox to the corresponding experiment Type
                    groupBoxExperimentOuterBox.Text = "Batch Running Settings";
                    deleteButtonIndex = 1;
                    groupBoxInitialNumberOfLabelling.Visible = false;

                    //Enlarge the RunType and the AddModel Button
                    comboBoxListOfRunTypes.Width = 727;
                  //  buttonAddModel.Width = 264;
                  //  buttonAddModel.Location = new Point(618, this.buttonAddModel.Location.Y);
                    break;

                case ExperimentType.ActiveLearning:

                    btnRunExperiment.Visible = true;
                    buttonRunBatchRunning.Visible = false;
                    deleteButtonIndex = 3;
                    //Enable task selection method and worker selection method
                    labelTaskSelectionMethod.Visible = true;
                    comboBoxListOfTaskSelectionMethods.Visible = true;
                    labelWorkerSelectionMethod.Visible = true;
                    dropDownListOfWorkerSelectionMethod.Visible = true;
                    groupBoxInitialNumberOfLabelling.Visible = true;
             
                    groupBoxExperimentOuterBox.Text = "Active Learning Settings";

                    //Reset the normal size of the RunType and the AddModel Button
                    comboBoxListOfRunTypes.Width = 256;
                 //   buttonAddModel.Width = 128;
                  //  buttonAddModel.Location = new Point(752, this.buttonAddModel.Location.Y);

                    break;

                default:
                    break;
            }

            initCurrentModels();
        } //end Constructor
        
        /// <summary>
        /// Load Default Experiment Settings 
        /// </summary>
        private void RunDefaultExperimentSettings()
        {
            //Current should be empty
        } 

        /// <summary>
        /// InitCurrentModels Grid View
        /// </summary>
        private void initCurrentModels()
        {
      
            dataGridViewOfCurrentModels.ColumnCount = deleteButtonIndex;
            string[] columnNames = { "Run Type", "Task Selection Strategy", "Worker Selection Strategy" };
            //set the column names and font to Bold in the dataGridViewOfCurrentModels
            
            for (int i = 0; i < dataGridViewOfCurrentModels.ColumnCount; i++)
            {
                dataGridViewOfCurrentModels.Columns[i].Name = columnNames[i];
                dataGridViewOfCurrentModels.Columns[i].HeaderCell.Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            }
                
            //add delete link
            DataGridViewButtonColumn btnDeleteModel = new DataGridViewButtonColumn();
            //Button btnDeleteModel = new Button();
           
            dataGridViewOfCurrentModels.Columns.Add(btnDeleteModel);
            dataGridViewOfCurrentModels.Columns[deleteButtonIndex].Width = 70;
            btnDeleteModel.HeaderText = "";
            btnDeleteModel.Text = "Delete";
            btnDeleteModel.Name = "btn";
            btnDeleteModel.UseColumnTextForButtonValue = true;
             
            //allows full row select
            dataGridViewOfCurrentModels.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //supports multiple rows selection, such that multiple popup 
            dataGridViewOfCurrentModels.MultiSelect = true;
        
        }
        /// <summary>
        /// Restore to set previous experiment setting
        /// </summary>
        /// <param name="exprSetting"></param>
        public void setPreviousExperimentSetting(ExperimentSetting exprSetting)
        {
            currentListOfExperimentModels = new List<ExperimentModel>();
            dataGridViewOfCurrentModels.Rows.Clear();
            comboBoxForSelectingDataset.SelectedIndex = GlobalVariables.getDatasetIndex(exprSetting.currentDataset);

            //for each experimentItem in the experimentSetting
            for (int i = 0; i < exprSetting.GetNumberOfExperiemntModels(); i++)
            {
                ExperimentModel currentExperimentItem = exprSetting.GetExperimentModel(i);
          //      currentExperimentModel.ResetWorkerValueRows();
                
                //add to the experimentList
                currentListOfExperimentModels.Add(currentExperimentItem);
                //add to the grid view
                Object[] tempRow = { currentExperimentItem.runType, currentExperimentItem.taskSelectionMethod, currentExperimentItem.WorkerSelectionMethod };
                dataGridViewOfCurrentModels.Rows.Add(tempRow);

            } //End for

        }
   

        /// <summary>
        /// Click event handler for running active learning experiment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRunExperiment_Click(object sender, EventArgs e)
        {
            RunExperiment();
        }

        private void RunExperiment()
        {
            if (!checkIfTheExperimentItemsListIsEmpty())
            {
                return;
            }

            SetExperimentSetting();
            //call the running experiment functions
            MainPage.mainPageForm.isExperimentRunning = true;

            //start the experiment according to the experiment type
            switch (currentExperimentType)
            {

                //Run Active Learning Experiment
                case ExperimentType.ActiveLearning:
                    //reset the parameters 
                    MainPage.mainPageForm.InitialiseActiveLearningExperiment();
                    if (comboBoxForSelectingDataset.SelectedIndex < GlobalVariables.communityCounts.Length)
                    {
                        MainPage.mainPageForm.currentExperimentSetting.communityCount = GlobalVariables.communityCounts[comboBoxForSelectingDataset.SelectedIndex];
                    }
                        MainPage.mainPageForm.StartActiveLearning();
                    break;

                //Run Batch Running Experiment
                case ExperimentType.BatchRunning:
                    MainPage.mainPageForm.InitialiseBatchRunningExperiment();
                    //check if the community count is set 
                    if (comboBoxForSelectingDataset.SelectedIndex < GlobalVariables.communityCounts.Length)
                    {
                        MainPage.mainPageForm.currentExperimentSetting.communityCount = GlobalVariables.communityCounts[comboBoxForSelectingDataset.SelectedIndex];
                    }
                    MainPage.mainPageForm.StartBatchRunning();
                    break;
       
            } //End switch
            
           
            //Close current ExperimentSetting Form
            this.Close();
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool checkIfTheExperimentItemsListIsEmpty()
        {
            //check if there is any model added 
            if (currentListOfExperimentModels.Count == 0)
            {
                MessageBox.Show("Please add a model first!");
                return false;
            }

            return true;
        
        }

        /// <summary>
        /// Set ExperimentSetting in the MainPage
        /// </summary>
        private void SetExperimentSetting() 
        {
            //get current Data Path String
            Dataset currentDataSet = GlobalVariables.loadedDatasets[comboBoxForSelectingDataset.SelectedIndex]; 
            
            double mabConstant = GlobalVariables.mabConstants[comboBoxForSelectingDataset.SelectedIndex];

            int initialNumberOfLabelsPerTask = trackBarNumberOfLabellingRounds.Value;

            //Get an array of different starting label points of each labelling round 
            currentStartingLabelPoints = currentDataSet.GetLabelStartingPoints();

            //Call the SetExperimentSettings in the mainPageForm
            MainPage.mainPageForm.SetNewExperimentSettings(currentDataSet, initialNumberOfLabelsPerTask, currentListOfExperimentModels,currentExperimentType,numberOfIterations);
        }
   
        /// <summary>
        /// Clear the existing selected model(s) when the user switches to other dataset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxForSelectingDataset_SelectedIndexChanged(object sender, EventArgs e)
        {
            //change labelling round maxvalue
            Dataset currentDataset = GlobalVariables.loadedDatasets[comboBoxForSelectingDataset.SelectedIndex];

            trackBarNumberOfLabellingRounds.Maximum = currentDataset.maximumOfLabellingRound;
            Debug.Write(currentDataset.maximumOfLabellingRound + "");
            labelMaximumLabellingRound.Text = currentDataset.maximumOfLabellingRound + "";
            trackBarNumberOfLabellingRounds.Invalidate();
            
            //clear the currentModels
            ClearSelectedModels();
          
        }

        /// <summary>
        /// Clear the selected models in the data grid view
        /// </summary>
        private void ClearSelectedModels()
        {
            //clear the currentModels
            dataGridViewOfCurrentModels.Rows.Clear();
            currentListOfExperimentModels = new List<ExperimentModel>();

        }


        /// <summary>
        /// Load the new dataset(s) into the application, and update the dataset names in the datasetNames comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadDataset_Click(object sender, EventArgs e)
        {
            MainPage.mainPageForm.LoadDataset();

            //add the loaded Datasets
            comboBoxForSelectingDataset.Items.Clear();
            comboBoxForSelectingDataset.Items.AddRange(GlobalVariables.getAllDatasetNames());
            comboBoxForSelectingDataset.SelectedIndex = comboBoxForSelectingDataset.Items.Count - 1 ;

        }

        
        /// <summary>
        /// Initial the Run Types Value in the comboBox
        /// </summary>
        private void SetComboBoxListOfRunTypesValue()
        {
            //Set the RunTypes into comboBox
            comboBoxListOfRunTypes.Items.Clear();
            comboBoxListOfRunTypes.Items.AddRange(Enum.GetNames(typeof(RunType)));

            comboBoxListOfTaskSelectionMethods.Items.Clear();
            comboBoxListOfTaskSelectionMethods.Items.AddRange(Enum.GetNames(typeof(TaskSelectionMethod)));

            //Remove VoteDistribution when it is BatchRunning Experiment
            if (currentExperimentType == ExperimentType.BatchRunning)
            {
                comboBoxListOfRunTypes.Items.Remove("VoteDistribution");
            }
                comboBoxListOfRunTypes.SelectedIndex = 0;
            comboBoxListOfTaskSelectionMethods.SelectedIndex = 0;
        }

        
        /// <summary>
        /// Confine the available taskSelection stragetory of the selected RunType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxListOfRunTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if it is majority voting, disable the combox box of task selection methods 
            if (comboBoxListOfRunTypes.SelectedIndex == (int)RunType.MajorityVote)
            {
                comboBoxListOfTaskSelectionMethods.SelectedIndex = (int)GlobalVariables.mvDefaultTaskSelectionMethod;
                comboBoxListOfTaskSelectionMethods.Enabled = false;
            }
            else 
            {
                comboBoxListOfTaskSelectionMethods.Enabled = true;
                comboBoxListOfTaskSelectionMethods.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Event Handler when user presses the AddModel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddModel_Click(object sender, EventArgs e)
        {
            AddModel();
        }

        /// <summary>
        /// Add the model into temporary experimentItem List
        /// </summary>
        private void AddModel()
        {
            Dataset currentDataset = GlobalVariables.loadedDatasets[comboBoxForSelectingDataset.SelectedIndex];
            currentStartingLabelPoints = currentDataset.GetLabelStartingPoints();

            //Add ExperimentModel when it is ActiveLearning Experiment
            if (currentExperimentType == ExperimentType.ActiveLearning)
            {
                ExperimentModel currentExpItem = getExperimentItem((RunType)(comboBoxListOfRunTypes.SelectedIndex), (TaskSelectionMethod)comboBoxListOfTaskSelectionMethods.SelectedIndex,
                (WorkerSelectionMethod)dropDownListOfWorkerSelectionMethod.SelectedIndex, currentStartingLabelPoints);

                //add to the experimentList
                currentListOfExperimentModels.Add(currentExpItem);
                Object[] tempRow = { (RunType)comboBoxListOfRunTypes.SelectedIndex, (TaskSelectionMethod)comboBoxListOfTaskSelectionMethods.SelectedIndex, (WorkerSelectionMethod)dropDownListOfWorkerSelectionMethod.SelectedIndex } ;
                dataGridViewOfCurrentModels.Rows.Add(tempRow);

            } //Add ExperimentModel when it is BatchRunning Experiment
            else if(currentExperimentType == ExperimentType.BatchRunning) 
            {
                ExperimentModel currentExpItem = getExperimentItem((RunType)(comboBoxListOfRunTypes.SelectedIndex + 1), (TaskSelectionMethod)comboBoxListOfTaskSelectionMethods.SelectedIndex,
                (WorkerSelectionMethod)dropDownListOfWorkerSelectionMethod.SelectedIndex, currentStartingLabelPoints);

                //add to the experimentList
                currentListOfExperimentModels.Add(currentExpItem);
                Object[] tempRow = { (RunType)(comboBoxListOfRunTypes.SelectedIndex + 1), (TaskSelectionMethod)comboBoxListOfTaskSelectionMethods.SelectedIndex, (WorkerSelectionMethod)dropDownListOfWorkerSelectionMethod.SelectedIndex};
                //add to the grid view
                dataGridViewOfCurrentModels.Rows.Add(tempRow);
            }

        } //End AddModel


        /// <summary>
        /// Initial the experimentItem according to the previous setting
        /// </summary>
        /// <param name="currentRunType"></param>
        /// <param name="currentTaskSelectionMethod"></param>
        /// <param name="currentWorkerSelectionMethod"></param>
        /// <param name="labelStartingPoints"></param>
        /// <param name="totalNumberOfLabels"></param>
        /// <returns></returns>
        private ExperimentModel getExperimentItem(RunType currentRunType, TaskSelectionMethod currentTaskSelectionMethod, WorkerSelectionMethod currentWorkerSelectionMethod, int[] labelStartingPoints)
        {
            //if the RunType is MajorityVote, no TaskSelectionMethods would be selected
            if (currentRunType == RunType.MajorityVote)
            {
                return new ExperimentModel(GlobalVariables.mvDefaultTaskSelectionMethod, currentRunType, 1, labelStartingPoints[0]);
            }

            //if it is an entropy task, add the different labelling rounds
            if (currentTaskSelectionMethod == TaskSelectionMethod.EntropyTask)
            {
                int currentLabellingRound = trackBarNumberOfLabellingRounds.Value;
                return new ExperimentModel(currentTaskSelectionMethod, currentRunType, currentLabellingRound, labelStartingPoints[currentLabellingRound - 1]);
                
            } //if it is EntropyMABTask, add a mabConstant into the ExperimentModel
            else if (currentTaskSelectionMethod == TaskSelectionMethod.EntropyMABTask)
            {
                double mabConstant = GlobalVariables.mabConstants[comboBoxForSelectingDataset.SelectedIndex];
                return new ExperimentModel(currentTaskSelectionMethod, currentRunType, 1, labelStartingPoints[0], mabConstant);
            }
            else//other taskSelectionMethods, or empty in the batch running 
            {
                return new ExperimentModel(currentTaskSelectionMethod, currentRunType, 1, labelStartingPoints[0]);
            }//end if  

        }

        /// <summary>
        /// Update the labelling round of the mabInstance when user scrolls the trackBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarNumberOfLabellingRounds_Scroll(object sender, EventArgs e)
        {
           //for each experiment model
           for(int i = 0 ; i < currentListOfExperimentModels.Count; i++)
           {
                //add the number of labelling round, if the TaskSelectionMethod is EntropyTask
                if(currentListOfExperimentModels[i].taskSelectionMethod == TaskSelectionMethod.EntropyTask)
                {
                    currentListOfExperimentModels[i].numberOfLabellingRound = trackBarNumberOfLabellingRounds.Value;
                    currentListOfExperimentModels[i].labelStartingPoint = currentStartingLabelPoints[trackBarNumberOfLabellingRounds.Value - 1];
                }
           } // end for each selected experiment items
            
        }
       
        /// <summary>
        /// Show "Load Dataset(s)" tooltip when user hovers the loadDataset button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadDataset_MouseHover(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.buttonLoadDataset, "Load Dataset(s)");
        }

        /// <summary>
        /// Switch to view dataset view when user clicks the "view dataset" button of the dataset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonViewDataset_Click(object sender, EventArgs e)
        {
            //switch to ViewDataset
            MainPage.mainPageForm.ViewDataset(comboBoxForSelectingDataset.SelectedIndex);

        }
        
        /// <summary>
        /// Delete Selected ExperimentModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewOfCurrentModels_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == deleteButtonIndex)
            {
                //Delete Selected ExperimentModel
                dataGridViewOfCurrentModels.Rows.RemoveAt(e.RowIndex);
                currentListOfExperimentModels.RemoveAt(e.RowIndex);
            }
        }

        /// <summary>
        /// Auto add the model when the user presses the enter button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormActiveLearningSetting_KeyDown(object sender, KeyEventArgs e)
        {
            //Add the model when an Enter key is pressed
            if (e.KeyCode == Keys.Enter)
            {
                AddModel();
            }
        }

        /// <summary>
        /// Event handler for deleting all selected mondels 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteAllSelectedModels_Click(object sender, EventArgs e)
        {
            //clear the currentModels
            ClearSelectedModels();
          
        }

        /// <summary>
        /// Event Handler for the button of loading default models 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDefaultModels_Click(object sender, EventArgs e)
        { 
            MessageBox.Show("No default model at the moment.");

        }

        /// <summary>
        /// Event handler for running the batchRunning 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRunBatchRunning_Click(object sender, EventArgs e)
        {
            RunExperiment();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void groupBoxInitialNumberOfLabelling_Enter(object sender, EventArgs e)
        {

        }

    } //end partial class
} //end namespace
