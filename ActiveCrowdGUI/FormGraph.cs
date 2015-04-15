using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
namespace AcriveCrowdGUI
{
    public partial class FormGraph : KryptonForm
    {
        GraphPane currentGraphPane;
        delegate void AccurracyFunctionDelegate();
        string graphType = "Accuracy";
        //delegate of the progress pane
        delegate void graphPaneDelegate(int currentLabellingRound);
        public FormGraph()
        {
            InitializeComponent();
 
        }

        public FormGraph(string graphType):this()
        {
            this.graphType = graphType;

        }

        private void FormGraph_Load(object sender, EventArgs e)
        {

           // AccurracyFunctionDelegate afd = LoadGraph;
           // afd.BeginInvoke(null, null); //restart activelearning
            LoadGraph();
        }

        private void LoadGraph() {

            //keep listening from the experiment in the MainPage
            //Set the current progress from the Mainpage
            labelCurrentLabellingRound.Text = MainPage.mainPageForm.GetCurrentProgressText();

            currentGraphPane = graphControlGraphLarge.GraphPane;
            if (graphType == "Accuracy")
            {
                SetAccuracyGraphSetting();
                currentGraphPane.CurveList = MainPage.mainPageForm.accuracyGraphCurveList;
            
            }


            backgroundWorkerLoadingGraph.RunWorkerAsync();
           
        }

        private void SetAccuracyGraphSetting()
        {
            currentGraphPane.CurveList.Clear(); //clear the curveList in the graphpane
            currentGraphPane.Title.Text = "Accuracy Graph";

            currentGraphPane.Title.FontSpec.Size = 24f;
            currentGraphPane.Title.FontSpec.Family = "Times New Roman";


            currentGraphPane.XAxis.Title.Text = "Number of Labels";
            currentGraphPane.YAxis.Title.Text = "Accuracy";

            //hide the control grey border
            currentGraphPane.Margin.All = 5;
         
            currentGraphPane.Border.IsVisible = false;

            //set x starting point of the AccuracyGraph
            //currentGraphPane.XAxis.Scale.Min = MainPage.mainPageForm.initialStartingLabel;
            currentGraphPane.XAxis.Scale.Min = MainPage.mainPageForm.currentExperimentSetting.initialStartingLabel - 1;
        
        }
        private void updateGraphData(int currentLabellingRound) 
        {
           // labelCurrentLabellingRound.Text = MainPage.mainPageForm.currentLabellingRound + "";

        }

        private void FormGraph_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }


        /// <summary>
        /// update the graph from the MainPanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerLoadingGraph_DoWork(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;
            while(!MainPage.mainPageForm.isExperimentComplete)
            {
                while (MainPage.mainPageForm.isExperimentRunning && graphControlGraphLarge!=null) 
                {
                    //make sure the Y asix is rescaled to accommodate actual data
                    try{
                        graphControlGraphLarge.AxisChange();
                        //redraw the graph
                        graphControlGraphLarge.Invalidate();
                        
                    }catch(Exception currentException)
                    {
                        Debug.Write(currentException.ToString());
                    }
                    worker.ReportProgress(0, null);
                }//end while isExperimentRunning
            }//end while experiment complete
        }


        /// <summary>
        /// Update the status, runs on the main thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerLoadingGraph_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelCurrentLabellingRound.Text = MainPage.mainPageForm.GetCurrentProgressText();
        }

        private void labelCurrentLabellingRound_Click(object sender, EventArgs e)
        {

        }
    }
}
