using ComponentFactory.Krypton.Toolkit;
using System.Windows.Forms;

namespace AcriveCrowdGUI
{


    partial class MainPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "test22",
            "test3",
            "test",
            "test",
            "test"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("");
            this.tabControlForMainPage = new AC.ExtendedRenderer.Navigator.KryptonTabControl();
            this.tabPageMainPage = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tabPageRunBatchRunning = new System.Windows.Forms.TabPage();
            this.splitContainerOuterBatchLearning = new System.Windows.Forms.SplitContainer();
            this.label13 = new System.Windows.Forms.Label();
            this.progressBatchRunning = new AC.ExtendedRenderer.Toolkit.KryptonProgressBar();
            this.button25 = new System.Windows.Forms.Button();
            this.buttonRestartBatchRunning = new System.Windows.Forms.Button();
            this.labelDatasetNameBatchRunning = new System.Windows.Forms.Label();
            this.buttonPlayAndPauseBatchRunning = new System.Windows.Forms.Button();
            this.buttonStopBatchRunning = new System.Windows.Forms.Button();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.kryptonTabControl3 = new AC.ExtendedRenderer.Navigator.KryptonTabControl();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.dataGridViewBatchRunningProgress = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label16 = new System.Windows.Forms.Label();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.dataGridViewBatchRunningAccuracy = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBoxBatchRunningModel = new System.Windows.Forms.ComboBox();
            this.tabControlBatchLearningInnerValues = new AC.ExtendedRenderer.Navigator.KryptonTabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.dataGridViewBatchRunningWorkers = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.dataGridViewBatchRunningTasks = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.tabPageBatchLearningInnerViewCommunity = new System.Windows.Forms.TabPage();
            this.dataGridViewBatchRunningCommunities = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.tabPageRunActiveLearning = new System.Windows.Forms.TabPage();
            this.splitContainerActiveLearningOuterContainer = new System.Windows.Forms.SplitContainer();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBarForActiveLearning = new AC.ExtendedRenderer.Toolkit.KryptonProgressBar();
            this.buttonActiveLearningCloseAllOpenedWindow = new System.Windows.Forms.Button();
            this.buttonRestartActiveLearning = new System.Windows.Forms.Button();
            this.labelDatasetName = new System.Windows.Forms.Label();
            this.btnPlayAndPauseExperiment = new System.Windows.Forms.Button();
            this.btnStopExperiment = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.TabControlActiveLearningGraphs = new AC.ExtendedRenderer.Navigator.KryptonTabControl();
            this.tabPageAccuracyGraph = new System.Windows.Forms.TabPage();
            this.graphControlAccuracyGraph = new ZedGraph.ZedGraphControl();
            this.btnActiveLearning_PopUpAccuracyGraph = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button16 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxShowCurrentModels = new System.Windows.Forms.ComboBox();
            this.tabControlForValues = new AC.ExtendedRenderer.Navigator.KryptonTabControl();
            this.tabPageInnerViewSchedule = new System.Windows.Forms.TabPage();
            this.dataGridViewActiveLearningSchedule = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.tabPageInnerViewWorkers = new System.Windows.Forms.TabPage();
            this.dataGridViewForInnerWorker = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.tabPageInnerViewDataset = new System.Windows.Forms.TabPage();
            this.dataGridViewForTaskValue = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.tabPageInnerViewCommunity = new System.Windows.Forms.TabPage();
            this.dataGridViewInnerCommunity = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.tabPageForViewWorkers = new System.Windows.Forms.TabPage();
            this.splitContainerForWorkers = new System.Windows.Forms.SplitContainer();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.splitContainerWokerDetails = new System.Windows.Forms.SplitContainer();
            this.kryptonListView1 = new AC.ExtendedRenderer.Toolkit.KryptonListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.kryptonListViewForWorkers = new AC.ExtendedRenderer.Toolkit.KryptonListView();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageViewDataset = new System.Windows.Forms.TabPage();
            this.splitContainerViewDataset = new System.Windows.Forms.SplitContainer();
            this.buttonLoadDataset = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxForSelectingDataset = new AC.ExtendedRenderer.Toolkit.KryptonComboBox();
            this.dataGridViewOfDataset = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDatasetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hELPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonLabel1 = new AC.ExtendedRenderer.Toolkit.KryptonLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kryptonLinkLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLinkLabel();
            this.kryptonListBox1 = new ComponentFactory.Krypton.Toolkit.KryptonListBox();
            this.kryptonListBox2 = new ComponentFactory.Krypton.Toolkit.KryptonListBox();
            this.kryptonImageComboBox1 = new AC.ExtendedRenderer.Toolkit.KryptonImageComboBox();
            this.kryptonTrackBar1 = new ComponentFactory.Krypton.Toolkit.KryptonTrackBar();
            this.backgroundWorkerForSimulation = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerForProgressBar = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerForAccuracyGraph = new System.ComponentModel.BackgroundWorker();
            this.kryptonContextMenu1 = new ComponentFactory.Krypton.Toolkit.KryptonContextMenu();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainerInnterBatchRunning = new System.Windows.Forms.SplitContainer();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.kryptonProgressBar1 = new AC.ExtendedRenderer.Toolkit.KryptonProgressBar();
            this.button6 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.button22 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.kryptonDataGridView5 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.kryptonTabControl1 = new AC.ExtendedRenderer.Navigator.KryptonTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.kryptonDataGridView1 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.kryptonDataGridView2 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.kryptonDataGridView3 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.kryptonDataGridView4 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.backgroundWorkerBatchRunning = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerBatchRunningValues = new System.ComponentModel.BackgroundWorker();
            this.mainPageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControlForMainPage.SuspendLayout();
            this.tabPageMainPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPageRunBatchRunning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOuterBatchLearning)).BeginInit();
            this.splitContainerOuterBatchLearning.Panel1.SuspendLayout();
            this.splitContainerOuterBatchLearning.Panel2.SuspendLayout();
            this.splitContainerOuterBatchLearning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.kryptonTabControl3.SuspendLayout();
            this.tabPage11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchRunningProgress)).BeginInit();
            this.tabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchRunningAccuracy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.tabControlBatchLearningInnerValues.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchRunningWorkers)).BeginInit();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchRunningTasks)).BeginInit();
            this.tabPageBatchLearningInnerViewCommunity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchRunningCommunities)).BeginInit();
            this.tabPageRunActiveLearning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerActiveLearningOuterContainer)).BeginInit();
            this.splitContainerActiveLearningOuterContainer.Panel1.SuspendLayout();
            this.splitContainerActiveLearningOuterContainer.Panel2.SuspendLayout();
            this.splitContainerActiveLearningOuterContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.TabControlActiveLearningGraphs.SuspendLayout();
            this.tabPageAccuracyGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControlForValues.SuspendLayout();
            this.tabPageInnerViewSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewActiveLearningSchedule)).BeginInit();
            this.tabPageInnerViewWorkers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForInnerWorker)).BeginInit();
            this.tabPageInnerViewDataset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForTaskValue)).BeginInit();
            this.tabPageInnerViewCommunity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInnerCommunity)).BeginInit();
            this.tabPageForViewWorkers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerForWorkers)).BeginInit();
            this.splitContainerForWorkers.Panel1.SuspendLayout();
            this.splitContainerForWorkers.Panel2.SuspendLayout();
            this.splitContainerForWorkers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWokerDetails)).BeginInit();
            this.splitContainerWokerDetails.Panel1.SuspendLayout();
            this.splitContainerWokerDetails.Panel2.SuspendLayout();
            this.splitContainerWokerDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabPageViewDataset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerViewDataset)).BeginInit();
            this.splitContainerViewDataset.Panel1.SuspendLayout();
            this.splitContainerViewDataset.Panel2.SuspendLayout();
            this.splitContainerViewDataset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOfDataset)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonImageComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInnterBatchRunning)).BeginInit();
            this.splitContainerInnterBatchRunning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView5)).BeginInit();
            this.kryptonTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView2)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView3)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainPageBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlForMainPage
            // 
            this.tabControlForMainPage.AllowCloseButton = false;
            this.tabControlForMainPage.AllowContextButton = true;
            this.tabControlForMainPage.AllowNavigatorButtons = false;
            this.tabControlForMainPage.AllowSelectedTabHigh = false;
            this.tabControlForMainPage.BorderWidth = 1;
            this.tabControlForMainPage.Controls.Add(this.tabPageMainPage);
            this.tabControlForMainPage.Controls.Add(this.tabPageRunBatchRunning);
            this.tabControlForMainPage.Controls.Add(this.tabPageRunActiveLearning);
            this.tabControlForMainPage.Controls.Add(this.tabPageForViewWorkers);
            this.tabControlForMainPage.Controls.Add(this.tabPageViewDataset);
            this.tabControlForMainPage.CornerRoundRadiusWidth = 12;
            this.tabControlForMainPage.CornerSymmetry = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornSymmetry.Both;
            this.tabControlForMainPage.CornerType = AC.ExtendedRenderer.Toolkit.Drawing.DrawingMethods.CornerType.Rounded;
            this.tabControlForMainPage.CornerWidth = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornWidth.Thin;
            this.tabControlForMainPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlForMainPage.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlForMainPage.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlForMainPage.HotTrack = true;
            this.tabControlForMainPage.ImageList = this.imageList1;
            this.tabControlForMainPage.ItemSize = new System.Drawing.Size(54, 30);
            this.tabControlForMainPage.Location = new System.Drawing.Point(0, 24);
            this.tabControlForMainPage.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.tabControlForMainPage.Name = "tabControlForMainPage";
            this.tabControlForMainPage.PreserveTabColor = false;
            this.tabControlForMainPage.SelectedIndex = 0;
            this.tabControlForMainPage.Size = new System.Drawing.Size(1092, 659);
            this.tabControlForMainPage.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControlForMainPage.TabIndex = 1;
            this.tabControlForMainPage.UseExtendedLayout = false;
            this.tabControlForMainPage.SelectedIndexChanged += new System.EventHandler(this.tabControlForMainPage_SelectedIndexChanged);
            // 
            // tabPageMainPage
            // 
            this.tabPageMainPage.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageMainPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageMainPage.Controls.Add(this.pictureBox1);
            this.tabPageMainPage.Controls.Add(this.label3);
            this.tabPageMainPage.Controls.Add(this.groupBox5);
            this.tabPageMainPage.Controls.Add(this.groupBox4);
            this.tabPageMainPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageMainPage.ImageIndex = 0;
            this.tabPageMainPage.Location = new System.Drawing.Point(4, 34);
            this.tabPageMainPage.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMainPage.Name = "tabPageMainPage";
            this.tabPageMainPage.Size = new System.Drawing.Size(1084, 621);
            this.tabPageMainPage.TabIndex = 0;
            this.tabPageMainPage.Tag = false;
            this.tabPageMainPage.Text = "Home";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(388, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(661, 541);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(53, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(317, 31);
            this.label3.TabIndex = 10;
            this.label3.Text = "The Active Crowd Toolkit";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.button10);
            this.groupBox5.Controls.Add(this.button9);
            this.groupBox5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(47, 130);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(316, 217);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Load Data";
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Image = global::AcriveCrowdGUI.Properties.Resources.workers_icon2;
            this.button10.Location = new System.Drawing.Point(24, 41);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(270, 71);
            this.button10.TabIndex = 4;
            this.button10.Text = "  Load Datasets";
            this.button10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.buttonToDatasetTabpage_Click_1);
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Image = global::AcriveCrowdGUI.Properties.Resources.list_view;
            this.button9.Location = new System.Drawing.Point(24, 118);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(270, 71);
            this.button9.TabIndex = 3;
            this.button9.Text = "    Load Test Datasets";
            this.button9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Visible = false;
            this.button9.Click += new System.EventHandler(this.buttonToViewDataset_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.button5);
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(47, 378);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(316, 207);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Run Experiments";
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Image = global::AcriveCrowdGUI.Properties.Resources.Batch_icon;
            this.button5.Location = new System.Drawing.Point(24, 39);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(270, 71);
            this.button5.TabIndex = 4;
            this.button5.Text = "    Run Batch Running";
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.buttonToBatchRunning_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Image = global::AcriveCrowdGUI.Properties.Resources.ActiveLearning_Icon;
            this.button4.Location = new System.Drawing.Point(24, 118);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(270, 71);
            this.button4.TabIndex = 3;
            this.button4.Text = "    Run Active Learning ";
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.buttonToActiveLearning_Click);
            // 
            // tabPageRunBatchRunning
            // 
            this.tabPageRunBatchRunning.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageRunBatchRunning.Controls.Add(this.splitContainerOuterBatchLearning);
            this.tabPageRunBatchRunning.ImageIndex = 1;
            this.tabPageRunBatchRunning.Location = new System.Drawing.Point(4, 34);
            this.tabPageRunBatchRunning.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageRunBatchRunning.Name = "tabPageRunBatchRunning";
            this.tabPageRunBatchRunning.Size = new System.Drawing.Size(1084, 621);
            this.tabPageRunBatchRunning.TabIndex = 4;
            this.tabPageRunBatchRunning.Tag = false;
            this.tabPageRunBatchRunning.Text = "Run Batch Running";
            // 
            // splitContainerOuterBatchLearning
            // 
            this.splitContainerOuterBatchLearning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOuterBatchLearning.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerOuterBatchLearning.Location = new System.Drawing.Point(0, 0);
            this.splitContainerOuterBatchLearning.Name = "splitContainerOuterBatchLearning";
            this.splitContainerOuterBatchLearning.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerOuterBatchLearning.Panel1
            // 
            this.splitContainerOuterBatchLearning.Panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.splitContainerOuterBatchLearning.Panel1.Controls.Add(this.label13);
            this.splitContainerOuterBatchLearning.Panel1.Controls.Add(this.progressBatchRunning);
            this.splitContainerOuterBatchLearning.Panel1.Controls.Add(this.button25);
            this.splitContainerOuterBatchLearning.Panel1.Controls.Add(this.buttonRestartBatchRunning);
            this.splitContainerOuterBatchLearning.Panel1.Controls.Add(this.labelDatasetNameBatchRunning);
            this.splitContainerOuterBatchLearning.Panel1.Controls.Add(this.buttonPlayAndPauseBatchRunning);
            this.splitContainerOuterBatchLearning.Panel1.Controls.Add(this.buttonStopBatchRunning);
            // 
            // splitContainerOuterBatchLearning.Panel2
            // 
            this.splitContainerOuterBatchLearning.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainerOuterBatchLearning.Size = new System.Drawing.Size(1084, 621);
            this.splitContainerOuterBatchLearning.SplitterDistance = 86;
            this.splitContainerOuterBatchLearning.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(87, 58);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 15);
            this.label13.TabIndex = 39;
            this.label13.Text = "Current Dataset:";
            // 
            // progressBatchRunning
            // 
            this.progressBatchRunning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBatchRunning.BackColor = System.Drawing.Color.Transparent;
            this.progressBatchRunning.DisplayText = "Launching Experiment";
            this.progressBatchRunning.EndColor = System.Drawing.Color.ForestGreen;
            this.progressBatchRunning.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressBatchRunning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.progressBatchRunning.Location = new System.Drawing.Point(170, 15);
            this.progressBatchRunning.Name = "progressBatchRunning";
            this.progressBatchRunning.Size = new System.Drawing.Size(740, 35);
            this.progressBatchRunning.StartColor = System.Drawing.Color.LawnGreen;
            this.progressBatchRunning.TabIndex = 38;
            // 
            // button25
            // 
            this.button25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button25.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.application_delete;
            this.button25.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button25.Location = new System.Drawing.Point(916, 15);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(35, 35);
            this.button25.TabIndex = 36;
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.buttonCloseAllWindowsBatchLearning_Click);
            // 
            // buttonRestartBatchRunning
            // 
            this.buttonRestartBatchRunning.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.arrow_redo;
            this.buttonRestartBatchRunning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRestartBatchRunning.Location = new System.Drawing.Point(129, 15);
            this.buttonRestartBatchRunning.Name = "buttonRestartBatchRunning";
            this.buttonRestartBatchRunning.Size = new System.Drawing.Size(35, 35);
            this.buttonRestartBatchRunning.TabIndex = 33;
            this.buttonRestartBatchRunning.UseVisualStyleBackColor = true;
            this.buttonRestartBatchRunning.Click += new System.EventHandler(this.buttonRestartBatchRunning_Click);
            // 
            // labelDatasetNameBatchRunning
            // 
            this.labelDatasetNameBatchRunning.AutoSize = true;
            this.labelDatasetNameBatchRunning.Location = new System.Drawing.Point(187, 58);
            this.labelDatasetNameBatchRunning.Name = "labelDatasetNameBatchRunning";
            this.labelDatasetNameBatchRunning.Size = new System.Drawing.Size(22, 15);
            this.labelDatasetNameBatchRunning.TabIndex = 32;
            this.labelDatasetNameBatchRunning.Text = "CF";
            // 
            // buttonPlayAndPauseBatchRunning
            // 
            this.buttonPlayAndPauseBatchRunning.BackColor = System.Drawing.SystemColors.Menu;
            this.buttonPlayAndPauseBatchRunning.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Pause_Hot_icon;
            this.buttonPlayAndPauseBatchRunning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonPlayAndPauseBatchRunning.FlatAppearance.BorderSize = 0;
            this.buttonPlayAndPauseBatchRunning.Location = new System.Drawing.Point(10, 15);
            this.buttonPlayAndPauseBatchRunning.Name = "buttonPlayAndPauseBatchRunning";
            this.buttonPlayAndPauseBatchRunning.Size = new System.Drawing.Size(72, 60);
            this.buttonPlayAndPauseBatchRunning.TabIndex = 31;
            this.buttonPlayAndPauseBatchRunning.UseVisualStyleBackColor = false;
            this.buttonPlayAndPauseBatchRunning.Click += new System.EventHandler(this.buttonPlayAndPauseBatchRunning_Click);
            // 
            // buttonStopBatchRunning
            // 
            this.buttonStopBatchRunning.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Stop_icon;
            this.buttonStopBatchRunning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonStopBatchRunning.FlatAppearance.BorderSize = 0;
            this.buttonStopBatchRunning.ForeColor = System.Drawing.Color.Transparent;
            this.buttonStopBatchRunning.Location = new System.Drawing.Point(88, 15);
            this.buttonStopBatchRunning.Name = "buttonStopBatchRunning";
            this.buttonStopBatchRunning.Size = new System.Drawing.Size(35, 35);
            this.buttonStopBatchRunning.TabIndex = 30;
            this.buttonStopBatchRunning.UseVisualStyleBackColor = true;
            this.buttonStopBatchRunning.Click += new System.EventHandler(this.buttonStopBatchRunning_Click);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.kryptonTabControl3);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer6);
            this.splitContainer4.Size = new System.Drawing.Size(1084, 531);
            this.splitContainer4.SplitterDistance = 550;
            this.splitContainer4.TabIndex = 0;
            // 
            // kryptonTabControl3
            // 
            this.kryptonTabControl3.AllowCloseButton = false;
            this.kryptonTabControl3.AllowContextButton = true;
            this.kryptonTabControl3.AllowNavigatorButtons = false;
            this.kryptonTabControl3.AllowSelectedTabHigh = false;
            this.kryptonTabControl3.BorderWidth = 1;
            this.kryptonTabControl3.Controls.Add(this.tabPage11);
            this.kryptonTabControl3.Controls.Add(this.tabPage9);
            this.kryptonTabControl3.CornerRoundRadiusWidth = 12;
            this.kryptonTabControl3.CornerSymmetry = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornSymmetry.Both;
            this.kryptonTabControl3.CornerType = AC.ExtendedRenderer.Toolkit.Drawing.DrawingMethods.CornerType.Rounded;
            this.kryptonTabControl3.CornerWidth = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornWidth.Thin;
            this.kryptonTabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonTabControl3.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.kryptonTabControl3.HotTrack = true;
            this.kryptonTabControl3.ImageList = this.imageList2;
            this.kryptonTabControl3.ItemSize = new System.Drawing.Size(113, 25);
            this.kryptonTabControl3.Location = new System.Drawing.Point(0, 0);
            this.kryptonTabControl3.Name = "kryptonTabControl3";
            this.kryptonTabControl3.PreserveTabColor = false;
            this.kryptonTabControl3.SelectedIndex = 0;
            this.kryptonTabControl3.ShowToolTips = true;
            this.kryptonTabControl3.Size = new System.Drawing.Size(550, 531);
            this.kryptonTabControl3.TabIndex = 16;
            this.kryptonTabControl3.UseExtendedLayout = false;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.dataGridViewBatchRunningProgress);
            this.tabPage11.Controls.Add(this.label16);
            this.tabPage11.ImageIndex = 2;
            this.tabPage11.Location = new System.Drawing.Point(4, 29);
            this.tabPage11.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new System.Drawing.Size(542, 498);
            this.tabPage11.TabIndex = 3;
            this.tabPage11.Tag = false;
            this.tabPage11.Text = "Work Breakdown";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // dataGridViewBatchRunningProgress
            // 
            this.dataGridViewBatchRunningProgress.AllowUserToAddRows = false;
            this.dataGridViewBatchRunningProgress.AllowUserToDeleteRows = false;
            this.dataGridViewBatchRunningProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewBatchRunningProgress.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBatchRunningProgress.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridViewBatchRunningProgress.HideOuterBorders = true;
            this.dataGridViewBatchRunningProgress.Location = new System.Drawing.Point(-9, 46);
            this.dataGridViewBatchRunningProgress.Name = "dataGridViewBatchRunningProgress";
            this.dataGridViewBatchRunningProgress.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Silver;
            this.dataGridViewBatchRunningProgress.RowTemplate.ReadOnly = true;
            this.dataGridViewBatchRunningProgress.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBatchRunningProgress.ShowEditingIcon = false;
            this.dataGridViewBatchRunningProgress.Size = new System.Drawing.Size(552, 490);
            this.dataGridViewBatchRunningProgress.TabIndex = 6;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.DarkBlue;
            this.label16.Location = new System.Drawing.Point(25, 17);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(126, 19);
            this.label16.TabIndex = 5;
            this.label16.Text = "Work Breakdown";
            // 
            // tabPage9
            // 
            this.tabPage9.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage9.Controls.Add(this.label14);
            this.tabPage9.Controls.Add(this.dataGridViewBatchRunningAccuracy);
            this.tabPage9.ImageIndex = 0;
            this.tabPage9.Location = new System.Drawing.Point(4, 29);
            this.tabPage9.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Size = new System.Drawing.Size(542, 498);
            this.tabPage9.TabIndex = 0;
            this.tabPage9.Tag = false;
            this.tabPage9.Text = "Accuracy";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.DarkBlue;
            this.label14.Location = new System.Drawing.Point(15, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(71, 19);
            this.label14.TabIndex = 8;
            this.label14.Text = "Accuracy";
            // 
            // dataGridViewBatchRunningAccuracy
            // 
            this.dataGridViewBatchRunningAccuracy.AllowUserToAddRows = false;
            this.dataGridViewBatchRunningAccuracy.AllowUserToDeleteRows = false;
            this.dataGridViewBatchRunningAccuracy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewBatchRunningAccuracy.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBatchRunningAccuracy.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridViewBatchRunningAccuracy.HideOuterBorders = true;
            this.dataGridViewBatchRunningAccuracy.Location = new System.Drawing.Point(-5, 46);
            this.dataGridViewBatchRunningAccuracy.Name = "dataGridViewBatchRunningAccuracy";
            this.dataGridViewBatchRunningAccuracy.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Silver;
            this.dataGridViewBatchRunningAccuracy.RowTemplate.ReadOnly = true;
            this.dataGridViewBatchRunningAccuracy.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBatchRunningAccuracy.ShowEditingIcon = false;
            this.dataGridViewBatchRunningAccuracy.Size = new System.Drawing.Size(552, 448);
            this.dataGridViewBatchRunningAccuracy.TabIndex = 7;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "chart_line.png");
            this.imageList2.Images.SetKeyName(1, "chart_curve.png");
            this.imageList2.Images.SetKeyName(2, "progressbar.png");
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.splitContainer6.Panel1.Controls.Add(this.label15);
            this.splitContainer6.Panel1.Controls.Add(this.comboBoxBatchRunningModel);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.tabControlBatchLearningInnerValues);
            this.splitContainer6.Size = new System.Drawing.Size(530, 531);
            this.splitContainer6.SplitterDistance = 42;
            this.splitContainer6.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(18, 15);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(88, 15);
            this.label15.TabIndex = 15;
            this.label15.Text = "Current Model";
            // 
            // comboBoxBatchRunningModel
            // 
            this.comboBoxBatchRunningModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBatchRunningModel.FormattingEnabled = true;
            this.comboBoxBatchRunningModel.Location = new System.Drawing.Point(112, 12);
            this.comboBoxBatchRunningModel.Name = "comboBoxBatchRunningModel";
            this.comboBoxBatchRunningModel.Size = new System.Drawing.Size(326, 23);
            this.comboBoxBatchRunningModel.TabIndex = 14;
            this.comboBoxBatchRunningModel.SelectedIndexChanged += new System.EventHandler(this.comboBoxBatchRunningModel_SelectedIndexChanged);
            // 
            // tabControlBatchLearningInnerValues
            // 
            this.tabControlBatchLearningInnerValues.AllowCloseButton = false;
            this.tabControlBatchLearningInnerValues.AllowContextButton = true;
            this.tabControlBatchLearningInnerValues.AllowNavigatorButtons = false;
            this.tabControlBatchLearningInnerValues.AllowSelectedTabHigh = false;
            this.tabControlBatchLearningInnerValues.BorderWidth = 1;
            this.tabControlBatchLearningInnerValues.Controls.Add(this.tabPage6);
            this.tabControlBatchLearningInnerValues.Controls.Add(this.tabPage7);
            this.tabControlBatchLearningInnerValues.Controls.Add(this.tabPageBatchLearningInnerViewCommunity);
            this.tabControlBatchLearningInnerValues.CornerRoundRadiusWidth = 12;
            this.tabControlBatchLearningInnerValues.CornerSymmetry = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornSymmetry.Both;
            this.tabControlBatchLearningInnerValues.CornerType = AC.ExtendedRenderer.Toolkit.Drawing.DrawingMethods.CornerType.Rounded;
            this.tabControlBatchLearningInnerValues.CornerWidth = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornWidth.Thin;
            this.tabControlBatchLearningInnerValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlBatchLearningInnerValues.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlBatchLearningInnerValues.HotTrack = true;
            this.tabControlBatchLearningInnerValues.ImageList = this.imageList3;
            this.tabControlBatchLearningInnerValues.ItemSize = new System.Drawing.Size(130, 25);
            this.tabControlBatchLearningInnerValues.Location = new System.Drawing.Point(0, 0);
            this.tabControlBatchLearningInnerValues.Name = "tabControlBatchLearningInnerValues";
            this.tabControlBatchLearningInnerValues.PreserveTabColor = false;
            this.tabControlBatchLearningInnerValues.SelectedIndex = 0;
            this.tabControlBatchLearningInnerValues.Size = new System.Drawing.Size(530, 485);
            this.tabControlBatchLearningInnerValues.TabIndex = 3;
            this.tabControlBatchLearningInnerValues.UseExtendedLayout = false;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.dataGridViewBatchRunningWorkers);
            this.tabPage6.ImageIndex = 1;
            this.tabPage6.Location = new System.Drawing.Point(4, 29);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(522, 452);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Tag = false;
            this.tabPage6.Text = "Workers";
            // 
            // dataGridViewBatchRunningWorkers
            // 
            this.dataGridViewBatchRunningWorkers.AllowUserToAddRows = false;
            this.dataGridViewBatchRunningWorkers.AllowUserToDeleteRows = false;
            this.dataGridViewBatchRunningWorkers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBatchRunningWorkers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridViewBatchRunningWorkers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBatchRunningWorkers.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewBatchRunningWorkers.Name = "dataGridViewBatchRunningWorkers";
            this.dataGridViewBatchRunningWorkers.ReadOnly = true;
            this.dataGridViewBatchRunningWorkers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBatchRunningWorkers.Size = new System.Drawing.Size(522, 452);
            this.dataGridViewBatchRunningWorkers.TabIndex = 0;
            this.dataGridViewBatchRunningWorkers.VirtualMode = true;
            this.dataGridViewBatchRunningWorkers.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridViewBatchRunningWorkers_CellValueNeeded);
            this.dataGridViewBatchRunningWorkers.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewActiveLearningWorkers1_RowPostPaint);
            this.dataGridViewBatchRunningWorkers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewBatchRunningWorkers_MouseDoubleClick);
            // 
            // tabPage7
            // 
            this.tabPage7.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage7.Controls.Add(this.dataGridViewBatchRunningTasks);
            this.tabPage7.ImageIndex = 2;
            this.tabPage7.Location = new System.Drawing.Point(4, 29);
            this.tabPage7.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(522, 452);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Tag = false;
            this.tabPage7.Text = "Tasks";
            // 
            // dataGridViewBatchRunningTasks
            // 
            this.dataGridViewBatchRunningTasks.AllowUserToAddRows = false;
            this.dataGridViewBatchRunningTasks.AllowUserToDeleteRows = false;
            this.dataGridViewBatchRunningTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBatchRunningTasks.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridViewBatchRunningTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBatchRunningTasks.HideOuterBorders = true;
            this.dataGridViewBatchRunningTasks.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewBatchRunningTasks.Name = "dataGridViewBatchRunningTasks";
            this.dataGridViewBatchRunningTasks.RowTemplate.ReadOnly = true;
            this.dataGridViewBatchRunningTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBatchRunningTasks.ShowEditingIcon = false;
            this.dataGridViewBatchRunningTasks.Size = new System.Drawing.Size(522, 452);
            this.dataGridViewBatchRunningTasks.TabIndex = 2;
            this.dataGridViewBatchRunningTasks.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewActiveLearningWorkers1_RowPostPaint);
            this.dataGridViewBatchRunningTasks.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewBatchRunningTasks_MouseDoubleClick);
            // 
            // tabPageBatchLearningInnerViewCommunity
            // 
            this.tabPageBatchLearningInnerViewCommunity.Controls.Add(this.dataGridViewBatchRunningCommunities);
            this.tabPageBatchLearningInnerViewCommunity.ImageIndex = 3;
            this.tabPageBatchLearningInnerViewCommunity.Location = new System.Drawing.Point(4, 29);
            this.tabPageBatchLearningInnerViewCommunity.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageBatchLearningInnerViewCommunity.Name = "tabPageBatchLearningInnerViewCommunity";
            this.tabPageBatchLearningInnerViewCommunity.Size = new System.Drawing.Size(522, 452);
            this.tabPageBatchLearningInnerViewCommunity.TabIndex = 4;
            this.tabPageBatchLearningInnerViewCommunity.Tag = false;
            this.tabPageBatchLearningInnerViewCommunity.Text = "Communities";
            this.tabPageBatchLearningInnerViewCommunity.UseVisualStyleBackColor = true;
            // 
            // dataGridViewBatchRunningCommunities
            // 
            this.dataGridViewBatchRunningCommunities.AllowUserToAddRows = false;
            this.dataGridViewBatchRunningCommunities.AllowUserToDeleteRows = false;
            this.dataGridViewBatchRunningCommunities.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBatchRunningCommunities.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridViewBatchRunningCommunities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBatchRunningCommunities.HideOuterBorders = true;
            this.dataGridViewBatchRunningCommunities.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewBatchRunningCommunities.Name = "dataGridViewBatchRunningCommunities";
            this.dataGridViewBatchRunningCommunities.RowTemplate.ReadOnly = true;
            this.dataGridViewBatchRunningCommunities.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBatchRunningCommunities.ShowEditingIcon = false;
            this.dataGridViewBatchRunningCommunities.Size = new System.Drawing.Size(522, 452);
            this.dataGridViewBatchRunningCommunities.TabIndex = 2;
            this.dataGridViewBatchRunningCommunities.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewActiveLearningWorkers1_RowPostPaint);
            this.dataGridViewBatchRunningCommunities.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewBatchRunningCommunities_MouseDoubleClick);
            // 
            // imageList3
            // 
            this.imageList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList3.ImageStream")));
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList3.Images.SetKeyName(0, "sheduled_task.png");
            this.imageList3.Images.SetKeyName(1, "workers_icon2.ico");
            this.imageList3.Images.SetKeyName(2, "date_task.png");
            this.imageList3.Images.SetKeyName(3, "report_user.png");
            // 
            // tabPageRunActiveLearning
            // 
            this.tabPageRunActiveLearning.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageRunActiveLearning.Controls.Add(this.splitContainerActiveLearningOuterContainer);
            this.tabPageRunActiveLearning.ImageIndex = 2;
            this.tabPageRunActiveLearning.Location = new System.Drawing.Point(4, 34);
            this.tabPageRunActiveLearning.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageRunActiveLearning.Name = "tabPageRunActiveLearning";
            this.tabPageRunActiveLearning.Size = new System.Drawing.Size(1084, 621);
            this.tabPageRunActiveLearning.TabIndex = 1;
            this.tabPageRunActiveLearning.Tag = false;
            this.tabPageRunActiveLearning.Text = "Run Active Learning";
            this.tabPageRunActiveLearning.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // splitContainerActiveLearningOuterContainer
            // 
            this.splitContainerActiveLearningOuterContainer.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerActiveLearningOuterContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerActiveLearningOuterContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainerActiveLearningOuterContainer.Name = "splitContainerActiveLearningOuterContainer";
            this.splitContainerActiveLearningOuterContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerActiveLearningOuterContainer.Panel1
            // 
            this.splitContainerActiveLearningOuterContainer.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(206)))), ((int)(((byte)(230)))));
            this.splitContainerActiveLearningOuterContainer.Panel1.Controls.Add(this.label5);
            this.splitContainerActiveLearningOuterContainer.Panel1.Controls.Add(this.progressBarForActiveLearning);
            this.splitContainerActiveLearningOuterContainer.Panel1.Controls.Add(this.buttonActiveLearningCloseAllOpenedWindow);
            this.splitContainerActiveLearningOuterContainer.Panel1.Controls.Add(this.buttonRestartActiveLearning);
            this.splitContainerActiveLearningOuterContainer.Panel1.Controls.Add(this.labelDatasetName);
            this.splitContainerActiveLearningOuterContainer.Panel1.Controls.Add(this.btnPlayAndPauseExperiment);
            this.splitContainerActiveLearningOuterContainer.Panel1.Controls.Add(this.btnStopExperiment);
            this.splitContainerActiveLearningOuterContainer.Panel1.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // splitContainerActiveLearningOuterContainer.Panel2
            // 
            this.splitContainerActiveLearningOuterContainer.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainerActiveLearningOuterContainer.Size = new System.Drawing.Size(1084, 621);
            this.splitContainerActiveLearningOuterContainer.SplitterDistance = 81;
            this.splitContainerActiveLearningOuterContainer.TabIndex = 0;
            this.splitContainerActiveLearningOuterContainer.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(89, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 15);
            this.label5.TabIndex = 29;
            this.label5.Text = "Current Dataset:";
            // 
            // progressBarForActiveLearning
            // 
            this.progressBarForActiveLearning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarForActiveLearning.BackColor = System.Drawing.Color.Transparent;
            this.progressBarForActiveLearning.DisplayText = "0/1720";
            this.progressBarForActiveLearning.EndColor = System.Drawing.Color.ForestGreen;
            this.progressBarForActiveLearning.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressBarForActiveLearning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.progressBarForActiveLearning.Location = new System.Drawing.Point(172, 14);
            this.progressBarForActiveLearning.MaxValue = 1420;
            this.progressBarForActiveLearning.Name = "progressBarForActiveLearning";
            this.progressBarForActiveLearning.Size = new System.Drawing.Size(740, 35);
            this.progressBarForActiveLearning.StartColor = System.Drawing.Color.LawnGreen;
            this.progressBarForActiveLearning.TabIndex = 28;
            this.progressBarForActiveLearning.ValueChanged += new AC.StdControls.Toolkit.Common.ProgressBar.ValueChangedHandler(this.progressBarForActiveLearning_ValueChanged);
            // 
            // buttonActiveLearningCloseAllOpenedWindow
            // 
            this.buttonActiveLearningCloseAllOpenedWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonActiveLearningCloseAllOpenedWindow.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.application_delete;
            this.buttonActiveLearningCloseAllOpenedWindow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonActiveLearningCloseAllOpenedWindow.Location = new System.Drawing.Point(918, 14);
            this.buttonActiveLearningCloseAllOpenedWindow.Name = "buttonActiveLearningCloseAllOpenedWindow";
            this.buttonActiveLearningCloseAllOpenedWindow.Size = new System.Drawing.Size(35, 35);
            this.buttonActiveLearningCloseAllOpenedWindow.TabIndex = 23;
            this.buttonActiveLearningCloseAllOpenedWindow.UseVisualStyleBackColor = true;
            this.buttonActiveLearningCloseAllOpenedWindow.Click += new System.EventHandler(this.buttonActiveLearningCloseAllOpenedWindow_Click);
            this.buttonActiveLearningCloseAllOpenedWindow.MouseHover += new System.EventHandler(this.buttonActiveLearningCloseAllOpenedWindow_MouseHover);
            // 
            // buttonRestartActiveLearning
            // 
            this.buttonRestartActiveLearning.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.arrow_redo;
            this.buttonRestartActiveLearning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRestartActiveLearning.Location = new System.Drawing.Point(131, 14);
            this.buttonRestartActiveLearning.Name = "buttonRestartActiveLearning";
            this.buttonRestartActiveLearning.Size = new System.Drawing.Size(35, 35);
            this.buttonRestartActiveLearning.TabIndex = 20;
            this.buttonRestartActiveLearning.UseVisualStyleBackColor = true;
            this.buttonRestartActiveLearning.Click += new System.EventHandler(this.buttonRestartActiveLearning_Click);
            // 
            // labelDatasetName
            // 
            this.labelDatasetName.AutoSize = true;
            this.labelDatasetName.Location = new System.Drawing.Point(189, 57);
            this.labelDatasetName.Name = "labelDatasetName";
            this.labelDatasetName.Size = new System.Drawing.Size(22, 15);
            this.labelDatasetName.TabIndex = 19;
            this.labelDatasetName.Text = "CF";
            this.labelDatasetName.Click += new System.EventHandler(this.labelDatasetName_Click);
            // 
            // btnPlayAndPauseExperiment
            // 
            this.btnPlayAndPauseExperiment.BackColor = System.Drawing.SystemColors.Menu;
            this.btnPlayAndPauseExperiment.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Pause_Hot_icon;
            this.btnPlayAndPauseExperiment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlayAndPauseExperiment.FlatAppearance.BorderSize = 0;
            this.btnPlayAndPauseExperiment.Location = new System.Drawing.Point(12, 14);
            this.btnPlayAndPauseExperiment.Name = "btnPlayAndPauseExperiment";
            this.btnPlayAndPauseExperiment.Size = new System.Drawing.Size(72, 60);
            this.btnPlayAndPauseExperiment.TabIndex = 17;
            this.btnPlayAndPauseExperiment.UseVisualStyleBackColor = false;
            this.btnPlayAndPauseExperiment.Click += new System.EventHandler(this.btnPlayAndPauseExperiment_Click);
            // 
            // btnStopExperiment
            // 
            this.btnStopExperiment.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Stop_icon;
            this.btnStopExperiment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStopExperiment.FlatAppearance.BorderSize = 0;
            this.btnStopExperiment.ForeColor = System.Drawing.Color.Transparent;
            this.btnStopExperiment.Location = new System.Drawing.Point(90, 14);
            this.btnStopExperiment.Name = "btnStopExperiment";
            this.btnStopExperiment.Size = new System.Drawing.Size(35, 35);
            this.btnStopExperiment.TabIndex = 15;
            this.btnStopExperiment.UseVisualStyleBackColor = true;
            this.btnStopExperiment.Click += new System.EventHandler(this.btnStopExperiment_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.TabControlActiveLearningGraphs);
            this.splitContainer3.Panel1.Controls.Add(this.button1);
            this.splitContainer3.Panel1.Controls.Add(this.button7);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer3.Size = new System.Drawing.Size(1084, 536);
            this.splitContainer3.SplitterDistance = 528;
            this.splitContainer3.TabIndex = 0;
            // 
            // TabControlActiveLearningGraphs
            // 
            this.TabControlActiveLearningGraphs.AllowCloseButton = false;
            this.TabControlActiveLearningGraphs.AllowContextButton = true;
            this.TabControlActiveLearningGraphs.AllowNavigatorButtons = false;
            this.TabControlActiveLearningGraphs.AllowSelectedTabHigh = false;
            this.TabControlActiveLearningGraphs.BorderWidth = 1;
            this.TabControlActiveLearningGraphs.Controls.Add(this.tabPageAccuracyGraph);
            this.TabControlActiveLearningGraphs.CornerRoundRadiusWidth = 12;
            this.TabControlActiveLearningGraphs.CornerSymmetry = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornSymmetry.Both;
            this.TabControlActiveLearningGraphs.CornerType = AC.ExtendedRenderer.Toolkit.Drawing.DrawingMethods.CornerType.Rounded;
            this.TabControlActiveLearningGraphs.CornerWidth = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornWidth.Thin;
            this.TabControlActiveLearningGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControlActiveLearningGraphs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.TabControlActiveLearningGraphs.HotTrack = true;
            this.TabControlActiveLearningGraphs.ImageList = this.imageList2;
            this.TabControlActiveLearningGraphs.ItemSize = new System.Drawing.Size(113, 25);
            this.TabControlActiveLearningGraphs.Location = new System.Drawing.Point(0, 0);
            this.TabControlActiveLearningGraphs.Name = "TabControlActiveLearningGraphs";
            this.TabControlActiveLearningGraphs.PreserveTabColor = false;
            this.TabControlActiveLearningGraphs.SelectedIndex = 0;
            this.TabControlActiveLearningGraphs.ShowToolTips = true;
            this.TabControlActiveLearningGraphs.Size = new System.Drawing.Size(528, 536);
            this.TabControlActiveLearningGraphs.TabIndex = 15;
            this.TabControlActiveLearningGraphs.UseExtendedLayout = false;
            // 
            // tabPageAccuracyGraph
            // 
            this.tabPageAccuracyGraph.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageAccuracyGraph.Controls.Add(this.graphControlAccuracyGraph);
            this.tabPageAccuracyGraph.Controls.Add(this.btnActiveLearning_PopUpAccuracyGraph);
            this.tabPageAccuracyGraph.Controls.Add(this.button16);
            this.tabPageAccuracyGraph.Controls.Add(this.label7);
            this.tabPageAccuracyGraph.ImageIndex = 0;
            this.tabPageAccuracyGraph.Location = new System.Drawing.Point(4, 29);
            this.tabPageAccuracyGraph.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageAccuracyGraph.Name = "tabPageAccuracyGraph";
            this.tabPageAccuracyGraph.Size = new System.Drawing.Size(520, 503);
            this.tabPageAccuracyGraph.TabIndex = 0;
            this.tabPageAccuracyGraph.Tag = false;
            this.tabPageAccuracyGraph.Text = "Accuracy Graph";
            // 
            // graphControlAccuracyGraph
            // 
            this.graphControlAccuracyGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphControlAccuracyGraph.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.graphControlAccuracyGraph.BackColor = System.Drawing.Color.Transparent;
            this.graphControlAccuracyGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.graphControlAccuracyGraph.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.graphControlAccuracyGraph.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.graphControlAccuracyGraph.IsZoomOnMouseCenter = true;
            this.graphControlAccuracyGraph.Location = new System.Drawing.Point(0, 45);
            this.graphControlAccuracyGraph.Margin = new System.Windows.Forms.Padding(0);
            this.graphControlAccuracyGraph.Name = "graphControlAccuracyGraph";
            this.graphControlAccuracyGraph.ScrollGrace = 0D;
            this.graphControlAccuracyGraph.ScrollMaxX = 0D;
            this.graphControlAccuracyGraph.ScrollMaxY = 0D;
            this.graphControlAccuracyGraph.ScrollMaxY2 = 0D;
            this.graphControlAccuracyGraph.ScrollMinX = 0D;
            this.graphControlAccuracyGraph.ScrollMinY = 0D;
            this.graphControlAccuracyGraph.ScrollMinY2 = 0D;
            this.graphControlAccuracyGraph.Size = new System.Drawing.Size(520, 462);
            this.graphControlAccuracyGraph.TabIndex = 3;
            this.graphControlAccuracyGraph.Load += new System.EventHandler(this.graphControlAccuracyGraph_Load);
            // 
            // btnActiveLearning_PopUpAccuracyGraph
            // 
            this.btnActiveLearning_PopUpAccuracyGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActiveLearning_PopUpAccuracyGraph.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.arrow_out;
            this.btnActiveLearning_PopUpAccuracyGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnActiveLearning_PopUpAccuracyGraph.ImageKey = "(none)";
            this.btnActiveLearning_PopUpAccuracyGraph.ImageList = this.imageList1;
            this.btnActiveLearning_PopUpAccuracyGraph.Location = new System.Drawing.Point(441, 7);
            this.btnActiveLearning_PopUpAccuracyGraph.Name = "btnActiveLearning_PopUpAccuracyGraph";
            this.btnActiveLearning_PopUpAccuracyGraph.Size = new System.Drawing.Size(35, 35);
            this.btnActiveLearning_PopUpAccuracyGraph.TabIndex = 2;
            this.btnActiveLearning_PopUpAccuracyGraph.UseVisualStyleBackColor = true;
            this.btnActiveLearning_PopUpAccuracyGraph.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnActiveLearning_PopUpAccuracyGraph_Click);
            this.btnActiveLearning_PopUpAccuracyGraph.MouseHover += new System.EventHandler(this.btnActiveLearning_PopUpAccuracyGraph_MouseHover);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Home_icon.ico");
            this.imageList1.Images.SetKeyName(1, "Batch_icon.ico");
            this.imageList1.Images.SetKeyName(2, "ActiveLearning_Icon.ico");
            this.imageList1.Images.SetKeyName(3, "workers_icon2.ico");
            this.imageList1.Images.SetKeyName(4, "table_multiple.png");
            // 
            // button16
            // 
            this.button16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button16.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources._1408377987_gear_wheel;
            this.button16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button16.Location = new System.Drawing.Point(482, 7);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(35, 35);
            this.button16.TabIndex = 1;
            this.button16.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.DarkBlue;
            this.label7.Location = new System.Drawing.Point(19, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 19);
            this.label7.TabIndex = 4;
            this.label7.Text = "Accuracy Graph";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(-115, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 68);
            this.button1.TabIndex = 14;
            this.button1.Text = " Play / Pause\r\n";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(538, 18);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(76, 65);
            this.button7.TabIndex = 12;
            this.button7.Text = "Current Settings";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(206)))), ((int)(((byte)(230)))));
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.comboBoxShowCurrentModels);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControlForValues);
            this.splitContainer2.Size = new System.Drawing.Size(552, 536);
            this.splitContainer2.SplitterDistance = 34;
            this.splitContainer2.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 15);
            this.label6.TabIndex = 13;
            this.label6.Text = "Current Model";
            // 
            // comboBoxShowCurrentModels
            // 
            this.comboBoxShowCurrentModels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxShowCurrentModels.FormattingEnabled = true;
            this.comboBoxShowCurrentModels.Location = new System.Drawing.Point(113, 6);
            this.comboBoxShowCurrentModels.Name = "comboBoxShowCurrentModels";
            this.comboBoxShowCurrentModels.Size = new System.Drawing.Size(308, 23);
            this.comboBoxShowCurrentModels.TabIndex = 12;
            this.comboBoxShowCurrentModels.SelectedIndexChanged += new System.EventHandler(this.comboBoxShowWorkersValues_SelectedIndexChanged);
            // 
            // tabControlForValues
            // 
            this.tabControlForValues.AllowCloseButton = false;
            this.tabControlForValues.AllowContextButton = true;
            this.tabControlForValues.AllowNavigatorButtons = false;
            this.tabControlForValues.AllowSelectedTabHigh = false;
            this.tabControlForValues.BorderWidth = 1;
            this.tabControlForValues.Controls.Add(this.tabPageInnerViewSchedule);
            this.tabControlForValues.Controls.Add(this.tabPageInnerViewWorkers);
            this.tabControlForValues.Controls.Add(this.tabPageInnerViewDataset);
            this.tabControlForValues.Controls.Add(this.tabPageInnerViewCommunity);
            this.tabControlForValues.CornerRoundRadiusWidth = 12;
            this.tabControlForValues.CornerSymmetry = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornSymmetry.Both;
            this.tabControlForValues.CornerType = AC.ExtendedRenderer.Toolkit.Drawing.DrawingMethods.CornerType.Rounded;
            this.tabControlForValues.CornerWidth = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornWidth.Thin;
            this.tabControlForValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlForValues.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlForValues.HotTrack = true;
            this.tabControlForValues.ImageList = this.imageList3;
            this.tabControlForValues.ItemSize = new System.Drawing.Size(130, 25);
            this.tabControlForValues.Location = new System.Drawing.Point(0, 0);
            this.tabControlForValues.Name = "tabControlForValues";
            this.tabControlForValues.PreserveTabColor = false;
            this.tabControlForValues.SelectedIndex = 0;
            this.tabControlForValues.Size = new System.Drawing.Size(552, 498);
            this.tabControlForValues.TabIndex = 2;
            this.tabControlForValues.UseExtendedLayout = false;
            // 
            // tabPageInnerViewSchedule
            // 
            this.tabPageInnerViewSchedule.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageInnerViewSchedule.Controls.Add(this.dataGridViewActiveLearningSchedule);
            this.tabPageInnerViewSchedule.ImageIndex = 0;
            this.tabPageInnerViewSchedule.Location = new System.Drawing.Point(4, 29);
            this.tabPageInnerViewSchedule.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageInnerViewSchedule.Name = "tabPageInnerViewSchedule";
            this.tabPageInnerViewSchedule.Size = new System.Drawing.Size(544, 465);
            this.tabPageInnerViewSchedule.TabIndex = 1;
            this.tabPageInnerViewSchedule.Tag = false;
            this.tabPageInnerViewSchedule.Text = "Schedule";
            // 
            // dataGridViewActiveLearningSchedule
            // 
            this.dataGridViewActiveLearningSchedule.AllowUserToAddRows = false;
            this.dataGridViewActiveLearningSchedule.AllowUserToDeleteRows = false;
            this.dataGridViewActiveLearningSchedule.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewActiveLearningSchedule.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridViewActiveLearningSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewActiveLearningSchedule.HideOuterBorders = true;
            this.dataGridViewActiveLearningSchedule.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewActiveLearningSchedule.Name = "dataGridViewActiveLearningSchedule";
            this.dataGridViewActiveLearningSchedule.RowTemplate.ReadOnly = true;
            this.dataGridViewActiveLearningSchedule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewActiveLearningSchedule.ShowEditingIcon = false;
            this.dataGridViewActiveLearningSchedule.Size = new System.Drawing.Size(544, 465);
            this.dataGridViewActiveLearningSchedule.TabIndex = 1;
            this.dataGridViewActiveLearningSchedule.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewActiveLearningWorkers1_RowPostPaint);
            this.dataGridViewActiveLearningSchedule.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewActiveLearningWorkers1_MouseDoubleClick);
            // 
            // tabPageInnerViewWorkers
            // 
            this.tabPageInnerViewWorkers.Controls.Add(this.dataGridViewForInnerWorker);
            this.tabPageInnerViewWorkers.ImageIndex = 1;
            this.tabPageInnerViewWorkers.Location = new System.Drawing.Point(4, 29);
            this.tabPageInnerViewWorkers.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageInnerViewWorkers.Name = "tabPageInnerViewWorkers";
            this.tabPageInnerViewWorkers.Size = new System.Drawing.Size(544, 465);
            this.tabPageInnerViewWorkers.TabIndex = 3;
            this.tabPageInnerViewWorkers.Tag = false;
            this.tabPageInnerViewWorkers.Text = "Workers";
            // 
            // dataGridViewForInnerWorker
            // 
            this.dataGridViewForInnerWorker.AllowUserToAddRows = false;
            this.dataGridViewForInnerWorker.AllowUserToDeleteRows = false;
            this.dataGridViewForInnerWorker.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewForInnerWorker.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridViewForInnerWorker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewForInnerWorker.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewForInnerWorker.Name = "dataGridViewForInnerWorker";
            this.dataGridViewForInnerWorker.ReadOnly = true;
            this.dataGridViewForInnerWorker.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewForInnerWorker.Size = new System.Drawing.Size(544, 465);
            this.dataGridViewForInnerWorker.TabIndex = 0;
            this.dataGridViewForInnerWorker.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewActiveLearningWorkers1_RowPostPaint);
            this.dataGridViewForInnerWorker.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewForInnerWorker_MouseDoubleClick);
            // 
            // tabPageInnerViewDataset
            // 
            this.tabPageInnerViewDataset.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageInnerViewDataset.Controls.Add(this.dataGridViewForTaskValue);
            this.tabPageInnerViewDataset.ImageIndex = 2;
            this.tabPageInnerViewDataset.Location = new System.Drawing.Point(4, 29);
            this.tabPageInnerViewDataset.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageInnerViewDataset.Name = "tabPageInnerViewDataset";
            this.tabPageInnerViewDataset.Size = new System.Drawing.Size(544, 465);
            this.tabPageInnerViewDataset.TabIndex = 2;
            this.tabPageInnerViewDataset.Tag = false;
            this.tabPageInnerViewDataset.Text = "Tasks";
            // 
            // dataGridViewForTaskValue
            // 
            this.dataGridViewForTaskValue.AllowUserToAddRows = false;
            this.dataGridViewForTaskValue.AllowUserToDeleteRows = false;
            this.dataGridViewForTaskValue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewForTaskValue.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridViewForTaskValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewForTaskValue.HideOuterBorders = true;
            this.dataGridViewForTaskValue.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewForTaskValue.Name = "dataGridViewForTaskValue";
            this.dataGridViewForTaskValue.RowTemplate.ReadOnly = true;
            this.dataGridViewForTaskValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewForTaskValue.ShowEditingIcon = false;
            this.dataGridViewForTaskValue.Size = new System.Drawing.Size(544, 465);
            this.dataGridViewForTaskValue.TabIndex = 2;
            this.dataGridViewForTaskValue.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewActiveLearningWorkers1_RowPostPaint);
            this.dataGridViewForTaskValue.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tabControlForValues_MouseDoubleClick);
            // 
            // tabPageInnerViewCommunity
            // 
            this.tabPageInnerViewCommunity.Controls.Add(this.dataGridViewInnerCommunity);
            this.tabPageInnerViewCommunity.ImageIndex = 3;
            this.tabPageInnerViewCommunity.Location = new System.Drawing.Point(4, 29);
            this.tabPageInnerViewCommunity.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageInnerViewCommunity.Name = "tabPageInnerViewCommunity";
            this.tabPageInnerViewCommunity.Size = new System.Drawing.Size(544, 465);
            this.tabPageInnerViewCommunity.TabIndex = 4;
            this.tabPageInnerViewCommunity.Tag = false;
            this.tabPageInnerViewCommunity.Text = "Communities";
            this.tabPageInnerViewCommunity.UseVisualStyleBackColor = true;
            // 
            // dataGridViewInnerCommunity
            // 
            this.dataGridViewInnerCommunity.AllowUserToAddRows = false;
            this.dataGridViewInnerCommunity.AllowUserToDeleteRows = false;
            this.dataGridViewInnerCommunity.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewInnerCommunity.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridViewInnerCommunity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewInnerCommunity.HideOuterBorders = true;
            this.dataGridViewInnerCommunity.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewInnerCommunity.Name = "dataGridViewInnerCommunity";
            this.dataGridViewInnerCommunity.RowTemplate.ReadOnly = true;
            this.dataGridViewInnerCommunity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewInnerCommunity.ShowEditingIcon = false;
            this.dataGridViewInnerCommunity.Size = new System.Drawing.Size(544, 465);
            this.dataGridViewInnerCommunity.TabIndex = 2;
            this.dataGridViewInnerCommunity.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewActiveLearningWorkers1_RowPostPaint);
            this.dataGridViewInnerCommunity.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewInnerCommunity_MouseDoubleClick);
            // 
            // tabPageForViewWorkers
            // 
            this.tabPageForViewWorkers.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageForViewWorkers.Controls.Add(this.splitContainerForWorkers);
            this.tabPageForViewWorkers.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabPageForViewWorkers.ImageIndex = 3;
            this.tabPageForViewWorkers.Location = new System.Drawing.Point(4, 34);
            this.tabPageForViewWorkers.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageForViewWorkers.Name = "tabPageForViewWorkers";
            this.tabPageForViewWorkers.Size = new System.Drawing.Size(1084, 621);
            this.tabPageForViewWorkers.TabIndex = 2;
            this.tabPageForViewWorkers.Tag = false;
            this.tabPageForViewWorkers.Text = "View Workers";
            // 
            // splitContainerForWorkers
            // 
            this.splitContainerForWorkers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerForWorkers.Location = new System.Drawing.Point(0, 0);
            this.splitContainerForWorkers.Name = "splitContainerForWorkers";
            this.splitContainerForWorkers.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerForWorkers.Panel1
            // 
            this.splitContainerForWorkers.Panel1.Controls.Add(this.button12);
            this.splitContainerForWorkers.Panel1.Controls.Add(this.button11);
            // 
            // splitContainerForWorkers.Panel2
            // 
            this.splitContainerForWorkers.Panel2.Controls.Add(this.splitContainerWokerDetails);
            this.splitContainerForWorkers.Size = new System.Drawing.Size(1084, 621);
            this.splitContainerForWorkers.SplitterDistance = 106;
            this.splitContainerForWorkers.TabIndex = 0;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(941, 17);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 1;
            this.button12.Text = "button12";
            this.button12.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(860, 17);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 0;
            this.button11.Text = "button11";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // splitContainerWokerDetails
            // 
            this.splitContainerWokerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerWokerDetails.Location = new System.Drawing.Point(0, 0);
            this.splitContainerWokerDetails.Name = "splitContainerWokerDetails";
            // 
            // splitContainerWokerDetails.Panel1
            // 
            this.splitContainerWokerDetails.Panel1.Controls.Add(this.kryptonListView1);
            // 
            // splitContainerWokerDetails.Panel2
            // 
            this.splitContainerWokerDetails.Panel2.Controls.Add(this.pictureBox2);
            this.splitContainerWokerDetails.Panel2.Controls.Add(this.kryptonListViewForWorkers);
            this.splitContainerWokerDetails.Panel2.Controls.Add(this.label4);
            this.splitContainerWokerDetails.Panel2.Controls.Add(this.label1);
            this.splitContainerWokerDetails.Size = new System.Drawing.Size(1084, 511);
            this.splitContainerWokerDetails.SplitterDistance = 479;
            this.splitContainerWokerDetails.TabIndex = 0;
            // 
            // kryptonListView1
            // 
            this.kryptonListView1.AlternateRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(230)))), ((int)(((byte)(232)))));
            this.kryptonListView1.AlternateRowColorEnabled = true;
            this.kryptonListView1.AutoSizeLastColumn = true;
            this.kryptonListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.kryptonListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonListView1.EnableDragDrop = false;
            this.kryptonListView1.EnableHeaderGlow = false;
            this.kryptonListView1.EnableHeaderHotTrack = false;
            this.kryptonListView1.EnableHeaderRendering = true;
            this.kryptonListView1.EnableSelectionBorder = false;
            this.kryptonListView1.EnableSorting = true;
            this.kryptonListView1.EnableVistaCheckBoxes = true;
            this.kryptonListView1.ForceLeftAlign = false;
            this.kryptonListView1.FullRowSelect = true;
            this.kryptonListView1.ItemHeight = 0;
            this.kryptonListView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.kryptonListView1.LineAfter = -1;
            this.kryptonListView1.LineBefore = -1;
            this.kryptonListView1.Location = new System.Drawing.Point(0, 0);
            this.kryptonListView1.Name = "kryptonListView1";
            this.kryptonListView1.OwnerDraw = true;
            this.kryptonListView1.PersistentColors = false;
            this.kryptonListView1.SelectEntireRowOnSubItem = true;
            this.kryptonListView1.Size = new System.Drawing.Size(479, 511);
            this.kryptonListView1.TabIndex = 0;
            this.kryptonListView1.UseCompatibleStateImageBehavior = false;
            this.kryptonListView1.UseKryptonRenderer = true;
            this.kryptonListView1.UseStyledColors = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox2.Image = global::AcriveCrowdGUI.Properties.Resources.sample_worker_graph;
            this.pictureBox2.Location = new System.Drawing.Point(21, 254);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(555, 237);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // kryptonListViewForWorkers
            // 
            this.kryptonListViewForWorkers.AlternateRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(230)))), ((int)(((byte)(232)))));
            this.kryptonListViewForWorkers.AlternateRowColorEnabled = true;
            this.kryptonListViewForWorkers.AutoSizeLastColumn = true;
            this.kryptonListViewForWorkers.EnableDragDrop = false;
            this.kryptonListViewForWorkers.EnableHeaderGlow = false;
            this.kryptonListViewForWorkers.EnableHeaderHotTrack = false;
            this.kryptonListViewForWorkers.EnableHeaderRendering = true;
            this.kryptonListViewForWorkers.EnableSelectionBorder = false;
            this.kryptonListViewForWorkers.EnableSorting = true;
            this.kryptonListViewForWorkers.EnableVistaCheckBoxes = true;
            this.kryptonListViewForWorkers.ForceLeftAlign = false;
            this.kryptonListViewForWorkers.FullRowSelect = true;
            this.kryptonListViewForWorkers.ItemHeight = 0;
            this.kryptonListViewForWorkers.LineAfter = -1;
            this.kryptonListViewForWorkers.LineBefore = -1;
            this.kryptonListViewForWorkers.Location = new System.Drawing.Point(2, 124);
            this.kryptonListViewForWorkers.Name = "kryptonListViewForWorkers";
            this.kryptonListViewForWorkers.OwnerDraw = true;
            this.kryptonListViewForWorkers.PersistentColors = false;
            this.kryptonListViewForWorkers.SelectEntireRowOnSubItem = true;
            this.kryptonListViewForWorkers.Size = new System.Drawing.Size(561, 105);
            this.kryptonListViewForWorkers.TabIndex = 2;
            this.kryptonListViewForWorkers.UseCompatibleStateImageBehavior = false;
            this.kryptonListViewForWorkers.UseKryptonRenderer = true;
            this.kryptonListViewForWorkers.UseStyledColors = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Worker ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Worker ID";
            // 
            // tabPageViewDataset
            // 
            this.tabPageViewDataset.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageViewDataset.Controls.Add(this.splitContainerViewDataset);
            this.tabPageViewDataset.ImageIndex = 4;
            this.tabPageViewDataset.Location = new System.Drawing.Point(4, 34);
            this.tabPageViewDataset.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageViewDataset.Name = "tabPageViewDataset";
            this.tabPageViewDataset.Size = new System.Drawing.Size(1084, 621);
            this.tabPageViewDataset.TabIndex = 3;
            this.tabPageViewDataset.Tag = false;
            this.tabPageViewDataset.Text = "View Dataset";
            // 
            // splitContainerViewDataset
            // 
            this.splitContainerViewDataset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerViewDataset.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerViewDataset.Location = new System.Drawing.Point(0, 0);
            this.splitContainerViewDataset.Name = "splitContainerViewDataset";
            this.splitContainerViewDataset.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerViewDataset.Panel1
            // 
            this.splitContainerViewDataset.Panel1.Controls.Add(this.buttonLoadDataset);
            this.splitContainerViewDataset.Panel1.Controls.Add(this.label8);
            this.splitContainerViewDataset.Panel1.Controls.Add(this.comboBoxForSelectingDataset);
            // 
            // splitContainerViewDataset.Panel2
            // 
            this.splitContainerViewDataset.Panel2.Controls.Add(this.dataGridViewOfDataset);
            this.splitContainerViewDataset.Size = new System.Drawing.Size(1084, 621);
            this.splitContainerViewDataset.SplitterDistance = 58;
            this.splitContainerViewDataset.TabIndex = 2;
            // 
            // buttonLoadDataset
            // 
            this.buttonLoadDataset.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.application_add_icon;
            this.buttonLoadDataset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonLoadDataset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoadDataset.Location = new System.Drawing.Point(508, 9);
            this.buttonLoadDataset.Name = "buttonLoadDataset";
            this.buttonLoadDataset.Size = new System.Drawing.Size(40, 40);
            this.buttonLoadDataset.TabIndex = 4;
            this.buttonLoadDataset.UseVisualStyleBackColor = true;
            this.buttonLoadDataset.Click += new System.EventHandler(this.buttonLoadDataset_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(8, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 19);
            this.label8.TabIndex = 3;
            this.label8.Text = "Current Dataset";
            // 
            // comboBoxForSelectingDataset
            // 
            this.comboBoxForSelectingDataset.DisableBorderColor = false;
            this.comboBoxForSelectingDataset.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxForSelectingDataset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxForSelectingDataset.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxForSelectingDataset.FormattingEnabled = true;
            this.comboBoxForSelectingDataset.ItemHeight = 30;
            this.comboBoxForSelectingDataset.Location = new System.Drawing.Point(132, 12);
            this.comboBoxForSelectingDataset.Name = "comboBoxForSelectingDataset";
            this.comboBoxForSelectingDataset.PersistentColors = false;
            this.comboBoxForSelectingDataset.Size = new System.Drawing.Size(370, 36);
            this.comboBoxForSelectingDataset.TabIndex = 2;
            this.comboBoxForSelectingDataset.UseStyledColors = false;
            this.comboBoxForSelectingDataset.SelectedIndexChanged += new System.EventHandler(this.comboBoxForSelectingDataset_SelectedIndexChanged);
            // 
            // dataGridViewOfDataset
            // 
            this.dataGridViewOfDataset.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.dataGridViewOfDataset.AllowUserToAddRows = false;
            this.dataGridViewOfDataset.AllowUserToDeleteRows = false;
            this.dataGridViewOfDataset.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewOfDataset.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewOfDataset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOfDataset.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewOfDataset.Name = "dataGridViewOfDataset";
            this.dataGridViewOfDataset.ReadOnly = true;
            this.dataGridViewOfDataset.Size = new System.Drawing.Size(1084, 559);
            this.dataGridViewOfDataset.TabIndex = 1;
            this.dataGridViewOfDataset.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewOfDataset_RowPostPaint);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fILEToolStripMenuItem,
            this.hELPToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(1, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1092, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fILEToolStripMenuItem
            // 
            this.fILEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDatasetToolStripMenuItem});
            this.fILEToolStripMenuItem.Name = "fILEToolStripMenuItem";
            this.fILEToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.fILEToolStripMenuItem.Text = "&FILE";
            // 
            // loadDatasetToolStripMenuItem
            // 
            this.loadDatasetToolStripMenuItem.Name = "loadDatasetToolStripMenuItem";
            this.loadDatasetToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.loadDatasetToolStripMenuItem.Text = "Load Dataset";
            this.loadDatasetToolStripMenuItem.Click += new System.EventHandler(this.loadDatasetToolStripMenuItem_Click);
            // 
            // hELPToolStripMenuItem
            // 
            this.hELPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.hELPToolStripMenuItem.Name = "hELPToolStripMenuItem";
            this.hELPToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.hELPToolStripMenuItem.Text = "&HELP";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.AutoSize = true;
            this.kryptonLabel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kryptonLabel1.Location = new System.Drawing.Point(0, 670);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(127, 13);
            this.kryptonLabel1.TabIndex = 4;
            this.kryptonLabel1.Text = "The Active Crowd Toolkit";
            this.kryptonLabel1.UseAlternateForeColor = false;
            this.kryptonLabel1.UseKryptonFont = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // kryptonLinkLabel1
            // 
            this.kryptonLinkLabel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonLinkLabel1.Name = "kryptonLinkLabel1";
            this.kryptonLinkLabel1.TabIndex = 0;
            // 
            // kryptonListBox1
            // 
            this.kryptonListBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonListBox1.Name = "kryptonListBox1";
            this.kryptonListBox1.TabIndex = 0;
            // 
            // kryptonListBox2
            // 
            this.kryptonListBox2.Location = new System.Drawing.Point(347, 17);
            this.kryptonListBox2.Name = "kryptonListBox2";
            this.kryptonListBox2.TabIndex = 0;
            // 
            // kryptonImageComboBox1
            // 
            this.kryptonImageComboBox1.DropDownWidth = 121;
            this.kryptonImageComboBox1.Location = new System.Drawing.Point(277, 24);
            this.kryptonImageComboBox1.Name = "kryptonImageComboBox1";
            this.kryptonImageComboBox1.TabIndex = 0;
            // 
            // kryptonTrackBar1
            // 
            this.kryptonTrackBar1.DrawBackground = true;
            this.kryptonTrackBar1.Location = new System.Drawing.Point(0, 0);
            this.kryptonTrackBar1.Name = "kryptonTrackBar1";
            this.kryptonTrackBar1.Size = new System.Drawing.Size(150, 27);
            this.kryptonTrackBar1.TabIndex = 0;
            // 
            // backgroundWorkerForSimulation
            // 
            this.backgroundWorkerForSimulation.WorkerReportsProgress = true;
            this.backgroundWorkerForSimulation.WorkerSupportsCancellation = true;
            this.backgroundWorkerForSimulation.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerForSimulation_DoWork);
            this.backgroundWorkerForSimulation.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerForSimulation_RunWorkerCompleted);
            // 
            // backgroundWorkerForProgressBar
            // 
            this.backgroundWorkerForProgressBar.WorkerSupportsCancellation = true;
            // 
            // backgroundWorkerForAccuracyGraph
            // 
            this.backgroundWorkerForAccuracyGraph.WorkerReportsProgress = true;
            this.backgroundWorkerForAccuracyGraph.WorkerSupportsCancellation = true;
            this.backgroundWorkerForAccuracyGraph.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerForAccuracyGraph_DoWork);
            this.backgroundWorkerForAccuracyGraph.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerForAccuracyGraph_ProgressChanged);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(206)))), ((int)(((byte)(230)))));
            this.splitContainer5.Size = new System.Drawing.Size(513, 404);
            this.splitContainer5.SplitterDistance = 38;
            this.splitContainer5.TabIndex = 0;
            // 
            // splitContainerInnterBatchRunning
            // 
            this.splitContainerInnterBatchRunning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerInnterBatchRunning.Location = new System.Drawing.Point(0, 0);
            this.splitContainerInnterBatchRunning.Name = "splitContainerInnterBatchRunning";
            this.splitContainerInnterBatchRunning.Size = new System.Drawing.Size(540, 479);
            this.splitContainerInnterBatchRunning.SplitterDistance = 180;
            this.splitContainerInnterBatchRunning.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.DarkBlue;
            this.label10.Location = new System.Drawing.Point(25, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 19);
            this.label10.TabIndex = 5;
            this.label10.Text = "Progress ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(87, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 15);
            this.label9.TabIndex = 39;
            this.label9.Text = "Current Dataset:";
            // 
            // kryptonProgressBar1
            // 
            this.kryptonProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.kryptonProgressBar1.DisplayText = "300/1720";
            this.kryptonProgressBar1.EndColor = System.Drawing.Color.ForestGreen;
            this.kryptonProgressBar1.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kryptonProgressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.kryptonProgressBar1.Location = new System.Drawing.Point(170, 21);
            this.kryptonProgressBar1.MaxValue = 1420;
            this.kryptonProgressBar1.Name = "kryptonProgressBar1";
            this.kryptonProgressBar1.Size = new System.Drawing.Size(740, 35);
            this.kryptonProgressBar1.StartColor = System.Drawing.Color.LawnGreen;
            this.kryptonProgressBar1.TabIndex = 38;
            this.kryptonProgressBar1.Value = 300;
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.application_double;
            this.button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button6.Location = new System.Drawing.Point(957, 21);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(35, 35);
            this.button6.TabIndex = 37;
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            this.button13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button13.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.application_delete;
            this.button13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button13.Location = new System.Drawing.Point(916, 21);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(35, 35);
            this.button13.TabIndex = 36;
            this.button13.UseVisualStyleBackColor = true;
            // 
            // button19
            // 
            this.button19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button19.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button19.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.information;
            this.button19.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button19.Location = new System.Drawing.Point(998, 21);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(35, 35);
            this.button19.TabIndex = 35;
            this.button19.UseVisualStyleBackColor = true;
            // 
            // button20
            // 
            this.button20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button20.BackColor = System.Drawing.Color.Transparent;
            this.button20.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources._1408377678_gear_wheel;
            this.button20.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button20.ForeColor = System.Drawing.Color.Black;
            this.button20.Location = new System.Drawing.Point(1039, 21);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(35, 35);
            this.button20.TabIndex = 34;
            this.button20.UseVisualStyleBackColor = false;
            // 
            // button21
            // 
            this.button21.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.arrow_redo;
            this.button21.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button21.Location = new System.Drawing.Point(129, 21);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(35, 35);
            this.button21.TabIndex = 33;
            this.button21.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(187, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "CF";
            // 
            // button22
            // 
            this.button22.BackColor = System.Drawing.SystemColors.Menu;
            this.button22.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Pause_Hot_icon;
            this.button22.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button22.FlatAppearance.BorderSize = 0;
            this.button22.Location = new System.Drawing.Point(10, 21);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(72, 60);
            this.button22.TabIndex = 31;
            this.button22.UseVisualStyleBackColor = false;
            // 
            // button23
            // 
            this.button23.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.Stop_icon;
            this.button23.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button23.FlatAppearance.BorderSize = 0;
            this.button23.ForeColor = System.Drawing.Color.Transparent;
            this.button23.Location = new System.Drawing.Point(88, 21);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(35, 35);
            this.button23.TabIndex = 30;
            this.button23.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.kryptonProgressBar1);
            this.splitContainer1.Panel1.Controls.Add(this.button6);
            this.splitContainer1.Panel1.Controls.Add(this.button13);
            this.splitContainer1.Panel1.Controls.Add(this.button19);
            this.splitContainer1.Panel1.Controls.Add(this.button20);
            this.splitContainer1.Panel1.Controls.Add(this.button21);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.button22);
            this.splitContainer1.Panel1.Controls.Add(this.button23);
            this.splitContainer1.Panel1.Controls.Add(this.kryptonDataGridView5);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.label12);
            this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainerInnterBatchRunning);
            this.splitContainer1.Panel2.Controls.Add(this.kryptonTabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(540, 526);
            this.splitContainer1.SplitterDistance = 43;
            this.splitContainer1.TabIndex = 0;
            // 
            // kryptonDataGridView5
            // 
            this.kryptonDataGridView5.AllowUserToAddRows = false;
            this.kryptonDataGridView5.AllowUserToDeleteRows = false;
            this.kryptonDataGridView5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonDataGridView5.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.kryptonDataGridView5.ContextMenuStrip = this.contextMenuStrip2;
            this.kryptonDataGridView5.HideOuterBorders = true;
            this.kryptonDataGridView5.Location = new System.Drawing.Point(29, 47);
            this.kryptonDataGridView5.Name = "kryptonDataGridView5";
            this.kryptonDataGridView5.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Silver;
            this.kryptonDataGridView5.RowTemplate.ReadOnly = true;
            this.kryptonDataGridView5.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonDataGridView5.ShowEditingIcon = false;
            this.kryptonDataGridView5.Size = new System.Drawing.Size(476, 391);
            this.kryptonDataGridView5.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(15, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 15);
            this.label12.TabIndex = 15;
            this.label12.Text = "Current Model";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(114, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(308, 21);
            this.comboBox1.TabIndex = 14;
            // 
            // kryptonTabControl1
            // 
            this.kryptonTabControl1.AllowCloseButton = false;
            this.kryptonTabControl1.AllowContextButton = true;
            this.kryptonTabControl1.AllowNavigatorButtons = false;
            this.kryptonTabControl1.AllowSelectedTabHigh = false;
            this.kryptonTabControl1.BorderWidth = 1;
            this.kryptonTabControl1.Controls.Add(this.tabPage1);
            this.kryptonTabControl1.Controls.Add(this.tabPage2);
            this.kryptonTabControl1.Controls.Add(this.tabPage3);
            this.kryptonTabControl1.Controls.Add(this.tabPage4);
            this.kryptonTabControl1.CornerRoundRadiusWidth = 12;
            this.kryptonTabControl1.CornerSymmetry = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornSymmetry.Both;
            this.kryptonTabControl1.CornerType = AC.ExtendedRenderer.Toolkit.Drawing.DrawingMethods.CornerType.Rounded;
            this.kryptonTabControl1.CornerWidth = AC.ExtendedRenderer.Navigator.KryptonTabControl.CornWidth.Thin;
            this.kryptonTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.kryptonTabControl1.HotTrack = true;
            this.kryptonTabControl1.ImageList = this.imageList3;
            this.kryptonTabControl1.ItemSize = new System.Drawing.Size(130, 25);
            this.kryptonTabControl1.Location = new System.Drawing.Point(0, 0);
            this.kryptonTabControl1.Name = "kryptonTabControl1";
            this.kryptonTabControl1.PreserveTabColor = false;
            this.kryptonTabControl1.SelectedIndex = 0;
            this.kryptonTabControl1.Size = new System.Drawing.Size(540, 479);
            this.kryptonTabControl1.TabIndex = 3;
            this.kryptonTabControl1.UseExtendedLayout = false;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage1.Controls.Add(this.kryptonDataGridView1);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(532, 446);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Tag = false;
            this.tabPage1.Text = "Schedule";
            // 
            // kryptonDataGridView1
            // 
            this.kryptonDataGridView1.AllowUserToAddRows = false;
            this.kryptonDataGridView1.AllowUserToDeleteRows = false;
            this.kryptonDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.kryptonDataGridView1.ContextMenuStrip = this.contextMenuStrip2;
            this.kryptonDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonDataGridView1.HideOuterBorders = true;
            this.kryptonDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.kryptonDataGridView1.Name = "kryptonDataGridView1";
            this.kryptonDataGridView1.RowTemplate.ReadOnly = true;
            this.kryptonDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonDataGridView1.ShowEditingIcon = false;
            this.kryptonDataGridView1.Size = new System.Drawing.Size(532, 446);
            this.kryptonDataGridView1.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.kryptonDataGridView2);
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(532, 446);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Tag = false;
            this.tabPage2.Text = "Workers";
            // 
            // kryptonDataGridView2
            // 
            this.kryptonDataGridView2.AllowUserToAddRows = false;
            this.kryptonDataGridView2.AllowUserToDeleteRows = false;
            this.kryptonDataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.kryptonDataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.kryptonDataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonDataGridView2.Location = new System.Drawing.Point(0, 0);
            this.kryptonDataGridView2.Name = "kryptonDataGridView2";
            this.kryptonDataGridView2.ReadOnly = true;
            this.kryptonDataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonDataGridView2.Size = new System.Drawing.Size(532, 446);
            this.kryptonDataGridView2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage3.Controls.Add(this.kryptonDataGridView3);
            this.tabPage3.ImageIndex = 2;
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(532, 446);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Tag = false;
            this.tabPage3.Text = "Tasks";
            // 
            // kryptonDataGridView3
            // 
            this.kryptonDataGridView3.AllowUserToAddRows = false;
            this.kryptonDataGridView3.AllowUserToDeleteRows = false;
            this.kryptonDataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.kryptonDataGridView3.ContextMenuStrip = this.contextMenuStrip2;
            this.kryptonDataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonDataGridView3.HideOuterBorders = true;
            this.kryptonDataGridView3.Location = new System.Drawing.Point(0, 0);
            this.kryptonDataGridView3.Name = "kryptonDataGridView3";
            this.kryptonDataGridView3.RowTemplate.ReadOnly = true;
            this.kryptonDataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonDataGridView3.ShowEditingIcon = false;
            this.kryptonDataGridView3.Size = new System.Drawing.Size(532, 446);
            this.kryptonDataGridView3.TabIndex = 2;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.kryptonDataGridView4);
            this.tabPage4.ImageIndex = 3;
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(532, 446);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Tag = false;
            this.tabPage4.Text = "Communities";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // kryptonDataGridView4
            // 
            this.kryptonDataGridView4.AllowUserToAddRows = false;
            this.kryptonDataGridView4.AllowUserToDeleteRows = false;
            this.kryptonDataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.kryptonDataGridView4.ContextMenuStrip = this.contextMenuStrip2;
            this.kryptonDataGridView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonDataGridView4.HideOuterBorders = true;
            this.kryptonDataGridView4.Location = new System.Drawing.Point(0, 0);
            this.kryptonDataGridView4.Name = "kryptonDataGridView4";
            this.kryptonDataGridView4.RowTemplate.ReadOnly = true;
            this.kryptonDataGridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonDataGridView4.ShowEditingIcon = false;
            this.kryptonDataGridView4.Size = new System.Drawing.Size(532, 446);
            this.kryptonDataGridView4.TabIndex = 2;
            // 
            // backgroundWorkerBatchRunning
            // 
            this.backgroundWorkerBatchRunning.WorkerReportsProgress = true;
            this.backgroundWorkerBatchRunning.WorkerSupportsCancellation = true;
            this.backgroundWorkerBatchRunning.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerBatchRunning_DoWork);
            this.backgroundWorkerBatchRunning.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerBatchRunning_ProgressChanged);
            // 
            // backgroundWorkerBatchRunningValues
            // 
            this.backgroundWorkerBatchRunningValues.WorkerReportsProgress = true;
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(206)))), ((int)(((byte)(230)))));
            this.ClientSize = new System.Drawing.Size(1092, 683);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.tabControlForMainPage);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainPage";
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "The Active Crowd Toolkit";
            this.Load += new System.EventHandler(this.MainPage_Load);
            this.tabControlForMainPage.ResumeLayout(false);
            this.tabPageMainPage.ResumeLayout(false);
            this.tabPageMainPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabPageRunBatchRunning.ResumeLayout(false);
            this.splitContainerOuterBatchLearning.Panel1.ResumeLayout(false);
            this.splitContainerOuterBatchLearning.Panel1.PerformLayout();
            this.splitContainerOuterBatchLearning.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOuterBatchLearning)).EndInit();
            this.splitContainerOuterBatchLearning.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.kryptonTabControl3.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.tabPage11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchRunningProgress)).EndInit();
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchRunningAccuracy)).EndInit();
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.tabControlBatchLearningInnerValues.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchRunningWorkers)).EndInit();
            this.tabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchRunningTasks)).EndInit();
            this.tabPageBatchLearningInnerViewCommunity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBatchRunningCommunities)).EndInit();
            this.tabPageRunActiveLearning.ResumeLayout(false);
            this.splitContainerActiveLearningOuterContainer.Panel1.ResumeLayout(false);
            this.splitContainerActiveLearningOuterContainer.Panel1.PerformLayout();
            this.splitContainerActiveLearningOuterContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerActiveLearningOuterContainer)).EndInit();
            this.splitContainerActiveLearningOuterContainer.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.TabControlActiveLearningGraphs.ResumeLayout(false);
            this.tabPageAccuracyGraph.ResumeLayout(false);
            this.tabPageAccuracyGraph.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControlForValues.ResumeLayout(false);
            this.tabPageInnerViewSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewActiveLearningSchedule)).EndInit();
            this.tabPageInnerViewWorkers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForInnerWorker)).EndInit();
            this.tabPageInnerViewDataset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForTaskValue)).EndInit();
            this.tabPageInnerViewCommunity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInnerCommunity)).EndInit();
            this.tabPageForViewWorkers.ResumeLayout(false);
            this.splitContainerForWorkers.Panel1.ResumeLayout(false);
            this.splitContainerForWorkers.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerForWorkers)).EndInit();
            this.splitContainerForWorkers.ResumeLayout(false);
            this.splitContainerWokerDetails.Panel1.ResumeLayout(false);
            this.splitContainerWokerDetails.Panel2.ResumeLayout(false);
            this.splitContainerWokerDetails.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWokerDetails)).EndInit();
            this.splitContainerWokerDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabPageViewDataset.ResumeLayout(false);
            this.splitContainerViewDataset.Panel1.ResumeLayout(false);
            this.splitContainerViewDataset.Panel1.PerformLayout();
            this.splitContainerViewDataset.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerViewDataset)).EndInit();
            this.splitContainerViewDataset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOfDataset)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonImageComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInnterBatchRunning)).EndInit();
            this.splitContainerInnterBatchRunning.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView5)).EndInit();
            this.kryptonTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView3)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainPageBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AC.ExtendedRenderer.Navigator.KryptonTabControl tabControlForMainPage;
        private System.Windows.Forms.TabPage tabPageMainPage;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hELPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageForViewWorkers;
        private AC.ExtendedRenderer.Toolkit.KryptonLabel kryptonLabel1;
        private System.Windows.Forms.ToolStripMenuItem loadDatasetToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabPage tabPageViewDataset;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dataGridViewOfDataset;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.SplitContainer splitContainerViewDataset;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private Button button10;
        private Label label3;
        private PictureBox pictureBox1;
        private SplitContainer splitContainerForWorkers;
        private Button button11;
        private Button button12;
        private KryptonLinkLabel kryptonLinkLabel1;
        private SplitContainer splitContainerWokerDetails;
        private AC.ExtendedRenderer.Toolkit.KryptonListView kryptonListView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader10;
        private KryptonListBox kryptonListBox1;
        private AC.ExtendedRenderer.Toolkit.KryptonListView kryptonListViewForWorkers;
        private Label label4;
        private Label label1;
        private PictureBox pictureBox2;
        private KryptonListBox kryptonListBox2;
        private ImageList imageList1;
        private AC.ExtendedRenderer.Toolkit.KryptonImageComboBox kryptonImageComboBox1;
        private TabPage tabPageRunActiveLearning;
        private KryptonTrackBar kryptonTrackBar1;
        private SplitContainer splitContainerActiveLearningOuterContainer;
        private SplitContainer splitContainer3;
        private Button button1;
        private Button button7;
        private Label labelDatasetName;
        private Button buttonRestartActiveLearning;
        private AC.ExtendedRenderer.Navigator.KryptonTabControl TabControlActiveLearningGraphs;
        private TabPage tabPageAccuracyGraph;
        private Button button16;
        private Button btnActiveLearning_PopUpAccuracyGraph;
        private Button buttonActiveLearningCloseAllOpenedWindow;
        private Button btnPlayAndPauseExperiment;
        private Button btnStopExperiment;
        private AC.ExtendedRenderer.Toolkit.KryptonProgressBar progressBarForActiveLearning;
        private ZedGraph.ZedGraphControl graphControlAccuracyGraph;
        private Label label6;
        private ComboBox comboBoxShowCurrentModels;
        private System.ComponentModel.BackgroundWorker backgroundWorkerForSimulation;
        private System.ComponentModel.BackgroundWorker backgroundWorkerForProgressBar;
        private System.ComponentModel.BackgroundWorker backgroundWorkerForAccuracyGraph;
        private Label label7;
        private KryptonContextMenu kryptonContextMenu1;
        private ContextMenuStrip contextMenuStrip2;
        private KryptonPanel kryptonPanel1;
        private SplitContainer splitContainer2;
        private AC.ExtendedRenderer.Navigator.KryptonTabControl tabControlForValues;
        private TabPage tabPageInnerViewSchedule;
        private SplitContainer splitContainer5;

        private TabPage tabPageInnerViewWorkers;
        private TabPage tabPageInnerViewDataset;
        private KryptonDataGridView dataGridViewForInnerWorker;
        private KryptonDataGridView dataGridViewActiveLearningSchedule;
        private TabPage tabPageInnerViewCommunity;
        private KryptonDataGridView dataGridViewForTaskValue;
        private KryptonDataGridView dataGridViewInnerCommunity;
        private AC.ExtendedRenderer.Toolkit.KryptonComboBox comboBoxForSelectingDataset;
        private Label label8;
        private Button buttonLoadDataset;
        private ImageList imageList2;
        private ImageList imageList3;
        private Button button9;
        private BindingSource mainPageBindingSource;
        private TabPage tabPageRunBatchRunning;
        private Label label5;
        private SplitContainer splitContainerOuterBatchLearning;
        private SplitContainer splitContainerInnterBatchRunning;
        private Label label10;
        private Label label9;
        private AC.ExtendedRenderer.Toolkit.KryptonProgressBar kryptonProgressBar1;
        private Button button6;
        private Button button13;
        private Button button19;
        private Button button20;
        private Button button21;
        private Label label11;
        private Button button22;
        private Button button23;
        private SplitContainer splitContainer1;
        private Label label12;
        private ComboBox comboBox1;
        private AC.ExtendedRenderer.Navigator.KryptonTabControl kryptonTabControl1;
        private TabPage tabPage1;
        private KryptonDataGridView kryptonDataGridView1;
        private TabPage tabPage2;
        private KryptonDataGridView kryptonDataGridView2;
        private TabPage tabPage3;
        private KryptonDataGridView kryptonDataGridView3;
        private TabPage tabPage4;
        private KryptonDataGridView kryptonDataGridView4;
        private KryptonDataGridView kryptonDataGridView5;
        private Label label13;
        private AC.ExtendedRenderer.Toolkit.KryptonProgressBar progressBatchRunning;
        private Button button25;
        private Button buttonRestartBatchRunning;
        private Button buttonPlayAndPauseBatchRunning;
        private Button buttonStopBatchRunning;
        private SplitContainer splitContainer4;
        private SplitContainer splitContainer6;
        private Label label15;
        private ComboBox comboBoxBatchRunningModel;
        private AC.ExtendedRenderer.Navigator.KryptonTabControl tabControlBatchLearningInnerValues;
        private TabPage tabPage6;
        private KryptonDataGridView dataGridViewBatchRunningWorkers;
        private TabPage tabPage7;
        private KryptonDataGridView dataGridViewBatchRunningTasks;
        private TabPage tabPageBatchLearningInnerViewCommunity;
        private KryptonDataGridView dataGridViewBatchRunningCommunities;
        private Label label16;
        private AC.ExtendedRenderer.Navigator.KryptonTabControl kryptonTabControl3;
        private TabPage tabPage9;
        private TabPage tabPage11;
        private KryptonDataGridView dataGridViewBatchRunningProgress;
        private System.ComponentModel.BackgroundWorker backgroundWorkerBatchRunning;
        private Label labelDatasetNameBatchRunning;
        private System.ComponentModel.BackgroundWorker backgroundWorkerBatchRunningValues;
        private KryptonDataGridView dataGridViewBatchRunningAccuracy;
        private Label label14;
    }
}