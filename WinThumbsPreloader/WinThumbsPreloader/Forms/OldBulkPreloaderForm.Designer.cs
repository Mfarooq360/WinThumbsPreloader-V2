namespace WinThumbsPreloader.Forms
{
    partial class OldBulkPreloaderForm
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
            CloseButton = new System.Windows.Forms.Button();
            BulkPreloaderOptionsGroupBox = new System.Windows.Forms.GroupBox();
            PresetsButton = new System.Windows.Forms.Button();
            MaximumInstancesLabel = new System.Windows.Forms.Label();
            DefaultMaximumInstancesButton = new System.Windows.Forms.Button();
            MaximumInstancesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ExportPathsButton = new System.Windows.Forms.Button();
            ThreadsLabel = new System.Windows.Forms.Label();
            DefaultThreadsButton = new System.Windows.Forms.Button();
            ThreadsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            BulkPreloaderMethodComboBox = new System.Windows.Forms.ComboBox();
            BulkPreloaderMethodLabel = new System.Windows.Forms.Label();
            MultithreadedCheckBox = new System.Windows.Forms.CheckBox();
            ClearButton = new System.Windows.Forms.Button();
            BrowseButton = new System.Windows.Forms.Button();
            PathsLabel = new System.Windows.Forms.Label();
            PathsTextBox = new System.Windows.Forms.TextBox();
            PresetsGroupBox = new System.Windows.Forms.GroupBox();
            ExportPresetsButton = new System.Windows.Forms.Button();
            PresetsTextBox = new System.Windows.Forms.TextBox();
            PresetsListBox = new System.Windows.Forms.ListBox();
            DeleteButton = new System.Windows.Forms.Button();
            SavePresetsButton = new System.Windows.Forms.Button();
            StartButton = new System.Windows.Forms.Button();
            BulkPreloaderOptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MaximumInstancesNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ThreadsNumericUpDown).BeginInit();
            PresetsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // CloseButton
            // 
            CloseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CloseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            CloseButton.Location = new System.Drawing.Point(634, 371);
            CloseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new System.Drawing.Size(88, 28);
            CloseButton.TabIndex = 6;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // BulkPreloaderOptionsGroupBox
            // 
            BulkPreloaderOptionsGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            BulkPreloaderOptionsGroupBox.Controls.Add(PresetsButton);
            BulkPreloaderOptionsGroupBox.Controls.Add(MaximumInstancesLabel);
            BulkPreloaderOptionsGroupBox.Controls.Add(DefaultMaximumInstancesButton);
            BulkPreloaderOptionsGroupBox.Controls.Add(MaximumInstancesNumericUpDown);
            BulkPreloaderOptionsGroupBox.Controls.Add(ExportPathsButton);
            BulkPreloaderOptionsGroupBox.Controls.Add(ThreadsLabel);
            BulkPreloaderOptionsGroupBox.Controls.Add(DefaultThreadsButton);
            BulkPreloaderOptionsGroupBox.Controls.Add(ThreadsNumericUpDown);
            BulkPreloaderOptionsGroupBox.Controls.Add(BulkPreloaderMethodComboBox);
            BulkPreloaderOptionsGroupBox.Controls.Add(BulkPreloaderMethodLabel);
            BulkPreloaderOptionsGroupBox.Controls.Add(MultithreadedCheckBox);
            BulkPreloaderOptionsGroupBox.Controls.Add(ClearButton);
            BulkPreloaderOptionsGroupBox.Controls.Add(BrowseButton);
            BulkPreloaderOptionsGroupBox.Controls.Add(PathsLabel);
            BulkPreloaderOptionsGroupBox.Controls.Add(PathsTextBox);
            BulkPreloaderOptionsGroupBox.Location = new System.Drawing.Point(12, 12);
            BulkPreloaderOptionsGroupBox.Name = "BulkPreloaderOptionsGroupBox";
            BulkPreloaderOptionsGroupBox.Size = new System.Drawing.Size(410, 353);
            BulkPreloaderOptionsGroupBox.TabIndex = 7;
            BulkPreloaderOptionsGroupBox.TabStop = false;
            BulkPreloaderOptionsGroupBox.Text = "Bulk Preloader Options";
            // 
            // PresetsButton
            // 
            PresetsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PresetsButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            PresetsButton.Location = new System.Drawing.Point(316, 319);
            PresetsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PresetsButton.Name = "PresetsButton";
            PresetsButton.Size = new System.Drawing.Size(88, 28);
            PresetsButton.TabIndex = 20;
            PresetsButton.Text = "Presets";
            PresetsButton.UseVisualStyleBackColor = true;
            PresetsButton.Click += PresetsButton_Click;
            // 
            // MaximumInstancesLabel
            // 
            MaximumInstancesLabel.AutoSize = true;
            MaximumInstancesLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            MaximumInstancesLabel.Location = new System.Drawing.Point(7, 83);
            MaximumInstancesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            MaximumInstancesLabel.Name = "MaximumInstancesLabel";
            MaximumInstancesLabel.Size = new System.Drawing.Size(117, 15);
            MaximumInstancesLabel.TabIndex = 19;
            MaximumInstancesLabel.Text = "Maximum Instances:";
            // 
            // DefaultMaximumInstancesButton
            // 
            DefaultMaximumInstancesButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            DefaultMaximumInstancesButton.Location = new System.Drawing.Point(228, 79);
            DefaultMaximumInstancesButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DefaultMaximumInstancesButton.Name = "DefaultMaximumInstancesButton";
            DefaultMaximumInstancesButton.Size = new System.Drawing.Size(88, 27);
            DefaultMaximumInstancesButton.TabIndex = 18;
            DefaultMaximumInstancesButton.Text = "Default";
            DefaultMaximumInstancesButton.UseVisualStyleBackColor = true;
            // 
            // MaximumInstancesNumericUpDown
            // 
            MaximumInstancesNumericUpDown.Location = new System.Drawing.Point(129, 81);
            MaximumInstancesNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximumInstancesNumericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            MaximumInstancesNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            MaximumInstancesNumericUpDown.Name = "MaximumInstancesNumericUpDown";
            MaximumInstancesNumericUpDown.Size = new System.Drawing.Size(91, 23);
            MaximumInstancesNumericUpDown.TabIndex = 17;
            MaximumInstancesNumericUpDown.ValueChanged += MaximumInstancesNumericUpDown_ValueChanged;
            // 
            // ExportPathsButton
            // 
            ExportPathsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ExportPathsButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            ExportPathsButton.Location = new System.Drawing.Point(105, 319);
            ExportPathsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ExportPathsButton.Name = "ExportPathsButton";
            ExportPathsButton.Size = new System.Drawing.Size(88, 28);
            ExportPathsButton.TabIndex = 16;
            ExportPathsButton.Text = "Export";
            ExportPathsButton.UseVisualStyleBackColor = true;
            // 
            // ThreadsLabel
            // 
            ThreadsLabel.AutoSize = true;
            ThreadsLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            ThreadsLabel.Location = new System.Drawing.Point(7, 50);
            ThreadsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ThreadsLabel.Name = "ThreadsLabel";
            ThreadsLabel.Size = new System.Drawing.Size(51, 15);
            ThreadsLabel.TabIndex = 16;
            ThreadsLabel.Text = "Threads:";
            // 
            // DefaultThreadsButton
            // 
            DefaultThreadsButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            DefaultThreadsButton.Location = new System.Drawing.Point(158, 45);
            DefaultThreadsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DefaultThreadsButton.Name = "DefaultThreadsButton";
            DefaultThreadsButton.Size = new System.Drawing.Size(88, 27);
            DefaultThreadsButton.TabIndex = 15;
            DefaultThreadsButton.Text = "Default";
            DefaultThreadsButton.UseVisualStyleBackColor = true;
            // 
            // ThreadsNumericUpDown
            // 
            ThreadsNumericUpDown.Location = new System.Drawing.Point(60, 47);
            ThreadsNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ThreadsNumericUpDown.Maximum = new decimal(new int[] { 256, 0, 0, 0 });
            ThreadsNumericUpDown.Name = "ThreadsNumericUpDown";
            ThreadsNumericUpDown.Size = new System.Drawing.Size(91, 23);
            ThreadsNumericUpDown.TabIndex = 14;
            ThreadsNumericUpDown.ValueChanged += ThreadsNumericUpDown_ValueChanged;
            // 
            // BulkPreloaderMethodComboBox
            // 
            BulkPreloaderMethodComboBox.FormattingEnabled = true;
            BulkPreloaderMethodComboBox.Items.AddRange(new object[] { "Sequentially", "Simultaneously" });
            BulkPreloaderMethodComboBox.Location = new System.Drawing.Point(140, 112);
            BulkPreloaderMethodComboBox.Name = "BulkPreloaderMethodComboBox";
            BulkPreloaderMethodComboBox.Size = new System.Drawing.Size(176, 23);
            BulkPreloaderMethodComboBox.TabIndex = 13;
            BulkPreloaderMethodComboBox.SelectedIndexChanged += BulkPreloaderMethodComboBox_SelectedIndexChanged;
            // 
            // BulkPreloaderMethodLabel
            // 
            BulkPreloaderMethodLabel.AutoSize = true;
            BulkPreloaderMethodLabel.Location = new System.Drawing.Point(6, 115);
            BulkPreloaderMethodLabel.Name = "BulkPreloaderMethodLabel";
            BulkPreloaderMethodLabel.Size = new System.Drawing.Size(131, 15);
            BulkPreloaderMethodLabel.TabIndex = 12;
            BulkPreloaderMethodLabel.Text = "Bulk Preloader Method:";
            // 
            // MultithreadedCheckBox
            // 
            MultithreadedCheckBox.AutoSize = true;
            MultithreadedCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            MultithreadedCheckBox.Location = new System.Drawing.Point(9, 22);
            MultithreadedCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MultithreadedCheckBox.Name = "MultithreadedCheckBox";
            MultithreadedCheckBox.Size = new System.Drawing.Size(108, 19);
            MultithreadedCheckBox.TabIndex = 11;
            MultithreadedCheckBox.Text = "Multi-Threaded";
            MultithreadedCheckBox.UseVisualStyleBackColor = true;
            MultithreadedCheckBox.CheckedChanged += MultithreadedCheckBox_CheckedChanged;
            // 
            // ClearButton
            // 
            ClearButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ClearButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            ClearButton.Location = new System.Drawing.Point(201, 319);
            ClearButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new System.Drawing.Size(88, 28);
            ClearButton.TabIndex = 10;
            ClearButton.Text = "Clear";
            ClearButton.UseVisualStyleBackColor = true;
            // 
            // BrowseButton
            // 
            BrowseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BrowseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            BrowseButton.Location = new System.Drawing.Point(9, 319);
            BrowseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BrowseButton.Name = "BrowseButton";
            BrowseButton.Size = new System.Drawing.Size(88, 28);
            BrowseButton.TabIndex = 9;
            BrowseButton.Text = "Browse";
            BrowseButton.UseVisualStyleBackColor = true;
            BrowseButton.Click += BrowseButton_Click;
            // 
            // PathsLabel
            // 
            PathsLabel.AutoSize = true;
            PathsLabel.Location = new System.Drawing.Point(7, 141);
            PathsLabel.Name = "PathsLabel";
            PathsLabel.Size = new System.Drawing.Size(36, 15);
            PathsLabel.TabIndex = 1;
            PathsLabel.Text = "Paths";
            // 
            // PathsTextBox
            // 
            PathsTextBox.Location = new System.Drawing.Point(9, 159);
            PathsTextBox.Multiline = true;
            PathsTextBox.Name = "PathsTextBox";
            PathsTextBox.Size = new System.Drawing.Size(395, 154);
            PathsTextBox.TabIndex = 0;
            PathsTextBox.TextChanged += PathsTextBox_TextChanged;
            // 
            // PresetsGroupBox
            // 
            PresetsGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            PresetsGroupBox.Controls.Add(ExportPresetsButton);
            PresetsGroupBox.Controls.Add(PresetsTextBox);
            PresetsGroupBox.Controls.Add(PresetsListBox);
            PresetsGroupBox.Controls.Add(DeleteButton);
            PresetsGroupBox.Controls.Add(SavePresetsButton);
            PresetsGroupBox.Location = new System.Drawing.Point(428, 12);
            PresetsGroupBox.Name = "PresetsGroupBox";
            PresetsGroupBox.Size = new System.Drawing.Size(294, 353);
            PresetsGroupBox.TabIndex = 8;
            PresetsGroupBox.TabStop = false;
            PresetsGroupBox.Text = "Presets";
            PresetsGroupBox.Enter += PresetsGroupBox_Enter;
            // 
            // ExportPresetsButton
            // 
            ExportPresetsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ExportPresetsButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            ExportPresetsButton.Location = new System.Drawing.Point(7, 319);
            ExportPresetsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ExportPresetsButton.Name = "ExportPresetsButton";
            ExportPresetsButton.Size = new System.Drawing.Size(88, 28);
            ExportPresetsButton.TabIndex = 15;
            ExportPresetsButton.Text = "Export";
            ExportPresetsButton.UseVisualStyleBackColor = true;
            ExportPresetsButton.Click += ExportPresetsButton_Click;
            // 
            // PresetsTextBox
            // 
            PresetsTextBox.Location = new System.Drawing.Point(8, 290);
            PresetsTextBox.Name = "PresetsTextBox";
            PresetsTextBox.Size = new System.Drawing.Size(280, 23);
            PresetsTextBox.TabIndex = 14;
            PresetsTextBox.TextChanged += PresetsTextBox_TextChanged;
            // 
            // PresetsListBox
            // 
            PresetsListBox.FormattingEnabled = true;
            PresetsListBox.ItemHeight = 15;
            PresetsListBox.Location = new System.Drawing.Point(8, 22);
            PresetsListBox.Name = "PresetsListBox";
            PresetsListBox.Size = new System.Drawing.Size(280, 259);
            PresetsListBox.TabIndex = 13;
            PresetsListBox.SelectedIndexChanged += PresetsListBox_SelectedIndexChanged;
            // 
            // DeleteButton
            // 
            DeleteButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            DeleteButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            DeleteButton.Location = new System.Drawing.Point(199, 319);
            DeleteButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new System.Drawing.Size(88, 28);
            DeleteButton.TabIndex = 12;
            DeleteButton.Text = "Delete";
            DeleteButton.UseVisualStyleBackColor = true;
            // 
            // SavePresetsButton
            // 
            SavePresetsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            SavePresetsButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            SavePresetsButton.Location = new System.Drawing.Point(103, 319);
            SavePresetsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SavePresetsButton.Name = "SavePresetsButton";
            SavePresetsButton.Size = new System.Drawing.Size(88, 28);
            SavePresetsButton.TabIndex = 11;
            SavePresetsButton.Text = "Save";
            SavePresetsButton.UseVisualStyleBackColor = true;
            SavePresetsButton.Click += SavePresetsButton_Click;
            // 
            // StartButton
            // 
            StartButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            StartButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            StartButton.Location = new System.Drawing.Point(12, 371);
            StartButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            StartButton.Name = "StartButton";
            StartButton.Size = new System.Drawing.Size(88, 28);
            StartButton.TabIndex = 9;
            StartButton.Text = "Start";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // BulkPreloaderForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(734, 411);
            Controls.Add(StartButton);
            Controls.Add(PresetsGroupBox);
            Controls.Add(BulkPreloaderOptionsGroupBox);
            Controls.Add(CloseButton);
            Name = "BulkPreloaderForm";
            Text = "WinThumbsPreloader - Bulk Preloader";
            Load += BulkPreloaderForm_Load;
            BulkPreloaderOptionsGroupBox.ResumeLayout(false);
            BulkPreloaderOptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MaximumInstancesNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)ThreadsNumericUpDown).EndInit();
            PresetsGroupBox.ResumeLayout(false);
            PresetsGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.GroupBox BulkPreloaderOptionsGroupBox;
        private System.Windows.Forms.Label PathsLabel;
        private System.Windows.Forms.TextBox PathsTextBox;
        private System.Windows.Forms.GroupBox PresetsGroupBox;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.ListBox PresetsListBox;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button SavePresetsButton;
        private System.Windows.Forms.TextBox PresetsTextBox;
        private System.Windows.Forms.CheckBox MultithreadedCheckBox;
        private System.Windows.Forms.ComboBox BulkPreloaderMethodComboBox;
        private System.Windows.Forms.Label BulkPreloaderMethodLabel;
        private System.Windows.Forms.Label ThreadsLabel;
        private System.Windows.Forms.Button DefaultThreadsButton;
        private System.Windows.Forms.NumericUpDown ThreadsNumericUpDown;
        private System.Windows.Forms.Button ExportPathsButton;
        private System.Windows.Forms.Button ExportPresetsButton;
        private System.Windows.Forms.Label MaximumInstancesLabel;
        private System.Windows.Forms.Button DefaultMaximumInstancesButton;
        private System.Windows.Forms.NumericUpDown MaximumInstancesNumericUpDown;
        private System.Windows.Forms.Button PresetsButton;
        private System.Windows.Forms.Button StartButton;
    }
}