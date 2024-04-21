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
            LoggingFrequencyComboBox = new System.Windows.Forms.ComboBox();
            LoggingFrequencyLabel = new System.Windows.Forms.Label();
            PresetsComboBox = new System.Windows.Forms.ComboBox();
            PresetsLabel = new System.Windows.Forms.Label();
            RequestedThumbnailSizesLabel = new System.Windows.Forms.Label();
            PreloaderThumbnailSizesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            DefaultButton = new System.Windows.Forms.Button();
            ExtensionsAutoFormattingComboBox = new System.Windows.Forms.ComboBox();
            ExtensionsAutoFormatLabel = new System.Windows.Forms.Label();
            PreloadFolderIconsForComboBox = new System.Windows.Forms.ComboBox();
            PreloadFolderIconsForLabel = new System.Windows.Forms.Label();
            CloseButton = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            AdvancedSettingsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // AdvancedSettingsGroupBox
            // 
            AdvancedSettingsGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            AdvancedSettingsGroupBox.Controls.Add(LoggingFrequencyComboBox);
            AdvancedSettingsGroupBox.Controls.Add(LoggingFrequencyLabel);
            AdvancedSettingsGroupBox.Controls.Add(PresetsComboBox);
            AdvancedSettingsGroupBox.Controls.Add(PresetsLabel);
            AdvancedSettingsGroupBox.Controls.Add(RequestedThumbnailSizesLabel);
            AdvancedSettingsGroupBox.Controls.Add(PreloaderThumbnailSizesCheckedListBox);
            AdvancedSettingsGroupBox.Controls.Add(DefaultButton);
            AdvancedSettingsGroupBox.Controls.Add(ExtensionsAutoFormattingComboBox);
            AdvancedSettingsGroupBox.Controls.Add(ExtensionsAutoFormatLabel);
            AdvancedSettingsGroupBox.Controls.Add(PreloadFolderIconsForComboBox);
            AdvancedSettingsGroupBox.Controls.Add(PreloadFolderIconsForLabel);
            resources.ApplyResources(AdvancedSettingsGroupBox, "AdvancedSettingsGroupBox");
            AdvancedSettingsGroupBox.Name = "AdvancedSettingsGroupBox";
            AdvancedSettingsGroupBox.TabStop = false;
            // 
            // LoggingFrequencyComboBox
            // 
            LoggingFrequencyComboBox.FormattingEnabled = true;
            LoggingFrequencyComboBox.Items.AddRange(new object[] { resources.GetString("LoggingFrequencyComboBox.Items"), resources.GetString("LoggingFrequencyComboBox.Items1"), resources.GetString("LoggingFrequencyComboBox.Items2"), resources.GetString("LoggingFrequencyComboBox.Items3"), resources.GetString("LoggingFrequencyComboBox.Items4") });
            resources.ApplyResources(LoggingFrequencyComboBox, "LoggingFrequencyComboBox");
            LoggingFrequencyComboBox.Name = "LoggingFrequencyComboBox";
            LoggingFrequencyComboBox.SelectedIndexChanged += LoggingFrequencyComboBox_SelectedIndexChanged;
            // 
            // LoggingFrequencyLabel
            // 
            resources.ApplyResources(LoggingFrequencyLabel, "LoggingFrequencyLabel");
            LoggingFrequencyLabel.Name = "LoggingFrequencyLabel";
            toolTip1.SetToolTip(LoggingFrequencyLabel, resources.GetString("LoggingFrequencyLabel.ToolTip"));
            // 
            // PresetsComboBox
            // 
            PresetsComboBox.FormattingEnabled = true;
            PresetsComboBox.Items.AddRange(new object[] { resources.GetString("PresetsComboBox.Items"), resources.GetString("PresetsComboBox.Items1") });
            resources.ApplyResources(PresetsComboBox, "PresetsComboBox");
            PresetsComboBox.Name = "PresetsComboBox";
            PresetsComboBox.SelectedIndexChanged += PresetsComboBox_SelectedIndexChanged;
            // 
            // PresetsLabel
            // 
            resources.ApplyResources(PresetsLabel, "PresetsLabel");
            PresetsLabel.Name = "PresetsLabel";
            // 
            // RequestedThumbnailSizesLabel
            // 
            resources.ApplyResources(RequestedThumbnailSizesLabel, "RequestedThumbnailSizesLabel");
            RequestedThumbnailSizesLabel.Name = "RequestedThumbnailSizesLabel";
            toolTip1.SetToolTip(RequestedThumbnailSizesLabel, resources.GetString("RequestedThumbnailSizesLabel.ToolTip"));
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
            // DefaultButton
            // 
            resources.ApplyResources(DefaultButton, "DefaultButton");
            DefaultButton.Name = "DefaultButton";
            DefaultButton.UseVisualStyleBackColor = true;
            DefaultButton.Click += DefaultButton_Click;
            // 
            // ExtensionsAutoFormattingComboBox
            // 
            ExtensionsAutoFormattingComboBox.FormattingEnabled = true;
            ExtensionsAutoFormattingComboBox.Items.AddRange(new object[] { resources.GetString("ExtensionsAutoFormattingComboBox.Items"), resources.GetString("ExtensionsAutoFormattingComboBox.Items1"), resources.GetString("ExtensionsAutoFormattingComboBox.Items2"), resources.GetString("ExtensionsAutoFormattingComboBox.Items3"), resources.GetString("ExtensionsAutoFormattingComboBox.Items4") });
            resources.ApplyResources(ExtensionsAutoFormattingComboBox, "ExtensionsAutoFormattingComboBox");
            ExtensionsAutoFormattingComboBox.Name = "ExtensionsAutoFormattingComboBox";
            ExtensionsAutoFormattingComboBox.SelectedIndexChanged += ExtensionsAutoFormattingComboBox_SelectedIndexChanged;
            // 
            // ExtensionsAutoFormatLabel
            // 
            resources.ApplyResources(ExtensionsAutoFormatLabel, "ExtensionsAutoFormatLabel");
            ExtensionsAutoFormatLabel.Name = "ExtensionsAutoFormatLabel";
            toolTip1.SetToolTip(ExtensionsAutoFormatLabel, resources.GetString("ExtensionsAutoFormatLabel.ToolTip"));
            // 
            // PreloadFolderIconsForComboBox
            // 
            PreloadFolderIconsForComboBox.FormattingEnabled = true;
            PreloadFolderIconsForComboBox.Items.AddRange(new object[] { resources.GetString("PreloadFolderIconsForComboBox.Items"), resources.GetString("PreloadFolderIconsForComboBox.Items1") });
            resources.ApplyResources(PreloadFolderIconsForComboBox, "PreloadFolderIconsForComboBox");
            PreloadFolderIconsForComboBox.Name = "PreloadFolderIconsForComboBox";
            PreloadFolderIconsForComboBox.SelectedIndexChanged += PreloadFolderIconsForComboBox_SelectedIndexChanged;
            // 
            // PreloadFolderIconsForLabel
            // 
            resources.ApplyResources(PreloadFolderIconsForLabel, "PreloadFolderIconsForLabel");
            PreloadFolderIconsForLabel.Name = "PreloadFolderIconsForLabel";
            toolTip1.SetToolTip(PreloadFolderIconsForLabel, resources.GetString("PreloadFolderIconsForLabel.ToolTip"));
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
            Controls.Add(CloseButton);
            Controls.Add(AdvancedSettingsGroupBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "AdvancedSettingsForm";
            Load += AdvancedSettingsForm_Load;
            AdvancedSettingsGroupBox.ResumeLayout(false);
            AdvancedSettingsGroupBox.PerformLayout();
            ResumeLayout(false);
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
    }
}