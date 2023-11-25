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
            groupBox1 = new System.Windows.Forms.GroupBox();
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
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AutoBackupIntervalNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AutoRestoreIntervalNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CacheSizeUpdateIntervalNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
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
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(310, 253);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Advanced Options";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // CacheSizeFormatComboBox
            // 
            CacheSizeFormatComboBox.FormattingEnabled = true;
            CacheSizeFormatComboBox.Items.AddRange(new object[] { "Auto", "KB", "MB", "GB" });
            CacheSizeFormatComboBox.Location = new System.Drawing.Point(120, 26);
            CacheSizeFormatComboBox.Name = "CacheSizeFormatComboBox";
            CacheSizeFormatComboBox.Size = new System.Drawing.Size(56, 23);
            CacheSizeFormatComboBox.TabIndex = 12;
            CacheSizeFormatComboBox.SelectedIndexChanged += CacheSizeFormatComboBox_SelectedIndexChanged;
            // 
            // CacheSizeFormatLabel
            // 
            CacheSizeFormatLabel.AutoSize = true;
            CacheSizeFormatLabel.Location = new System.Drawing.Point(6, 30);
            CacheSizeFormatLabel.Name = "CacheSizeFormatLabel";
            CacheSizeFormatLabel.Size = new System.Drawing.Size(107, 15);
            CacheSizeFormatLabel.TabIndex = 11;
            CacheSizeFormatLabel.Text = "Cache Size Format:";
            // 
            // SaveButton
            // 
            SaveButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            SaveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            SaveButton.Location = new System.Drawing.Point(119, 219);
            SaveButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(88, 28);
            SaveButton.TabIndex = 10;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // DefaultButton
            // 
            DefaultButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            DefaultButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            DefaultButton.Location = new System.Drawing.Point(215, 219);
            DefaultButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DefaultButton.Name = "DefaultButton";
            DefaultButton.Size = new System.Drawing.Size(88, 28);
            DefaultButton.TabIndex = 9;
            DefaultButton.Text = "Default";
            DefaultButton.UseVisualStyleBackColor = true;
            DefaultButton.Click += DefaultButton_Click;
            // 
            // MilisecondsLabel
            // 
            MilisecondsLabel.AutoSize = true;
            MilisecondsLabel.Location = new System.Drawing.Point(222, 74);
            MilisecondsLabel.Name = "MilisecondsLabel";
            MilisecondsLabel.Size = new System.Drawing.Size(23, 15);
            MilisecondsLabel.TabIndex = 8;
            MilisecondsLabel.Text = "ms";
            // 
            // BackupSecondsLabel
            // 
            BackupSecondsLabel.AutoSize = true;
            BackupSecondsLabel.Location = new System.Drawing.Point(176, 113);
            BackupSecondsLabel.Name = "BackupSecondsLabel";
            BackupSecondsLabel.Size = new System.Drawing.Size(12, 15);
            BackupSecondsLabel.TabIndex = 7;
            BackupSecondsLabel.Text = "s";
            // 
            // RestoreSecondsLabel
            // 
            RestoreSecondsLabel.AutoSize = true;
            RestoreSecondsLabel.Location = new System.Drawing.Point(176, 153);
            RestoreSecondsLabel.Name = "RestoreSecondsLabel";
            RestoreSecondsLabel.Size = new System.Drawing.Size(12, 15);
            RestoreSecondsLabel.TabIndex = 6;
            RestoreSecondsLabel.Text = "s";
            // 
            // AutoBackupIntervalNumericUpDown
            // 
            AutoBackupIntervalNumericUpDown.Location = new System.Drawing.Point(132, 106);
            AutoBackupIntervalNumericUpDown.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            AutoBackupIntervalNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            AutoBackupIntervalNumericUpDown.Name = "AutoBackupIntervalNumericUpDown";
            AutoBackupIntervalNumericUpDown.Size = new System.Drawing.Size(43, 23);
            AutoBackupIntervalNumericUpDown.TabIndex = 5;
            AutoBackupIntervalNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            AutoBackupIntervalNumericUpDown.ValueChanged += AutoBackupIntervalNumericUpDown_ValueChanged;
            // 
            // AutoRestoreIntervalNumericUpDown
            // 
            AutoRestoreIntervalNumericUpDown.Location = new System.Drawing.Point(132, 147);
            AutoRestoreIntervalNumericUpDown.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            AutoRestoreIntervalNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            AutoRestoreIntervalNumericUpDown.Name = "AutoRestoreIntervalNumericUpDown";
            AutoRestoreIntervalNumericUpDown.Size = new System.Drawing.Size(43, 23);
            AutoRestoreIntervalNumericUpDown.TabIndex = 4;
            AutoRestoreIntervalNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            AutoRestoreIntervalNumericUpDown.ValueChanged += AutoRestoreIntervalNumericUpDown_ValueChanged;
            // 
            // CacheSizeUpdateIntervalNumericUpDown
            // 
            CacheSizeUpdateIntervalNumericUpDown.Location = new System.Drawing.Point(161, 67);
            CacheSizeUpdateIntervalNumericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            CacheSizeUpdateIntervalNumericUpDown.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            CacheSizeUpdateIntervalNumericUpDown.Name = "CacheSizeUpdateIntervalNumericUpDown";
            CacheSizeUpdateIntervalNumericUpDown.Size = new System.Drawing.Size(61, 23);
            CacheSizeUpdateIntervalNumericUpDown.TabIndex = 3;
            CacheSizeUpdateIntervalNumericUpDown.Value = new decimal(new int[] { 250, 0, 0, 0 });
            CacheSizeUpdateIntervalNumericUpDown.ValueChanged += CacheSizeUpdateIntervalNumericUpDown_ValueChanged;
            // 
            // AutoRestoreIntervalLabel
            // 
            AutoRestoreIntervalLabel.AutoSize = true;
            AutoRestoreIntervalLabel.Location = new System.Drawing.Point(6, 150);
            AutoRestoreIntervalLabel.Name = "AutoRestoreIntervalLabel";
            AutoRestoreIntervalLabel.Size = new System.Drawing.Size(120, 15);
            AutoRestoreIntervalLabel.TabIndex = 2;
            AutoRestoreIntervalLabel.Text = "Auto Restore Interval:";
            // 
            // AutoBackupIntervalLabel
            // 
            AutoBackupIntervalLabel.AutoSize = true;
            AutoBackupIntervalLabel.Location = new System.Drawing.Point(6, 110);
            AutoBackupIntervalLabel.Name = "AutoBackupIntervalLabel";
            AutoBackupIntervalLabel.Size = new System.Drawing.Size(120, 15);
            AutoBackupIntervalLabel.TabIndex = 1;
            AutoBackupIntervalLabel.Text = "Auto Backup Interval:";
            // 
            // CacheSizeUpdateIntervalLabel
            // 
            CacheSizeUpdateIntervalLabel.AutoSize = true;
            CacheSizeUpdateIntervalLabel.Location = new System.Drawing.Point(6, 70);
            CacheSizeUpdateIntervalLabel.Name = "CacheSizeUpdateIntervalLabel";
            CacheSizeUpdateIntervalLabel.Size = new System.Drawing.Size(149, 15);
            CacheSizeUpdateIntervalLabel.TabIndex = 0;
            CacheSizeUpdateIntervalLabel.Text = "Cache Size Update Interval:";
            // 
            // CloseButton
            // 
            CloseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CloseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            CloseButton.Location = new System.Drawing.Point(233, 271);
            CloseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new System.Drawing.Size(88, 28);
            CloseButton.TabIndex = 8;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // AdvancedCacheForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(334, 311);
            Controls.Add(CloseButton);
            Controls.Add(groupBox1);
            Name = "AdvancedCacheForm";
            Text = "WinThumbsPreloader - Advanced Cache";
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
    }
}