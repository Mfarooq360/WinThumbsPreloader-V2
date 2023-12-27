namespace WinThumbsPreloader
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            AdminCheckBox = new System.Windows.Forms.CheckBox();
            MultithreadedCheckBox = new System.Windows.Forms.CheckBox();
            OptionsGroupBox = new System.Windows.Forms.GroupBox();
            ThreadsLabel = new System.Windows.Forms.Label();
            DefaultThreadsButton = new System.Windows.Forms.Button();
            ThreadsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ClearExtensionsButton = new System.Windows.Forms.Button();
            DefaultExtensionsButton = new System.Windows.Forms.Button();
            ExtensionsGroupBox = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            ExportButton = new System.Windows.Forms.Button();
            FolderIconsCheckBox = new System.Windows.Forms.CheckBox();
            ExtensionsTextBox = new System.Windows.Forms.TextBox();
            CloseButton = new System.Windows.Forms.Button();
            ClearExtensionsToolTip = new System.Windows.Forms.ToolTip(components);
            DefaultExtensionsToolTip = new System.Windows.Forms.ToolTip(components);
            MultithreadedToolTip = new System.Windows.Forms.ToolTip(components);
            AdminToolTip = new System.Windows.Forms.ToolTip(components);
            ScheduleButton = new System.Windows.Forms.Button();
            ThreadsToolTip = new System.Windows.Forms.ToolTip(components);
            CacheButton = new System.Windows.Forms.Button();
            PreloadFolderIconsToolTip = new System.Windows.Forms.ToolTip(components);
            SettingsToolTips = new System.Windows.Forms.ToolTip(components);
            AdvancedButton = new System.Windows.Forms.Button();
            OptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ThreadsNumericUpDown).BeginInit();
            ExtensionsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // AdminCheckBox
            // 
            resources.ApplyResources(AdminCheckBox, "AdminCheckBox");
            AdminCheckBox.Name = "AdminCheckBox";
            SettingsToolTips.SetToolTip(AdminCheckBox, resources.GetString("AdminCheckBox.ToolTip"));
            AdminCheckBox.UseVisualStyleBackColor = true;
            AdminCheckBox.CheckedChanged += AdminCheckBox_CheckedChanged;
            // 
            // MultithreadedCheckBox
            // 
            resources.ApplyResources(MultithreadedCheckBox, "MultithreadedCheckBox");
            MultithreadedCheckBox.Name = "MultithreadedCheckBox";
            SettingsToolTips.SetToolTip(MultithreadedCheckBox, resources.GetString("MultithreadedCheckBox.ToolTip"));
            MultithreadedCheckBox.UseVisualStyleBackColor = true;
            MultithreadedCheckBox.CheckedChanged += MultithreadedCheckBox_CheckedChanged;
            // 
            // OptionsGroupBox
            // 
            OptionsGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            OptionsGroupBox.Controls.Add(ThreadsLabel);
            OptionsGroupBox.Controls.Add(DefaultThreadsButton);
            OptionsGroupBox.Controls.Add(ThreadsNumericUpDown);
            OptionsGroupBox.Controls.Add(AdminCheckBox);
            OptionsGroupBox.Controls.Add(MultithreadedCheckBox);
            resources.ApplyResources(OptionsGroupBox, "OptionsGroupBox");
            OptionsGroupBox.Name = "OptionsGroupBox";
            OptionsGroupBox.TabStop = false;
            // 
            // ThreadsLabel
            // 
            resources.ApplyResources(ThreadsLabel, "ThreadsLabel");
            ThreadsLabel.Name = "ThreadsLabel";
            SettingsToolTips.SetToolTip(ThreadsLabel, resources.GetString("ThreadsLabel.ToolTip"));
            // 
            // DefaultThreadsButton
            // 
            resources.ApplyResources(DefaultThreadsButton, "DefaultThreadsButton");
            DefaultThreadsButton.Name = "DefaultThreadsButton";
            SettingsToolTips.SetToolTip(DefaultThreadsButton, resources.GetString("DefaultThreadsButton.ToolTip"));
            DefaultThreadsButton.UseVisualStyleBackColor = true;
            DefaultThreadsButton.Click += DefaultThreadsButton_Click;
            // 
            // ThreadsNumericUpDown
            // 
            resources.ApplyResources(ThreadsNumericUpDown, "ThreadsNumericUpDown");
            ThreadsNumericUpDown.Maximum = new decimal(new int[] { 256, 0, 0, 0 });
            ThreadsNumericUpDown.Name = "ThreadsNumericUpDown";
            ThreadsNumericUpDown.ValueChanged += ThreadsNumericUpDown_ValueChanged;
            // 
            // ClearExtensionsButton
            // 
            resources.ApplyResources(ClearExtensionsButton, "ClearExtensionsButton");
            ClearExtensionsButton.Name = "ClearExtensionsButton";
            SettingsToolTips.SetToolTip(ClearExtensionsButton, resources.GetString("ClearExtensionsButton.ToolTip"));
            ClearExtensionsButton.UseVisualStyleBackColor = true;
            ClearExtensionsButton.Click += ClearExtensionsButton_Click;
            // 
            // DefaultExtensionsButton
            // 
            resources.ApplyResources(DefaultExtensionsButton, "DefaultExtensionsButton");
            DefaultExtensionsButton.Name = "DefaultExtensionsButton";
            SettingsToolTips.SetToolTip(DefaultExtensionsButton, resources.GetString("DefaultExtensionsButton.ToolTip"));
            DefaultExtensionsButton.UseVisualStyleBackColor = true;
            DefaultExtensionsButton.Click += DefaultExtensionsButton_Click;
            // 
            // ExtensionsGroupBox
            // 
            ExtensionsGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            ExtensionsGroupBox.Controls.Add(label1);
            ExtensionsGroupBox.Controls.Add(ExportButton);
            ExtensionsGroupBox.Controls.Add(FolderIconsCheckBox);
            ExtensionsGroupBox.Controls.Add(ExtensionsTextBox);
            ExtensionsGroupBox.Controls.Add(DefaultExtensionsButton);
            ExtensionsGroupBox.Controls.Add(ClearExtensionsButton);
            resources.ApplyResources(ExtensionsGroupBox, "ExtensionsGroupBox");
            ExtensionsGroupBox.Name = "ExtensionsGroupBox";
            ExtensionsGroupBox.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // ExportButton
            // 
            resources.ApplyResources(ExportButton, "ExportButton");
            ExportButton.Name = "ExportButton";
            SettingsToolTips.SetToolTip(ExportButton, resources.GetString("ExportButton.ToolTip"));
            ExportButton.UseVisualStyleBackColor = true;
            ExportButton.Click += ExportButton_Click;
            // 
            // FolderIconsCheckBox
            // 
            resources.ApplyResources(FolderIconsCheckBox, "FolderIconsCheckBox");
            FolderIconsCheckBox.Name = "FolderIconsCheckBox";
            SettingsToolTips.SetToolTip(FolderIconsCheckBox, resources.GetString("FolderIconsCheckBox.ToolTip"));
            FolderIconsCheckBox.UseVisualStyleBackColor = true;
            FolderIconsCheckBox.CheckedChanged += FolderIconsCheckBox_CheckedChanged;
            // 
            // ExtensionsTextBox
            // 
            ExtensionsTextBox.AllowDrop = true;
            resources.ApplyResources(ExtensionsTextBox, "ExtensionsTextBox");
            ExtensionsTextBox.Name = "ExtensionsTextBox";
            ExtensionsTextBox.TextChanged += ExtensionsTextBox_TextChanged;
            // 
            // CloseButton
            // 
            resources.ApplyResources(CloseButton, "CloseButton");
            CloseButton.Name = "CloseButton";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // ScheduleButton
            // 
            resources.ApplyResources(ScheduleButton, "ScheduleButton");
            ScheduleButton.Name = "ScheduleButton";
            ScheduleButton.UseVisualStyleBackColor = true;
            ScheduleButton.Click += ScheduleButton_Click;
            // 
            // CacheButton
            // 
            resources.ApplyResources(CacheButton, "CacheButton");
            CacheButton.Name = "CacheButton";
            CacheButton.UseVisualStyleBackColor = true;
            CacheButton.Click += CacheButton_Click;
            // 
            // AdvancedButton
            // 
            resources.ApplyResources(AdvancedButton, "AdvancedButton");
            AdvancedButton.Name = "AdvancedButton";
            AdvancedButton.UseVisualStyleBackColor = true;
            AdvancedButton.Click += AdvancedButton_Click;
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(AdvancedButton);
            Controls.Add(CacheButton);
            Controls.Add(ScheduleButton);
            Controls.Add(CloseButton);
            Controls.Add(ExtensionsGroupBox);
            Controls.Add(OptionsGroupBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "SettingsForm";
            Load += SettingsForm_Load;
            OptionsGroupBox.ResumeLayout(false);
            OptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ThreadsNumericUpDown).EndInit();
            ExtensionsGroupBox.ResumeLayout(false);
            ExtensionsGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.CheckBox AdminCheckBox;
        private System.Windows.Forms.CheckBox MultithreadedCheckBox;
        private System.Windows.Forms.GroupBox OptionsGroupBox;
        private System.Windows.Forms.NumericUpDown ThreadsNumericUpDown;
        private System.Windows.Forms.GroupBox ExtensionsGroupBox;
        private System.Windows.Forms.TextBox ExtensionsTextBox;
        private System.Windows.Forms.Button DefaultThreadsButton;
        private System.Windows.Forms.Button DefaultExtensionsButton;
        private System.Windows.Forms.Button ClearExtensionsButton;
        private System.Windows.Forms.ToolTip DefaultExtensionsToolTip;
        private System.Windows.Forms.ToolTip ClearExtensionsToolTip;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ToolTip MultithreadedToolTip;
        private System.Windows.Forms.ToolTip AdminToolTip;
        private System.Windows.Forms.Button ScheduleButton;
        private System.Windows.Forms.Label ThreadsLabel;
        private System.Windows.Forms.ToolTip ThreadsToolTip;
        private System.Windows.Forms.CheckBox FolderIconsCheckBox;
        private System.Windows.Forms.Button CacheButton;
        private System.Windows.Forms.ToolTip PreloadFolderIconsToolTip;
        private System.Windows.Forms.ToolTip SettingsToolTips;
        private System.Windows.Forms.Button AdvancedButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Label label1;
    }
}