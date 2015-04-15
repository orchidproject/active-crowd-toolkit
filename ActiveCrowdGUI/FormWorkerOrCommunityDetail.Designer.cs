
using System.Drawing;
using System.Windows.Forms;
namespace AcriveCrowdGUI
{
    partial class FormWorkerOrCommunityDetail
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
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.labelTypeName = new System.Windows.Forms.Label();
            this.labelForWorkerId = new System.Windows.Forms.Label();
            this.confusionMatrixGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelConfusionMatrix = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.backgroundUpdateConfusionMatrix = new System.ComponentModel.BackgroundWorker();
            this.labelModelDetail = new System.Windows.Forms.Label();
            this.textBoxConfusionMatrix = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.confusionMatrixGraph)).BeginInit();
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
            this.labelTypeName.Location = new System.Drawing.Point(9, 33);
            this.labelTypeName.Name = "labelTypeName";
            this.labelTypeName.Size = new System.Drawing.Size(91, 19);
            this.labelTypeName.TabIndex = 0;
            this.labelTypeName.Text = "Worker ID: ";
            // 
            // labelForWorkerId
            // 
            this.labelForWorkerId.AutoSize = true;
            this.labelForWorkerId.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelForWorkerId.Location = new System.Drawing.Point(121, 33);
            this.labelForWorkerId.Name = "labelForWorkerId";
            this.labelForWorkerId.Size = new System.Drawing.Size(70, 19);
            this.labelForWorkerId.TabIndex = 4;
            this.labelForWorkerId.Text = "Loading...";
            // 
            // workerConfusionMatrixGraph
            // 
            this.confusionMatrixGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.confusionMatrixGraph.BackColor = System.Drawing.Color.Transparent;
            this.confusionMatrixGraph.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            this.confusionMatrixGraph.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.confusionMatrixGraph.BorderSkin.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Area3DStyle.Enable3D = true;
            chartArea1.AxisX.Title = "True Label";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX2.Title = " testt";
            chartArea1.AxisY.Title = "Probability";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY2.Title = "AxisY2";
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.BorderWidth = 0;
            chartArea1.CursorX.SelectionColor = System.Drawing.Color.Transparent;
            chartArea1.CursorY.SelectionColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "Default";
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.confusionMatrixGraph.ChartAreas.Add(chartArea1);
            this.confusionMatrixGraph.Cursor = System.Windows.Forms.Cursors.Default;
            legend1.Name = "Legend1";
            legend1.ShadowColor = System.Drawing.Color.Transparent;
            this.confusionMatrixGraph.Legends.Add(legend1);
            this.confusionMatrixGraph.Location = new System.Drawing.Point(12, 68);
            this.confusionMatrixGraph.Name = "workerConfusionMatrixGraph";
            this.confusionMatrixGraph.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            series1.ChartArea = "Default";
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.confusionMatrixGraph.Series.Add(series1);
            this.confusionMatrixGraph.Size = new System.Drawing.Size(461, 305);
            this.confusionMatrixGraph.TabIndex = 5;
            this.confusionMatrixGraph.Text = "workerConfusionMatrixGraph";
            title1.Alignment = System.Drawing.ContentAlignment.TopCenter;
            title1.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold);
            title1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            title1.Name = "Confusion Matrix";
            title1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            title1.ShadowOffset = 3;
            title1.Text = "Confusion Matrix";
            title2.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            title2.DockedToChartArea = "Default";
            title2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            title2.DockingOffset = 8;
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title2.Name = "workerLabel";
            title2.Text = "Worker Label";
            title2.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            this.confusionMatrixGraph.Titles.Add(title1);
            this.confusionMatrixGraph.Titles.Add(title2);
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(0, 0);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            // 
            // labelConfusionMatrix
            // 
            this.labelConfusionMatrix.AutoSize = true;
            this.labelConfusionMatrix.Location = new System.Drawing.Point(254, 76);
            this.labelConfusionMatrix.Name = "labelConfusionMatrix";
            this.labelConfusionMatrix.Size = new System.Drawing.Size(0, 13);
            this.labelConfusionMatrix.TabIndex = 7;
            this.labelConfusionMatrix.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "Model:";
            // 
            // backgroundUpdateConfusionMatrix
            // 
            this.backgroundUpdateConfusionMatrix.WorkerReportsProgress = true;
            this.backgroundUpdateConfusionMatrix.WorkerSupportsCancellation = true;
            this.backgroundUpdateConfusionMatrix.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundUpdateConfusionMatrix_DoWork);
            this.backgroundUpdateConfusionMatrix.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundUpdateConfusionMatrix_ProgressChanged);
            // 
            // labelModelDetail
            // 
            this.labelModelDetail.AutoSize = true;
            this.labelModelDetail.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModelDetail.Location = new System.Drawing.Point(121, 8);
            this.labelModelDetail.Name = "labelModelDetail";
            this.labelModelDetail.Size = new System.Drawing.Size(70, 19);
            this.labelModelDetail.TabIndex = 9;
            this.labelModelDetail.Text = "Loading...";
            // 
            // textBoxConfusionMatrix
            // 
            this.textBoxConfusionMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConfusionMatrix.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxConfusionMatrix.Location = new System.Drawing.Point(12, 379);
            this.textBoxConfusionMatrix.Multiline = true;
            this.textBoxConfusionMatrix.Name = "textBoxConfusionMatrix";
            this.textBoxConfusionMatrix.ReadOnly = true;
            this.textBoxConfusionMatrix.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxConfusionMatrix.Size = new System.Drawing.Size(461, 105);
            this.textBoxConfusionMatrix.TabIndex = 10;
            this.textBoxConfusionMatrix.WordWrap = false;
            // 
            // FormWorkerOrCommunityDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(485, 496);
            this.Controls.Add(this.textBoxConfusionMatrix);
            this.Controls.Add(this.labelModelDetail);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelConfusionMatrix);
            this.Controls.Add(this.confusionMatrixGraph);
            this.Controls.Add(this.labelForWorkerId);
            this.Controls.Add(this.labelTypeName);
            this.Name = "FormWorkerOrCommunityDetail";
            this.Opacity = 0.85D;
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Black;
            this.Text = "WorkID - Worker Details";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormWorkerDetail_FormClosed);
            this.Load += new System.EventHandler(this.FormWorkerDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.confusionMatrixGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private System.Windows.Forms.Label labelTypeName;
        private Label labelForWorkerId;
        private System.Windows.Forms.DataVisualization.Charting.Chart confusionMatrixGraph;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private Label labelConfusionMatrix;
        private Label label4;
        private System.ComponentModel.BackgroundWorker backgroundUpdateConfusionMatrix;
        private Label labelModelDetail;
        private TextBox textBoxConfusionMatrix;
    }
}