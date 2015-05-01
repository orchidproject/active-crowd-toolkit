namespace AcriveCrowdGUI
{
    partial class FormExperimentSetting
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonViewDataset = new System.Windows.Forms.Button();
            this.buttonLoadDataset = new System.Windows.Forms.Button();
            this.comboBoxForSelectingDataset = new AC.ExtendedRenderer.Toolkit.KryptonComboBox();
            this.labelMaximumLabellingRound = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarNumberOfLabellingRounds = new System.Windows.Forms.TrackBar();
            this.groupBoxExperimentOuterBox = new System.Windows.Forms.GroupBox();
            this.buttonRunBatchRunning = new System.Windows.Forms.Button();
            this.groupBoxInitialNumberOfLabelling = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dataGridViewOfCurrentModels = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.dropDownListOfWorkerSelectionMethod = new System.Windows.Forms.ComboBox();
            this.labelWorkerSelectionMethod = new System.Windows.Forms.Label();
            this.buttonDeleteAllModels = new System.Windows.Forms.Button();
            this.buttonAddModel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxListOfTaskSelectionMethods = new System.Windows.Forms.ComboBox();
            this.labelTaskSelectionMethod = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxListOfRunTypes = new System.Windows.Forms.ComboBox();
            this.btnRunExperiment = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.kryptonSplitContainer1 = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonComboBox1 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNumberOfLabellingRounds)).BeginInit();
            this.groupBoxExperimentOuterBox.SuspendLayout();
            this.groupBoxInitialNumberOfLabelling.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOfCurrentModels)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(200, 20);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(113, 23);
            this.kryptonButton1.TabIndex = 0;
            this.kryptonButton1.Values.Text = "Unselect test";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonViewDataset);
            this.groupBox4.Controls.Add(this.buttonLoadDataset);
            this.groupBox4.Controls.Add(this.comboBoxForSelectingDataset);
            this.groupBox4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(6, 21);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(352, 81);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Dataset";
            // 
            // buttonViewDataset
            // 
            this.buttonViewDataset.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.application_form_magnify_icon;
            this.buttonViewDataset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonViewDataset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonViewDataset.Location = new System.Drawing.Point(255, 23);
            this.buttonViewDataset.Name = "buttonViewDataset";
            this.buttonViewDataset.Size = new System.Drawing.Size(39, 36);
            this.buttonViewDataset.TabIndex = 3;
            this.buttonViewDataset.UseVisualStyleBackColor = true;
            this.buttonViewDataset.Click += new System.EventHandler(this.buttonViewDataset_Click);
            // 
            // buttonLoadDataset
            // 
            this.buttonLoadDataset.BackgroundImage = global::AcriveCrowdGUI.Properties.Resources.application_add_icon;
            this.buttonLoadDataset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonLoadDataset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoadDataset.Location = new System.Drawing.Point(300, 23);
            this.buttonLoadDataset.Name = "buttonLoadDataset";
            this.buttonLoadDataset.Size = new System.Drawing.Size(39, 36);
            this.buttonLoadDataset.TabIndex = 2;
            this.buttonLoadDataset.UseVisualStyleBackColor = true;
            this.buttonLoadDataset.Click += new System.EventHandler(this.buttonLoadDataset_Click);
            this.buttonLoadDataset.MouseHover += new System.EventHandler(this.buttonLoadDataset_MouseHover);
            // 
            // comboBoxForSelectingDataset
            // 
            this.comboBoxForSelectingDataset.DisableBorderColor = false;
            this.comboBoxForSelectingDataset.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxForSelectingDataset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxForSelectingDataset.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxForSelectingDataset.FormattingEnabled = true;
            this.comboBoxForSelectingDataset.ItemHeight = 30;
            this.comboBoxForSelectingDataset.Location = new System.Drawing.Point(24, 23);
            this.comboBoxForSelectingDataset.Name = "comboBoxForSelectingDataset";
            this.comboBoxForSelectingDataset.PersistentColors = false;
            this.comboBoxForSelectingDataset.Size = new System.Drawing.Size(225, 36);
            this.comboBoxForSelectingDataset.TabIndex = 1;
            this.comboBoxForSelectingDataset.UseStyledColors = false;
            this.comboBoxForSelectingDataset.SelectedIndexChanged += new System.EventHandler(this.comboBoxForSelectingDataset_SelectedIndexChanged);
            // 
            // labelMaximumLabellingRound
            // 
            this.labelMaximumLabellingRound.AutoSize = true;
            this.labelMaximumLabellingRound.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaximumLabellingRound.Location = new System.Drawing.Point(234, 63);
            this.labelMaximumLabellingRound.Name = "labelMaximumLabellingRound";
            this.labelMaximumLabellingRound.Size = new System.Drawing.Size(15, 15);
            this.labelMaximumLabellingRound.TabIndex = 2;
            this.labelMaximumLabellingRound.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "1";
            // 
            // trackBarNumberOfLabellingRounds
            // 
            this.trackBarNumberOfLabellingRounds.AllowDrop = true;
            this.trackBarNumberOfLabellingRounds.LargeChange = 1;
            this.trackBarNumberOfLabellingRounds.Location = new System.Drawing.Point(12, 21);
            this.trackBarNumberOfLabellingRounds.Maximum = 5;
            this.trackBarNumberOfLabellingRounds.Minimum = 1;
            this.trackBarNumberOfLabellingRounds.Name = "trackBarNumberOfLabellingRounds";
            this.trackBarNumberOfLabellingRounds.Size = new System.Drawing.Size(237, 45);
            this.trackBarNumberOfLabellingRounds.TabIndex = 0;
            this.trackBarNumberOfLabellingRounds.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarNumberOfLabellingRounds.Value = 1;
            this.trackBarNumberOfLabellingRounds.Scroll += new System.EventHandler(this.trackBarNumberOfLabellingRounds_Scroll);
            // 
            // groupBoxExperimentOuterBox
            // 
            this.groupBoxExperimentOuterBox.Controls.Add(this.buttonRunBatchRunning);
            this.groupBoxExperimentOuterBox.Controls.Add(this.groupBoxInitialNumberOfLabelling);
            this.groupBoxExperimentOuterBox.Controls.Add(this.groupBox6);
            this.groupBoxExperimentOuterBox.Controls.Add(this.btnRunExperiment);
            this.groupBoxExperimentOuterBox.Controls.Add(this.groupBox1);
            this.groupBoxExperimentOuterBox.Controls.Add(this.groupBox4);
            this.groupBoxExperimentOuterBox.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxExperimentOuterBox.Location = new System.Drawing.Point(12, 12);
            this.groupBoxExperimentOuterBox.Name = "groupBoxExperimentOuterBox";
            this.groupBoxExperimentOuterBox.Size = new System.Drawing.Size(927, 499);
            this.groupBoxExperimentOuterBox.TabIndex = 8;
            this.groupBoxExperimentOuterBox.TabStop = false;
            this.groupBoxExperimentOuterBox.Text = "Active Learning Settings";
            // 
            // buttonRunBatchRunning
            // 
            this.buttonRunBatchRunning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRunBatchRunning.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRunBatchRunning.Location = new System.Drawing.Point(6, 408);
            this.buttonRunBatchRunning.Name = "buttonRunBatchRunning";
            this.buttonRunBatchRunning.Size = new System.Drawing.Size(898, 82);
            this.buttonRunBatchRunning.TabIndex = 9;
            this.buttonRunBatchRunning.Text = "Start";
            this.buttonRunBatchRunning.UseVisualStyleBackColor = true;
            this.buttonRunBatchRunning.Visible = false;
            this.buttonRunBatchRunning.Click += new System.EventHandler(this.buttonRunBatchRunning_Click);
            // 
            // groupBoxInitialNumberOfLabelling
            // 
            this.groupBoxInitialNumberOfLabelling.Controls.Add(this.trackBarNumberOfLabellingRounds);
            this.groupBoxInitialNumberOfLabelling.Controls.Add(this.label1);
            this.groupBoxInitialNumberOfLabelling.Controls.Add(this.labelMaximumLabellingRound);
            this.groupBoxInitialNumberOfLabelling.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxInitialNumberOfLabelling.Location = new System.Drawing.Point(649, 21);
            this.groupBoxInitialNumberOfLabelling.Name = "groupBoxInitialNumberOfLabelling";
            this.groupBoxInitialNumberOfLabelling.Size = new System.Drawing.Size(255, 81);
            this.groupBoxInitialNumberOfLabelling.TabIndex = 7;
            this.groupBoxInitialNumberOfLabelling.TabStop = false;
            this.groupBoxInitialNumberOfLabelling.Text = "Initial Number of Labels Per Task";
            this.groupBoxInitialNumberOfLabelling.Enter += new System.EventHandler(this.groupBoxInitialNumberOfLabelling_Enter);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dataGridViewOfCurrentModels);
            this.groupBox6.Controls.Add(this.dropDownListOfWorkerSelectionMethod);
            this.groupBox6.Controls.Add(this.labelWorkerSelectionMethod);
            this.groupBox6.Controls.Add(this.buttonDeleteAllModels);
            this.groupBox6.Controls.Add(this.buttonAddModel);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.comboBoxListOfTaskSelectionMethods);
            this.groupBox6.Controls.Add(this.labelTaskSelectionMethod);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.comboBoxListOfRunTypes);
            this.groupBox6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(6, 108);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(898, 294);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Active Learning Strategies";
            this.groupBox6.Enter += new System.EventHandler(this.groupBox6_Enter);
            // 
            // dataGridViewOfCurrentModels
            // 
            this.dataGridViewOfCurrentModels.AllowUserToAddRows = false;
            this.dataGridViewOfCurrentModels.AllowUserToDeleteRows = false;
            this.dataGridViewOfCurrentModels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewOfCurrentModels.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewOfCurrentModels.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewOfCurrentModels.Location = new System.Drawing.Point(19, 124);
            this.dataGridViewOfCurrentModels.Name = "dataGridViewOfCurrentModels";
            this.dataGridViewOfCurrentModels.ReadOnly = true;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewOfCurrentModels.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewOfCurrentModels.Size = new System.Drawing.Size(861, 158);
            this.dataGridViewOfCurrentModels.TabIndex = 11;
            this.dataGridViewOfCurrentModels.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewOfCurrentModels_CellClick);
            // 
            // dropDownListOfWorkerSelectionMethod
            // 
            this.dropDownListOfWorkerSelectionMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropDownListOfWorkerSelectionMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dropDownListOfWorkerSelectionMethod.FormattingEnabled = true;
            this.dropDownListOfWorkerSelectionMethod.Location = new System.Drawing.Point(545, 41);
            this.dropDownListOfWorkerSelectionMethod.Name = "dropDownListOfWorkerSelectionMethod";
            this.dropDownListOfWorkerSelectionMethod.Size = new System.Drawing.Size(201, 23);
            this.dropDownListOfWorkerSelectionMethod.TabIndex = 10;
            this.dropDownListOfWorkerSelectionMethod.SelectedIndexChanged += new System.EventHandler(this.dropDownListOfWorkerSelectionMethod_SelectedIndexChanged);
            // 
            // labelWorkerSelectionMethod
            // 
            this.labelWorkerSelectionMethod.AutoSize = true;
            this.labelWorkerSelectionMethod.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.labelWorkerSelectionMethod.Location = new System.Drawing.Point(542, 22);
            this.labelWorkerSelectionMethod.Name = "labelWorkerSelectionMethod";
            this.labelWorkerSelectionMethod.Size = new System.Drawing.Size(178, 17);
            this.labelWorkerSelectionMethod.TabIndex = 9;
            this.labelWorkerSelectionMethod.Text = "Worker Selection Method";
            this.labelWorkerSelectionMethod.Click += new System.EventHandler(this.labelWorkerSelectionMethod_Click);
            // 
            // buttonDeleteAllModels
            // 
            this.buttonDeleteAllModels.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonDeleteAllModels.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDeleteAllModels.Image = global::AcriveCrowdGUI.Properties.Resources.table_delete1;
            this.buttonDeleteAllModels.Location = new System.Drawing.Point(752, 76);
            this.buttonDeleteAllModels.Name = "buttonDeleteAllModels";
            this.buttonDeleteAllModels.Size = new System.Drawing.Size(128, 42);
            this.buttonDeleteAllModels.TabIndex = 8;
            this.buttonDeleteAllModels.Text = "Delete All";
            this.buttonDeleteAllModels.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonDeleteAllModels.UseVisualStyleBackColor = true;
            this.buttonDeleteAllModels.Click += new System.EventHandler(this.buttonDeleteAllSelectedModels_Click);
            // 
            // buttonAddModel
            // 
            this.buttonAddModel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonAddModel.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddModel.Image = global::AcriveCrowdGUI.Properties.Resources.add1;
            this.buttonAddModel.Location = new System.Drawing.Point(752, 22);
            this.buttonAddModel.Name = "buttonAddModel";
            this.buttonAddModel.Size = new System.Drawing.Size(128, 42);
            this.buttonAddModel.TabIndex = 6;
            this.buttonAddModel.Text = " Add Model";
            this.buttonAddModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonAddModel.UseVisualStyleBackColor = true;
            this.buttonAddModel.Click += new System.EventHandler(this.buttonAddModel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 19);
            this.label4.TabIndex = 4;
            this.label4.Text = "Selected Strategies";
            // 
            // comboBoxListOfTaskSelectionMethods
            // 
            this.comboBoxListOfTaskSelectionMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxListOfTaskSelectionMethods.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxListOfTaskSelectionMethods.FormattingEnabled = true;
            this.comboBoxListOfTaskSelectionMethods.Location = new System.Drawing.Point(284, 41);
            this.comboBoxListOfTaskSelectionMethods.Name = "comboBoxListOfTaskSelectionMethods";
            this.comboBoxListOfTaskSelectionMethods.Size = new System.Drawing.Size(255, 23);
            this.comboBoxListOfTaskSelectionMethods.TabIndex = 3;
            // 
            // labelTaskSelectionMethod
            // 
            this.labelTaskSelectionMethod.AutoSize = true;
            this.labelTaskSelectionMethod.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTaskSelectionMethod.Location = new System.Drawing.Point(278, 22);
            this.labelTaskSelectionMethod.Name = "labelTaskSelectionMethod";
            this.labelTaskSelectionMethod.Size = new System.Drawing.Size(160, 17);
            this.labelTaskSelectionMethod.TabIndex = 2;
            this.labelTaskSelectionMethod.Text = "Task Selection Method";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(16, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Aggregation Model";
            // 
            // comboBoxListOfRunTypes
            // 
            this.comboBoxListOfRunTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxListOfRunTypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxListOfRunTypes.FormattingEnabled = true;
            this.comboBoxListOfRunTypes.Location = new System.Drawing.Point(19, 41);
            this.comboBoxListOfRunTypes.Name = "comboBoxListOfRunTypes";
            this.comboBoxListOfRunTypes.Size = new System.Drawing.Size(256, 23);
            this.comboBoxListOfRunTypes.TabIndex = 0;
            this.comboBoxListOfRunTypes.SelectedIndexChanged += new System.EventHandler(this.comboBoxListOfRunTypes_SelectedIndexChanged);
            // 
            // btnRunExperiment
            // 
            this.btnRunExperiment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRunExperiment.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunExperiment.Location = new System.Drawing.Point(6, 408);
            this.btnRunExperiment.Name = "btnRunExperiment";
            this.btnRunExperiment.Size = new System.Drawing.Size(898, 82);
            this.btnRunExperiment.TabIndex = 3;
            this.btnRunExperiment.Text = "Start";
            this.btnRunExperiment.UseVisualStyleBackColor = true;
            this.btnRunExperiment.Visible = false;
            this.btnRunExperiment.Click += new System.EventHandler(this.btnRunExperiment_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(364, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 81);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Random Seed";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(187, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(86, 26);
            this.textBox1.TabIndex = 2;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(109, 32);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(77, 21);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Custom";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(13, 33);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(95, 21);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "By Default";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            this.kryptonSplitContainer1.TabIndex = 0;
            // 
            // kryptonComboBox1
            // 
            this.kryptonComboBox1.DropDownWidth = 121;
            this.kryptonComboBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonComboBox1.Name = "kryptonComboBox1";
            this.kryptonComboBox1.TabIndex = 0;
            // 
            // FormExperimentSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(950, 518);
            this.Controls.Add(this.groupBoxExperimentOuterBox);
            this.HelpButton = true;
            this.KeyPreview = true;
            this.Name = "FormExperimentSetting";
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormActiveLearningSetting_KeyDown);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNumberOfLabellingRounds)).EndInit();
            this.groupBoxExperimentOuterBox.ResumeLayout(false);
            this.groupBoxInitialNumberOfLabelling.ResumeLayout(false);
            this.groupBoxInitialNumberOfLabelling.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOfCurrentModels)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private System.Windows.Forms.GroupBox groupBox4;
        private AC.ExtendedRenderer.Toolkit.KryptonComboBox comboBoxForSelectingDataset;
        private System.Windows.Forms.Label labelMaximumLabellingRound;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarNumberOfLabellingRounds;
        private System.Windows.Forms.Button btnRunExperiment;
        private System.Windows.Forms.GroupBox groupBoxExperimentOuterBox;
        private ComponentFactory.Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button buttonLoadDataset;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxListOfRunTypes;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBox1;
        private System.Windows.Forms.ComboBox comboBoxListOfTaskSelectionMethods;
        private System.Windows.Forms.Label labelTaskSelectionMethod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonAddModel;
        private System.Windows.Forms.Button buttonDeleteAllModels;
        private System.Windows.Forms.Label labelWorkerSelectionMethod;
        private System.Windows.Forms.ComboBox dropDownListOfWorkerSelectionMethod;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dataGridViewOfCurrentModels;
        private System.Windows.Forms.GroupBox groupBoxInitialNumberOfLabelling;
        private System.Windows.Forms.Button buttonViewDataset;
        private System.Windows.Forms.Button buttonRunBatchRunning;
    }
}