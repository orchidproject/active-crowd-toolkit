using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows;
using ComponentFactory.Krypton.Toolkit;
using LINQtoCSV;
using System.Diagnostics;
using ZedGraph;
using CrowdsourcingModels;
using AC.ExtendedRenderer.Toolkit;
namespace AcriveCrowdGUI
{   
    /// <summary>
    /// The Main Page of the GUI 
    /// </summary>
    public partial class MainPage : KryptonForm
    {
        //Setting form for running the Active Learning Experiment
        private FormExperimentSetting experimentSettingForm = null;
        
        //set up current timestep for assigning labels
        public Boolean isExperimentRunning = false;
        public Boolean isExperimentComplete = true;
        public Boolean isGraphLoaded = false;
        public static MainPage mainPageForm  = null;
   
        public CurveList accuracyGraphCurveList;
        public ExperimentSetting currentExperimentSetting = null;
      
        int currentRunningExperimentItemIndex = 0;
        
        // List of FormWorkerDetail,
        // Use for closing all the existing FormWorkerDetail Form
        List<KryptonForm> openedForms = new List<KryptonForm>();
      

        ///Graph Panes

        //Accuracy Graph Pane in the left pane
        GraphPane accuracyGraphPane;

        //Individual window form of the accuracy graph 
        FormGraph accuracyGraphForActiveLearning;

        //Curves' colour, should have at least 17 colours
        Color[] colourOptions = { Color.Blue, Color.Black, Color.Violet, Color.Green, Color.DarkGray, 
                                        Color.Brown, Color.SaddleBrown, Color.Orange, Color.Indigo, 
                                        Color.Magenta, Color.Olive, Color.Aquamarine, Color.DarkGoldenrod, Color.Cyan, Color.Pink, Color.Yellow };

        public MainPage()
        {
            InitializeComponent();

            //Load Datasets
            GlobalVariables.LoadDatasets();
            //display Main page when the program starts
            tabControlForMainPage.SelectedIndex = 0;

            //Temporary remove tabpages "View Workers"
            tabControlForMainPage.TabPages.RemoveAt(3);

            //The top panel(progress pane) of active learning reamins the same size when the form is resized 
            splitContainerActiveLearningOuterContainer.FixedPanel = FixedPanel.Panel1;

            //set MainPageForm instance
            mainPageForm = this;
           
            //AccuracyGraph Settings
            InitAccuracyGraphPane();

            isGraphLoaded = true;

            //Set the datagridviews in the Active Learning

            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.DataPropertyName = "WorkerId";
            col1.HeaderText = "WorkerId";
            col1.Name = "worker_id";
            dataGridViewActiveLearningSchedule.Columns.Add(col1);

            DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            col2.DataPropertyName = "TaskId";
            col2.HeaderText = "TaskId";
            col2.Name = "Task_Id";
            dataGridViewActiveLearningSchedule.Columns.Add(col2);

            dataGridViewInnerCommunity.ColumnCount = 1;
            dataGridViewInnerCommunity.Columns[0].HeaderText = "Community";
            SetDataGridViewHeaderToBold(dataGridViewInnerCommunity);
        }

    
        private void MainPage_Load(object sender, EventArgs e)
        {
            LoadActiveLearningWorkerDataGridView();
            PopulatedataGridViewActiveLearningWorkers1();

            LoadBatchRunning();

        }

        /// <summary>
        /// [Active Learning] Init the Accuracy Graph Pane
        /// </summary>
        private void InitAccuracyGraphPane()
        {
            accuracyGraphPane = graphControlAccuracyGraph.GraphPane;

            accuracyGraphPane.Title.IsVisible = false;
            accuracyGraphPane.XAxis.Title.Text = "Number of Labels";
            accuracyGraphPane.YAxis.Title.Text = "Accuracy";
            //hide the control grey border
            accuracyGraphPane.Margin.All = 5;
            graphControlAccuracyGraph.MasterPane.Border.IsVisible = false;
            accuracyGraphPane.Border.IsVisible = false;

            // Set the colors to white show it shows up on a dark background
            accuracyGraphPane.XAxis.Color = Color.DarkBlue;
            accuracyGraphPane.YAxis.Color = Color.DarkBlue;
            accuracyGraphPane.XAxis.Scale.FontSpec.FontColor = Color.DarkBlue;
            accuracyGraphPane.XAxis.Title.FontSpec.FontColor = Color.DarkBlue;
            accuracyGraphPane.YAxis.Scale.FontSpec.FontColor = Color.DarkBlue;
            accuracyGraphPane.YAxis.Title.FontSpec.FontColor = Color.DarkBlue;
            accuracyGraphPane.Chart.Border.Color = Color.DarkBlue;
            accuracyGraphPane.XAxis.MajorGrid.Color = Color.DarkBlue;
            accuracyGraphPane.YAxis.MajorGrid.Color = Color.DarkBlue;
            accuracyGraphPane.Legend.Border.Color = Color.DarkBlue;
        
        }

        /// <summary>
        /// Initial the datagrid view of the batch running experiment 
        /// </summary>
        private void LoadBatchRunning() 
        {
            //Set work breakdrown 
            dataGridViewBatchRunningProgress.ColumnCount = 2;
            string[] columnNames = { "Run Type", "Progress" };
            SetDataGridViewHeaderToBold(dataGridViewBatchRunningProgress, columnNames);

            //Accuracy data grid view
            dataGridViewBatchRunningAccuracy.ColumnCount = 2;
            string[] columnNamesForAccuracy = { "Run Type", "Accuracy" };
            SetDataGridViewHeaderToBold(dataGridViewBatchRunningAccuracy, columnNamesForAccuracy);

        } //End LoadBatchRunning
         
        /// <summary>
        /// Set DataGridViewHeader to bold
        /// </summary>
        /// <param name="currentDataGridView"></param>
        /// <param name="columnNames"></param>
        private void SetDataGridViewHeaderToBold(DataGridView currentDataGridView, string[] columnNames = null)
        {
            //set the column names and font to Bold in the dataGridViewBatchRunningProgress
            for (int i = 0; i < currentDataGridView.ColumnCount; i++)
            {
                //Set the column name into the column
                if (columnNames != null)
                {
                    currentDataGridView.Columns[i].Name = columnNames[i];
                }
                
                //Set the column to Bold
                currentDataGridView.Columns[i].HeaderCell.Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
            } //end for
        
        }

        /// <summary>
        /// Load BatchRunning Progress bar
        /// Run in background thread                                          
        /// </summary
        private void InitBatchRunningProgress()
        { 
            //get from currentExperemnt setting
            dataGridViewBatchRunningProgress.Rows.Clear();
            dataGridViewBatchRunningAccuracy.Rows.Clear();
            comboBoxBatchRunningModel.Items.Clear();

            //Add row into the dataGridview
            for (int i = 0; i < currentExperimentSetting.GetNumberOfExperiemntModels(); i++)
            {
                ExperimentModel currentExperimentItem = currentExperimentSetting.GetExperimentModel(i);
                Object[] tempRow = { (RunType)currentExperimentItem.runType, "Waiting to proceed" };
                dataGridViewBatchRunningProgress.Rows.Add(tempRow);
                dataGridViewBatchRunningAccuracy.Rows.Add(tempRow);
                //add model into the combo box
                comboBoxBatchRunningModel.Items.Add((RunType)currentExperimentItem.runType);
            }//End for each selected model

            comboBoxBatchRunningModel.SelectedIndex = 0;
        }



        /// <summary>
        /// Create an ExperimentSetting instance for the selected dataset
        /// </summary>
        /// <param name="datasetPath">the dataset path going to examine</param>
        /// <param name="totalLabellingRound">the total number of labelling round</param>
        /// <pa
        /// ram name="currentTaskSelectionMethods">An array containing the selected task selection methods from user</param>
        /// <param name="currentRunTypes">An array containing the selected run types from user</param>
        /// <param name="mabConstant">The mab constant value for the EntropyMABTask</param>
        public void SetNewExperimentSettings(Dataset currentDataset, int totalLabellingRound, List<ExperimentModel> experimentItemList, ExperimentType experimentType, int numberOfIterations = 1)
        {
            currentExperimentSetting = new ExperimentSetting(totalLabellingRound, currentDataset, experimentType, numberOfIterations);

            if (totalLabellingRound > 1) 
            {
                if (experimentItemList.Count == 1) 
                {
                    currentExperimentSetting.numberOfLabellingRound = experimentItemList[0].numberOfLabellingRound;
                }
            }
            for (int i = 0; i < experimentItemList.Count; i++)
            {
                currentExperimentSetting.AddExperimentModel(experimentItemList[i]);
            }
            InitialiseActiveLearningExperiment();
        }


        /// <summary>
        /// This event handler is called when the background thread finishes.
        /// This method runs on the main thread. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerForSimulation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Display the error message
            if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }

            //Update the progressbar
            progressBarForActiveLearning.DisplayText = "Experiment Running Complete";
        }


        /// <summary>
        /// Background Thread for running the simulation in parallel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerForSimulation_DoWork(object sender, DoWorkEventArgs e)
        {
            // This event handler is where the actual work is done. 
            // This method runs on the background thread. 

            // Get the BackgroundWorker object that raised this event.
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;
            int totalNumberOfExperimentItems = currentExperimentSetting.GetNumberOfExperiemntModels();

            //call the RunParallelActiveLearning method for the ExperimentSetting 
            currentExperimentSetting.RunParallelActiveLearning(worker, e);

        }

        /// <summary>
        /// Start Running ActiveLearning Thread
        /// </summary>
        private void StartActiveLearningThread()
        {
   
            isExperimentComplete = false;
            currentExperimentSetting.experimentType = ExperimentType.ActiveLearning;
            // Start the asynchronous operation.
            backgroundWorkerForSimulation.RunWorkerAsync();
      
            backgroundWorkerForAccuracyGraph.RunWorkerAsync();
        }

        /// <summary>
        /// Thread: Start running Active Learning Experiment
        /// </summary> 
        public void StartActiveLearning() 
        {

            if (isExperimentRunning) {
                SetDatasetNameLabel(labelDatasetName);
                //change to the pause icon
                this.btnPlayAndPauseExperiment.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Pause_Hot_icon;
                StartActiveLearningThread();
            }
            
                  
        } //end startActiveLearning

        /// <summary>
        /// Set the dataset name to the label  
        /// </summary>
        /// <param name="currentLabel"></param>
        private void SetDatasetNameLabel(System.Windows.Forms.Label currentLabel)
        {
            currentLabel.Text = currentExperimentSetting.currentDataset.datasetName;
             
        }

        private void loadDatasetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDataset();

        }

        /// <summary>
        /// Allow Users to load datasets by showing an openFileDialog
        /// </summary>
        public void LoadDataset() 
        {
            //Only allow importing CSV files as datasets
            openFileDialog1.Filter = "Comma Separated Values  (.csv)|*.csv";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = true;
            //Display the dataset content
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //string filePathString = openFileDialog1.FileName;
                // string filePathString = System.IO.Path.GetTempPath();
                string filePathString = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                filePathString = filePathString.Replace("\\", "/");

                //Load multiple datasets into the application
                try
                {
                    //Add the imported datasets
                    for (int i = 0; i < openFileDialog1.FileNames.Length; i++)
                    {
                        filePathString = openFileDialog1.FileNames[i].Replace("\\", "/");
                        GlobalVariables.loadedDatasets.Add(new Dataset(filePathString, openFileDialog1.SafeFileNames[i]));

                    }

                    //Display a messageBox if all are successfully loaded
                    string datasetString = "Dataset";
                    if (openFileDialog1.FileNames.Length > 1)
                    {
                        datasetString = "Datasets";
                    }
                
                    MessageBox.Show(datasetString + " loaded");

                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("File has already been opened by other application(s) ");
                    return;
                }

            }//end function         
        }

        /// <summary>
        /// Initialise the experiment
        /// </summary>
        public void InitialiseActiveLearningExperiment() 
        {
            //Close the previous form if it exists
            if (FormExperimentSetting.currentFormInstance != null) {
                FormExperimentSetting.currentFormInstance.Close();
                FormExperimentSetting.currentFormInstance = null;
            }
   

            //initialise the progress bar
            progressBarForActiveLearning.MaxValue = currentExperimentSetting.GetTotalNumberOfLabellingRows();
            progressBarForActiveLearning.MinValue = 0;
            progressBarForActiveLearning.Value = 0;
            progressBarForActiveLearning.DisplayText = 0 + "/" + currentExperimentSetting.GetTotalNumberOfLabellingRows();
            currentRunningExperimentItemIndex = 0;

            //initiate ActiveLearning Graphs
            InitialiseActiveLearningGraphs();
            InitialiseGridViewWorkerValue();
        }

        /// <summary>
        /// Initialise the Batch Running experiment
        /// </summary>
        public void InitialiseBatchRunningExperiment()
        {
            
            InitBatchRunningProgress();
        
        }

        /// <summary>
        /// [Batch Running] Update the models values in the dataGridView according to the experimentModelIndex
        /// </summary>
        /// <param name="experimentModelIndex"></param>
        private void UpdateModelValuesForBatchRunning(int experimentModelIndex)
        {

            //Check if it is the CBCC model --> only display the tab and the data grid view if the model supports community
            RunType currentRunType = currentExperimentSetting.GetExperimentModel(experimentModelIndex).runType;
            tabControlBatchLearningInnerValues.TabPages.Remove(tabPageBatchLearningInnerViewCommunity);
            //Show community tabpage if it is the CBCC model
            if (currentRunType == RunType.CBCC)
            {
                tabControlBatchLearningInnerValues.TabPages.Add(tabPageBatchLearningInnerViewCommunity);
            }

            try
            {
                //if the selected model result has not been loaded yet, return
                if (experimentModelIndex > currentExperimentSetting.results.Count)
                {
                    dataGridViewBatchRunningWorkers.DataSource = null;
                    dataGridViewBatchRunningTasks.DataSource = null;
                    dataGridViewBatchRunningCommunities.DataSource = null;
                    return;
                }

                //Worker GridView
                //Convert the worker string list into string values, such that can be shown in the dataGridView
                var values = from data in currentExperimentSetting.results[experimentModelIndex].Mapping.WorkerIndexToId select new { WorkerId = data };
                dataGridViewBatchRunningWorkers.DataSource = values.ToList();
                SetDataGridViewHeaderToBold(dataGridViewBatchRunningWorkers);

                //Tasks GridView
                var taskValues = from data in currentExperimentSetting.results[experimentModelIndex].Mapping.TaskIndexToId select new { TaskId = data };
                dataGridViewBatchRunningTasks.DataSource = taskValues.ToList();
                SetDataGridViewHeaderToBold(dataGridViewBatchRunningTasks);

                //Show community tabpage if it is the CBCC model
                if (currentRunType == RunType.CBCC)
                {
                    //update the community
                    var communityValues = from data in currentExperimentSetting.results[experimentModelIndex].Mapping.CommunityIndexToId select new { Community = data };
                    dataGridViewBatchRunningCommunities.DataSource = communityValues.ToList();
                    SetDataGridViewHeaderToBold(dataGridViewBatchRunningCommunities);
                }

            }
            catch (Exception nfe)
            {
                Debug.Write(nfe.Message);
                return;
            }
           
        }

        /// <summary>
        /// Set the comboBox values according to the experiment items
        /// </summary>
        private void InitialiseGridViewWorkerValue()
        {
            DisplaySelectedModels(comboBoxShowCurrentModels);
            resetDataGridView();

        }

        /// <summary>
        /// Add the selected models into the combobox of the experiment
        /// </summary>
        /// <param name="currentComboBox"></param>
        private void DisplaySelectedModels(ComboBox currentComboBox)
        {
            currentComboBox.Items.Clear();

            List<ExperimentModel> currentExperimentItemsList = currentExperimentSetting.experimentModels;
                
            //For each ExperimentModel
            for (int i = 0; i < currentExperimentItemsList.Count; i++)
            {
                if (currentExperimentSetting.experimentType == ExperimentType.ActiveLearning)
                {
                    currentComboBox.Items.Add(currentExperimentItemsList[i].ToString());

                }
                else 
                {
                    currentComboBox.Items.Add(currentExperimentItemsList[i].GetRunTypeString());

                }
              
            } //end for

            currentComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Start BatchRunning Experiment
        /// </summary>
        public void StartBatchRunning()
        { 
            //Get the number of iterations
            int numberOfIterations = currentExperimentSetting.numberOfIterations;

            //Initialise the experiment components
            if (isExperimentRunning)
            {
                SetDatasetNameLabel(labelDatasetNameBatchRunning);
                //change to the pause icon
                this.buttonPlayAndPauseBatchRunning.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Pause_Hot_icon;
            
                //set the progress bar Value to 0;
                progressBatchRunning.Value = 0;
        
                // start background thread
                StartBatchRunningThread();
            }
        }

        /// <summary>
        /// Launch the running thread of Batch Running
        /// </summary>
        private void StartBatchRunningThread()
        {
            isExperimentComplete = false;
            //Set current experiment type to BatchRunning
            currentExperimentSetting.experimentType = ExperimentType.BatchRunning;
            // Start the asynchronous operation.
            //run through the experiment in the background thread
            
            //create a new backgroundRunningThread
            backgroundWorkerBatchRunning = new System.ComponentModel.BackgroundWorker();

            backgroundWorkerBatchRunning.WorkerReportsProgress = true;
            backgroundWorkerBatchRunning.WorkerSupportsCancellation = true;
            backgroundWorkerBatchRunning.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerBatchRunning_DoWork);
            backgroundWorkerBatchRunning.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerBatchRunning_ProgressChanged);
            backgroundWorkerBatchRunning.RunWorkerAsync();

            //Run BackgroundWorker
            //Load the value into the right pane
            backgroundWorkerBatchRunningValues.RunWorkerAsync();
        }

        /// <summary>
        /// Reset DataGridViews
        /// </summary>
        private void resetDataGridView()
        {
            dataGridViewActiveLearningSchedule.Rows.Clear();
           // dataGridViewForInnerWorker.Rows.Clear();
            dataGridViewForTaskValue.Rows.Clear();
            dataGridViewInnerCommunity.Rows.Clear();
        }
 

        /// <summary>
        /// Load the filePathString into the DataGridView (View Dataset Tab)
        /// </summary>
        /// <param name="filePathString"></param>
        public void loadDatasetToDataGridView(string filePathString) 
        {

            IEnumerable<DatasetItem> datasetItemEnumerableList = Program.ReadFromCsv(filePathString.Replace("\\", "/"));
            IEnumerable<DatasetItem> datasetItemsList = new List<DatasetItem>(datasetItemEnumerableList);
            dataGridViewOfDataset.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //set the DataSource into GridView
            BindingSource gridDataBinder = new BindingSource();
            gridDataBinder.DataSource = datasetItemsList;
            dataGridViewOfDataset.DataSource = gridDataBinder;

        }
       
        /// <summary>
        /// Change to Batch Running Tabpage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonToBatchRunning_Click(object sender, EventArgs e)
        {
            //batch running button
            tabControlForMainPage.SelectedIndex = 1;
        }

        /// <summary>
        /// Change to Active Learning Tabpage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonToActiveLearning_Click(object sender, EventArgs e)
        {
            //Actuve Running 
            tabControlForMainPage.SelectedIndex = 2;
        }

      
        /// <summary>
        /// Change to View Datasets Tabpage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonToViewDataset_Click(object sender, EventArgs e)
        {
            //view datasets
            tabControlForMainPage.SelectedIndex = 4;
        }


        private void tabPage2_Click(object sender, EventArgs e)
        {
            if (experimentSettingForm != null) 
            { 
                experimentSettingForm.TopMost = true;
            }
            
        }

        private void tabControlForMainPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentTabIndex = tabControlForMainPage.SelectedIndex;
            
            switch (currentTabIndex) 
            {
                //Run Batch Running Tab
                case 1:

                    CheckIfAnotherExperimentIsRunning(ExperimentType.BatchRunning);
                    //open the Batch Running setting pane, including the number of iteration
                    OpenExperimentSettingPane(ExperimentType.BatchRunning);
                    currentRunningExperimentItemIndex = currentTabIndex;
                    break;

                //Active Learning Tab
                case 2:
                    CheckIfAnotherExperimentIsRunning(ExperimentType.ActiveLearning);
                    //ask user to set experiment settings to run activeLearning
                    OpenExperimentSettingPane(ExperimentType.ActiveLearning);
                    currentRunningExperimentItemIndex = currentTabIndex;
                    break;

                //View Dataset Tab
                case 3:
                    CloseExperimentSettingPane();
                    
                    //Load the datasetnames in the comboBox
                    List<String> datasetNames = GlobalVariables.loadedDatasets.Select(d => d.datasetName).ToList();
                    comboBoxForSelectingDataset.DataSource = datasetNames;
                    break;

                default:
                    CloseExperimentSettingPane();
                    break;
            
            } //End switch case of selectIndexTab
          
        }// End Event Handler

        /// <summary>
        /// Check if another experiment is running
        /// </summary>
        /// <param name="currentExperimentType"></param>
        private void CheckIfAnotherExperimentIsRunning(ExperimentType currentExperimentType)
        {
            //pop up a confirm dialog whether to terminate another running experiment
            if (isExperimentRunning && currentExperimentSetting.experimentType != currentExperimentType)
            {

                tabControlForMainPage.SelectedIndex = currentRunningExperimentItemIndex;
            }           
                        
        }

        /// <summary>
        /// Open the experiment setting pane with the required experiment Type (ie ActiveLearning, BatchRunning)
        /// </summary>
        /// <param name="currentExperimentType"></param>
        private void OpenExperimentSettingPane(ExperimentType currentExperimentType)
        {
            //If the experiment is not started or is stopped
            if (!isExperimentRunning && isExperimentComplete)
            {
                //Close current experiment settingPane
                CloseExperimentSettingPane();
                experimentSettingForm = new FormExperimentSetting(currentExperimentType);

                //enable the FormActiveLearningSetting window always on the top
                experimentSettingForm.Owner = this;
                experimentSettingForm.Show();
            }
        }

        /// <summary>
        /// Close the Experiment Setting Pane if it is opened
        /// </summary>
        private void CloseExperimentSettingPane() 
        {
            //Check if the ExperimentSettingPane is opened
            if (experimentSettingForm != null)
            {
                experimentSettingForm.Close();
                experimentSettingForm = null;
            }

        }

        /// <summary>
        /// Display the accuracy graph in a new window form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActiveLearning_PopUpAccuracyGraph_Click(object sender, MouseEventArgs e)
        {
            //Close opened grpahWindow if it exists
            if (accuracyGraphForActiveLearning != null)
            {
                //remove from the openedFormist
                openedForms.Remove(accuracyGraphForActiveLearning);

                //close the window 
                accuracyGraphForActiveLearning.Close();
            }

            accuracyGraphForActiveLearning = new FormGraph();

            //add the form into the openedFormList
            openedForms.Add(accuracyGraphForActiveLearning);

            //display the form
            accuracyGraphForActiveLearning.Show();
        }

        /// <summary>
        /// Initial Load DataGridViewActiveLearningWorker
        /// </summary>
        private void LoadActiveLearningWorkerDataGridView() 
        {
            //Set the number of column 
            dataGridViewActiveLearningSchedule.ColumnCount = 2;
        
            //Allow full row selection
            dataGridViewActiveLearningSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //supports multiple rows selection, such that multiple popup 
            dataGridViewActiveLearningSchedule.MultiSelect = true;
        }

        /// <summary>
        /// Populate Dummy data to dataGridViewActiveLearningWorkers1 for testing
        /// </summary>
        private void PopulatedataGridViewActiveLearningWorkers1()
        {
            dataGridViewActiveLearningSchedule.Columns[0].DisplayIndex = 0;
            dataGridViewActiveLearningSchedule.Columns[1].DisplayIndex = 1;
            dataGridViewActiveLearningSchedule.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewActiveLearningSchedule.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        
        /// <summary>
        /// Pause and start the active learning experiment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlayAndPauseExperiment_Click(object sender, EventArgs e)
        {

            HandlePlayAndPauseExperiment(ExperimentType.ActiveLearning, this.btnPlayAndPauseExperiment);

        } //end of button handler

        /// <summary>
        /// General Event Handler of playing and pause the experiment
        /// </summary>
        /// <param name="currentExpType"></param>
        private void HandlePlayAndPauseExperiment(ExperimentType currentExpType, Button playAndPauseButton)
        {

            //Change the PlayAndPauseButton according to current ExperimentType,
            //pop up the setting form if the experiment is complete
            if (isExperimentComplete)
            {
                //initial TheExperiment and pop up the 
                experimentSettingForm = new FormExperimentSetting(currentExpType);
                experimentSettingForm.setPreviousExperimentSetting(currentExperimentSetting);
                //enable the FormActiveLearningSetting window always on the top
                experimentSettingForm.Owner = this;
                experimentSettingForm.Show();

                //change to the pause icon
                playAndPauseButton.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Pause_Hot_icon;

                return;
            }

            isExperimentRunning = !isExperimentRunning;

            //Pause the experiment
            if (isExperimentRunning)
            {
                //change to the pause icon
                playAndPauseButton.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Pause_Hot_icon;
         
            }
            else
            {
                //change to the play icon
                playAndPauseButton.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Play_1_Normal_icon;

            }
        
        }

        /// <summary>
        /// Initliase the Active Learning graph
        /// </summary>
        private void InitialiseActiveLearningGraphs()
        {
            //AccuracyGraph Settings
            accuracyGraphPane = graphControlAccuracyGraph.GraphPane;
            accuracyGraphPane.CurveList.Clear() ; //clear the curveList in the graphpane

            //set x starting point of the AccuracyGraph
            accuracyGraphPane.XAxis.Scale.Min = currentExperimentSetting.initialStartingLabel - 1;

            //add curves into the accuracyGraphPane
            List<ExperimentModel> currentExperimentItemsList = currentExperimentSetting.experimentModels;

            for (int i = 0; i < currentExperimentItemsList.Count; i++)
            {
                // The RollingPointPairList is an efficient storage class that always
                // keeps a rolling set of point data without needing to shift any data values
                RollingPointPairList list = new RollingPointPairList(currentExperimentSetting.currentDataset.totalNumberOfLabellingRows);

                //Add the curve with an empty datalist into the accuracyGraphPane
                LineItem curve = accuracyGraphPane.AddCurve(currentExperimentItemsList[i].ToString(), list, colourOptions[i], SymbolType.None);
                curve.Line.Width = 2.0f;
            }

            //scale the axes
            accuracyGraphPane.AxisChange();
            //set the CurveList
            accuracyGraphCurveList = accuracyGraphPane.CurveList;
            graphControlAccuracyGraph.Visible = true;
           
        }


        /// <summary>
        /// Handle stop experiment event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopExperiment_Click(object sender, EventArgs e)
        {
            HandleStopButton(ExperimentType.ActiveLearning, btnPlayAndPauseExperiment, progressBarForActiveLearning);
         
        }

        /// <summary>
        /// Handle Stop Button Event
        /// </summary>
        /// <param name="currentExperimentType"></param>
        /// <param name="playAndPauseButton"></param>
        /// <param name="currentProgressBar"></param>
        private void HandleStopButton(ExperimentType currentExperimentType, Button playAndPauseButton, KryptonProgressBar currentProgressBar )
        {
            DialogResult messageBoxResult = MessageBox.Show("Are you sure? The simulation will be stopped.", "Stop Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Confirm before stopping the experiment
            if (messageBoxResult == DialogResult.Yes)
            {
                isExperimentComplete = true;
                isExperimentRunning = false;

                // Cancel the asynchronous operation. 
                //Stop backgroundWorkers if it is the ActiveLearning Experiment 
                if (currentExperimentType == ExperimentType.ActiveLearning)
                {
                    this.backgroundWorkerForSimulation.CancelAsync();
                    this.backgroundWorkerForAccuracyGraph.CancelAsync();
                    ActiveLearning.isExperimentCompleted = true;

                }// when it is the BatchRunning Experiment
                else if(currentExperimentType == ExperimentType.BatchRunning)
                {
                    this.backgroundWorkerBatchRunning.CancelAsync();
                }

                //change the progress bar status
                currentProgressBar.DisplayText = "Simulation Stopped";

                //change to the play icon
                playAndPauseButton.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Play_1_Normal_icon;

            } //End if confirm to stop the experiment
        } //End HandleStopButton

  
        /// <summary>
        /// Update Graph and progress pane in the background thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerForAccuracyGraph_DoWork(object sender, DoWorkEventArgs e)
        {
            Debug.Print("AccuracyGraphStarted");
            //Get the Backgroundworker
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;
            progressBarForActiveLearning.DisplayText = "Running Exploration...";

            //Initial the CurrentParallelState object
            ExperimentSetting.CurrentParallelState state = new ExperimentSetting.CurrentParallelState();
            int totalNumberOfLabellingRows = currentExperimentSetting.GetTotalNumberOfLabellingRows();
            int totalNumberOfExperimentItems = currentExperimentSetting.GetNumberOfExperiemntModels();

            //wait until the graphs finish loading 
            while (!isGraphLoaded)
            { 
                
            }

            //wait until the activeLearning thread is running
            while (currentExperimentSetting.accuracyArrayOfAllExperimentModels == null)
            { 
            
            } //end while

            while (ActiveLearning.results == null)
            {
            }

            //Set resultsLists
            currentExperimentSetting.results = ActiveLearning.results.ToList();
            Debug.Print("Simulation started");

             //For each labelling round
            for (int currentLabellingRound = currentExperimentSetting.initialStartingLabel; currentLabellingRound <= totalNumberOfLabellingRows; currentLabellingRound++)
            {
                Debug.Print("Current Labelling Round = " + currentLabellingRound);


                //stops if the experiment is not running
                while (!isExperimentRunning && !isExperimentComplete)
                {
                }

             
                int requiredLabelIndex = currentLabellingRound - currentExperimentSetting.initialStartingLabel;

                while (requiredLabelIndex >= currentExperimentSetting.accuracyArrayOfAllExperimentModels[0].Count && !isExperimentComplete)
                {
                    //do nothing and wait 
                    //

                }

                if (isExperimentComplete)
                {
                    return;
                }


                //Update the labelling round text in the progress pane
                progressBarForActiveLearning.Value = currentLabellingRound;
                progressBarForActiveLearning.DisplayText = "Running: " + (currentLabellingRound - currentExperimentSetting.initialStartingLabel + 1) + "/" + (totalNumberOfLabellingRows - currentExperimentSetting.initialStartingLabel + 1);
          
                //For each experimentItem
                for (int curveItemIndex = 0; curveItemIndex < totalNumberOfExperimentItems; curveItemIndex++)
                {
                    //stops if the experiment is not running
                    while (!isExperimentRunning && !isExperimentComplete)
                    {
                    }

                    //teminate the program if the user requests to stop
                    if (isExperimentComplete)
                    {
                        return;
                    }

                    //if the labelstarting point is greater or equals to current labelstarting point
                    if (currentExperimentSetting.isExperimentItemStarted(curveItemIndex, currentLabellingRound))
                    {
                        LineItem currentCurve = graphControlAccuracyGraph.GraphPane.CurveList[curveItemIndex] as LineItem;
                        IPointListEdit curveList = currentCurve.Points as IPointListEdit;

                        //update the curve 
                        curveList.Add(currentLabellingRound, currentExperimentSetting.getAccuracy(curveItemIndex,currentLabellingRound));
                        Debug.WriteLine("AddCurve: ExperimentIndex=" + curveItemIndex + "----" + currentLabellingRound);
                        
                        //updat the workerList
                        state.currentActiveLearningResult = currentExperimentSetting.GetActiveLearningResult(curveItemIndex, currentLabellingRound);
                        state.currentExperimentModel = currentExperimentSetting.GetExperimentModel(curveItemIndex);
                        currentRunningExperimentItemIndex = curveItemIndex;

                        //only notify the UI thread when the current datapane is the current curve
                        worker.ReportProgress(0, state);
                        
                     }//end if the labelstarting point is greater

                } //end for each experiment item


                //invalidate the graphpane 
                graphControlAccuracyGraph.AxisChange();
                //redraw the graph
                graphControlAccuracyGraph.Invalidate();

            } //end for each labelling round

            //Display the simulation is complete
            progressBarForActiveLearning.DisplayText = "Simulation completed";

            //change to the play icon
            this.btnPlayAndPauseExperiment.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Play_1_Normal_icon;

        }


        /// <summary>
        /// Run on main thread, update the grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerForAccuracyGraph_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ExperimentSetting.CurrentParallelState state = (ExperimentSetting.CurrentParallelState)e.UserState;

            if (state == null || state.currentActiveLearningResult == null) 
            {
                return;
            }

            ExperimentModel currentExprItem = state.currentExperimentModel;
            //add the accuracy result into the ExperimentModel
            currentExprItem.resultsBindingList.Add(state.currentActiveLearningResult);

           
            //only update the workerValuePane if it is the current running experimentItem
            if (currentRunningExperimentItemIndex == comboBoxShowCurrentModels.SelectedIndex)
            { 
     
                SetWorkerValues(currentExprItem);
   
                SetTaskValues(currentExprItem);

                
                while (state.currentActiveLearningResult == null)
                 { 
                 } 

                if (!currentExperimentSetting.ContainWorkerId(currentRunningExperimentItemIndex, state.currentActiveLearningResult.WorkerId))
                {

                }


                //dataGridViewForTaskValue
                if (!currentExperimentSetting.ContainTaskId(currentRunningExperimentItemIndex, state.currentActiveLearningResult.TaskId))
                {
                    //dataGridViewForTaskValue.Rows.Add(state.currentActiveLearningResult.TaskId);
                   // dataGridViewForTaskValue.Refresh();
                }


                if (currentExprItem.runType == RunType.CBCC && dataGridViewInnerCommunity.Rows.Count == 0)
                {
                     
                    for (int i = 0; i < currentExperimentSetting.communityCount; i++)
                    {
                        dataGridViewInnerCommunity.Rows.Add("Community " + (i));
                    }

                }

            }
          

        } //End backgroundWorkerForAccuracyGraph

        /// <summary>
        /// Load selected model in the Active Learning Experiment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxShowWorkersValues_SelectedIndexChanged(object sender, EventArgs e)
        {   
            
            ExperimentModel currentExperimentModel = currentExperimentSetting.GetExperimentModel(comboBoxShowCurrentModels.SelectedIndex);
            
            //Binding Schedule list into Schedule
            BindingSource bsForSchedules = new BindingSource();
            bsForSchedules.DataSource = currentExperimentModel.resultsBindingList;

            dataGridViewActiveLearningSchedule.DataSource = bsForSchedules;
            dataGridViewActiveLearningSchedule.Columns[3].Visible = false;
            dataGridViewActiveLearningSchedule.Columns[4].Visible = false;
            dataGridViewActiveLearningSchedule.Columns[2].HeaderText = "Task Value";

            // make sure to do it before binding DataGridView control
            dataGridViewActiveLearningSchedule.AutoGenerateColumns = false;

            //set the header to bold
            SetDataGridViewHeaderToBold(dataGridViewActiveLearningSchedule);

            //set the worker datagridview
            SetWorkerValues(currentExperimentModel);
            SetTaskValues(currentExperimentModel);

            tabControlForValues.TabPages.Remove(tabPageInnerViewCommunity);

            //Show community tabpage if it is the CBCC model
            if (currentExperimentModel.runType == RunType.CBCC)
            {
                tabControlForValues.TabPages.Add(tabPageInnerViewCommunity);
               
            } 
           
        }//end function

        /// <summary>
        /// Set worker values
        /// </summary>
        /// <param name="currentExperimentModel"></param>
        private void SetWorkerValues(ExperimentModel currentExperimentItem)
        {
            //Binging list into Worker
            BindingSource bsForWorker = new BindingSource();
            List<System.Linq.IGrouping<string, CrowdsourcingModels.ActiveLearningResult>> customQuery = (from alr in currentExperimentItem.resultsBindingList
                               group alr by alr.WorkerId into workerIdgroup
                               select workerIdgroup).ToList();


            int scrollPosition = dataGridViewForInnerWorker.FirstDisplayedScrollingRowIndex;
            int rowIndex = -1;

            if (dataGridViewForInnerWorker.SelectedRows.Count > 0) 
            {
                rowIndex = dataGridViewForInnerWorker.SelectedRows[0].Index;    
            }


            dataGridViewForInnerWorker.DataSource = customQuery;

            if (scrollPosition != -1) 
            {
                if (dataGridViewForInnerWorker.FirstDisplayedScrollingRowIndex != -1) 
                {
                    dataGridViewForInnerWorker.FirstDisplayedScrollingRowIndex = scrollPosition;
                }

                if (rowIndex != -1 && rowIndex < dataGridViewForInnerWorker.Rows.Count)
                {

                    dataGridViewForInnerWorker.CurrentCell = dataGridViewForInnerWorker.Rows[rowIndex].Cells[0];
                    dataGridViewForInnerWorker.Rows[rowIndex].Selected = true;

                }

             }

            dataGridViewForInnerWorker.Columns[0].HeaderText = "Worker Id";
            SetDataGridViewHeaderToBold(dataGridViewForInnerWorker);

            dataGridViewForInnerWorker.AutoGenerateColumns = false;

        }

        /// <summary>
        /// [Active Learning] Set Task Values of the selected experiment model
        /// </summary>
        /// <param name="currentExperimentModel"></param>
        /// <param name="changeBindingSource"></param>
        private void SetTaskValues(ExperimentModel currentExperimentModel)
        {
   
            //Binding list into Worker
            BindingSource bsForWorker = new BindingSource();
            List<System.Linq.IGrouping<string, CrowdsourcingModels.ActiveLearningResult>> customQuery = (from alr in currentExperimentModel.resultsBindingList
                                                                                                        group alr by alr.TaskId into TaskIdgroup
                                                                                                        select TaskIdgroup).ToList();


            BindingList<List<System.Linq.IGrouping<string, CrowdsourcingModels.ActiveLearningResult>>> workerValuesList = new BindingList<List<System.Linq.IGrouping<string, CrowdsourcingModels.ActiveLearningResult>>>();
            workerValuesList.Add(customQuery);
            bsForWorker.DataSource = workerValuesList;

     
            int scrollPosition = dataGridViewForTaskValue.FirstDisplayedScrollingRowIndex;
            int rowIndex = -1;

            if (dataGridViewForTaskValue.SelectedRows.Count > 0)
            {
                rowIndex = dataGridViewForTaskValue.SelectedRows[0].Index;
            }


            bsForWorker.DataSource = customQuery;
            dataGridViewForTaskValue.DataSource = bsForWorker;

            if (scrollPosition != -1)
            {
                if (dataGridViewForTaskValue.FirstDisplayedScrollingRowIndex != -1)
                {
                    dataGridViewForTaskValue.FirstDisplayedScrollingRowIndex = scrollPosition;
                }

                if (rowIndex != -1 && rowIndex < dataGridViewForTaskValue.Rows.Count)
                {

                    dataGridViewForTaskValue.CurrentCell = dataGridViewForTaskValue.Rows[rowIndex].Cells[0];
                    dataGridViewForTaskValue.Rows[rowIndex].Selected = true;

                }

            }

            dataGridViewForTaskValue.Columns[0].HeaderText = "Task Id";
            SetDataGridViewHeaderToBold(dataGridViewForTaskValue);
            Debug.WriteLine("Inner Worker Court" + dataGridViewForTaskValue.ColumnCount + "");

            dataGridViewForTaskValue.AutoGenerateColumns = false;

        }

       
        /// <summary>
        /// Get current progress text of the ProgressBar
        /// </summary>
        /// <returns></returns>
        public string GetCurrentProgressText()
        {
            return progressBarForActiveLearning.DisplayText;
        }

 
        /// <summary>
        /// Pop up a worker form when a user double clicks the row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewActiveLearningWorkers1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //At least one row is selected
            if (dataGridViewInnerCommunity.SelectedRows.Count >= 1)
            {
                //for each worker row selected
                for (int i = 0; i < dataGridViewInnerCommunity.SelectedRows.Count; i++)
                {
                    //open a new form showing the worker's detail
                    int currentSelectedRowIndex = dataGridViewInnerCommunity.SelectedRows[i].Index;

                    ExperimentModel relatedExperimentItem = currentExperimentSetting.GetExperimentModel(comboBoxShowCurrentModels.SelectedIndex);
                    FormWorkerOrCommunityDetail communityDetail = new FormWorkerOrCommunityDetail(currentSelectedRowIndex, relatedExperimentItem);
                    openedForms.Add(communityDetail);
                    communityDetail.Show();

                } //End for each worker row selected
            }

        }
  
        /// <summary>
        /// Print row index of the dataGridView 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewActiveLearningWorkers1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            int intitialStartPoint = currentExperimentSetting.GetExperimentModel(comboBoxShowCurrentModels.SelectedIndex).labelStartingPoint;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        /// <summary>
        /// Show Tooltip message when mouse hovers the control
        /// </summary>
        /// <param name="tooltipMsg"></param>
        /// <param name="currentControl"></param>
        private void ShowTooltip(String tooltipMsg, Control currentControl)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(currentControl, tooltipMsg);
        }


        private void buttonToDatasetTabpage_Click_1(object sender, EventArgs e)
        {
            LoadDataset();
        }

        /// <summary>
        /// Input Dataset in the View Dataset Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadDataset_Click(object sender, EventArgs e)
        {
            LoadDataset();
            //display the latest dataset
            Dataset selectedDataset = GlobalVariables.loadedDatasets[comboBoxForSelectingDataset.Items.Count - 1];
            loadDatasetToDataGridView(selectedDataset.datasetPath);
            //update the combolbox 
            List<String> datasetNames = GlobalVariables.loadedDatasets.Select(d => d.datasetName).ToList();
            comboBoxForSelectingDataset.DataSource = datasetNames;
            comboBoxForSelectingDataset.SelectedIndex = datasetNames.Count - 1;
        }

        /// <summary>
        /// Event Handler when the dataset comboBox chnages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxForSelectingDataset_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dataset selectedDataset = GlobalVariables.loadedDatasets[comboBoxForSelectingDataset.SelectedIndex];
            loadDatasetToDataGridView(selectedDataset.datasetPath);
        }

        private void dataGridViewOfDataset_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);

        }

        /// <summary>
        /// View the selected Dataset
        /// </summary>
        /// <param name="selectedDatasetIndex">Index of the Dataset in the combo box</param>
        public void ViewDataset(int selectedDatasetIndex)
        {
            tabControlForMainPage.SelectedIndex = 3;
            //Display that Dataset
            Dataset selectedDataset = GlobalVariables.loadedDatasets[selectedDatasetIndex];
            comboBoxForSelectingDataset.SelectedIndex = selectedDatasetIndex;
            //change the combox as well
            loadDatasetToDataGridView(selectedDataset.datasetPath);
        }

        /// <summary>
        /// Event handler for restarting active learning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRestartActiveLearning_Click(object sender, EventArgs e)
        {
            HandleRestartExperiment(ExperimentType.ActiveLearning, btnPlayAndPauseExperiment, progressBarForActiveLearning);
        }

       
        /// <summary>
        /// Restart the experiment
        /// </summary>
        /// <param name="currentExperimentType"></param>
        /// <param name="playAndPauseButton"></param>
        /// <param name="currentProgressBar"></param>
        private void HandleRestartExperiment(ExperimentType currentExperimentType, Button playAndPauseButton, KryptonProgressBar currentProgressBar )
        {
            DialogResult messageBoxResult = MessageBox.Show("Are you sure to restart the simulation?", "Restart Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Confirm before stopping the experiment
            if (messageBoxResult == DialogResult.Yes)
            {
                isExperimentComplete = true;
                isExperimentRunning = false;

                // Cancel the asynchronous operation. 
                //Stop backgroundWorkers
                if (currentExperimentType == ExperimentType.ActiveLearning)
                {
                    this.backgroundWorkerForSimulation.CancelAsync();
                    this.backgroundWorkerForAccuracyGraph.CancelAsync();
                    ActiveLearning.isExperimentCompleted = true;
                }
                else if (currentExperimentType == ExperimentType.BatchRunning)
                {
                    this.backgroundWorkerBatchRunning.CancelAsync();
                }

                //Show Stopped message on the progress bar
                currentProgressBar.DisplayText = "Simulation Stopped";

                //initialTheExperiment and pop up the 
                experimentSettingForm = new FormExperimentSetting(currentExperimentType);
                experimentSettingForm.setPreviousExperimentSetting(currentExperimentSetting);
                //enable the FormActiveLearningSetting window always on the top
                experimentSettingForm.Owner = this;
                experimentSettingForm.Show();
                resetDataGridView();
                return;
            }
        
        }

        /// <summary>
        /// [Active Learning] Show tool tip when mouse hover the Enlarge Accuracy Graph Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActiveLearning_PopUpAccuracyGraph_MouseHover(object sender, EventArgs e)
        {
            ShowTooltip("Show Larger Graph", this.btnActiveLearning_PopUpAccuracyGraph);
        }

        /// <summary>
        /// [Active Learning] Show tool tip when mouse hover the "Close All Popups" Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActiveLearningCloseAllOpenedWindow_MouseHover(object sender, EventArgs e)
        {
            ShowTooltip("Close All Popups", this.buttonActiveLearningCloseAllOpenedWindow);
   
        }

        /// <summary>
        /// Display tooltip for buttonOpenAllPopups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenAllPopUps_MouseHover(object sender, EventArgs e)
        {
            ShowTooltip("Open All Popups", this.buttonOpenAllPopups);
        }

        /// <summary> 
        /// Display Worker Detail when mouse double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewForInnerWorker_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            DisplayPopUp(comboBoxShowCurrentModels, dataGridViewForInnerWorker, "worker");
     
        }

        /// <summary>
        /// Display Popup of the corresponding popup
        /// </summary>
        /// <param name="currentComboBox"></param>
        /// <param name="currentDataGridView"></param>
        /// <param name="currentType"></param>
        private void DisplayPopUp(ComboBox currentComboBox, DataGridView currentDataGridView, string currentType)
        {
            //At least one row is selected
            if (currentDataGridView.SelectedRows.Count >= 1)
            {
                ExperimentModel relatedExperimentItem = currentExperimentSetting.GetExperimentModel(currentComboBox.SelectedIndex);
          
                //for each worker row selected
                for (int i = 0; i < currentDataGridView.SelectedRows.Count; i++)
                {
                    //open a new form showing the worker's detail

                    int currentSelectedRowIndex = currentDataGridView.SelectedRows[i].Index;
                    KryptonForm currentForm = null;
                    String currentId = currentDataGridView.Rows[currentSelectedRowIndex].Cells[0].Value.ToString();
                       
                    if (currentType == "worker")
                    {
                        Worker tempWorker = new Worker();
                        tempWorker.WorkerId = currentId;
                        currentForm = new FormWorkerOrCommunityDetail(tempWorker, relatedExperimentItem);
                    } 
                    //Open task form
                    else if (currentType == "task")
                    {
                        currentForm = new FormTaskDetails(currentId, relatedExperimentItem);
                    }
                    //Open the Community Form
                    else 
                    {
                        currentForm = new FormWorkerOrCommunityDetail(currentSelectedRowIndex, relatedExperimentItem);
                    }
  
                    //Add this openedForm into the openForms List
                    openedForms.Add(currentForm);
                    currentForm.Show();

                } //End For selected dataGridViewForInnerWorker
            }// if there is more than one selected rows 
        
        
        } //End DisplayPopUp
        
        /// <summary>
        /// Remove the closed popup from the opendForms list
        /// </summary>
        /// <param name="closedForm"></param>
        public void RemoveClosedPopUp(KryptonForm closedForm)
        {
            openedForms.Remove(closedForm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewInnerCommunity_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //At least one row is selected
            if (dataGridViewInnerCommunity.SelectedRows.Count >= 1)
            {
                //for each worker row selected
                for (int i = 0; i < dataGridViewInnerCommunity.SelectedRows.Count; i++)
                {
                    //open a new form showing the community's detail
                    int currentSelectedRowIndex = dataGridViewInnerCommunity.SelectedRows[i].Index;
                
                    ExperimentModel relatedExperimentItem = currentExperimentSetting.GetExperimentModel(comboBoxShowCurrentModels.SelectedIndex);
                    FormWorkerOrCommunityDetail form1 = new FormWorkerOrCommunityDetail(currentSelectedRowIndex, relatedExperimentItem);
                    openedForms.Add(form1);
                    form1.Show();
                }
            } 
        }

        private void tabControlForValues_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DisplayPopUp(comboBoxShowCurrentModels, dataGridViewForTaskValue, "task");

        }

        private void CloseAllOpenedWindows()
        { 
            DialogResult messageBoxResult = MessageBox.Show("Are you sure to close all pop ups?", "Restart Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Confirm before closing all windows
            if (messageBoxResult == DialogResult.Yes)
            {
                //Run Through the whole openedFormList, close all of them
                openedForms.ToList().ForEach(e => e.Close());

                //Create an empty openedForms List
                openedForms = new List<KryptonForm>();

            } //End Clsoing all windows 
        }

        private void backgroundWorkerBatchRunning_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs on the background thread. 

            // Get the BackgroundWorker object that raised this event.
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;

            //Start working the runBatchRunningExperiment in the ExperimentSetting
            currentExperimentSetting.RunBatchRunningExperiment(worker, e);
            
        }
        /// <summary>
        /// Handle Progress Change of the BatchRunning worker thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerBatchRunning_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //obtain the current state
            ExperimentSetting.CurrentParallelState state = (ExperimentSetting.CurrentParallelState)e.UserState;
            int currentExperimentModelIndex = state.currentExperimentModelIndex;

            //update the progress bar & the workbreakdown table
            if(state.isCurrentModelCompleted)
            {
                //only update if the currentExperimentModel Index equals to the combo box one
                if (comboBoxBatchRunningModel.SelectedIndex == currentExperimentModelIndex)
                { 
                    //update the values in the right pane
                    UpdateModelValuesForBatchRunning(currentExperimentModelIndex);
                }
                    //update the workbreakdown table
                dataGridViewBatchRunningProgress.Rows[currentExperimentModelIndex].Cells[1].Value = "Completed";
                dataGridViewBatchRunningAccuracy.Rows[currentExperimentModelIndex].Cells[1].Value = String.Format("{0:0.0000}", currentExperimentSetting.results[currentExperimentModelIndex].Accuracy);
                    
                //update the progress bar
                progressBatchRunning.Value = (int)(((currentExperimentModelIndex + 1.0) / currentExperimentSetting.GetNumberOfExperiemntModels()) * 100.0);

                //if the progress is completed, show is completed
                if(state.isRunningComplete)
                {
                    progressBatchRunning.DisplayText = "Running Complete";
                    isExperimentComplete = true;
                    isExperimentRunning = false;
                    //change to the pause icon
                    this.buttonPlayAndPauseBatchRunning.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Play_1_Normal_icon;
                  
                }

            }else //showing the current experiment Model
            {
                int progressPercentage = (int)(((currentExperimentModelIndex + 0.0) / currentExperimentSetting.GetNumberOfExperiemntModels()) * 100.0);

                progressBatchRunning.DisplayText = "Running " + currentExperimentSetting.GetExperimentModel(state.currentExperimentModelIndex).GetRunTypeString() + " (" + progressPercentage + "%)";
                 //update the workbreakdown table
                dataGridViewBatchRunningProgress.Rows[state.currentExperimentModelIndex].Cells[1].Value = "Running";
            }
            
            
        }

        private void buttonPlayAndPauseBatchRunning_Click(object sender, EventArgs e)
        {
            HandlePlayAndPauseExperiment(ExperimentType.BatchRunning, this.buttonPlayAndPauseBatchRunning);
        }


        /// <summary>
        /// Update the grid view according to the selected index in the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxBatchRunningModel_SelectedIndexChanged(object sender, EventArgs e)
        {

            dataGridViewBatchRunningWorkers.DataSource = null;
            dataGridViewBatchRunningTasks.DataSource = null;
            dataGridViewBatchRunningCommunities.DataSource = null;
            UpdateModelValuesForBatchRunning(comboBoxBatchRunningModel.SelectedIndex);
        }

        private void dataGridViewBatchRunningWorkers_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // Optional: check for column index if you got more columns
            e.Value = dataGridViewBatchRunningWorkers.Rows[e.RowIndex].DataBoundItem.ToString();
        }

        /// <summary>
        /// [Batch Running] Display the worker detail when double click the worker ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewBatchRunningWorkers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DisplayPopUp(comboBoxBatchRunningModel, dataGridViewBatchRunningWorkers, "worker");
        }

        private void dataGridViewBatchRunningTasks_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            DisplayPopUp(comboBoxBatchRunningModel, dataGridViewBatchRunningTasks, "task");
            
        }

        private void dataGridViewBatchRunningCommunities_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            DisplayPopUp(comboBoxBatchRunningModel, dataGridViewBatchRunningCommunities, "community");
        }

        private void buttonActiveLearningCloseAllOpenedWindow_Click(object sender, EventArgs e)
        {
            CloseAllOpenedWindows();
        }
      
        /// <summary>
        /// Event Handler of stopping batch running epxeriment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStopBatchRunning_Click(object sender, EventArgs e)
        {
            HandleStopButton(ExperimentType.BatchRunning, buttonPlayAndPauseBatchRunning, progressBatchRunning);
    
        }

        /// <summary>
        /// Event Handler of restarting the Batch Running Experiment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRestartBatchRunning_Click(object sender, EventArgs e)
        {
            HandleRestartExperiment(ExperimentType.BatchRunning, buttonPlayAndPauseBatchRunning, progressBatchRunning);
        
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
     
    }//end partial class

} //End namespace
