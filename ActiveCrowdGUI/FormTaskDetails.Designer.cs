using System.Drawing;
using System.Windows.Forms;
namespace AcriveCrowdGUI
{
    partial class FormTaskDetails
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.labelTypeName = new System.Windows.Forms.Label();
            this.labelForTaskId = new System.Windows.Forms.Label();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelConfusionMatrix = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.backgroundTaskValues = new System.ComponentModel.BackgroundWorker();
            this.labelModelDetail = new System.Windows.Forms.Label();
            this.textBoxTaskValue = new System.Windows.Forms.TextBox();
            this.workerConfusionMatrixGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.labelForDataHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.workerConfusionMatrixGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.TabIndex = 0;
            // 
            // labelTypeName
            // 
            this.labelTypeName.AutoSize = true;
            this.labelTypeName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTypeName.Location = new System.Drawing.Point(9, 36);
            this.labelTypeName.Name = "labelTypeName";
            this.labelTypeName.Size = new System.Drawing.Size(73, 19);
            this.labelTypeName.TabIndex = 0;
            this.labelTypeName.Text = "Task ID: ";
            // 
            // labelForTaskId
            // 
            this.labelForTaskId.AutoSize = true;
            this.labelForTaskId.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelForTaskId.Location = new System.Drawing.Point(92, 37);
            this.labelForTaskId.Name = "labelForTaskId";
            this.labelForTaskId.Size = new System.Drawing.Size(70, 19);
            this.labelForTaskId.TabIndex = 4;
            this.labelForTaskId.Text = "Loading...";
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(0, 0);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            // 
            // labelConfusionMatrix
            // 
            this.labelConfusionMatrix.AutoSize = true;
            this.labelConfusionMatrix.Location = new System.Drawing.Point(23, 79);
            this.labelConfusionMatrix.Name = "labelConfusionMatrix";
            this.labelConfusionMatrix.Size = new System.Drawing.Size(0, 13);
            this.labelConfusionMatrix.TabIndex = 7;
            this.labelConfusionMatrix.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "Model:";
            // 
            // backgroundTaskValues
            // 
            this.backgroundTaskValues.WorkerReportsProgress = true;
            this.backgroundTaskValues.WorkerSupportsCancellation = true;
            this.backgroundTaskValues.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerUpdateConfusionMatrix_DoWork);
            this.backgroundTaskValues.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerUpdateConfusionMatrix_ProgressChanged);
            // 
            // labelModelDetail
            // 
            this.labelModelDetail.AutoSize = true;
            this.labelModelDetail.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModelDetail.Location = new System.Drawing.Point(92, 11);
            this.labelModelDetail.Name = "labelModelDetail";
            this.labelModelDetail.Size = new System.Drawing.Size(70, 19);
            this.labelModelDetail.TabIndex = 9;
            this.labelModelDetail.Text = "Loading...";
            // 
            // textBoxTaskValue
            // 
            this.textBoxTaskValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTaskValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTaskValue.Location = new System.Drawing.Point(12, 379);
            this.textBoxTaskValue.Multiline = true;
            this.textBoxTaskValue.Name = "textBoxTaskValue";
            this.textBoxTaskValue.ReadOnly = true;
            this.textBoxTaskValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxTaskValue.Size = new System.Drawing.Size(412, 59);
            this.textBoxTaskValue.TabIndex = 10;
            this.textBoxTaskValue.Text = "\r\nLabel 1      Label 2     Label 3      Label 4    Label 5\r\n0.002294   0.002291  " +
    "0.002293    0.9908     0.002297";
            this.textBoxTaskValue.WordWrap = false;
            // 
            // workerConfusionMatrixGraph
            // 
            this.workerConfusionMatrixGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workerConfusionMatrixGraph.BackColor = System.Drawing.Color.Silver;
            this.workerConfusionMatrixGraph.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.Area3DStyle.Inclination = 15;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.Perspective = 10;
            chartArea1.Area3DStyle.Rotation = 10;
            chartArea1.Area3DStyle.WallWidth = 0;
            chartArea1.AxisX.LabelAutoFitMaxFontSize = 8;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.LineWidth = 0;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.LabelAutoFitMaxFontSize = 8;
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineWidth = 0;
            chartArea1.AxisY.Title = "Probability";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold);
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = System.Drawing.Color.Transparent;
            chartArea1.BorderWidth = 0;
            chartArea1.IsSameFontSizeForAllAxes = true;
            chartArea1.Name = "Default";
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.workerConfusionMatrixGraph.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.Enabled = false;
            legend1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            legend1.IsTextAutoFit = false;
            legend1.Name = "Default";
            this.workerConfusionMatrixGraph.Legends.Add(legend1);
            this.workerConfusionMatrixGraph.Location = new System.Drawing.Point(12, 60);
            this.workerConfusionMatrixGraph.Name = "workerConfusionMatrixGraph";
            this.workerConfusionMatrixGraph.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.workerConfusionMatrixGraph.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))))};
            this.workerConfusionMatrixGraph.Size = new System.Drawing.Size(410, 300);
            this.workerConfusionMatrixGraph.TabIndex = 0;
            title1.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold);
            title1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            title1.Name = "Title1";
            title1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            title1.ShadowOffset = 3;
            title1.Text = "Label Posterior Probability";
            this.workerConfusionMatrixGraph.Titles.Add(title1);
            // 
            // labelForDataHeader
            // 
            this.labelForDataHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelForDataHeader.AutoSize = true;
            this.labelForDataHeader.Location = new System.Drawing.Point(9, 366);
            this.labelForDataHeader.Name = "labelForDataHeader";
            this.labelForDataHeader.Size = new System.Drawing.Size(0, 13);
            this.labelForDataHeader.TabIndex = 11;
            // 
            // FormTaskDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(434, 447);
            this.Controls.Add(this.labelForDataHeader);
            this.Controls.Add(this.workerConfusionMatrixGraph);
            this.Controls.Add(this.textBoxTaskValue);
            this.Controls.Add(this.labelModelDetail);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelConfusionMatrix);
            this.Controls.Add(this.labelForTaskId);
            this.Controls.Add(this.labelTypeName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "FormTaskDetails";
            this.Opacity = 0.85D;
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Black;
            this.Text = "TaskD - Task Details";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormWorkerDetail_FormClosed);
            this.Load += new System.EventHandler(this.FormWorkerDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.workerConfusionMatrixGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private System.Windows.Forms.Label labelTypeName;
        private Label labelForTaskId;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private Label labelConfusionMatrix;
        private Label label4;
        private System.ComponentModel.BackgroundWorker backgroundTaskValues;
        private Label labelModelDetail;
        private TextBox textBoxTaskValue;
        private System.Windows.Forms.DataVisualization.Charting.Chart workerConfusionMatrixGraph;
        private Label labelForDataHeader;
    }
}