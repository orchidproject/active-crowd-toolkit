﻿using ComponentFactory.Krypton.Toolkit;
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
using CrowdsourcingModels;
using System.Diagnostics;


namespace AcriveCrowdGUI
{
    /// <summary>
    /// Worker Detail Form: showing the details of worker, as well as the confusion matrix
    /// </summary>
    public partial class FormWorkerOrCommunityDetail : KryptonForm
    {
        private Worker currentWorker;
        private ExperimentModel relatedExperimentItem;
        private double[,] printableConfusionMatrix;
        String matriceText;
        int indexOfExperimentItem;
        bool isWorker = true; 

        int communityIndex;

        public void LoadConfusionMatrix()
        {

            if (printableConfusionMatrix != null)
            {
                paintConfusionMatriceGraph(printableConfusionMatrix, true);

            }

            setUpChart();
        }


        /// <summary>
        /// Initialise the form with the Worker Object
        /// </summary>
        /// <param name="currentWorker"></param>
        public FormWorkerOrCommunityDetail(Worker currentWorker, ExperimentModel relatedExperimentItem)
        {
            InitializeComponent();
            this.currentWorker = currentWorker;

            //Display the workerId in the form title
            this.Text = "[Worker]" + currentWorker.WorkerId;
            this.relatedExperimentItem = relatedExperimentItem;
            this.labelForWorkerId.Text = currentWorker.WorkerId;
            this.labelModelDetail.Text = relatedExperimentItem.ToString();
            RunType[] runTypesHaveWorkerMatrices = { RunType.DawidSkene, RunType.BCC, RunType.CBCC };

            //Only display the confusion matrices if it matches the runType
            if (runTypesHaveWorkerMatrices.Contains(relatedExperimentItem.runType))
            {
                indexOfExperimentItem = MainPage.mainPageForm.currentExperimentSetting.GetExperimenModelIndex(relatedExperimentItem);
                printableConfusionMatrix = MainPage.mainPageForm.currentExperimentSetting.getWorkerResults(indexOfExperimentItem, currentWorker.WorkerId);
                LoadConfusionMatrix();
                //SetUpChart();
                labelConfusionMatrix.Text = MainPage.mainPageForm.currentExperimentSetting.getConfusionMatrixString(printableConfusionMatrix);
                textBoxConfusionMatrix.Text = labelConfusionMatrix.Text;
                //start background thread to update the confusion matrice accordingly if it is activeLearning
                if (MainPage.mainPageForm.currentExperimentSetting.experimentType == ExperimentType.ActiveLearning)
                {
                    backgroundUpdateConfusionMatrix.RunWorkerAsync();
                }

            }
            else //for the RunType that does not have confusion matrix 
            {
                //hide all controls related to confusion matrix
                confusionMatrixGraph.Visible = false;
                textBoxConfusionMatrix.Visible = false;
            }

        }

        /// <summary>
        /// Initialise the form with the Worker Object
        /// </summary>
        /// <param name="currentWorker"></param>
        public FormWorkerOrCommunityDetail(int communityIndex, ExperimentModel relatedExperimentItem)
        {
            InitializeComponent();
            isWorker = false;
            this.communityIndex = communityIndex;

            //Display the workerId in the form title
            this.Text = "[Community]" + (communityIndex );
            this.labelForWorkerId.Text = "Community " + (communityIndex );
            labelTypeName.Text = "Community";

            this.relatedExperimentItem = relatedExperimentItem;
            this.labelModelDetail.Text = relatedExperimentItem.ToString();

            //The RunType that has Community Matrix
            RunType[] runTypesHaveCommunityMatrices = { RunType.CBCC };

            //Only display the confusion matrices if it matches the runType
            if (runTypesHaveCommunityMatrices.Contains(relatedExperimentItem.runType))
            {
                indexOfExperimentItem = MainPage.mainPageForm.currentExperimentSetting.GetExperimenModelIndex(relatedExperimentItem);

                //get printableConfusionMatrix 
                printableConfusionMatrix = MainPage.mainPageForm.currentExperimentSetting.getCommunityResults(indexOfExperimentItem, communityIndex);
                LoadConfusionMatrix();
    
                labelConfusionMatrix.Text = MainPage.mainPageForm.currentExperimentSetting.getConfusionMatrixString(printableConfusionMatrix);
                textBoxConfusionMatrix.Text = labelConfusionMatrix.Text;
                //start background thread to update the confusion matrice accordingly
                backgroundUpdateConfusionMatrix.RunWorkerAsync();
            }
            else
            {
                //hide all controls related to confusion matrix
                confusionMatrixGraph.Visible = false;
                textBoxConfusionMatrix.Visible = false;
            }

        } // End the constructor for the community 

        /// <summary>
        /// Paint the confusionMatriceGraph
        /// </summary>
        /// <param name="printableConfusionMatrix"></param>
        private void paintConfusionMatriceGraph(double[,] printableConfusionMatrix, bool isFirstTimeToAdd = false)
        {

            int labelCount = printableConfusionMatrix.GetLength(0);

            for (int goldLabelPos = labelCount - 1; goldLabelPos >= 0; goldLabelPos--)
            {
                Series currentSeries = new Series();

                if (isFirstTimeToAdd)
                {
                    currentSeries = new Series();
                    currentSeries.ChartArea = "Default";
                    currentSeries.Name = "Label" + (goldLabelPos + 1);
                  
                }
                else
                {
                    currentSeries = confusionMatrixGraph.Series[labelCount - goldLabelPos - 1];
                }


                //Update the datapoint of each true and worker label pair
                for (int workerLabelPos = 0; workerLabelPos < labelCount; workerLabelPos++)
                {
                    double pointValue = printableConfusionMatrix[goldLabelPos, workerLabelPos];
                    DataPoint currentDataPoint = new DataPoint(0D, pointValue);
                    currentSeries.Points.Add(currentDataPoint);
                }

                currentSeries.ChartType = SeriesChartType.Column;
                currentSeries.IsVisibleInLegend = false;

                
                //If it is the first time to initial the chart, add to the series
                if (isFirstTimeToAdd)
                {
                    currentSeries.IsXValueIndexed = true;
                    confusionMatrixGraph.Series.Add(currentSeries);

                }

                confusionMatrixGraph.Series[labelCount - goldLabelPos - 1].XValueMember = "X";
                confusionMatrixGraph.Series[labelCount - goldLabelPos - 1].YValueMembers = "Y";

            }//end for goldLabel


            this.confusionMatrixGraph.Invalidate();
            this.confusionMatrixGraph.Update();

        }


        private void setUpChart()
        {
            //set Point Gap Depth off each chart Type
            confusionMatrixGraph.ChartAreas["Default"].Area3DStyle.PointGapDepth = (int)3;

            //enable axisX 
            confusionMatrixGraph.ChartAreas["Default"].AxisX.Enabled = AxisEnabled.True;

            // Disable X axis margin
            confusionMatrixGraph.ChartAreas["Default"].AxisX.IsMarginVisible = false;

            // Enable 3D charts
            confusionMatrixGraph.ChartAreas["Default"].Area3DStyle.Enable3D = true;

            // Set Rotation angles
            confusionMatrixGraph.ChartAreas["Default"].Area3DStyle.Rotation = 45;
            confusionMatrixGraph.ChartAreas["Default"].Area3DStyle.Inclination = 45;
            confusionMatrixGraph.ChartAreas["Default"].Area3DStyle.IsClustered = false;

            // Disable/enable right angle axis
            confusionMatrixGraph.ChartAreas["Default"].Area3DStyle.IsRightAngleAxes = false;

            //show all the label on the x-axis
            this.confusionMatrixGraph.ChartAreas["Default"].AxisX.LabelStyle.Interval = 1;
            this.confusionMatrixGraph.ChartAreas["Default"].AxisX2.LabelStyle.Interval = 1;

            //Hide the gridline of the confusion matrix graph
            confusionMatrixGraph.ChartAreas["Default"].AxisY.MajorGrid.Enabled = false;
            confusionMatrixGraph.ChartAreas["Default"].AxisX.MajorGrid.Enabled = false;
        }


        /// <summary>
        /// Load the worker details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormWorkerDetail_Load(object sender, EventArgs e)
        {
            //Only set the workerId if it is a workerDetail
            if (currentWorker != null)
            {
                labelForWorkerId.Text = currentWorker.WorkerId;
            }
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
        /// Background Thread for updating the confusion matrix from the experiment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundUpdateConfusionMatrix_DoWork(object sender, DoWorkEventArgs e)
        {

            //Get the Backgroundworker
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;

            //listen the main page while the experiment is running
            while (!MainPage.mainPageForm.isExperimentComplete)
            {
                while (MainPage.mainPageForm.isExperimentRunning)
                {
                    if (isWorker)
                    {
                        printableConfusionMatrix = MainPage.mainPageForm.currentExperimentSetting.getWorkerResults(indexOfExperimentItem, currentWorker.WorkerId);
                        Debug.WriteLine("worker " + indexOfExperimentItem + "-" + currentWorker.WorkerId);
                    }
                    else 
                    {
                        printableConfusionMatrix = MainPage.mainPageForm.currentExperimentSetting.getCommunityResults(indexOfExperimentItem, communityIndex);
                        Debug.WriteLine( "community " + indexOfExperimentItem + "-" + communityIndex);
                    }
                    matriceText = MainPage.mainPageForm.currentExperimentSetting.getConfusionMatrixString(printableConfusionMatrix);
                    Debug.WriteLine(matriceText);
                    
                    try
                    {
                        //notify the graph to change
                        if (matriceText != labelConfusionMatrix.Text)
                        {
                            worker.ReportProgress(0, null);
                        }
                    }
                    catch (Exception exc)
                    {
                        Debug.Write(exc.Message);
                        break;

                    }
                    System.Threading.Thread.Sleep(100);
                }

            }

        }

        private void backgroundUpdateConfusionMatrix_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelConfusionMatrix.Text = matriceText;
            textBoxConfusionMatrix.Text = labelConfusionMatrix.Text;
            if (confusionMatrixGraph != null)
            {
                try
                {
                    //clear the graph
                    foreach (var series in this.confusionMatrixGraph.Series)
                    {
                        series.Points.Clear();
                    }

                    paintConfusionMatriceGraph(printableConfusionMatrix);
                }
                catch (NullReferenceException npe)
                {
                    Debug.Write(npe.Message);
                }

            }
        } // End Progress Change Method

    
    } // End class
} // End namespace
