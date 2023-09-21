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
            CacheOptionsGroupBox = new System.Windows.Forms.GroupBox();
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
            comboBox1 = new System.Windows.Forms.ComboBox();
            CacheOptionsGroupBox.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // CacheOptionsGroupBox
            // 
            CacheOptionsGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            CacheOptionsGroupBox.Controls.Add(comboBox1);
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
            CacheOptionsGroupBox.Location = new System.Drawing.Point(13, 12);
            CacheOptionsGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CacheOptionsGroupBox.Name = "CacheOptionsGroupBox";
            CacheOptionsGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CacheOptionsGroupBox.Size = new System.Drawing.Size(333, 279);
            CacheOptionsGroupBox.TabIndex = 3;
            CacheOptionsGroupBox.TabStop = false;
            CacheOptionsGroupBox.Text = "Cache Options";
            // 
            // StartWithWindowsCheckBox
            // 
            StartWithWindowsCheckBox.AutoSize = true;
            StartWithWindowsCheckBox.Location = new System.Drawing.Point(16, 92);
            StartWithWindowsCheckBox.Name = "StartWithWindowsCheckBox";
            StartWithWindowsCheckBox.Size = new System.Drawing.Size(128, 19);
            StartWithWindowsCheckBox.TabIndex = 16;
            StartWithWindowsCheckBox.Text = "Start with Windows";
            CacheToolTips.SetToolTip(StartWithWindowsCheckBox, "Enables the program to start in the system tray");
            StartWithWindowsCheckBox.UseVisualStyleBackColor = true;
            StartWithWindowsCheckBox.CheckedChanged += StartWithWindowsCheckBox_CheckedChanged;
            // 
            // AutoRestoreCheckBox
            // 
            AutoRestoreCheckBox.AutoSize = true;
            AutoRestoreCheckBox.Location = new System.Drawing.Point(16, 167);
            AutoRestoreCheckBox.Name = "AutoRestoreCheckBox";
            AutoRestoreCheckBox.Size = new System.Drawing.Size(94, 19);
            AutoRestoreCheckBox.TabIndex = 15;
            AutoRestoreCheckBox.Text = "Auto Restore";
            CacheToolTips.SetToolTip(AutoRestoreCheckBox, "Enables restoring of the thumbnail cache from\r\nbackup when the cache size is less than the backup.");
            AutoRestoreCheckBox.UseVisualStyleBackColor = true;
            AutoRestoreCheckBox.CheckedChanged += AutoRestoreCheckBox_CheckedChanged;
            // 
            // BackupSizeLabel
            // 
            BackupSizeLabel.AutoSize = true;
            BackupSizeLabel.Location = new System.Drawing.Point(110, 28);
            BackupSizeLabel.Name = "BackupSizeLabel";
            BackupSizeLabel.Size = new System.Drawing.Size(72, 15);
            BackupSizeLabel.TabIndex = 14;
            BackupSizeLabel.Text = "Backup Size:";
            // 
            // CacheSizeLabel
            // 
            CacheSizeLabel.AutoSize = true;
            CacheSizeLabel.Location = new System.Drawing.Point(110, 62);
            CacheSizeLabel.Name = "CacheSizeLabel";
            CacheSizeLabel.Size = new System.Drawing.Size(66, 15);
            CacheSizeLabel.TabIndex = 13;
            CacheSizeLabel.Text = "Cache Size:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 198);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(48, 15);
            label1.TabIndex = 12;
            label1.Text = "Output:";
            // 
            // ClearCacheButton
            // 
            ClearCacheButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ClearCacheButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            ClearCacheButton.Location = new System.Drawing.Point(103, 245);
            ClearCacheButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ClearCacheButton.Name = "ClearCacheButton";
            ClearCacheButton.Size = new System.Drawing.Size(88, 28);
            ClearCacheButton.TabIndex = 11;
            ClearCacheButton.Text = "Open Cache";
            CacheToolTips.SetToolTip(ClearCacheButton, "Opens thumbnail cache folder\r\nHolding \"SHIFT\" will prompt to clear the cache");
            ClearCacheButton.UseVisualStyleBackColor = true;
            ClearCacheButton.Click += ClearCacheButton_Click;
            // 
            // OutputTextBox
            // 
            OutputTextBox.Location = new System.Drawing.Point(7, 216);
            OutputTextBox.Name = "OutputTextBox";
            OutputTextBox.ReadOnly = true;
            OutputTextBox.Size = new System.Drawing.Size(319, 23);
            OutputTextBox.TabIndex = 11;
            OutputTextBox.TextChanged += OutputTextBox_TextChanged;
            // 
            // RestoreButton
            // 
            RestoreButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            RestoreButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            RestoreButton.Location = new System.Drawing.Point(15, 56);
            RestoreButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RestoreButton.Name = "RestoreButton";
            RestoreButton.Size = new System.Drawing.Size(88, 28);
            RestoreButton.TabIndex = 9;
            RestoreButton.Text = "Restore";
            CacheToolTips.SetToolTip(RestoreButton, "Restores the thumbnail cache from backup");
            RestoreButton.UseVisualStyleBackColor = true;
            RestoreButton.Click += RestoreButton_Click;
            // 
            // ClearButton
            // 
            ClearButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ClearButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            ClearButton.Location = new System.Drawing.Point(7, 245);
            ClearButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new System.Drawing.Size(88, 28);
            ClearButton.TabIndex = 10;
            ClearButton.Text = "Open Backup";
            CacheToolTips.SetToolTip(ClearButton, "Opens backup folder\r\nHolding \"SHIFT\" will prompt to clear the backup");
            ClearButton.UseVisualStyleBackColor = true;
            ClearButton.Click += ClearButton_Click;
            // 
            // BackupButton
            // 
            BackupButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BackupButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            BackupButton.Location = new System.Drawing.Point(15, 22);
            BackupButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BackupButton.Name = "BackupButton";
            BackupButton.Size = new System.Drawing.Size(88, 28);
            BackupButton.TabIndex = 8;
            BackupButton.Text = "Backup";
            CacheToolTips.SetToolTip(BackupButton, "Creates a backup of the thumbnail cache");
            BackupButton.UseVisualStyleBackColor = true;
            BackupButton.Click += BackupButton_Click;
            // 
            // AlertCheckBox
            // 
            AlertCheckBox.AutoSize = true;
            AlertCheckBox.Location = new System.Drawing.Point(16, 117);
            AlertCheckBox.Name = "AlertCheckBox";
            AlertCheckBox.Size = new System.Drawing.Size(135, 19);
            AlertCheckBox.TabIndex = 1;
            AlertCheckBox.Text = "Alert on Cache Reset";
            CacheToolTips.SetToolTip(AlertCheckBox, "Creates a popup on startup that alerts the user\r\nwhen the program detects a cache reset.");
            AlertCheckBox.UseVisualStyleBackColor = true;
            AlertCheckBox.CheckedChanged += AlertCheckBox_CheckedChanged;
            // 
            // AutoBackupCheckBox
            // 
            AutoBackupCheckBox.AutoSize = true;
            AutoBackupCheckBox.Location = new System.Drawing.Point(16, 142);
            AutoBackupCheckBox.Name = "AutoBackupCheckBox";
            AutoBackupCheckBox.Size = new System.Drawing.Size(94, 19);
            AutoBackupCheckBox.TabIndex = 0;
            AutoBackupCheckBox.Text = "Auto Backup";
            CacheToolTips.SetToolTip(AutoBackupCheckBox, "Enables backing up of thumbnail cache\r\nin the background at set intervals");
            AutoBackupCheckBox.UseVisualStyleBackColor = true;
            AutoBackupCheckBox.CheckedChanged += AutoBackupCheckBox_CheckedChanged;
            // 
            // CloseButton
            // 
            CloseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CloseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            CloseButton.Location = new System.Drawing.Point(258, 297);
            CloseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new System.Drawing.Size(88, 28);
            CloseButton.TabIndex = 7;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Text = "WinThumbsPreloader";
            notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, startWithWindowsToolStripMenuItem, toolStripSeparator1, toggleAutoBackupToolStripMenuItem, toggleAutoRestoreToolStripMenuItem, toggleCacheResetAlertToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.ShowCheckMargin = true;
            contextMenuStrip1.ShowImageMargin = false;
            contextMenuStrip1.Size = new System.Drawing.Size(177, 148);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // startWithWindowsToolStripMenuItem
            // 
            startWithWindowsToolStripMenuItem.Name = "startWithWindowsToolStripMenuItem";
            startWithWindowsToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            startWithWindowsToolStripMenuItem.Text = "Start with Windows";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(173, 6);
            // 
            // toggleAutoBackupToolStripMenuItem
            // 
            toggleAutoBackupToolStripMenuItem.Name = "toggleAutoBackupToolStripMenuItem";
            toggleAutoBackupToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            toggleAutoBackupToolStripMenuItem.Text = "Auto Backup";
            toggleAutoBackupToolStripMenuItem.Click += toggleAutoBackupToolStripMenuItem_Click;
            // 
            // toggleAutoRestoreToolStripMenuItem
            // 
            toggleAutoRestoreToolStripMenuItem.Name = "toggleAutoRestoreToolStripMenuItem";
            toggleAutoRestoreToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            toggleAutoRestoreToolStripMenuItem.Text = "Auto Restore";
            toggleAutoRestoreToolStripMenuItem.Click += toggleAutoRestoreToolStripMenuItem_Click;
            // 
            // toggleCacheResetAlertToolStripMenuItem
            // 
            toggleCacheResetAlertToolStripMenuItem.Name = "toggleCacheResetAlertToolStripMenuItem";
            toggleCacheResetAlertToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            toggleCacheResetAlertToolStripMenuItem.Text = "Cache Reset Alert";
            toggleCacheResetAlertToolStripMenuItem.Click += toggleCacheResetAlertToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(173, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(110, 140);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(75, 23);
            comboBox1.TabIndex = 17;
            // 
            // CacheForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(359, 336);
            Controls.Add(CloseButton);
            Controls.Add(CacheOptionsGroupBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "CacheForm";
            Text = "WinThumbsPreloader - Cache";
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
        private System.Windows.Forms.ComboBox comboBox1;
    }
}