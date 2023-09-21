namespace WinThumbsPreloader
{
    partial class ScheduleForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleForm));
            MultithreadedCheckBox = new System.Windows.Forms.CheckBox();
            ScheduleOptionsGroupBox = new System.Windows.Forms.GroupBox();
            EnabledCheckBox = new System.Windows.Forms.CheckBox();
            ThreadsLabel = new System.Windows.Forms.Label();
            ThreadsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            AtLabel = new System.Windows.Forms.Label();
            TimeOfDayComboBox = new System.Windows.Forms.ComboBox();
            TimeDigitsComboBox = new System.Windows.Forms.ComboBox();
            CheckedListBox1 = new System.Windows.Forms.CheckedListBox();
            ScheduleSaveButton = new System.Windows.Forms.Button();
            OnIdleCheckBox = new System.Windows.Forms.CheckBox();
            RunEveryLabel = new System.Windows.Forms.Label();
            RunEveryComboBox = new System.Windows.Forms.ComboBox();
            DrivesLabel = new System.Windows.Forms.Label();
            CloseButton = new System.Windows.Forms.Button();
            ScheduleSettingsGroupBox = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            OutputTextBox = new System.Windows.Forms.TextBox();
            OpenTaskSchedulerButton = new System.Windows.Forms.Button();
            ScheduleToolTips = new System.Windows.Forms.ToolTip(components);
            ScheduleOptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ThreadsNumericUpDown).BeginInit();
            ScheduleSettingsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // MultithreadedCheckBox
            // 
            resources.ApplyResources(MultithreadedCheckBox, "MultithreadedCheckBox");
            MultithreadedCheckBox.Name = "MultithreadedCheckBox";
            ScheduleToolTips.SetToolTip(MultithreadedCheckBox, resources.GetString("MultithreadedCheckBox.ToolTip"));
            MultithreadedCheckBox.UseVisualStyleBackColor = true;
            MultithreadedCheckBox.CheckedChanged += MultithreadedCheckBox_CheckedChanged;
            // 
            // ScheduleOptionsGroupBox
            // 
            ScheduleOptionsGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            ScheduleOptionsGroupBox.Controls.Add(EnabledCheckBox);
            ScheduleOptionsGroupBox.Controls.Add(ThreadsLabel);
            ScheduleOptionsGroupBox.Controls.Add(ThreadsNumericUpDown);
            ScheduleOptionsGroupBox.Controls.Add(MultithreadedCheckBox);
            resources.ApplyResources(ScheduleOptionsGroupBox, "ScheduleOptionsGroupBox");
            ScheduleOptionsGroupBox.Name = "ScheduleOptionsGroupBox";
            ScheduleOptionsGroupBox.TabStop = false;
            // 
            // EnabledCheckBox
            // 
            resources.ApplyResources(EnabledCheckBox, "EnabledCheckBox");
            EnabledCheckBox.Name = "EnabledCheckBox";
            ScheduleToolTips.SetToolTip(EnabledCheckBox, resources.GetString("EnabledCheckBox.ToolTip"));
            EnabledCheckBox.UseVisualStyleBackColor = true;
            EnabledCheckBox.CheckedChanged += EnabledCheckBox_CheckedChanged;
            // 
            // ThreadsLabel
            // 
            resources.ApplyResources(ThreadsLabel, "ThreadsLabel");
            ThreadsLabel.Name = "ThreadsLabel";
            ScheduleToolTips.SetToolTip(ThreadsLabel, resources.GetString("ThreadsLabel.ToolTip"));
            // 
            // ThreadsNumericUpDown
            // 
            resources.ApplyResources(ThreadsNumericUpDown, "ThreadsNumericUpDown");
            ThreadsNumericUpDown.Name = "ThreadsNumericUpDown";
            ThreadsNumericUpDown.ValueChanged += ThreadsNumericUpDown_ValueChanged;
            // 
            // AtLabel
            // 
            resources.ApplyResources(AtLabel, "AtLabel");
            AtLabel.Name = "AtLabel";
            // 
            // TimeOfDayComboBox
            // 
            TimeOfDayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            TimeOfDayComboBox.FormattingEnabled = true;
            TimeOfDayComboBox.Items.AddRange(new object[] { resources.GetString("TimeOfDayComboBox.Items"), resources.GetString("TimeOfDayComboBox.Items1") });
            resources.ApplyResources(TimeOfDayComboBox, "TimeOfDayComboBox");
            TimeOfDayComboBox.Name = "TimeOfDayComboBox";
            TimeOfDayComboBox.SelectedIndexChanged += TimeOfDayComboBox_SelectedIndexChanged;
            // 
            // TimeDigitsComboBox
            // 
            TimeDigitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            TimeDigitsComboBox.FormattingEnabled = true;
            TimeDigitsComboBox.Items.AddRange(new object[] { resources.GetString("TimeDigitsComboBox.Items"), resources.GetString("TimeDigitsComboBox.Items1"), resources.GetString("TimeDigitsComboBox.Items2"), resources.GetString("TimeDigitsComboBox.Items3"), resources.GetString("TimeDigitsComboBox.Items4"), resources.GetString("TimeDigitsComboBox.Items5"), resources.GetString("TimeDigitsComboBox.Items6"), resources.GetString("TimeDigitsComboBox.Items7"), resources.GetString("TimeDigitsComboBox.Items8"), resources.GetString("TimeDigitsComboBox.Items9"), resources.GetString("TimeDigitsComboBox.Items10"), resources.GetString("TimeDigitsComboBox.Items11") });
            resources.ApplyResources(TimeDigitsComboBox, "TimeDigitsComboBox");
            TimeDigitsComboBox.Name = "TimeDigitsComboBox";
            TimeDigitsComboBox.SelectedIndexChanged += TimeDigitsComboBox_SelectedIndexChanged;
            // 
            // CheckedListBox1
            // 
            resources.ApplyResources(CheckedListBox1, "CheckedListBox1");
            CheckedListBox1.FormattingEnabled = true;
            CheckedListBox1.Items.AddRange(new object[] { resources.GetString("CheckedListBox1.Items"), resources.GetString("CheckedListBox1.Items1"), resources.GetString("CheckedListBox1.Items2"), resources.GetString("CheckedListBox1.Items3"), resources.GetString("CheckedListBox1.Items4"), resources.GetString("CheckedListBox1.Items5"), resources.GetString("CheckedListBox1.Items6"), resources.GetString("CheckedListBox1.Items7"), resources.GetString("CheckedListBox1.Items8"), resources.GetString("CheckedListBox1.Items9"), resources.GetString("CheckedListBox1.Items10"), resources.GetString("CheckedListBox1.Items11"), resources.GetString("CheckedListBox1.Items12"), resources.GetString("CheckedListBox1.Items13"), resources.GetString("CheckedListBox1.Items14"), resources.GetString("CheckedListBox1.Items15"), resources.GetString("CheckedListBox1.Items16"), resources.GetString("CheckedListBox1.Items17"), resources.GetString("CheckedListBox1.Items18"), resources.GetString("CheckedListBox1.Items19"), resources.GetString("CheckedListBox1.Items20"), resources.GetString("CheckedListBox1.Items21"), resources.GetString("CheckedListBox1.Items22"), resources.GetString("CheckedListBox1.Items23"), resources.GetString("CheckedListBox1.Items24"), resources.GetString("CheckedListBox1.Items25") });
            CheckedListBox1.MultiColumn = true;
            CheckedListBox1.Name = "CheckedListBox1";
            CheckedListBox1.SelectedIndexChanged += CheckedListBox1_SelectedIndexChanged;
            // 
            // ScheduleSaveButton
            // 
            resources.ApplyResources(ScheduleSaveButton, "ScheduleSaveButton");
            ScheduleSaveButton.Name = "ScheduleSaveButton";
            ScheduleSaveButton.UseVisualStyleBackColor = true;
            ScheduleSaveButton.Click += ScheduleSaveButton_Click;
            // 
            // OnIdleCheckBox
            // 
            resources.ApplyResources(OnIdleCheckBox, "OnIdleCheckBox");
            OnIdleCheckBox.Name = "OnIdleCheckBox";
            ScheduleToolTips.SetToolTip(OnIdleCheckBox, resources.GetString("OnIdleCheckBox.ToolTip"));
            OnIdleCheckBox.UseVisualStyleBackColor = true;
            OnIdleCheckBox.CheckedChanged += OnIdleCheckBox_CheckedChanged;
            // 
            // RunEveryLabel
            // 
            resources.ApplyResources(RunEveryLabel, "RunEveryLabel");
            RunEveryLabel.Name = "RunEveryLabel";
            ScheduleToolTips.SetToolTip(RunEveryLabel, resources.GetString("RunEveryLabel.ToolTip"));
            // 
            // RunEveryComboBox
            // 
            RunEveryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            RunEveryComboBox.FormattingEnabled = true;
            RunEveryComboBox.Items.AddRange(new object[] { resources.GetString("RunEveryComboBox.Items"), resources.GetString("RunEveryComboBox.Items1"), resources.GetString("RunEveryComboBox.Items2") });
            resources.ApplyResources(RunEveryComboBox, "RunEveryComboBox");
            RunEveryComboBox.Name = "RunEveryComboBox";
            RunEveryComboBox.SelectedIndexChanged += RunEveryComboBox_SelectedIndexChanged;
            // 
            // DrivesLabel
            // 
            resources.ApplyResources(DrivesLabel, "DrivesLabel");
            DrivesLabel.Name = "DrivesLabel";
            ScheduleToolTips.SetToolTip(DrivesLabel, resources.GetString("DrivesLabel.ToolTip"));
            // 
            // CloseButton
            // 
            resources.ApplyResources(CloseButton, "CloseButton");
            CloseButton.Name = "CloseButton";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // ScheduleSettingsGroupBox
            // 
            ScheduleSettingsGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            ScheduleSettingsGroupBox.Controls.Add(label1);
            ScheduleSettingsGroupBox.Controls.Add(OutputTextBox);
            ScheduleSettingsGroupBox.Controls.Add(CheckedListBox1);
            ScheduleSettingsGroupBox.Controls.Add(AtLabel);
            ScheduleSettingsGroupBox.Controls.Add(OnIdleCheckBox);
            ScheduleSettingsGroupBox.Controls.Add(DrivesLabel);
            ScheduleSettingsGroupBox.Controls.Add(TimeOfDayComboBox);
            ScheduleSettingsGroupBox.Controls.Add(RunEveryLabel);
            ScheduleSettingsGroupBox.Controls.Add(TimeDigitsComboBox);
            ScheduleSettingsGroupBox.Controls.Add(RunEveryComboBox);
            resources.ApplyResources(ScheduleSettingsGroupBox, "ScheduleSettingsGroupBox");
            ScheduleSettingsGroupBox.Name = "ScheduleSettingsGroupBox";
            ScheduleSettingsGroupBox.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // OutputTextBox
            // 
            resources.ApplyResources(OutputTextBox, "OutputTextBox");
            OutputTextBox.Name = "OutputTextBox";
            OutputTextBox.ReadOnly = true;
            OutputTextBox.TextChanged += OutputTextBox_TextChanged;
            // 
            // OpenTaskSchedulerButton
            // 
            resources.ApplyResources(OpenTaskSchedulerButton, "OpenTaskSchedulerButton");
            OpenTaskSchedulerButton.Name = "OpenTaskSchedulerButton";
            OpenTaskSchedulerButton.UseVisualStyleBackColor = true;
            OpenTaskSchedulerButton.Click += OpenTaskSchedulerButton_Click;
            // 
            // ScheduleForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(OpenTaskSchedulerButton);
            Controls.Add(ScheduleSettingsGroupBox);
            Controls.Add(CloseButton);
            Controls.Add(ScheduleOptionsGroupBox);
            Controls.Add(ScheduleSaveButton);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "ScheduleForm";
            Load += ScheduleForm_Load;
            ScheduleOptionsGroupBox.ResumeLayout(false);
            ScheduleOptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ThreadsNumericUpDown).EndInit();
            ScheduleSettingsGroupBox.ResumeLayout(false);
            ScheduleSettingsGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.CheckBox MultithreadedCheckBox;
        private System.Windows.Forms.GroupBox ScheduleOptionsGroupBox;
        private System.Windows.Forms.NumericUpDown ThreadsNumericUpDown;
        private System.Windows.Forms.Label DrivesLabel;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label ThreadsLabel;
        private System.Windows.Forms.Label RunEveryLabel;
        private System.Windows.Forms.CheckBox OnIdleCheckBox;
        private System.Windows.Forms.ComboBox RunEveryComboBox;
        private System.Windows.Forms.Button ScheduleSaveButton;
        private System.Windows.Forms.CheckBox EnabledCheckBox;
        private System.Windows.Forms.CheckedListBox CheckedListBox1;
        private System.Windows.Forms.ComboBox TimeOfDayComboBox;
        private System.Windows.Forms.ComboBox TimeDigitsComboBox;
        private System.Windows.Forms.Label AtLabel;
        private System.Windows.Forms.GroupBox ScheduleSettingsGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.Button OpenTaskSchedulerButton;
        private System.Windows.Forms.ToolTip ScheduleToolTips;
    }
}