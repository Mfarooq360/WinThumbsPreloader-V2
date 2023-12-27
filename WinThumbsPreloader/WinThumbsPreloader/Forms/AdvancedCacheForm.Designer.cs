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
            ExplorerCacheDeletionMethodComboBox = new System.Windows.Forms.ComboBox();
            ExplorerCacheDeletionMethodLabel = new System.Windows.Forms.Label();
            CacheSizeFormatComboBox = new System.Windows.Forms.ComboBox();
            CacheSizeFormatLabel = new System.Windows.Forms.Label();
            SaveButton = new System.Windows.Forms.Button();
            DefaultButton = new System.Windows.Forms.Button();
            MilisecondsLabel = new System.Windows.Forms.Label();
            BackupSecondsLabel = new System.Windows.Forms.Label();
            RestoreSecondsLabel = new System.Windows.Forms.Label();
            AutoBackupIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            AutoRestoreIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            CacheSizeUpdateIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            AutoRestoreIntervalLabel = new System.Windows.Forms.Label();
            AutoBackupIntervalLabel = new System.Windows.Forms.Label();
            CacheSizeUpdateIntervalLabel = new System.Windows.Forms.Label();
            CloseButton = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AutoBackupIntervalNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AutoRestoreIntervalNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CacheSizeUpdateIntervalNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            groupBox1.Controls.Add(ExplorerCacheDeletionMethodComboBox);
            groupBox1.Controls.Add(ExplorerCacheDeletionMethodLabel);
            groupBox1.Controls.Add(CacheSizeFormatComboBox);
            groupBox1.Controls.Add(CacheSizeFormatLabel);
            groupBox1.Controls.Add(SaveButton);
            groupBox1.Controls.Add(DefaultButton);
            groupBox1.Controls.Add(MilisecondsLabel);
            groupBox1.Controls.Add(BackupSecondsLabel);
            groupBox1.Controls.Add(RestoreSecondsLabel);
            groupBox1.Controls.Add(AutoBackupIntervalNumericUpDown);
            groupBox1.Controls.Add(AutoRestoreIntervalNumericUpDown);
            groupBox1.Controls.Add(CacheSizeUpdateIntervalNumericUpDown);
            groupBox1.Controls.Add(AutoRestoreIntervalLabel);
            groupBox1.Controls.Add(AutoBackupIntervalLabel);
            groupBox1.Controls.Add(CacheSizeUpdateIntervalLabel);
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            groupBox1.Enter += groupBox1_Enter;
            // 
            // ExplorerCacheDeletionMethodComboBox
            // 
            ExplorerCacheDeletionMethodComboBox.FormattingEnabled = true;
            ExplorerCacheDeletionMethodComboBox.Items.AddRange(new object[] { resources.GetString("ExplorerCacheDeletionMethodComboBox.Items"), resources.GetString("ExplorerCacheDeletionMethodComboBox.Items1") });
            resources.ApplyResources(ExplorerCacheDeletionMethodComboBox, "ExplorerCacheDeletionMethodComboBox");
            ExplorerCacheDeletionMethodComboBox.Name = "ExplorerCacheDeletionMethodComboBox";
            // 
            // ExplorerCacheDeletionMethodLabel
            // 
            resources.ApplyResources(ExplorerCacheDeletionMethodLabel, "ExplorerCacheDeletionMethodLabel");
            ExplorerCacheDeletionMethodLabel.Name = "ExplorerCacheDeletionMethodLabel";
            toolTip1.SetToolTip(ExplorerCacheDeletionMethodLabel, resources.GetString("ExplorerCacheDeletionMethodLabel.ToolTip"));
            // 
            // CacheSizeFormatComboBox
            // 
            CacheSizeFormatComboBox.FormattingEnabled = true;
            CacheSizeFormatComboBox.Items.AddRange(new object[] { resources.GetString("CacheSizeFormatComboBox.Items"), resources.GetString("CacheSizeFormatComboBox.Items1"), resources.GetString("CacheSizeFormatComboBox.Items2"), resources.GetString("CacheSizeFormatComboBox.Items3") });
            resources.ApplyResources(CacheSizeFormatComboBox, "CacheSizeFormatComboBox");
            CacheSizeFormatComboBox.Name = "CacheSizeFormatComboBox";
            CacheSizeFormatComboBox.SelectedIndexChanged += CacheSizeFormatComboBox_SelectedIndexChanged;
            // 
            // CacheSizeFormatLabel
            // 
            resources.ApplyResources(CacheSizeFormatLabel, "CacheSizeFormatLabel");
            CacheSizeFormatLabel.Name = "CacheSizeFormatLabel";
            toolTip1.SetToolTip(CacheSizeFormatLabel, resources.GetString("CacheSizeFormatLabel.ToolTip"));
            CacheSizeFormatLabel.Click += CacheSizeFormatLabel_Click;
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
            // MilisecondsLabel
            // 
            resources.ApplyResources(MilisecondsLabel, "MilisecondsLabel");
            MilisecondsLabel.Name = "MilisecondsLabel";
            // 
            // BackupSecondsLabel
            // 
            resources.ApplyResources(BackupSecondsLabel, "BackupSecondsLabel");
            BackupSecondsLabel.Name = "BackupSecondsLabel";
            // 
            // RestoreSecondsLabel
            // 
            resources.ApplyResources(RestoreSecondsLabel, "RestoreSecondsLabel");
            RestoreSecondsLabel.Name = "RestoreSecondsLabel";
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
            // AutoRestoreIntervalNumericUpDown
            // 
            resources.ApplyResources(AutoRestoreIntervalNumericUpDown, "AutoRestoreIntervalNumericUpDown");
            AutoRestoreIntervalNumericUpDown.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            AutoRestoreIntervalNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            AutoRestoreIntervalNumericUpDown.Name = "AutoRestoreIntervalNumericUpDown";
            AutoRestoreIntervalNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            AutoRestoreIntervalNumericUpDown.ValueChanged += AutoRestoreIntervalNumericUpDown_ValueChanged;
            // 
            // CacheSizeUpdateIntervalNumericUpDown
            // 
            resources.ApplyResources(CacheSizeUpdateIntervalNumericUpDown, "CacheSizeUpdateIntervalNumericUpDown");
            CacheSizeUpdateIntervalNumericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            CacheSizeUpdateIntervalNumericUpDown.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            CacheSizeUpdateIntervalNumericUpDown.Name = "CacheSizeUpdateIntervalNumericUpDown";
            CacheSizeUpdateIntervalNumericUpDown.Value = new decimal(new int[] { 250, 0, 0, 0 });
            CacheSizeUpdateIntervalNumericUpDown.ValueChanged += CacheSizeUpdateIntervalNumericUpDown_ValueChanged;
            // 
            // AutoRestoreIntervalLabel
            // 
            resources.ApplyResources(AutoRestoreIntervalLabel, "AutoRestoreIntervalLabel");
            AutoRestoreIntervalLabel.Name = "AutoRestoreIntervalLabel";
            toolTip1.SetToolTip(AutoRestoreIntervalLabel, resources.GetString("AutoRestoreIntervalLabel.ToolTip"));
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
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)AutoBackupIntervalNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)AutoRestoreIntervalNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)CacheSizeUpdateIntervalNumericUpDown).EndInit();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ExplorerCacheDeletionMethodComboBox;
        private System.Windows.Forms.Label ExplorerCacheDeletionMethodLabel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}