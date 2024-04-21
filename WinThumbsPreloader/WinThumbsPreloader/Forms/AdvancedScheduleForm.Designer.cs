namespace WinThumbsPreloader.Forms
{
    partial class AdvancedScheduleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedScheduleForm));
            groupBox1 = new System.Windows.Forms.GroupBox();
            DefaultButton = new System.Windows.Forms.Button();
            DetailedTimeModeCheckBox = new System.Windows.Forms.CheckBox();
            TwentyFourHourCheckBox = new System.Windows.Forms.CheckBox();
            DriveSortLabel = new System.Windows.Forms.Label();
            DriveSortComboBox = new System.Windows.Forms.ComboBox();
            DriveSelectionCheckBox = new System.Windows.Forms.CheckBox();
            CloseButton = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            GeneralDescriptionLabel = new System.Windows.Forms.Label();
            GeneralDescriptionTextBox = new System.Windows.Forms.TextBox();
            GeneralHiddenCheckBox = new System.Windows.Forms.CheckBox();
            GeneralSecurityOptionsGroupBox = new System.Windows.Forms.GroupBox();
            RunWithHighestPrivilegesCheckBox = new System.Windows.Forms.CheckBox();
            RunOnlyWhenUserIsLoggedOnRadioButton = new System.Windows.Forms.RadioButton();
            RunWhetherUserIsLoggedOnOrNotRadioButton = new System.Windows.Forms.RadioButton();
            tabPage2 = new System.Windows.Forms.TabPage();
            tabPage3 = new System.Windows.Forms.TabPage();
            groupBox4 = new System.Windows.Forms.GroupBox();
            StartTheTaskOnlyIfTheComputerIsIdleForComboBox = new System.Windows.Forms.ComboBox();
            WaitForIdleForComboBox = new System.Windows.Forms.ComboBox();
            WaitForIdleForLabel = new System.Windows.Forms.Label();
            RestartIfTheIdleStateResumesCheckBox = new System.Windows.Forms.CheckBox();
            StopIfTheComputerCeasesToBeIdleCheckBox = new System.Windows.Forms.CheckBox();
            StartTheTaskOnlyIfTheComputerIsIdleForCheckBox = new System.Windows.Forms.CheckBox();
            groupBox5 = new System.Windows.Forms.GroupBox();
            StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox = new System.Windows.Forms.CheckBox();
            StopIfTheComputerSwitchesToBatteryPowerCheckBox = new System.Windows.Forms.CheckBox();
            WakeTheComputerToRunThisTaskCheckBox = new System.Windows.Forms.CheckBox();
            tabPage4 = new System.Windows.Forms.TabPage();
            StopTheTaskIfItRunsLongerThanComboBox = new System.Windows.Forms.ComboBox();
            TimesLabel = new System.Windows.Forms.Label();
            AttemptToRestartUpToTextBox = new System.Windows.Forms.TextBox();
            IfTheTaskFailsRestartEveryComboBox = new System.Windows.Forms.ComboBox();
            AttemptToRestartUpToLabel = new System.Windows.Forms.Label();
            IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox = new System.Windows.Forms.ComboBox();
            IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel = new System.Windows.Forms.Label();
            IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox = new System.Windows.Forms.CheckBox();
            StopTheTaskIfItRunsLongerThanCheckBox = new System.Windows.Forms.CheckBox();
            IfTheTaskFailsRestartEveryCheckBox = new System.Windows.Forms.CheckBox();
            RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox = new System.Windows.Forms.CheckBox();
            AllowTaskToBeRunOnDemandCheckBox = new System.Windows.Forms.CheckBox();
            OpenTaskSchedulerButton = new System.Windows.Forms.Button();
            AdvancedScheduleSaveButton = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            GeneralSecurityOptionsGroupBox.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            tabPage4.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.SystemColors.Window;
            groupBox1.Controls.Add(DefaultButton);
            groupBox1.Controls.Add(DetailedTimeModeCheckBox);
            groupBox1.Controls.Add(TwentyFourHourCheckBox);
            groupBox1.Controls.Add(DriveSortLabel);
            groupBox1.Controls.Add(DriveSortComboBox);
            groupBox1.Controls.Add(DriveSelectionCheckBox);
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // DefaultButton
            // 
            resources.ApplyResources(DefaultButton, "DefaultButton");
            DefaultButton.Name = "DefaultButton";
            DefaultButton.UseVisualStyleBackColor = true;
            DefaultButton.Click += DefaultButton_Click;
            // 
            // DetailedTimeModeCheckBox
            // 
            resources.ApplyResources(DetailedTimeModeCheckBox, "DetailedTimeModeCheckBox");
            DetailedTimeModeCheckBox.Name = "DetailedTimeModeCheckBox";
            DetailedTimeModeCheckBox.UseVisualStyleBackColor = true;
            DetailedTimeModeCheckBox.CheckedChanged += DetailedTimeModeCheckBox_CheckedChanged;
            // 
            // TwentyFourHourCheckBox
            // 
            resources.ApplyResources(TwentyFourHourCheckBox, "TwentyFourHourCheckBox");
            TwentyFourHourCheckBox.Name = "TwentyFourHourCheckBox";
            TwentyFourHourCheckBox.UseVisualStyleBackColor = true;
            TwentyFourHourCheckBox.CheckedChanged += TwentyFourHourCheckBox_CheckedChanged;
            // 
            // DriveSortLabel
            // 
            resources.ApplyResources(DriveSortLabel, "DriveSortLabel");
            DriveSortLabel.Name = "DriveSortLabel";
            // 
            // DriveSortComboBox
            // 
            DriveSortComboBox.FormattingEnabled = true;
            DriveSortComboBox.Items.AddRange(new object[] { resources.GetString("DriveSortComboBox.Items"), resources.GetString("DriveSortComboBox.Items1") });
            resources.ApplyResources(DriveSortComboBox, "DriveSortComboBox");
            DriveSortComboBox.Name = "DriveSortComboBox";
            DriveSortComboBox.SelectedIndexChanged += DriveSortComboBox_SelectedIndexChanged;
            // 
            // DriveSelectionCheckBox
            // 
            resources.ApplyResources(DriveSelectionCheckBox, "DriveSelectionCheckBox");
            DriveSelectionCheckBox.Name = "DriveSelectionCheckBox";
            DriveSelectionCheckBox.UseVisualStyleBackColor = true;
            DriveSelectionCheckBox.CheckedChanged += DriveSelectionCheckBox_CheckedChanged;
            // 
            // CloseButton
            // 
            resources.ApplyResources(CloseButton, "CloseButton");
            CloseButton.Name = "CloseButton";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = System.Drawing.SystemColors.Window;
            groupBox2.Controls.Add(tabControl1);
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            resources.ApplyResources(tabControl1, "tabControl1");
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(GeneralDescriptionLabel);
            tabPage1.Controls.Add(GeneralDescriptionTextBox);
            tabPage1.Controls.Add(GeneralHiddenCheckBox);
            tabPage1.Controls.Add(GeneralSecurityOptionsGroupBox);
            resources.ApplyResources(tabPage1, "tabPage1");
            tabPage1.Name = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // GeneralDescriptionLabel
            // 
            resources.ApplyResources(GeneralDescriptionLabel, "GeneralDescriptionLabel");
            GeneralDescriptionLabel.Name = "GeneralDescriptionLabel";
            // 
            // GeneralDescriptionTextBox
            // 
            resources.ApplyResources(GeneralDescriptionTextBox, "GeneralDescriptionTextBox");
            GeneralDescriptionTextBox.Name = "GeneralDescriptionTextBox";
            GeneralDescriptionTextBox.TextChanged += GeneralDescriptionTextBox_TextChanged;
            // 
            // GeneralHiddenCheckBox
            // 
            resources.ApplyResources(GeneralHiddenCheckBox, "GeneralHiddenCheckBox");
            GeneralHiddenCheckBox.Name = "GeneralHiddenCheckBox";
            GeneralHiddenCheckBox.UseVisualStyleBackColor = true;
            GeneralHiddenCheckBox.CheckedChanged += GeneralHiddenCheckBox_CheckedChanged;
            // 
            // GeneralSecurityOptionsGroupBox
            // 
            GeneralSecurityOptionsGroupBox.Controls.Add(RunWithHighestPrivilegesCheckBox);
            GeneralSecurityOptionsGroupBox.Controls.Add(RunOnlyWhenUserIsLoggedOnRadioButton);
            GeneralSecurityOptionsGroupBox.Controls.Add(RunWhetherUserIsLoggedOnOrNotRadioButton);
            resources.ApplyResources(GeneralSecurityOptionsGroupBox, "GeneralSecurityOptionsGroupBox");
            GeneralSecurityOptionsGroupBox.Name = "GeneralSecurityOptionsGroupBox";
            GeneralSecurityOptionsGroupBox.TabStop = false;
            // 
            // RunWithHighestPrivilegesCheckBox
            // 
            resources.ApplyResources(RunWithHighestPrivilegesCheckBox, "RunWithHighestPrivilegesCheckBox");
            RunWithHighestPrivilegesCheckBox.Name = "RunWithHighestPrivilegesCheckBox";
            RunWithHighestPrivilegesCheckBox.UseVisualStyleBackColor = true;
            RunWithHighestPrivilegesCheckBox.CheckedChanged += RunWithHighestPrivilegesCheckBox_CheckedChanged;
            // 
            // RunOnlyWhenUserIsLoggedOnRadioButton
            // 
            resources.ApplyResources(RunOnlyWhenUserIsLoggedOnRadioButton, "RunOnlyWhenUserIsLoggedOnRadioButton");
            RunOnlyWhenUserIsLoggedOnRadioButton.Name = "RunOnlyWhenUserIsLoggedOnRadioButton";
            RunOnlyWhenUserIsLoggedOnRadioButton.TabStop = true;
            RunOnlyWhenUserIsLoggedOnRadioButton.UseVisualStyleBackColor = true;
            RunOnlyWhenUserIsLoggedOnRadioButton.CheckedChanged += RunOnlyWhenUserIsLoggedOnRadioButton_CheckedChanged;
            // 
            // RunWhetherUserIsLoggedOnOrNotRadioButton
            // 
            resources.ApplyResources(RunWhetherUserIsLoggedOnOrNotRadioButton, "RunWhetherUserIsLoggedOnOrNotRadioButton");
            RunWhetherUserIsLoggedOnOrNotRadioButton.Name = "RunWhetherUserIsLoggedOnOrNotRadioButton";
            RunWhetherUserIsLoggedOnOrNotRadioButton.TabStop = true;
            RunWhetherUserIsLoggedOnOrNotRadioButton.UseVisualStyleBackColor = true;
            RunWhetherUserIsLoggedOnOrNotRadioButton.CheckedChanged += RunWhetherUserIsLoggedOnOrNotRadioButton_CheckedChanged;
            // 
            // tabPage2
            // 
            resources.ApplyResources(tabPage2, "tabPage2");
            tabPage2.Name = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(groupBox4);
            tabPage3.Controls.Add(groupBox5);
            resources.ApplyResources(tabPage3, "tabPage3");
            tabPage3.Name = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(StartTheTaskOnlyIfTheComputerIsIdleForComboBox);
            groupBox4.Controls.Add(WaitForIdleForComboBox);
            groupBox4.Controls.Add(WaitForIdleForLabel);
            groupBox4.Controls.Add(RestartIfTheIdleStateResumesCheckBox);
            groupBox4.Controls.Add(StopIfTheComputerCeasesToBeIdleCheckBox);
            groupBox4.Controls.Add(StartTheTaskOnlyIfTheComputerIsIdleForCheckBox);
            resources.ApplyResources(groupBox4, "groupBox4");
            groupBox4.Name = "groupBox4";
            groupBox4.TabStop = false;
            // 
            // StartTheTaskOnlyIfTheComputerIsIdleForComboBox
            // 
            StartTheTaskOnlyIfTheComputerIsIdleForComboBox.FormattingEnabled = true;
            StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Items.AddRange(new object[] { resources.GetString("StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Items"), resources.GetString("StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Items1"), resources.GetString("StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Items2"), resources.GetString("StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Items3"), resources.GetString("StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Items4"), resources.GetString("StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Items5") });
            resources.ApplyResources(StartTheTaskOnlyIfTheComputerIsIdleForComboBox, "StartTheTaskOnlyIfTheComputerIsIdleForComboBox");
            StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Name = "StartTheTaskOnlyIfTheComputerIsIdleForComboBox";
            StartTheTaskOnlyIfTheComputerIsIdleForComboBox.SelectedIndexChanged += StartTheTaskOnlyIfTheComputerIsIdleForComboBox_SelectedIndexChanged;
            // 
            // WaitForIdleForComboBox
            // 
            WaitForIdleForComboBox.FormattingEnabled = true;
            WaitForIdleForComboBox.Items.AddRange(new object[] { resources.GetString("WaitForIdleForComboBox.Items"), resources.GetString("WaitForIdleForComboBox.Items1"), resources.GetString("WaitForIdleForComboBox.Items2"), resources.GetString("WaitForIdleForComboBox.Items3"), resources.GetString("WaitForIdleForComboBox.Items4"), resources.GetString("WaitForIdleForComboBox.Items5"), resources.GetString("WaitForIdleForComboBox.Items6"), resources.GetString("WaitForIdleForComboBox.Items7") });
            resources.ApplyResources(WaitForIdleForComboBox, "WaitForIdleForComboBox");
            WaitForIdleForComboBox.Name = "WaitForIdleForComboBox";
            WaitForIdleForComboBox.SelectedIndexChanged += WaitForIdleForComboBox_SelectedIndexChanged;
            // 
            // WaitForIdleForLabel
            // 
            resources.ApplyResources(WaitForIdleForLabel, "WaitForIdleForLabel");
            WaitForIdleForLabel.Name = "WaitForIdleForLabel";
            WaitForIdleForLabel.Click += WaitForIdleForLabel_Click;
            // 
            // RestartIfTheIdleStateResumesCheckBox
            // 
            RestartIfTheIdleStateResumesCheckBox.AutoCheck = false;
            resources.ApplyResources(RestartIfTheIdleStateResumesCheckBox, "RestartIfTheIdleStateResumesCheckBox");
            RestartIfTheIdleStateResumesCheckBox.Name = "RestartIfTheIdleStateResumesCheckBox";
            RestartIfTheIdleStateResumesCheckBox.UseVisualStyleBackColor = true;
            RestartIfTheIdleStateResumesCheckBox.CheckedChanged += RestartIfTheIdleStateResumesCheckBox_CheckedChanged;
            // 
            // StopIfTheComputerCeasesToBeIdleCheckBox
            // 
            resources.ApplyResources(StopIfTheComputerCeasesToBeIdleCheckBox, "StopIfTheComputerCeasesToBeIdleCheckBox");
            StopIfTheComputerCeasesToBeIdleCheckBox.Name = "StopIfTheComputerCeasesToBeIdleCheckBox";
            StopIfTheComputerCeasesToBeIdleCheckBox.UseVisualStyleBackColor = true;
            StopIfTheComputerCeasesToBeIdleCheckBox.CheckedChanged += StopIfTheComputerCeasesToBeIdleCheckBox_CheckedChanged;
            // 
            // StartTheTaskOnlyIfTheComputerIsIdleForCheckBox
            // 
            resources.ApplyResources(StartTheTaskOnlyIfTheComputerIsIdleForCheckBox, "StartTheTaskOnlyIfTheComputerIsIdleForCheckBox");
            StartTheTaskOnlyIfTheComputerIsIdleForCheckBox.Name = "StartTheTaskOnlyIfTheComputerIsIdleForCheckBox";
            StartTheTaskOnlyIfTheComputerIsIdleForCheckBox.UseVisualStyleBackColor = true;
            StartTheTaskOnlyIfTheComputerIsIdleForCheckBox.CheckedChanged += StartTheTaskOnlyIfTheComputerIsIdleForCheckBox_CheckedChanged;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox);
            groupBox5.Controls.Add(StopIfTheComputerSwitchesToBatteryPowerCheckBox);
            groupBox5.Controls.Add(WakeTheComputerToRunThisTaskCheckBox);
            resources.ApplyResources(groupBox5, "groupBox5");
            groupBox5.Name = "groupBox5";
            groupBox5.TabStop = false;
            // 
            // StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox
            // 
            resources.ApplyResources(StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox, "StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox");
            StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox.Name = "StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox";
            StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox.UseVisualStyleBackColor = true;
            StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox.CheckedChanged += StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox_CheckedChanged;
            // 
            // StopIfTheComputerSwitchesToBatteryPowerCheckBox
            // 
            resources.ApplyResources(StopIfTheComputerSwitchesToBatteryPowerCheckBox, "StopIfTheComputerSwitchesToBatteryPowerCheckBox");
            StopIfTheComputerSwitchesToBatteryPowerCheckBox.Name = "StopIfTheComputerSwitchesToBatteryPowerCheckBox";
            StopIfTheComputerSwitchesToBatteryPowerCheckBox.UseVisualStyleBackColor = true;
            StopIfTheComputerSwitchesToBatteryPowerCheckBox.CheckedChanged += StopIfTheComputerSwitchesToBatteryPowerCheckBox_CheckedChanged;
            // 
            // WakeTheComputerToRunThisTaskCheckBox
            // 
            resources.ApplyResources(WakeTheComputerToRunThisTaskCheckBox, "WakeTheComputerToRunThisTaskCheckBox");
            WakeTheComputerToRunThisTaskCheckBox.Name = "WakeTheComputerToRunThisTaskCheckBox";
            WakeTheComputerToRunThisTaskCheckBox.UseVisualStyleBackColor = true;
            WakeTheComputerToRunThisTaskCheckBox.CheckedChanged += WakeTheComputerToRunThisTaskCheckBox_CheckedChanged;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(StopTheTaskIfItRunsLongerThanComboBox);
            tabPage4.Controls.Add(TimesLabel);
            tabPage4.Controls.Add(AttemptToRestartUpToTextBox);
            tabPage4.Controls.Add(IfTheTaskFailsRestartEveryComboBox);
            tabPage4.Controls.Add(AttemptToRestartUpToLabel);
            tabPage4.Controls.Add(IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox);
            tabPage4.Controls.Add(IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel);
            tabPage4.Controls.Add(IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox);
            tabPage4.Controls.Add(StopTheTaskIfItRunsLongerThanCheckBox);
            tabPage4.Controls.Add(IfTheTaskFailsRestartEveryCheckBox);
            tabPage4.Controls.Add(RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox);
            tabPage4.Controls.Add(AllowTaskToBeRunOnDemandCheckBox);
            resources.ApplyResources(tabPage4, "tabPage4");
            tabPage4.Name = "tabPage4";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // StopTheTaskIfItRunsLongerThanComboBox
            // 
            StopTheTaskIfItRunsLongerThanComboBox.FormattingEnabled = true;
            StopTheTaskIfItRunsLongerThanComboBox.Items.AddRange(new object[] { resources.GetString("StopTheTaskIfItRunsLongerThanComboBox.Items"), resources.GetString("StopTheTaskIfItRunsLongerThanComboBox.Items1"), resources.GetString("StopTheTaskIfItRunsLongerThanComboBox.Items2"), resources.GetString("StopTheTaskIfItRunsLongerThanComboBox.Items3"), resources.GetString("StopTheTaskIfItRunsLongerThanComboBox.Items4"), resources.GetString("StopTheTaskIfItRunsLongerThanComboBox.Items5"), resources.GetString("StopTheTaskIfItRunsLongerThanComboBox.Items6") });
            resources.ApplyResources(StopTheTaskIfItRunsLongerThanComboBox, "StopTheTaskIfItRunsLongerThanComboBox");
            StopTheTaskIfItRunsLongerThanComboBox.Name = "StopTheTaskIfItRunsLongerThanComboBox";
            StopTheTaskIfItRunsLongerThanComboBox.SelectedIndexChanged += StopTheTaskIfItRunsLongerThanComboBox_SelectedIndexChanged;
            // 
            // TimesLabel
            // 
            resources.ApplyResources(TimesLabel, "TimesLabel");
            TimesLabel.Name = "TimesLabel";
            // 
            // AttemptToRestartUpToTextBox
            // 
            resources.ApplyResources(AttemptToRestartUpToTextBox, "AttemptToRestartUpToTextBox");
            AttemptToRestartUpToTextBox.Name = "AttemptToRestartUpToTextBox";
            AttemptToRestartUpToTextBox.TextChanged += AttemptToRestartUpToTextBox_TextChanged;
            AttemptToRestartUpToTextBox.KeyPress += textBox1_KeyPress;
            // 
            // IfTheTaskFailsRestartEveryComboBox
            // 
            IfTheTaskFailsRestartEveryComboBox.FormattingEnabled = true;
            IfTheTaskFailsRestartEveryComboBox.Items.AddRange(new object[] { resources.GetString("IfTheTaskFailsRestartEveryComboBox.Items"), resources.GetString("IfTheTaskFailsRestartEveryComboBox.Items1"), resources.GetString("IfTheTaskFailsRestartEveryComboBox.Items2"), resources.GetString("IfTheTaskFailsRestartEveryComboBox.Items3"), resources.GetString("IfTheTaskFailsRestartEveryComboBox.Items4"), resources.GetString("IfTheTaskFailsRestartEveryComboBox.Items5"), resources.GetString("IfTheTaskFailsRestartEveryComboBox.Items6") });
            resources.ApplyResources(IfTheTaskFailsRestartEveryComboBox, "IfTheTaskFailsRestartEveryComboBox");
            IfTheTaskFailsRestartEveryComboBox.Name = "IfTheTaskFailsRestartEveryComboBox";
            IfTheTaskFailsRestartEveryComboBox.SelectedIndexChanged += IfTheTaskFailsRestartEveryComboBox_SelectedIndexChanged;
            // 
            // AttemptToRestartUpToLabel
            // 
            resources.ApplyResources(AttemptToRestartUpToLabel, "AttemptToRestartUpToLabel");
            AttemptToRestartUpToLabel.Name = "AttemptToRestartUpToLabel";
            AttemptToRestartUpToLabel.Click += AttemptToRestartUpToLabel_Click;
            // 
            // IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox
            // 
            IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox.FormattingEnabled = true;
            IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox.Items.AddRange(new object[] { resources.GetString("IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox.Items"), resources.GetString("IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox.Items1"), resources.GetString("IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox.Items2"), resources.GetString("IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox.Items3") });
            resources.ApplyResources(IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox, "IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox");
            IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox.Name = "IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox";
            IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox.SelectedIndexChanged += IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox_SelectedIndexChanged;
            // 
            // IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel
            // 
            resources.ApplyResources(IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel, "IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel");
            IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel.Name = "IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel";
            IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel.Click += IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel_Click;
            // 
            // IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox
            // 
            resources.ApplyResources(IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox, "IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox");
            IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox.Name = "IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox";
            IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox.UseVisualStyleBackColor = true;
            IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox.CheckedChanged += IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox_CheckedChanged;
            // 
            // StopTheTaskIfItRunsLongerThanCheckBox
            // 
            resources.ApplyResources(StopTheTaskIfItRunsLongerThanCheckBox, "StopTheTaskIfItRunsLongerThanCheckBox");
            StopTheTaskIfItRunsLongerThanCheckBox.Name = "StopTheTaskIfItRunsLongerThanCheckBox";
            StopTheTaskIfItRunsLongerThanCheckBox.UseVisualStyleBackColor = true;
            StopTheTaskIfItRunsLongerThanCheckBox.CheckedChanged += StopTheTaskIfItRunsLongerThanCheckBox_CheckedChanged;
            // 
            // IfTheTaskFailsRestartEveryCheckBox
            // 
            resources.ApplyResources(IfTheTaskFailsRestartEveryCheckBox, "IfTheTaskFailsRestartEveryCheckBox");
            IfTheTaskFailsRestartEveryCheckBox.Name = "IfTheTaskFailsRestartEveryCheckBox";
            IfTheTaskFailsRestartEveryCheckBox.UseVisualStyleBackColor = true;
            IfTheTaskFailsRestartEveryCheckBox.CheckedChanged += IfTheTaskFailsRestartEveryCheckBox_CheckedChanged;
            // 
            // RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox
            // 
            resources.ApplyResources(RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox, "RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox");
            RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox.Name = "RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox";
            RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox.UseVisualStyleBackColor = true;
            RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox.CheckedChanged += RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox_CheckedChanged;
            // 
            // AllowTaskToBeRunOnDemandCheckBox
            // 
            resources.ApplyResources(AllowTaskToBeRunOnDemandCheckBox, "AllowTaskToBeRunOnDemandCheckBox");
            AllowTaskToBeRunOnDemandCheckBox.Name = "AllowTaskToBeRunOnDemandCheckBox";
            AllowTaskToBeRunOnDemandCheckBox.UseVisualStyleBackColor = true;
            AllowTaskToBeRunOnDemandCheckBox.CheckedChanged += AllowTaskToBeRunOnDemandCheckBox_CheckedChanged;
            // 
            // OpenTaskSchedulerButton
            // 
            resources.ApplyResources(OpenTaskSchedulerButton, "OpenTaskSchedulerButton");
            OpenTaskSchedulerButton.Name = "OpenTaskSchedulerButton";
            OpenTaskSchedulerButton.UseVisualStyleBackColor = true;
            OpenTaskSchedulerButton.Click += OpenTaskSchedulerButton_Click;
            // 
            // AdvancedScheduleSaveButton
            // 
            resources.ApplyResources(AdvancedScheduleSaveButton, "AdvancedScheduleSaveButton");
            AdvancedScheduleSaveButton.Name = "AdvancedScheduleSaveButton";
            AdvancedScheduleSaveButton.UseVisualStyleBackColor = true;
            AdvancedScheduleSaveButton.Click += AdvancedScheduleSaveButton_Click;
            // 
            // AdvancedScheduleForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(AdvancedScheduleSaveButton);
            Controls.Add(OpenTaskSchedulerButton);
            Controls.Add(groupBox2);
            Controls.Add(CloseButton);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "AdvancedScheduleForm";
            Load += ScheduleOptionsForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            GeneralSecurityOptionsGroupBox.ResumeLayout(false);
            GeneralSecurityOptionsGroupBox.PerformLayout();
            tabPage3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox DriveSelectionCheckBox;
        private System.Windows.Forms.Label DriveSortLabel;
        private System.Windows.Forms.ComboBox DriveSortComboBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox TwentyFourHourCheckBox;
        private System.Windows.Forms.CheckBox DetailedTimeModeCheckBox;
        private System.Windows.Forms.Button OpenTaskSchedulerButton;
        private System.Windows.Forms.RadioButton RunWhetherUserIsLoggedOnOrNotRadioButton;
        private System.Windows.Forms.RadioButton RunOnlyWhenUserIsLoggedOnRadioButton;
        private System.Windows.Forms.CheckBox RunWithHighestPrivilegesCheckBox;
        private System.Windows.Forms.CheckBox GeneralHiddenCheckBox;
        private System.Windows.Forms.GroupBox GeneralSecurityOptionsGroupBox;
        private System.Windows.Forms.CheckBox StartTheTaskOnlyIfTheComputerIsIdleForCheckBox;
        private System.Windows.Forms.CheckBox WakeTheComputerToRunThisTaskCheckBox;
        private System.Windows.Forms.CheckBox StopIfTheComputerSwitchesToBatteryPowerCheckBox;
        private System.Windows.Forms.CheckBox StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox;
        private System.Windows.Forms.CheckBox StopIfTheComputerCeasesToBeIdleCheckBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label WaitForIdleForLabel;
        private System.Windows.Forms.ComboBox StartTheTaskOnlyIfTheComputerIsIdleForComboBox;
        private System.Windows.Forms.ComboBox WaitForIdleForComboBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox;
        private System.Windows.Forms.CheckBox StopTheTaskIfItRunsLongerThanCheckBox;
        private System.Windows.Forms.CheckBox IfTheTaskFailsRestartEveryCheckBox;
        private System.Windows.Forms.CheckBox RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox;
        private System.Windows.Forms.CheckBox AllowTaskToBeRunOnDemandCheckBox;
        private System.Windows.Forms.Label IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel;
        private System.Windows.Forms.ComboBox IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox;
        private System.Windows.Forms.ComboBox IfTheTaskFailsRestartEveryComboBox;
        private System.Windows.Forms.Label AttemptToRestartUpToLabel;
        private System.Windows.Forms.Label TimesLabel;
        private System.Windows.Forms.TextBox AttemptToRestartUpToTextBox;
        private System.Windows.Forms.ComboBox StopTheTaskIfItRunsLongerThanComboBox;
        private System.Windows.Forms.Label GeneralDescriptionLabel;
        private System.Windows.Forms.TextBox GeneralDescriptionTextBox;
        private System.Windows.Forms.Button DefaultButton;
        private System.Windows.Forms.CheckBox RestartIfTheIdleStateResumesCheckBox;
        private System.Windows.Forms.Button AdvancedScheduleSaveButton;
    }
}