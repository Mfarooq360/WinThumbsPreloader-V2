﻿using System.Windows.Forms;

namespace WinThumbsPreloader
{
    partial class AboutForm
    {
        /// <summary>
        /// Обязательная переменная конструктора. (Required constructor variable.)
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы. (Release all used resources.)
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно. (True if the managed resource is to be deleted; otherwise false.)</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // (Code automatically generated by Windows Form Designer)
        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте
        /// содержимое этого метода с помощью редактора кода.
        /// (Required method for constructor support — do not change
        /// the contents of this method using the code editor.)
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            CloseButton = new Button();
            LicenceButton = new Button();
            BorderBottom = new Label();
            BottomPanel = new Panel();
            PreloadButton = new Button();
            SettingsButton = new Button();
            BorderTop = new Label();
            HeaderPanel = new Panel();
            AppIconPictureBox = new PictureBox();
            UpdateLabel = new Label();
            AppNameLabel = new Label();
            ContentPanel = new Panel();
            RichTextBox = new RichTextBox();
            PreloadTooltip = new ToolTip(components);
            BottomPanel.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AppIconPictureBox).BeginInit();
            ContentPanel.SuspendLayout();
            SuspendLayout();
            // 
            // CloseButton
            // 
            resources.ApplyResources(CloseButton, "CloseButton");
            CloseButton.Name = "CloseButton";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // LicenceButton
            // 
            resources.ApplyResources(LicenceButton, "LicenceButton");
            LicenceButton.Name = "LicenceButton";
            LicenceButton.UseVisualStyleBackColor = true;
            LicenceButton.Click += LicenceButton_Click;
            // 
            // BorderBottom
            // 
            BorderBottom.BackColor = System.Drawing.Color.FromArgb(223, 223, 223);
            resources.ApplyResources(BorderBottom, "BorderBottom");
            BorderBottom.Name = "BorderBottom";
            // 
            // BottomPanel
            // 
            BottomPanel.BackColor = System.Drawing.SystemColors.Control;
            BottomPanel.Controls.Add(PreloadButton);
            BottomPanel.Controls.Add(SettingsButton);
            BottomPanel.Controls.Add(CloseButton);
            BottomPanel.Controls.Add(LicenceButton);
            resources.ApplyResources(BottomPanel, "BottomPanel");
            BottomPanel.Name = "BottomPanel";
            // 
            // PreloadButton
            // 
            resources.ApplyResources(PreloadButton, "PreloadButton");
            PreloadButton.Name = "PreloadButton";
            PreloadTooltip.SetToolTip(PreloadButton, resources.GetString("PreloadButton.ToolTip"));
            PreloadButton.UseVisualStyleBackColor = true;
            PreloadButton.Click += PreloadButton_Click;
            // 
            // SettingsButton
            // 
            resources.ApplyResources(SettingsButton, "SettingsButton");
            SettingsButton.Name = "SettingsButton";
            SettingsButton.UseVisualStyleBackColor = true;
            SettingsButton.Click += SettingsButton_Click;
            // 
            // BorderTop
            // 
            BorderTop.BackColor = System.Drawing.Color.FromArgb(223, 223, 223);
            resources.ApplyResources(BorderTop, "BorderTop");
            BorderTop.Name = "BorderTop";
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.SystemColors.Window;
            HeaderPanel.Controls.Add(AppIconPictureBox);
            HeaderPanel.Controls.Add(UpdateLabel);
            HeaderPanel.Controls.Add(AppNameLabel);
            resources.ApplyResources(HeaderPanel, "HeaderPanel");
            HeaderPanel.Name = "HeaderPanel";
            // 
            // AppIconPictureBox
            // 
            resources.ApplyResources(AppIconPictureBox, "AppIconPictureBox");
            AppIconPictureBox.Name = "AppIconPictureBox";
            AppIconPictureBox.TabStop = false;
            // 
            // UpdateLabel
            // 
            resources.ApplyResources(UpdateLabel, "UpdateLabel");
            UpdateLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            UpdateLabel.Name = "UpdateLabel";
            UpdateLabel.Click += UpdateLabel_Click;
            // 
            // AppNameLabel
            // 
            resources.ApplyResources(AppNameLabel, "AppNameLabel");
            AppNameLabel.Name = "AppNameLabel";
            AppNameLabel.Click += AppNameLabel_Click;
            // 
            // ContentPanel
            // 
            ContentPanel.Controls.Add(RichTextBox);
            resources.ApplyResources(ContentPanel, "ContentPanel");
            ContentPanel.Name = "ContentPanel";
            // 
            // RichTextBox
            // 
            RichTextBox.BackColor = System.Drawing.Color.White;
            RichTextBox.BorderStyle = BorderStyle.None;
            resources.ApplyResources(RichTextBox, "RichTextBox");
            RichTextBox.Name = "RichTextBox";
            RichTextBox.ReadOnly = true;
            RichTextBox.ShortcutsEnabled = false;
            RichTextBox.TabStop = false;
            RichTextBox.LinkClicked += RichTextBox_LinkClicked;
            // 
            // AboutForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(ContentPanel);
            Controls.Add(BorderTop);
            Controls.Add(BorderBottom);
            Controls.Add(BottomPanel);
            Controls.Add(HeaderPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "AboutForm";
            Load += AboutForm_Load;
            BottomPanel.ResumeLayout(false);
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)AppIconPictureBox).EndInit();
            ContentPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button CloseButton;
        private Button LicenceButton;
        private Label BorderBottom;
        private Panel BottomPanel;
        private Label BorderTop;
        private Panel HeaderPanel;
        private PictureBox AppIconPictureBox;
        private Label AppNameLabel;
        private Panel ContentPanel;
        private RichTextBox RichTextBox;
        private Label UpdateLabel;
        private Button SettingsButton;
        private Button PreloadButton;
        private ToolTip PreloadTooltip;
    }
}

