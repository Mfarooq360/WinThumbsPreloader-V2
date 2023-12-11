namespace WinThumbsPreloader
{
    partial class CacheForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CacheForm));
            CacheOptionsGroupBox = new System.Windows.Forms.GroupBox();
            progressBarRestore = new System.Windows.Forms.ProgressBar();
            StartWithWindowsCheckBox = new System.Windows.Forms.CheckBox();
            AutoRestoreCheckBox = new System.Windows.Forms.CheckBox();
            BackupSizeLabel = new System.Windows.Forms.Label();
            CacheSizeLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ClearCacheButton = new System.Windows.Forms.Button();
            OutputTextBox = new System.Windows.Forms.TextBox();
            RestoreButton = new System.Windows.Forms.Button();
            ClearButton = new System.Windows.Forms.Button();
            BackupButton = new System.Windows.Forms.Button();
            AlertCheckBox = new System.Windows.Forms.CheckBox();
            AutoBackupCheckBox = new System.Windows.Forms.CheckBox();
            CloseButton = new System.Windows.Forms.Button();
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            startWithWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toggleAutoBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleAutoRestoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleCacheResetAlertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CacheToolTips = new System.Windows.Forms.ToolTip(components);
            AdvancedButton = new System.Windows.Forms.Button();
            CacheOptionsGroupBox.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // CacheOptionsGroupBox
            // 
            CacheOptionsGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            CacheOptionsGroupBox.Controls.Add(progressBarRestore);
            CacheOptionsGroupBox.Controls.Add(StartWithWindowsCheckBox);
            CacheOptionsGroupBox.Controls.Add(AutoRestoreCheckBox);
            CacheOptionsGroupBox.Controls.Add(BackupSizeLabel);
            CacheOptionsGroupBox.Controls.Add(CacheSizeLabel);
            CacheOptionsGroupBox.Controls.Add(label1);
            CacheOptionsGroupBox.Controls.Add(ClearCacheButton);
            CacheOptionsGroupBox.Controls.Add(OutputTextBox);
            CacheOptionsGroupBox.Controls.Add(RestoreButton);
            CacheOptionsGroupBox.Controls.Add(ClearButton);
            CacheOptionsGroupBox.Controls.Add(BackupButton);
            CacheOptionsGroupBox.Controls.Add(AlertCheckBox);
            CacheOptionsGroupBox.Controls.Add(AutoBackupCheckBox);
            resources.ApplyResources(CacheOptionsGroupBox, "CacheOptionsGroupBox");
            CacheOptionsGroupBox.Name = "CacheOptionsGroupBox";
            CacheOptionsGroupBox.TabStop = false;
            // 
            // progressBarRestore
            // 
            resources.ApplyResources(progressBarRestore, "progressBarRestore");
            progressBarRestore.Name = "progressBarRestore";
            // 
            // StartWithWindowsCheckBox
            // 
            resources.ApplyResources(StartWithWindowsCheckBox, "StartWithWindowsCheckBox");
            StartWithWindowsCheckBox.Name = "StartWithWindowsCheckBox";
            CacheToolTips.SetToolTip(StartWithWindowsCheckBox, resources.GetString("StartWithWindowsCheckBox.ToolTip"));
            StartWithWindowsCheckBox.UseVisualStyleBackColor = true;
            StartWithWindowsCheckBox.CheckedChanged += StartWithWindowsCheckBox_CheckedChanged;
            // 
            // AutoRestoreCheckBox
            // 
            resources.ApplyResources(AutoRestoreCheckBox, "AutoRestoreCheckBox");
            AutoRestoreCheckBox.Name = "AutoRestoreCheckBox";
            CacheToolTips.SetToolTip(AutoRestoreCheckBox, resources.GetString("AutoRestoreCheckBox.ToolTip"));
            AutoRestoreCheckBox.UseVisualStyleBackColor = true;
            AutoRestoreCheckBox.CheckedChanged += AutoRestoreCheckBox_CheckedChanged;
            // 
            // BackupSizeLabel
            // 
            resources.ApplyResources(BackupSizeLabel, "BackupSizeLabel");
            BackupSizeLabel.Name = "BackupSizeLabel";
            // 
            // CacheSizeLabel
            // 
            resources.ApplyResources(CacheSizeLabel, "CacheSizeLabel");
            CacheSizeLabel.Name = "CacheSizeLabel";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            CacheToolTips.SetToolTip(label1, resources.GetString("label1.ToolTip"));
            // 
            // ClearCacheButton
            // 
            resources.ApplyResources(ClearCacheButton, "ClearCacheButton");
            ClearCacheButton.Name = "ClearCacheButton";
            CacheToolTips.SetToolTip(ClearCacheButton, resources.GetString("ClearCacheButton.ToolTip"));
            ClearCacheButton.UseVisualStyleBackColor = true;
            ClearCacheButton.Click += ClearCacheButton_Click;
            // 
            // OutputTextBox
            // 
            resources.ApplyResources(OutputTextBox, "OutputTextBox");
            OutputTextBox.Name = "OutputTextBox";
            OutputTextBox.ReadOnly = true;
            // 
            // RestoreButton
            // 
            resources.ApplyResources(RestoreButton, "RestoreButton");
            RestoreButton.Name = "RestoreButton";
            CacheToolTips.SetToolTip(RestoreButton, resources.GetString("RestoreButton.ToolTip"));
            RestoreButton.UseVisualStyleBackColor = true;
            RestoreButton.Click += RestoreButton_Click;
            // 
            // ClearButton
            // 
            resources.ApplyResources(ClearButton, "ClearButton");
            ClearButton.Name = "ClearButton";
            CacheToolTips.SetToolTip(ClearButton, resources.GetString("ClearButton.ToolTip"));
            ClearButton.UseVisualStyleBackColor = true;
            ClearButton.Click += ClearButton_Click;
            // 
            // BackupButton
            // 
            resources.ApplyResources(BackupButton, "BackupButton");
            BackupButton.Name = "BackupButton";
            CacheToolTips.SetToolTip(BackupButton, resources.GetString("BackupButton.ToolTip"));
            BackupButton.UseVisualStyleBackColor = true;
            BackupButton.Click += BackupButton_Click;
            // 
            // AlertCheckBox
            // 
            resources.ApplyResources(AlertCheckBox, "AlertCheckBox");
            AlertCheckBox.Name = "AlertCheckBox";
            CacheToolTips.SetToolTip(AlertCheckBox, resources.GetString("AlertCheckBox.ToolTip"));
            AlertCheckBox.UseVisualStyleBackColor = true;
            AlertCheckBox.CheckedChanged += AlertCheckBox_CheckedChanged;
            // 
            // AutoBackupCheckBox
            // 
            resources.ApplyResources(AutoBackupCheckBox, "AutoBackupCheckBox");
            AutoBackupCheckBox.Name = "AutoBackupCheckBox";
            CacheToolTips.SetToolTip(AutoBackupCheckBox, resources.GetString("AutoBackupCheckBox.ToolTip"));
            AutoBackupCheckBox.UseVisualStyleBackColor = true;
            AutoBackupCheckBox.CheckedChanged += AutoBackupCheckBox_CheckedChanged;
            // 
            // CloseButton
            // 
            resources.ApplyResources(CloseButton, "CloseButton");
            CloseButton.Name = "CloseButton";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            resources.ApplyResources(notifyIcon1, "notifyIcon1");
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, startWithWindowsToolStripMenuItem, toolStripSeparator1, toggleAutoBackupToolStripMenuItem, toggleAutoRestoreToolStripMenuItem, toggleCacheResetAlertToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.ShowCheckMargin = true;
            contextMenuStrip1.ShowImageMargin = false;
            resources.ApplyResources(contextMenuStrip1, "contextMenuStrip1");
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(openToolStripMenuItem, "openToolStripMenuItem");
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // startWithWindowsToolStripMenuItem
            // 
            startWithWindowsToolStripMenuItem.Name = "startWithWindowsToolStripMenuItem";
            resources.ApplyResources(startWithWindowsToolStripMenuItem, "startWithWindowsToolStripMenuItem");
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
            // 
            // toggleAutoBackupToolStripMenuItem
            // 
            toggleAutoBackupToolStripMenuItem.Name = "toggleAutoBackupToolStripMenuItem";
            resources.ApplyResources(toggleAutoBackupToolStripMenuItem, "toggleAutoBackupToolStripMenuItem");
            toggleAutoBackupToolStripMenuItem.Click += toggleAutoBackupToolStripMenuItem_Click;
            // 
            // toggleAutoRestoreToolStripMenuItem
            // 
            toggleAutoRestoreToolStripMenuItem.Name = "toggleAutoRestoreToolStripMenuItem";
            resources.ApplyResources(toggleAutoRestoreToolStripMenuItem, "toggleAutoRestoreToolStripMenuItem");
            toggleAutoRestoreToolStripMenuItem.Click += toggleAutoRestoreToolStripMenuItem_Click;
            // 
            // toggleCacheResetAlertToolStripMenuItem
            // 
            toggleCacheResetAlertToolStripMenuItem.Name = "toggleCacheResetAlertToolStripMenuItem";
            resources.ApplyResources(toggleCacheResetAlertToolStripMenuItem, "toggleCacheResetAlertToolStripMenuItem");
            toggleCacheResetAlertToolStripMenuItem.Click += toggleCacheResetAlertToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(toolStripSeparator2, "toolStripSeparator2");
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(exitToolStripMenuItem, "exitToolStripMenuItem");
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // AdvancedButton
            // 
            resources.ApplyResources(AdvancedButton, "AdvancedButton");
            AdvancedButton.Name = "AdvancedButton";
            AdvancedButton.UseVisualStyleBackColor = true;
            AdvancedButton.Click += AdvancedButton_Click;
            // 
            // CacheForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(AdvancedButton);
            Controls.Add(CloseButton);
            Controls.Add(CacheOptionsGroupBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "CacheForm";
            Load += CacheForm_Load;
            CacheOptionsGroupBox.ResumeLayout(false);
            CacheOptionsGroupBox.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox CacheOptionsGroupBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.CheckBox AlertCheckBox;
        private System.Windows.Forms.CheckBox AutoBackupCheckBox;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button RestoreButton;
        private System.Windows.Forms.Button BackupButton;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.Button ClearCacheButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CacheSizeLabel;
        private System.Windows.Forms.Label BackupSizeLabel;
        private System.Windows.Forms.CheckBox AutoRestoreCheckBox;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toggleAutoBackupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleAutoRestoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.CheckBox StartWithWindowsCheckBox;
        private System.Windows.Forms.ToolStripMenuItem startWithWindowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleCacheResetAlertToolStripMenuItem;
        private System.Windows.Forms.ToolTip CacheToolTips;
        private System.Windows.Forms.ProgressBar progressBarRestore;
        private System.Windows.Forms.Button AdvancedButton;
    }
}