namespace WinThumbsPreloader.Forms
{
    partial class AdvancedCacheForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedCacheForm));
            groupBox1 = new System.Windows.Forms.GroupBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            BackupTabPage = new System.Windows.Forms.TabPage();
            StatusLabel = new System.Windows.Forms.Label();
            OutputTextBox1 = new System.Windows.Forms.TextBox();
            OutputTextBox = new System.Windows.Forms.TextBox();
            OutputLabel = new System.Windows.Forms.Label();
            BackupFolderLabel = new System.Windows.Forms.Label();
            ClearBackupFolderButton = new System.Windows.Forms.Button();
            DefaultBackupFolderButton = new System.Windows.Forms.Button();
            BrowseBackupFolderButton = new System.Windows.Forms.Button();
            BackupFolderPathTextBox = new System.Windows.Forms.TextBox();
            CacheTabPage = new System.Windows.Forms.TabPage();
            AutoBackupAfterPreloadCheckBox = new System.Windows.Forms.CheckBox();
            ExplorerCloseFrequencyComboBox = new System.Windows.Forms.ComboBox();
            BackupRestoreClearSafetyComboBox = new System.Windows.Forms.ComboBox();
            ExplorerCacheDeletionMethodLabel = new System.Windows.Forms.Label();
            ExplorerCacheDeletionMethodComboBox = new System.Windows.Forms.ComboBox();
            ExplorerCloseFrequencyLabel = new System.Windows.Forms.Label();
            BackupRestoreClearSafetyLabel = new System.Windows.Forms.Label();
            GeneralTabPage = new System.Windows.Forms.TabPage();
            AutoBackupIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            AutoBackupIntervalLabel = new System.Windows.Forms.Label();
            CacheSizeUpdateIntervalLabel = new System.Windows.Forms.Label();
            BackupSecondsLabel = new System.Windows.Forms.Label();
            CacheSizeUpdateIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            CacheSizeFormatLabel = new System.Windows.Forms.Label();
            AutoRestoreIntervalLabel = new System.Windows.Forms.Label();
            MilisecondsLabel = new System.Windows.Forms.Label();
            AutoRestoreIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            CacheSizeFormatComboBox = new System.Windows.Forms.ComboBox();
            RestoreSecondsLabel = new System.Windows.Forms.Label();
            SaveButton = new System.Windows.Forms.Button();
            DefaultButton = new System.Windows.Forms.Button();
            CloseButton = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            groupBox1.SuspendLayout();
            tabControl1.SuspendLayout();
            BackupTabPage.SuspendLayout();
            CacheTabPage.SuspendLayout();
            GeneralTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AutoBackupIntervalNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CacheSizeUpdateIntervalNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AutoRestoreIntervalNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            groupBox1.Controls.Add(tabControl1);
            groupBox1.Controls.Add(SaveButton);
            groupBox1.Controls.Add(DefaultButton);
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(BackupTabPage);
            tabControl1.Controls.Add(CacheTabPage);
            tabControl1.Controls.Add(GeneralTabPage);
            resources.ApplyResources(tabControl1, "tabControl1");
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            // 
            // BackupTabPage
            // 
            BackupTabPage.Controls.Add(StatusLabel);
            BackupTabPage.Controls.Add(OutputTextBox1);
            BackupTabPage.Controls.Add(OutputTextBox);
            BackupTabPage.Controls.Add(OutputLabel);
            BackupTabPage.Controls.Add(BackupFolderLabel);
            BackupTabPage.Controls.Add(ClearBackupFolderButton);
            BackupTabPage.Controls.Add(DefaultBackupFolderButton);
            BackupTabPage.Controls.Add(BrowseBackupFolderButton);
            BackupTabPage.Controls.Add(BackupFolderPathTextBox);
            resources.ApplyResources(BackupTabPage, "BackupTabPage");
            BackupTabPage.Name = "BackupTabPage";
            BackupTabPage.UseVisualStyleBackColor = true;
            // 
            // StatusLabel
            // 
            resources.ApplyResources(StatusLabel, "StatusLabel");
            StatusLabel.Name = "StatusLabel";
            // 
            // OutputTextBox1
            // 
            resources.ApplyResources(OutputTextBox1, "OutputTextBox1");
            OutputTextBox1.Name = "OutputTextBox1";
            OutputTextBox1.ReadOnly = true;
            // 
            // OutputTextBox
            // 
            resources.ApplyResources(OutputTextBox, "OutputTextBox");
            OutputTextBox.Name = "OutputTextBox";
            OutputTextBox.ReadOnly = true;
            // 
            // OutputLabel
            // 
            resources.ApplyResources(OutputLabel, "OutputLabel");
            OutputLabel.Name = "OutputLabel";
            // 
            // BackupFolderLabel
            // 
            resources.ApplyResources(BackupFolderLabel, "BackupFolderLabel");
            BackupFolderLabel.Name = "BackupFolderLabel";
            // 
            // ClearBackupFolderButton
            // 
            resources.ApplyResources(ClearBackupFolderButton, "ClearBackupFolderButton");
            ClearBackupFolderButton.Name = "ClearBackupFolderButton";
            toolTip1.SetToolTip(ClearBackupFolderButton, resources.GetString("ClearBackupFolderButton.ToolTip"));
            ClearBackupFolderButton.UseVisualStyleBackColor = true;
            ClearBackupFolderButton.Click += ClearBackupFolderButton_Click;
            // 
            // DefaultBackupFolderButton
            // 
            resources.ApplyResources(DefaultBackupFolderButton, "DefaultBackupFolderButton");
            DefaultBackupFolderButton.Name = "DefaultBackupFolderButton";
            toolTip1.SetToolTip(DefaultBackupFolderButton, resources.GetString("DefaultBackupFolderButton.ToolTip"));
            DefaultBackupFolderButton.UseVisualStyleBackColor = true;
            DefaultBackupFolderButton.Click += DefaultBackupFolderButton_Click;
            // 
            // BrowseBackupFolderButton
            // 
            resources.ApplyResources(BrowseBackupFolderButton, "BrowseBackupFolderButton");
            BrowseBackupFolderButton.Name = "BrowseBackupFolderButton";
            BrowseBackupFolderButton.UseVisualStyleBackColor = true;
            BrowseBackupFolderButton.Click += BrowseBackupFolderButton_Click;
            // 
            // BackupFolderPathTextBox
            // 
            resources.ApplyResources(BackupFolderPathTextBox, "BackupFolderPathTextBox");
            BackupFolderPathTextBox.Name = "BackupFolderPathTextBox";
            BackupFolderPathTextBox.ReadOnly = true;
            BackupFolderPathTextBox.TextChanged += BackupFolderPathTextBox_TextChanged;
            // 
            // CacheTabPage
            // 
            CacheTabPage.Controls.Add(AutoBackupAfterPreloadCheckBox);
            CacheTabPage.Controls.Add(ExplorerCloseFrequencyComboBox);
            CacheTabPage.Controls.Add(BackupRestoreClearSafetyComboBox);
            CacheTabPage.Controls.Add(ExplorerCacheDeletionMethodLabel);
            CacheTabPage.Controls.Add(ExplorerCacheDeletionMethodComboBox);
            CacheTabPage.Controls.Add(ExplorerCloseFrequencyLabel);
            CacheTabPage.Controls.Add(BackupRestoreClearSafetyLabel);
            resources.ApplyResources(CacheTabPage, "CacheTabPage");
            CacheTabPage.Name = "CacheTabPage";
            CacheTabPage.UseVisualStyleBackColor = true;
            // 
            // AutoBackupAfterPreloadCheckBox
            // 
            resources.ApplyResources(AutoBackupAfterPreloadCheckBox, "AutoBackupAfterPreloadCheckBox");
            AutoBackupAfterPreloadCheckBox.Name = "AutoBackupAfterPreloadCheckBox";
            toolTip1.SetToolTip(AutoBackupAfterPreloadCheckBox, resources.GetString("AutoBackupAfterPreloadCheckBox.ToolTip"));
            AutoBackupAfterPreloadCheckBox.UseVisualStyleBackColor = true;
            // 
            // ExplorerCloseFrequencyComboBox
            // 
            ExplorerCloseFrequencyComboBox.FormattingEnabled = true;
            ExplorerCloseFrequencyComboBox.Items.AddRange(new object[] { resources.GetString("ExplorerCloseFrequencyComboBox.Items"), resources.GetString("ExplorerCloseFrequencyComboBox.Items1") });
            resources.ApplyResources(ExplorerCloseFrequencyComboBox, "ExplorerCloseFrequencyComboBox");
            ExplorerCloseFrequencyComboBox.Name = "ExplorerCloseFrequencyComboBox";
            // 
            // BackupRestoreClearSafetyComboBox
            // 
            BackupRestoreClearSafetyComboBox.FormattingEnabled = true;
            BackupRestoreClearSafetyComboBox.Items.AddRange(new object[] { resources.GetString("BackupRestoreClearSafetyComboBox.Items"), resources.GetString("BackupRestoreClearSafetyComboBox.Items1") });
            resources.ApplyResources(BackupRestoreClearSafetyComboBox, "BackupRestoreClearSafetyComboBox");
            BackupRestoreClearSafetyComboBox.Name = "BackupRestoreClearSafetyComboBox";
            // 
            // ExplorerCacheDeletionMethodLabel
            // 
            resources.ApplyResources(ExplorerCacheDeletionMethodLabel, "ExplorerCacheDeletionMethodLabel");
            ExplorerCacheDeletionMethodLabel.Name = "ExplorerCacheDeletionMethodLabel";
            toolTip1.SetToolTip(ExplorerCacheDeletionMethodLabel, resources.GetString("ExplorerCacheDeletionMethodLabel.ToolTip"));
            // 
            // ExplorerCacheDeletionMethodComboBox
            // 
            ExplorerCacheDeletionMethodComboBox.FormattingEnabled = true;
            ExplorerCacheDeletionMethodComboBox.Items.AddRange(new object[] { resources.GetString("ExplorerCacheDeletionMethodComboBox.Items"), resources.GetString("ExplorerCacheDeletionMethodComboBox.Items1") });
            resources.ApplyResources(ExplorerCacheDeletionMethodComboBox, "ExplorerCacheDeletionMethodComboBox");
            ExplorerCacheDeletionMethodComboBox.Name = "ExplorerCacheDeletionMethodComboBox";
            // 
            // ExplorerCloseFrequencyLabel
            // 
            resources.ApplyResources(ExplorerCloseFrequencyLabel, "ExplorerCloseFrequencyLabel");
            ExplorerCloseFrequencyLabel.Name = "ExplorerCloseFrequencyLabel";
            toolTip1.SetToolTip(ExplorerCloseFrequencyLabel, resources.GetString("ExplorerCloseFrequencyLabel.ToolTip"));
            // 
            // BackupRestoreClearSafetyLabel
            // 
            resources.ApplyResources(BackupRestoreClearSafetyLabel, "BackupRestoreClearSafetyLabel");
            BackupRestoreClearSafetyLabel.Name = "BackupRestoreClearSafetyLabel";
            toolTip1.SetToolTip(BackupRestoreClearSafetyLabel, resources.GetString("BackupRestoreClearSafetyLabel.ToolTip"));
            // 
            // GeneralTabPage
            // 
            GeneralTabPage.Controls.Add(AutoBackupIntervalNumericUpDown);
            GeneralTabPage.Controls.Add(AutoBackupIntervalLabel);
            GeneralTabPage.Controls.Add(CacheSizeUpdateIntervalLabel);
            GeneralTabPage.Controls.Add(BackupSecondsLabel);
            GeneralTabPage.Controls.Add(CacheSizeUpdateIntervalNumericUpDown);
            GeneralTabPage.Controls.Add(CacheSizeFormatLabel);
            GeneralTabPage.Controls.Add(AutoRestoreIntervalLabel);
            GeneralTabPage.Controls.Add(MilisecondsLabel);
            GeneralTabPage.Controls.Add(AutoRestoreIntervalNumericUpDown);
            GeneralTabPage.Controls.Add(CacheSizeFormatComboBox);
            GeneralTabPage.Controls.Add(RestoreSecondsLabel);
            resources.ApplyResources(GeneralTabPage, "GeneralTabPage");
            GeneralTabPage.Name = "GeneralTabPage";
            GeneralTabPage.UseVisualStyleBackColor = true;
            // 
            // AutoBackupIntervalNumericUpDown
            // 
            resources.ApplyResources(AutoBackupIntervalNumericUpDown, "AutoBackupIntervalNumericUpDown");
            AutoBackupIntervalNumericUpDown.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            AutoBackupIntervalNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            AutoBackupIntervalNumericUpDown.Name = "AutoBackupIntervalNumericUpDown";
            AutoBackupIntervalNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            AutoBackupIntervalNumericUpDown.ValueChanged += AutoBackupIntervalNumericUpDown_ValueChanged;
            // 
            // AutoBackupIntervalLabel
            // 
            resources.ApplyResources(AutoBackupIntervalLabel, "AutoBackupIntervalLabel");
            AutoBackupIntervalLabel.Name = "AutoBackupIntervalLabel";
            toolTip1.SetToolTip(AutoBackupIntervalLabel, resources.GetString("AutoBackupIntervalLabel.ToolTip"));
            // 
            // CacheSizeUpdateIntervalLabel
            // 
            resources.ApplyResources(CacheSizeUpdateIntervalLabel, "CacheSizeUpdateIntervalLabel");
            CacheSizeUpdateIntervalLabel.Name = "CacheSizeUpdateIntervalLabel";
            toolTip1.SetToolTip(CacheSizeUpdateIntervalLabel, resources.GetString("CacheSizeUpdateIntervalLabel.ToolTip"));
            // 
            // BackupSecondsLabel
            // 
            resources.ApplyResources(BackupSecondsLabel, "BackupSecondsLabel");
            BackupSecondsLabel.Name = "BackupSecondsLabel";
            // 
            // CacheSizeUpdateIntervalNumericUpDown
            // 
            CacheSizeUpdateIntervalNumericUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            resources.ApplyResources(CacheSizeUpdateIntervalNumericUpDown, "CacheSizeUpdateIntervalNumericUpDown");
            CacheSizeUpdateIntervalNumericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            CacheSizeUpdateIntervalNumericUpDown.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            CacheSizeUpdateIntervalNumericUpDown.Name = "CacheSizeUpdateIntervalNumericUpDown";
            CacheSizeUpdateIntervalNumericUpDown.Value = new decimal(new int[] { 250, 0, 0, 0 });
            CacheSizeUpdateIntervalNumericUpDown.ValueChanged += CacheSizeUpdateIntervalNumericUpDown_ValueChanged;
            // 
            // CacheSizeFormatLabel
            // 
            resources.ApplyResources(CacheSizeFormatLabel, "CacheSizeFormatLabel");
            CacheSizeFormatLabel.Name = "CacheSizeFormatLabel";
            toolTip1.SetToolTip(CacheSizeFormatLabel, resources.GetString("CacheSizeFormatLabel.ToolTip"));
            // 
            // AutoRestoreIntervalLabel
            // 
            resources.ApplyResources(AutoRestoreIntervalLabel, "AutoRestoreIntervalLabel");
            AutoRestoreIntervalLabel.Name = "AutoRestoreIntervalLabel";
            toolTip1.SetToolTip(AutoRestoreIntervalLabel, resources.GetString("AutoRestoreIntervalLabel.ToolTip"));
            // 
            // MilisecondsLabel
            // 
            resources.ApplyResources(MilisecondsLabel, "MilisecondsLabel");
            MilisecondsLabel.Name = "MilisecondsLabel";
            // 
            // AutoRestoreIntervalNumericUpDown
            // 
            resources.ApplyResources(AutoRestoreIntervalNumericUpDown, "AutoRestoreIntervalNumericUpDown");
            AutoRestoreIntervalNumericUpDown.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            AutoRestoreIntervalNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            AutoRestoreIntervalNumericUpDown.Name = "AutoRestoreIntervalNumericUpDown";
            AutoRestoreIntervalNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            AutoRestoreIntervalNumericUpDown.ValueChanged += AutoRestoreIntervalNumericUpDown_ValueChanged;
            // 
            // CacheSizeFormatComboBox
            // 
            CacheSizeFormatComboBox.FormattingEnabled = true;
            CacheSizeFormatComboBox.Items.AddRange(new object[] { resources.GetString("CacheSizeFormatComboBox.Items"), resources.GetString("CacheSizeFormatComboBox.Items1"), resources.GetString("CacheSizeFormatComboBox.Items2"), resources.GetString("CacheSizeFormatComboBox.Items3") });
            resources.ApplyResources(CacheSizeFormatComboBox, "CacheSizeFormatComboBox");
            CacheSizeFormatComboBox.Name = "CacheSizeFormatComboBox";
            // 
            // RestoreSecondsLabel
            // 
            resources.ApplyResources(RestoreSecondsLabel, "RestoreSecondsLabel");
            RestoreSecondsLabel.Name = "RestoreSecondsLabel";
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
            // CloseButton
            // 
            resources.ApplyResources(CloseButton, "CloseButton");
            CloseButton.Name = "CloseButton";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // AdvancedCacheForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(CloseButton);
            Controls.Add(groupBox1);
            Name = "AdvancedCacheForm";
            Load += AdvancedCacheForm_Load;
            groupBox1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            BackupTabPage.ResumeLayout(false);
            BackupTabPage.PerformLayout();
            CacheTabPage.ResumeLayout(false);
            CacheTabPage.PerformLayout();
            GeneralTabPage.ResumeLayout(false);
            GeneralTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)AutoBackupIntervalNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)CacheSizeUpdateIntervalNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)AutoRestoreIntervalNumericUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label CacheSizeUpdateIntervalLabel;
        private System.Windows.Forms.Label AutoRestoreIntervalLabel;
        private System.Windows.Forms.Label AutoBackupIntervalLabel;
        private System.Windows.Forms.Label RestoreSecondsLabel;
        private System.Windows.Forms.NumericUpDown AutoBackupIntervalNumericUpDown;
        private System.Windows.Forms.NumericUpDown AutoRestoreIntervalNumericUpDown;
        private System.Windows.Forms.NumericUpDown CacheSizeUpdateIntervalNumericUpDown;
        private System.Windows.Forms.Label MilisecondsLabel;
        private System.Windows.Forms.Label BackupSecondsLabel;
        private System.Windows.Forms.Button DefaultButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ComboBox CacheSizeFormatComboBox;
        private System.Windows.Forms.Label CacheSizeFormatLabel;
        private System.Windows.Forms.ComboBox ExplorerCacheDeletionMethodComboBox;
        private System.Windows.Forms.Label ExplorerCacheDeletionMethodLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox ExplorerCloseFrequencyComboBox;
        private System.Windows.Forms.ComboBox BackupRestoreClearSafetyComboBox;
        private System.Windows.Forms.Label BackupRestoreClearSafetyLabel;
        private System.Windows.Forms.Label ExplorerCloseFrequencyLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage CacheTabPage;
        private System.Windows.Forms.TabPage GeneralTabPage;
        private System.Windows.Forms.TabPage BackupTabPage;
        private System.Windows.Forms.TextBox BackupFolderPathTextBox;
        private System.Windows.Forms.Button BrowseBackupFolderButton;
        private System.Windows.Forms.Button DefaultBackupFolderButton;
        private System.Windows.Forms.Button ClearBackupFolderButton;
        private System.Windows.Forms.Label BackupFolderLabel;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.TextBox OutputTextBox1;
        private System.Windows.Forms.CheckBox AutoBackupAfterPreloadCheckBox;
    }
}