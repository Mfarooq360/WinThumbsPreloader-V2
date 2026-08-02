namespace WinThumbsPreloader.Forms
{
    partial class AdvancedSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSettingsForm));
            AdvancedSettingsGroupBox = new System.Windows.Forms.GroupBox();
            SaveButton = new System.Windows.Forms.Button();
            DefaultButton = new System.Windows.Forms.Button();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            ExtensionsAutoFormatLabel = new System.Windows.Forms.Label();
            PresetsComboBox = new System.Windows.Forms.ComboBox();
            PresetsLabel = new System.Windows.Forms.Label();
            ExtensionsAutoFormattingComboBox = new System.Windows.Forms.ComboBox();
            PreloaderThumbnailSizesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            RequestedThumbnailSizesLabel = new System.Windows.Forms.Label();
            PreloadFolderIconsForLabel = new System.Windows.Forms.Label();
            PreloadFolderIconsForComboBox = new System.Windows.Forms.ComboBox();
            tabPage2 = new System.Windows.Forms.TabPage();
            PreloaderProcessPriorityComboBox = new System.Windows.Forms.ComboBox();
            PreloaderProcessPriorityLabel = new System.Windows.Forms.Label();
            WaitCacheComboBox = new System.Windows.Forms.ComboBox();
            WaitCacheNumericUpDown = new System.Windows.Forms.NumericUpDown();
            WaitPreloadComboBox = new System.Windows.Forms.ComboBox();
            WaitPreloadNumericUpDown = new System.Windows.Forms.NumericUpDown();
            WaitAfterCacheBackupCheckBox = new System.Windows.Forms.CheckBox();
            ProgressDialogUpdateSpeedMsLabel = new System.Windows.Forms.Label();
            ProgressDialogUpdateSpeedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ProgressDialogUpdateSpeedLabel = new System.Windows.Forms.Label();
            WaitAfterPreloadingCompletionCheckBox = new System.Windows.Forms.CheckBox();
            tabPage3 = new System.Windows.Forms.TabPage();
            OutputTextBox = new System.Windows.Forms.TextBox();
            LoggingFrequencyLabel = new System.Windows.Forms.Label();
            OutputLabel = new System.Windows.Forms.Label();
            LoggingFrequencyComboBox = new System.Windows.Forms.ComboBox();
            LoggerFolderStatusLabel = new System.Windows.Forms.Label();
            LogsFolderLabel = new System.Windows.Forms.Label();
            LogRetentionDaysLabel = new System.Windows.Forms.Label();
            LogRetentionDaysNumericUpDown = new System.Windows.Forms.NumericUpDown();
            AutoDeleteLogsByAgeCheckBox = new System.Windows.Forms.CheckBox();
            ClearLoggerFolderButton = new System.Windows.Forms.Button();
            DefaultLoggerFolderButton = new System.Windows.Forms.Button();
            BrowseLoggerFolderButton = new System.Windows.Forms.Button();
            LoggerFolderPathTextBox = new System.Windows.Forms.TextBox();
            LogsSizeLabel = new System.Windows.Forms.Label();
            LogButton = new System.Windows.Forms.Button();
            CloseButton = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            AdvancedSettingsGroupBox.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)WaitCacheNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)WaitPreloadNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ProgressDialogUpdateSpeedNumericUpDown).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LogRetentionDaysNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // AdvancedSettingsGroupBox
            // 
            AdvancedSettingsGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            AdvancedSettingsGroupBox.Controls.Add(SaveButton);
            AdvancedSettingsGroupBox.Controls.Add(DefaultButton);
            AdvancedSettingsGroupBox.Controls.Add(tabControl1);
            resources.ApplyResources(AdvancedSettingsGroupBox, "AdvancedSettingsGroupBox");
            AdvancedSettingsGroupBox.Name = "AdvancedSettingsGroupBox";
            AdvancedSettingsGroupBox.TabStop = false;
            // 
            // SaveButton
            // 
            resources.ApplyResources(SaveButton, "SaveButton");
            SaveButton.Name = "SaveButton";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // DefaultButton
            // 
            resources.ApplyResources(DefaultButton, "DefaultButton");
            DefaultButton.Name = "DefaultButton";
            DefaultButton.UseVisualStyleBackColor = true;
            DefaultButton.Click += DefaultButton_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            resources.ApplyResources(tabControl1, "tabControl1");
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(ExtensionsAutoFormatLabel);
            tabPage1.Controls.Add(PresetsComboBox);
            tabPage1.Controls.Add(PresetsLabel);
            tabPage1.Controls.Add(ExtensionsAutoFormattingComboBox);
            tabPage1.Controls.Add(PreloaderThumbnailSizesCheckedListBox);
            tabPage1.Controls.Add(RequestedThumbnailSizesLabel);
            tabPage1.Controls.Add(PreloadFolderIconsForLabel);
            tabPage1.Controls.Add(PreloadFolderIconsForComboBox);
            resources.ApplyResources(tabPage1, "tabPage1");
            tabPage1.Name = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // ExtensionsAutoFormatLabel
            // 
            resources.ApplyResources(ExtensionsAutoFormatLabel, "ExtensionsAutoFormatLabel");
            ExtensionsAutoFormatLabel.Name = "ExtensionsAutoFormatLabel";
            toolTip1.SetToolTip(ExtensionsAutoFormatLabel, resources.GetString("ExtensionsAutoFormatLabel.ToolTip"));
            // 
            // PresetsComboBox
            // 
            PresetsComboBox.FormattingEnabled = true;
            PresetsComboBox.Items.AddRange(new object[] { resources.GetString("PresetsComboBox.Items"), resources.GetString("PresetsComboBox.Items1"), resources.GetString("PresetsComboBox.Items2") });
            resources.ApplyResources(PresetsComboBox, "PresetsComboBox");
            PresetsComboBox.Name = "PresetsComboBox";
            PresetsComboBox.SelectedIndexChanged += PresetsComboBox_SelectedIndexChanged;
            // 
            // PresetsLabel
            // 
            resources.ApplyResources(PresetsLabel, "PresetsLabel");
            PresetsLabel.Name = "PresetsLabel";
            // 
            // ExtensionsAutoFormattingComboBox
            // 
            ExtensionsAutoFormattingComboBox.FormattingEnabled = true;
            ExtensionsAutoFormattingComboBox.Items.AddRange(new object[] { resources.GetString("ExtensionsAutoFormattingComboBox.Items"), resources.GetString("ExtensionsAutoFormattingComboBox.Items1"), resources.GetString("ExtensionsAutoFormattingComboBox.Items2"), resources.GetString("ExtensionsAutoFormattingComboBox.Items3"), resources.GetString("ExtensionsAutoFormattingComboBox.Items4") });
            resources.ApplyResources(ExtensionsAutoFormattingComboBox, "ExtensionsAutoFormattingComboBox");
            ExtensionsAutoFormattingComboBox.Name = "ExtensionsAutoFormattingComboBox";
            // 
            // PreloaderThumbnailSizesCheckedListBox
            // 
            PreloaderThumbnailSizesCheckedListBox.CheckOnClick = true;
            resources.ApplyResources(PreloaderThumbnailSizesCheckedListBox, "PreloaderThumbnailSizesCheckedListBox");
            PreloaderThumbnailSizesCheckedListBox.FormattingEnabled = true;
            PreloaderThumbnailSizesCheckedListBox.Items.AddRange(new object[] { resources.GetString("PreloaderThumbnailSizesCheckedListBox.Items"), resources.GetString("PreloaderThumbnailSizesCheckedListBox.Items1"), resources.GetString("PreloaderThumbnailSizesCheckedListBox.Items2"), resources.GetString("PreloaderThumbnailSizesCheckedListBox.Items3"), resources.GetString("PreloaderThumbnailSizesCheckedListBox.Items4"), resources.GetString("PreloaderThumbnailSizesCheckedListBox.Items5"), resources.GetString("PreloaderThumbnailSizesCheckedListBox.Items6"), resources.GetString("PreloaderThumbnailSizesCheckedListBox.Items7"), resources.GetString("PreloaderThumbnailSizesCheckedListBox.Items8") });
            PreloaderThumbnailSizesCheckedListBox.MultiColumn = true;
            PreloaderThumbnailSizesCheckedListBox.Name = "PreloaderThumbnailSizesCheckedListBox";
            // 
            // RequestedThumbnailSizesLabel
            // 
            resources.ApplyResources(RequestedThumbnailSizesLabel, "RequestedThumbnailSizesLabel");
            RequestedThumbnailSizesLabel.Name = "RequestedThumbnailSizesLabel";
            toolTip1.SetToolTip(RequestedThumbnailSizesLabel, resources.GetString("RequestedThumbnailSizesLabel.ToolTip"));
            // 
            // PreloadFolderIconsForLabel
            // 
            resources.ApplyResources(PreloadFolderIconsForLabel, "PreloadFolderIconsForLabel");
            PreloadFolderIconsForLabel.Name = "PreloadFolderIconsForLabel";
            toolTip1.SetToolTip(PreloadFolderIconsForLabel, resources.GetString("PreloadFolderIconsForLabel.ToolTip"));
            // 
            // PreloadFolderIconsForComboBox
            // 
            PreloadFolderIconsForComboBox.FormattingEnabled = true;
            PreloadFolderIconsForComboBox.Items.AddRange(new object[] { resources.GetString("PreloadFolderIconsForComboBox.Items"), resources.GetString("PreloadFolderIconsForComboBox.Items1") });
            resources.ApplyResources(PreloadFolderIconsForComboBox, "PreloadFolderIconsForComboBox");
            PreloadFolderIconsForComboBox.Name = "PreloadFolderIconsForComboBox";
            PreloadFolderIconsForComboBox.SelectedIndexChanged += PreloadFolderIconsForComboBox_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(PreloaderProcessPriorityComboBox);
            tabPage2.Controls.Add(PreloaderProcessPriorityLabel);
            tabPage2.Controls.Add(WaitCacheComboBox);
            tabPage2.Controls.Add(WaitCacheNumericUpDown);
            tabPage2.Controls.Add(WaitPreloadComboBox);
            tabPage2.Controls.Add(WaitPreloadNumericUpDown);
            tabPage2.Controls.Add(WaitAfterCacheBackupCheckBox);
            tabPage2.Controls.Add(ProgressDialogUpdateSpeedMsLabel);
            tabPage2.Controls.Add(ProgressDialogUpdateSpeedNumericUpDown);
            tabPage2.Controls.Add(ProgressDialogUpdateSpeedLabel);
            tabPage2.Controls.Add(WaitAfterPreloadingCompletionCheckBox);
            resources.ApplyResources(tabPage2, "tabPage2");
            tabPage2.Name = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // PreloaderProcessPriorityComboBox
            // 
            PreloaderProcessPriorityComboBox.FormattingEnabled = true;
            PreloaderProcessPriorityComboBox.Items.AddRange(new object[] { resources.GetString("PreloaderProcessPriorityComboBox.Items"), resources.GetString("PreloaderProcessPriorityComboBox.Items1"), resources.GetString("PreloaderProcessPriorityComboBox.Items2"), resources.GetString("PreloaderProcessPriorityComboBox.Items3"), resources.GetString("PreloaderProcessPriorityComboBox.Items4"), resources.GetString("PreloaderProcessPriorityComboBox.Items5") });
            resources.ApplyResources(PreloaderProcessPriorityComboBox, "PreloaderProcessPriorityComboBox");
            PreloaderProcessPriorityComboBox.Name = "PreloaderProcessPriorityComboBox";
            PreloaderProcessPriorityComboBox.SelectedIndexChanged += PreloaderProcessPriorityComboBox_SelectedIndexChanged;
            // 
            // PreloaderProcessPriorityLabel
            // 
            resources.ApplyResources(PreloaderProcessPriorityLabel, "PreloaderProcessPriorityLabel");
            PreloaderProcessPriorityLabel.Name = "PreloaderProcessPriorityLabel";
            toolTip1.SetToolTip(PreloaderProcessPriorityLabel, resources.GetString("PreloaderProcessPriorityLabel.ToolTip"));
            // 
            // WaitCacheComboBox
            // 
            WaitCacheComboBox.FormattingEnabled = true;
            WaitCacheComboBox.Items.AddRange(new object[] { resources.GetString("WaitCacheComboBox.Items"), resources.GetString("WaitCacheComboBox.Items1"), resources.GetString("WaitCacheComboBox.Items2") });
            resources.ApplyResources(WaitCacheComboBox, "WaitCacheComboBox");
            WaitCacheComboBox.Name = "WaitCacheComboBox";
            WaitCacheComboBox.SelectedIndexChanged += WaitCacheComboBox_SelectedIndexChanged;
            // 
            // WaitCacheNumericUpDown
            // 
            resources.ApplyResources(WaitCacheNumericUpDown, "WaitCacheNumericUpDown");
            WaitCacheNumericUpDown.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            WaitCacheNumericUpDown.Name = "WaitCacheNumericUpDown";
            WaitCacheNumericUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // WaitPreloadComboBox
            // 
            WaitPreloadComboBox.FormattingEnabled = true;
            WaitPreloadComboBox.Items.AddRange(new object[] { resources.GetString("WaitPreloadComboBox.Items"), resources.GetString("WaitPreloadComboBox.Items1"), resources.GetString("WaitPreloadComboBox.Items2") });
            resources.ApplyResources(WaitPreloadComboBox, "WaitPreloadComboBox");
            WaitPreloadComboBox.Name = "WaitPreloadComboBox";
            WaitPreloadComboBox.SelectedIndexChanged += WaitPreloadComboBox_SelectedIndexChanged;
            // 
            // WaitPreloadNumericUpDown
            // 
            resources.ApplyResources(WaitPreloadNumericUpDown, "WaitPreloadNumericUpDown");
            WaitPreloadNumericUpDown.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            WaitPreloadNumericUpDown.Name = "WaitPreloadNumericUpDown";
            WaitPreloadNumericUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // WaitAfterCacheBackupCheckBox
            // 
            resources.ApplyResources(WaitAfterCacheBackupCheckBox, "WaitAfterCacheBackupCheckBox");
            WaitAfterCacheBackupCheckBox.Checked = true;
            WaitAfterCacheBackupCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            WaitAfterCacheBackupCheckBox.Name = "WaitAfterCacheBackupCheckBox";
            toolTip1.SetToolTip(WaitAfterCacheBackupCheckBox, resources.GetString("WaitAfterCacheBackupCheckBox.ToolTip"));
            WaitAfterCacheBackupCheckBox.UseVisualStyleBackColor = true;
            // 
            // ProgressDialogUpdateSpeedMsLabel
            // 
            resources.ApplyResources(ProgressDialogUpdateSpeedMsLabel, "ProgressDialogUpdateSpeedMsLabel");
            ProgressDialogUpdateSpeedMsLabel.Name = "ProgressDialogUpdateSpeedMsLabel";
            // 
            // ProgressDialogUpdateSpeedNumericUpDown
            // 
            ProgressDialogUpdateSpeedNumericUpDown.Increment = new decimal(new int[] { 50, 0, 0, 0 });
            resources.ApplyResources(ProgressDialogUpdateSpeedNumericUpDown, "ProgressDialogUpdateSpeedNumericUpDown");
            ProgressDialogUpdateSpeedNumericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            ProgressDialogUpdateSpeedNumericUpDown.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            ProgressDialogUpdateSpeedNumericUpDown.Name = "ProgressDialogUpdateSpeedNumericUpDown";
            ProgressDialogUpdateSpeedNumericUpDown.Value = new decimal(new int[] { 250, 0, 0, 0 });
            // 
            // ProgressDialogUpdateSpeedLabel
            // 
            resources.ApplyResources(ProgressDialogUpdateSpeedLabel, "ProgressDialogUpdateSpeedLabel");
            ProgressDialogUpdateSpeedLabel.Name = "ProgressDialogUpdateSpeedLabel";
            toolTip1.SetToolTip(ProgressDialogUpdateSpeedLabel, resources.GetString("ProgressDialogUpdateSpeedLabel.ToolTip"));
            // 
            // WaitAfterPreloadingCompletionCheckBox
            // 
            resources.ApplyResources(WaitAfterPreloadingCompletionCheckBox, "WaitAfterPreloadingCompletionCheckBox");
            WaitAfterPreloadingCompletionCheckBox.Checked = true;
            WaitAfterPreloadingCompletionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            WaitAfterPreloadingCompletionCheckBox.Name = "WaitAfterPreloadingCompletionCheckBox";
            toolTip1.SetToolTip(WaitAfterPreloadingCompletionCheckBox, resources.GetString("WaitAfterPreloadingCompletionCheckBox.ToolTip"));
            WaitAfterPreloadingCompletionCheckBox.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(OutputTextBox);
            tabPage3.Controls.Add(LoggingFrequencyLabel);
            tabPage3.Controls.Add(OutputLabel);
            tabPage3.Controls.Add(LoggingFrequencyComboBox);
            tabPage3.Controls.Add(LoggerFolderStatusLabel);
            tabPage3.Controls.Add(LogsFolderLabel);
            tabPage3.Controls.Add(LogRetentionDaysLabel);
            tabPage3.Controls.Add(LogRetentionDaysNumericUpDown);
            tabPage3.Controls.Add(AutoDeleteLogsByAgeCheckBox);
            tabPage3.Controls.Add(ClearLoggerFolderButton);
            tabPage3.Controls.Add(DefaultLoggerFolderButton);
            tabPage3.Controls.Add(BrowseLoggerFolderButton);
            tabPage3.Controls.Add(LoggerFolderPathTextBox);
            resources.ApplyResources(tabPage3, "tabPage3");
            tabPage3.Name = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // OutputTextBox
            // 
            resources.ApplyResources(OutputTextBox, "OutputTextBox");
            OutputTextBox.Name = "OutputTextBox";
            OutputTextBox.ReadOnly = true;
            // 
            // LoggingFrequencyLabel
            // 
            resources.ApplyResources(LoggingFrequencyLabel, "LoggingFrequencyLabel");
            LoggingFrequencyLabel.Name = "LoggingFrequencyLabel";
            toolTip1.SetToolTip(LoggingFrequencyLabel, resources.GetString("LoggingFrequencyLabel.ToolTip"));
            // 
            // OutputLabel
            // 
            resources.ApplyResources(OutputLabel, "OutputLabel");
            OutputLabel.Name = "OutputLabel";
            // 
            // LoggingFrequencyComboBox
            // 
            LoggingFrequencyComboBox.FormattingEnabled = true;
            LoggingFrequencyComboBox.Items.AddRange(new object[] { resources.GetString("LoggingFrequencyComboBox.Items"), resources.GetString("LoggingFrequencyComboBox.Items1"), resources.GetString("LoggingFrequencyComboBox.Items2"), resources.GetString("LoggingFrequencyComboBox.Items3"), resources.GetString("LoggingFrequencyComboBox.Items4") });
            resources.ApplyResources(LoggingFrequencyComboBox, "LoggingFrequencyComboBox");
            LoggingFrequencyComboBox.Name = "LoggingFrequencyComboBox";
            LoggingFrequencyComboBox.SelectedIndexChanged += LoggingFrequencyComboBox_SelectedIndexChanged;
            // 
            // LoggerFolderStatusLabel
            // 
            resources.ApplyResources(LoggerFolderStatusLabel, "LoggerFolderStatusLabel");
            LoggerFolderStatusLabel.Name = "LoggerFolderStatusLabel";
            // 
            // LogsFolderLabel
            // 
            resources.ApplyResources(LogsFolderLabel, "LogsFolderLabel");
            LogsFolderLabel.Name = "LogsFolderLabel";
            // 
            // LogRetentionDaysLabel
            // 
            resources.ApplyResources(LogRetentionDaysLabel, "LogRetentionDaysLabel");
            LogRetentionDaysLabel.Name = "LogRetentionDaysLabel";
            // 
            // LogRetentionDaysNumericUpDown
            // 
            resources.ApplyResources(LogRetentionDaysNumericUpDown, "LogRetentionDaysNumericUpDown");
            LogRetentionDaysNumericUpDown.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            LogRetentionDaysNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            LogRetentionDaysNumericUpDown.Name = "LogRetentionDaysNumericUpDown";
            LogRetentionDaysNumericUpDown.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // AutoDeleteLogsByAgeCheckBox
            // 
            resources.ApplyResources(AutoDeleteLogsByAgeCheckBox, "AutoDeleteLogsByAgeCheckBox");
            AutoDeleteLogsByAgeCheckBox.Name = "AutoDeleteLogsByAgeCheckBox";
            toolTip1.SetToolTip(AutoDeleteLogsByAgeCheckBox, resources.GetString("AutoDeleteLogsByAgeCheckBox.ToolTip"));
            AutoDeleteLogsByAgeCheckBox.UseVisualStyleBackColor = true;
            // 
            // ClearLoggerFolderButton
            // 
            resources.ApplyResources(ClearLoggerFolderButton, "ClearLoggerFolderButton");
            ClearLoggerFolderButton.Name = "ClearLoggerFolderButton";
            toolTip1.SetToolTip(ClearLoggerFolderButton, resources.GetString("ClearLoggerFolderButton.ToolTip"));
            ClearLoggerFolderButton.UseVisualStyleBackColor = true;
            // 
            // DefaultLoggerFolderButton
            // 
            resources.ApplyResources(DefaultLoggerFolderButton, "DefaultLoggerFolderButton");
            DefaultLoggerFolderButton.Name = "DefaultLoggerFolderButton";
            toolTip1.SetToolTip(DefaultLoggerFolderButton, resources.GetString("DefaultLoggerFolderButton.ToolTip"));
            DefaultLoggerFolderButton.UseVisualStyleBackColor = true;
            // 
            // BrowseLoggerFolderButton
            // 
            resources.ApplyResources(BrowseLoggerFolderButton, "BrowseLoggerFolderButton");
            BrowseLoggerFolderButton.Name = "BrowseLoggerFolderButton";
            BrowseLoggerFolderButton.UseVisualStyleBackColor = true;
            // 
            // LoggerFolderPathTextBox
            // 
            resources.ApplyResources(LoggerFolderPathTextBox, "LoggerFolderPathTextBox");
            LoggerFolderPathTextBox.Name = "LoggerFolderPathTextBox";
            LoggerFolderPathTextBox.ReadOnly = true;
            // 
            // LogsSizeLabel
            // 
            resources.ApplyResources(LogsSizeLabel, "LogsSizeLabel");
            LogsSizeLabel.Name = "LogsSizeLabel";
            // 
            // LogButton
            // 
            resources.ApplyResources(LogButton, "LogButton");
            LogButton.Name = "LogButton";
            toolTip1.SetToolTip(LogButton, resources.GetString("LogButton.ToolTip"));
            LogButton.UseVisualStyleBackColor = true;
            LogButton.Click += LogButton_Click;
            // 
            // CloseButton
            // 
            resources.ApplyResources(CloseButton, "CloseButton");
            CloseButton.Name = "CloseButton";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // AdvancedSettingsForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(LogsSizeLabel);
            Controls.Add(CloseButton);
            Controls.Add(LogButton);
            Controls.Add(AdvancedSettingsGroupBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "AdvancedSettingsForm";
            Load += AdvancedSettingsForm_Load;
            AdvancedSettingsGroupBox.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)WaitCacheNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)WaitPreloadNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)ProgressDialogUpdateSpeedNumericUpDown).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LogRetentionDaysNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox AdvancedSettingsGroupBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ComboBox PreloadFolderIconsForComboBox;
        private System.Windows.Forms.Label PreloadFolderIconsForLabel;
        private System.Windows.Forms.Label ExtensionsAutoFormatLabel;
        private System.Windows.Forms.ComboBox ExtensionsAutoFormattingComboBox;
        private System.Windows.Forms.Button DefaultButton;
        private System.Windows.Forms.Label RequestedThumbnailSizesLabel;
        private System.Windows.Forms.CheckedListBox PreloaderThumbnailSizesCheckedListBox;
        private System.Windows.Forms.ComboBox PresetsComboBox;
        private System.Windows.Forms.Label PresetsLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox LoggingFrequencyComboBox;
        private System.Windows.Forms.Label LoggingFrequencyLabel;
        private System.Windows.Forms.Button LogButton;
        private System.Windows.Forms.Label LogsSizeLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox AutoDeleteLogsByAgeCheckBox;
        private System.Windows.Forms.NumericUpDown ProgressDialogUpdateSpeedNumericUpDown;
        private System.Windows.Forms.Label ProgressDialogUpdateSpeedMsLabel;
        private System.Windows.Forms.Label ProgressDialogUpdateSpeedLabel;
        private System.Windows.Forms.CheckBox WaitAfterPreloadingCompletionCheckBox;
        private System.Windows.Forms.CheckBox WaitAfterCacheBackupCheckBox;
        private System.Windows.Forms.ComboBox WaitPreloadComboBox;
        private System.Windows.Forms.NumericUpDown WaitPreloadNumericUpDown;
        private System.Windows.Forms.ComboBox WaitCacheComboBox;
        private System.Windows.Forms.NumericUpDown WaitCacheNumericUpDown;
        private System.Windows.Forms.Label PreloaderProcessPriorityLabel;
        private System.Windows.Forms.ComboBox PreloaderProcessPriorityComboBox;
        private System.Windows.Forms.TextBox LoggerFolderPathTextBox;
        private System.Windows.Forms.Button BrowseLoggerFolderButton;
        private System.Windows.Forms.Button DefaultLoggerFolderButton;
        private System.Windows.Forms.Button ClearLoggerFolderButton;
        private System.Windows.Forms.NumericUpDown LogRetentionDaysNumericUpDown;
        private System.Windows.Forms.Label LogRetentionDaysLabel;
        private System.Windows.Forms.Label LogsFolderLabel;
        private System.Windows.Forms.Label LoggerFolderStatusLabel;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.Button SaveButton;
    }
}