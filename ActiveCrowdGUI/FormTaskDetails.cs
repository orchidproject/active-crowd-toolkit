using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MicrosoftResearch.Infer.Distributions;
using CrowdsourcingModels;
using System.Diagnostics;
namespace AcriveCrowdGUI
{
    /// <summary>
    /// Worker Detail Form: showing the details of worker, as well as the confusion matrix
    /// </summary>
    public partial class FormTaskDetails : KryptonForm
    {
        public Worker currentWorker;
        public ExperimentModel relatedExperimentItem;

        string probabilityText;
        int indexOfExperimentItem;
        Discrete probabilitiesArray;
        string taskId;
        string labelHeader;

        /// <summary>
        /// Initialise the form with the Worker Object
        /// </summary>
        /// <param name="currentWorker"></param>
        public FormTaskDetails(string taskId, ExperimentModel relatedExperimentItem)
        {
            InitializeComponent();

            this.taskId = taskId;
            //Display the workerId in the form title
            this.Text = "[Task]" + taskId;
            this.relatedExperimentItem = relatedExperimentItem;
            this.labelForTaskId.Text = taskId;
            this.labelModelDetail.Text = relatedExperimentItem.ToString();

            //load data
            labelConfusionMatrix.Text = probabilityText;
            indexOfExperimentItem = MainPage.mainPageForm.currentExperimentSetting.GetExperimenModelIndex(relatedExperimentItem);
            probabilitiesArray = MainPage.mainPageForm.currentExperimentSetting.GetTaskTrueLabel(indexOfExperimentItem, taskId);
            labelHeader = "";
            int labelCount = 0;

            //Initialise the probabilitiesArray
            if (probabilitiesArray != null)
            {
                labelCount = probabilitiesArray.Dimension;
                Enumerable.Range(1, labelCount).ToList().ForEach(i => labelHeader += "Label" + i + "       ");
                // labelForDataHeader.Text = labelHeader;
                SetUpChart();

                //Only sync the background thread if it is activeLearning
                if (MainPage.mainPageForm.currentExperimentSetting.experimentType == ExperimentType.ActiveLearning)
                {
                    backgroundTaskValues.RunWorkerAsync();
                }
            }// End if the probabilitiesArray is not null
        } // End Constructor


        /// <summary>
        /// Paint the label Posterior Probability graph
        /// </summary>
        /// <param name="printableConfusionMatrix"></param>
        private void PaintLabelPosteriorProbabilityGraph(Discrete probabilityMatrix , bool isFirstTimeToAdd = false)
        {
            int labelCount = probabilityMatrix.Dimension;
            Series currentSeries; 

            //initial the graph at the first time
            if (isFirstTimeToAdd)
            {
                currentSeries = new Series();
                currentSeries.ChartArea = "Default";
                currentSeries.ChartType = SeriesChartType.Column;
                currentSeries.IsVisibleInLegend = false;
                currentSeries.IsXValueIndexed = true;

                this.workerConfusionMatrixGraph.ChartAreas[0].AxisX.Interval = 1;
                this.workerConfusionMatrixGraph.Series.Add(currentSeries);
            }
            else //obtain the current series
            {
                currentSeries = workerConfusionMatrixGraph.Series[0];
            }

            probabilityText = "";

            //for each label, add the corresponding datapoint
            for (int currentLabelPos = 0; currentLabelPos < labelCount; currentLabelPos++)
            {
               double pointValue = probabilitiesArray[currentLabelPos];
               DataPoint currentDataPoint = new DataPoint(currentLabelPos, pointValue);
               currentDataPoint.AxisLabel = "Label " + (currentLabelPos + 1) ;
               currentSeries.Points.Add(currentDataPoint);
               probabilityText += String.Format("{0:0.0000}       ", pointValue);

            }//end for goldLabel

            textBoxTaskValue.Text = labelHeader + Environment.NewLine + probabilityText;
            
        }

        //Initial the WorkerConfusionMatrix
        private void SetUpChart() 
        {
            PaintLabelPosteriorProbabilityGraph(probabilitiesArray, true);
        }

        /// <summary>
        /// Load the worker details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormWorkerDetail_Load(object sender, EventArgs e)
        {
        }

        //Remove this instance in the list of workDetailForms in the MainPage while closing 
        private void FormWorkerDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MainPage.mainPageForm != null)
            {
                MainPage.mainPageForm.RemoveClosedPopUp(this);
            }
 
        }

 
        /// <summary> 
        /// Background Thread for updating the worker confusion matrix
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerUpdateConfusionMatrix_DoWork(object sender, DoWorkEventArgs e)
        {
            //Get the Backgroundworker
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;

            //listen the main page while the experiment is running
            while (!MainPage.mainPageForm.isExperimentComplete)
            {
                while (MainPage.mainPageForm.isExperimentRunning)
                {
                    probabilitiesArray = MainPage.mainPageForm.currentExperimentSetting.GetTaskTrueLabel(indexOfExperimentItem, taskId);

                    try
                    {
                        //notify the graph to change
                        worker.ReportProgress(0, null);

                    }
                    catch (Exception)
                    {
                       break;

                    }
                    //Check update after a period of time 
                    System.Threading.Thread.Sleep(500);

                } //End while Experiment is running

            } //End While Experiment is completed
         
        } //End BackgroundUpdatingThread Method


        /// <summary>
        /// Update the workerConfusionMatrix
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerUpdateConfusionMatrix_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //clear the confusion matrix if it exists
           if (workerConfusionMatrixGraph != null)
           {
                try
                {
                    //clear the graph
                    foreach (var series in this.workerConfusionMatrixGraph.Series)
                    {
                        series.Points.Clear();
                    }
                    //Repaint the Graph
                    PaintLabelPosteriorProbabilityGraph(probabilitiesArray);
                }
                catch (NullReferenceException)
                {
                }

            } // End if the workerConfustionMatrixGraph is not empty
          
        } //End BackgroundWorkerUpdate Thread

    } //End Class
} //End namespace
