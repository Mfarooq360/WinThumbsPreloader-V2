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
            AdvancedButton = new System.Windows.Forms.Button();
            AtLabel = new System.Windows.Forms.Label();
            TimeOfDayComboBox = new System.Windows.Forms.ComboBox();
            TimeDigitsComboBox = new System.Windows.Forms.ComboBox();
            DrivesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            ScheduleSaveButton = new System.Windows.Forms.Button();
            OnIdleCheckBox = new System.Windows.Forms.CheckBox();
            RunEveryLabel = new System.Windows.Forms.Label();
            RunEveryComboBox = new System.Windows.Forms.ComboBox();
            DrivesLabel = new System.Windows.Forms.Label();
            CloseButton = new System.Windows.Forms.Button();
            ScheduleSettingsGroupBox = new System.Windows.Forms.GroupBox();
            dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            OutputTextBox1 = new System.Windows.Forms.TextBox();
            PathsTextBox = new System.Windows.Forms.TextBox();
            PathsLabel = new System.Windows.Forms.Label();
            OutputLabel = new System.Windows.Forms.Label();
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
            // AdvancedButton
            // 
            resources.ApplyResources(AdvancedButton, "AdvancedButton");
            AdvancedButton.Name = "AdvancedButton";
            ScheduleToolTips.SetToolTip(AdvancedButton, resources.GetString("AdvancedButton.ToolTip"));
            AdvancedButton.UseVisualStyleBackColor = true;
            AdvancedButton.Click += AdvancedButton_Click;
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
            // DrivesCheckedListBox
            // 
            DrivesCheckedListBox.CheckOnClick = true;
            resources.ApplyResources(DrivesCheckedListBox, "DrivesCheckedListBox");
            DrivesCheckedListBox.FormattingEnabled = true;
            DrivesCheckedListBox.Items.AddRange(new object[] { resources.GetString("DrivesCheckedListBox.Items"), resources.GetString("DrivesCheckedListBox.Items1"), resources.GetString("DrivesCheckedListBox.Items2"), resources.GetString("DrivesCheckedListBox.Items3"), resources.GetString("DrivesCheckedListBox.Items4"), resources.GetString("DrivesCheckedListBox.Items5"), resources.GetString("DrivesCheckedListBox.Items6"), resources.GetString("DrivesCheckedListBox.Items7"), resources.GetString("DrivesCheckedListBox.Items8"), resources.GetString("DrivesCheckedListBox.Items9"), resources.GetString("DrivesCheckedListBox.Items10"), resources.GetString("DrivesCheckedListBox.Items11"), resources.GetString("DrivesCheckedListBox.Items12"), resources.GetString("DrivesCheckedListBox.Items13"), resources.GetString("DrivesCheckedListBox.Items14"), resources.GetString("DrivesCheckedListBox.Items15"), resources.GetString("DrivesCheckedListBox.Items16"), resources.GetString("DrivesCheckedListBox.Items17"), resources.GetString("DrivesCheckedListBox.Items18"), resources.GetString("DrivesCheckedListBox.Items19"), resources.GetString("DrivesCheckedListBox.Items20"), resources.GetString("DrivesCheckedListBox.Items21"), resources.GetString("DrivesCheckedListBox.Items22"), resources.GetString("DrivesCheckedListBox.Items23"), resources.GetString("DrivesCheckedListBox.Items24"), resources.GetString("DrivesCheckedListBox.Items25") });
            DrivesCheckedListBox.MultiColumn = true;
            DrivesCheckedListBox.Name = "DrivesCheckedListBox";
            DrivesCheckedListBox.SelectedIndexChanged += DrivesCheckedListBox_SelectedIndexChanged;
            // 
            // ScheduleSaveButton
            // 
            resources.ApplyResources(ScheduleSaveButton, "ScheduleSaveButton");
            ScheduleSaveButton.Name = "ScheduleSaveButton";
            ScheduleToolTips.SetToolTip(ScheduleSaveButton, resources.GetString("ScheduleSaveButton.ToolTip"));
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
            RunEveryComboBox.Items.AddRange(new object[] { resources.GetString("RunEveryComboBox.Items"), resources.GetString("RunEveryComboBox.Items1"), resources.GetString("RunEveryComboBox.Items2"), resources.GetString("RunEveryComboBox.Items3") });
            resources.ApplyResources(RunEveryComboBox, "RunEveryComboBox");
            RunEveryComboBox.Name = "RunEveryComboBox";
            RunEveryComboBox.SelectedIndexChanged += RunEveryComboBox_SelectedIndexChanged;
            // 
            // DrivesLabel
            // 
            resources.ApplyResources(DrivesLabel, "DrivesLabel");
            DrivesLabel.Name = "DrivesLabel";
            ScheduleToolTips.SetToolTip(DrivesLabel, resources.GetString("DrivesLabel.ToolTip"));
            DrivesLabel.Click += DrivesLabel_Click;
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
            ScheduleSettingsGroupBox.Controls.Add(dateTimePicker1);
            ScheduleSettingsGroupBox.Controls.Add(OutputTextBox1);
            ScheduleSettingsGroupBox.Controls.Add(PathsTextBox);
            ScheduleSettingsGroupBox.Controls.Add(PathsLabel);
            ScheduleSettingsGroupBox.Controls.Add(OutputLabel);
            ScheduleSettingsGroupBox.Controls.Add(OutputTextBox);
            ScheduleSettingsGroupBox.Controls.Add(DrivesCheckedListBox);
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
            // dateTimePicker1
            // 
            resources.ApplyResources(dateTimePicker1, "dateTimePicker1");
            dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.ShowUpDown = true;
            // 
            // OutputTextBox1
            // 
            resources.ApplyResources(OutputTextBox1, "OutputTextBox1");
            OutputTextBox1.Name = "OutputTextBox1";
            OutputTextBox1.ReadOnly = true;
            OutputTextBox1.TextChanged += OutputTextBox1_TextChanged;
            // 
            // PathsTextBox
            // 
            resources.ApplyResources(PathsTextBox, "PathsTextBox");
            PathsTextBox.Name = "PathsTextBox";
            PathsTextBox.TextChanged += PathsTextBox_TextChanged;
            // 
            // PathsLabel
            // 
            resources.ApplyResources(PathsLabel, "PathsLabel");
            PathsLabel.Name = "PathsLabel";
            ScheduleToolTips.SetToolTip(PathsLabel, resources.GetString("PathsLabel.ToolTip"));
            PathsLabel.Click += PathsLabel_Click;
            // 
            // OutputLabel
            // 
            resources.ApplyResources(OutputLabel, "OutputLabel");
            OutputLabel.Name = "OutputLabel";
            ScheduleToolTips.SetToolTip(OutputLabel, resources.GetString("OutputLabel.ToolTip"));
            OutputLabel.Click += OutputLabel_Click;
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
            ScheduleToolTips.SetToolTip(OpenTaskSchedulerButton, resources.GetString("OpenTaskSchedulerButton.ToolTip"));
            OpenTaskSchedulerButton.UseVisualStyleBackColor = true;
            OpenTaskSchedulerButton.Click += OpenTaskSchedulerButton_Click;
            // 
            // ScheduleForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(AdvancedButton);
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
        private System.Windows.Forms.CheckedListBox DrivesCheckedListBox;
        public System.Windows.Forms.ComboBox TimeOfDayComboBox;
        private System.Windows.Forms.ComboBox TimeDigitsComboBox;
        private System.Windows.Forms.Label AtLabel;
        private System.Windows.Forms.GroupBox ScheduleSettingsGroupBox;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.Button OpenTaskSchedulerButton;
        private System.Windows.Forms.ToolTip ScheduleToolTips;
        private System.Windows.Forms.Label PathsLabel;
        private System.Windows.Forms.TextBox PathsTextBox;
        private System.Windows.Forms.TextBox OutputTextBox1;
        private System.Windows.Forms.Button AdvancedButton;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}