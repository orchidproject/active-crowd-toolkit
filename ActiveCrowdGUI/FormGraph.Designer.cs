namespace AcriveCrowdGUI
{
    partial class FormGraph
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
            this.labelCurrentLabellingRound = new System.Windows.Forms.Label();
            this.graphControlGraphLarge = new ZedGraph.ZedGraphControl();
            this.backgroundWorkerLoadingGraph = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // labelCurrentLabellingRound
            // 
            this.labelCurrentLabellingRound.AutoSize = true;
            this.labelCurrentLabellingRound.BackColor = System.Drawing.Color.Transparent;
            this.labelCurrentLabellingRound.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentLabellingRound.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelCurrentLabellingRound.Location = new System.Drawing.Point(12, 8);
            this.labelCurrentLabellingRound.Name = "labelCurrentLabellingRound";
            this.labelCurrentLabellingRound.Size = new System.Drawing.Size(99, 24);
            this.labelCurrentLabellingRound.TabIndex = 1;
            this.labelCurrentLabellingRound.Text = "Loading...";
            this.labelCurrentLabellingRound.Click += new System.EventHandler(this.labelCurrentLabellingRound_Click);
            // 
            // graphControlGraphLarge
            // 
            this.graphControlGraphLarge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphControlGraphLarge.BackColor = System.Drawing.Color.Transparent;
            this.graphControlGraphLarge.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.graphControlGraphLarge.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.graphControlGraphLarge.Location = new System.Drawing.Point(-3, 36);
            this.graphControlGraphLarge.Name = "graphControlGraphLarge";
            this.graphControlGraphLarge.ScrollGrace = 0D;
            this.graphControlGraphLarge.ScrollMaxX = 0D;
            this.graphControlGraphLarge.ScrollMaxY = 0D;
            this.graphControlGraphLarge.ScrollMaxY2 = 0D;
            this.graphControlGraphLarge.ScrollMinX = 0D;
            this.graphControlGraphLarge.ScrollMinY = 0D;
            this.graphControlGraphLarge.ScrollMinY2 = 0D;
            this.graphControlGraphLarge.Size = new System.Drawing.Size(837, 380);
            this.graphControlGraphLarge.TabIndex = 2;
            // 
            // backgroundWorkerLoadingGraph
            // 
            this.backgroundWorkerLoadingGraph.WorkerReportsProgress = true;
            this.backgroundWorkerLoadingGraph.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerLoadingGraph_DoWork);
            this.backgroundWorkerLoadingGraph.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerLoadingGraph_ProgressChanged);
            // 
            // FormGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(832, 417);
            this.Controls.Add(this.graphControlGraphLarge);
            this.Controls.Add(this.labelCurrentLabellingRound);
            this.Name = "FormGraph";
            this.Opacity = 0.95D;
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Black;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Accuracy Graph";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormGraph_FormClosed);
            this.Load += new System.EventHandler(this.FormGraph_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCurrentLabellingRound;
        private ZedGraph.ZedGraphControl graphControlGraphLarge;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLoadingGraph;
    }
}