﻿namespace WinThumbsPreloader
{
    partial class AboutForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.CloseButton = new System.Windows.Forms.Button();
            this.LicenceButton = new System.Windows.Forms.Button();
            this.BorderBottom = new System.Windows.Forms.Label();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.BorderTop = new System.Windows.Forms.Label();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.AppIconPictureBox = new System.Windows.Forms.PictureBox();
            this.UpdateLabel = new System.Windows.Forms.Label();
            this.AppNameLabel = new System.Windows.Forms.Label();
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.RichTextBox = new System.Windows.Forms.RichTextBox();
            this.BottomPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AppIconPictureBox)).BeginInit();
            this.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // LicenceButton
            // 
            resources.ApplyResources(this.LicenceButton, "LicenceButton");
            this.LicenceButton.Name = "LicenceButton";
            this.LicenceButton.UseVisualStyleBackColor = true;
            this.LicenceButton.Click += new System.EventHandler(this.LicenceButton_Click);
            // 
            // BorderBottom
            // 
            this.BorderBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            resources.ApplyResources(this.BorderBottom, "BorderBottom");
            this.BorderBottom.Name = "BorderBottom";
            // 
            // BottomPanel
            // 
            this.BottomPanel.BackColor = System.Drawing.SystemColors.Control;
            this.BottomPanel.Controls.Add(this.CloseButton);
            this.BottomPanel.Controls.Add(this.LicenceButton);
            resources.ApplyResources(this.BottomPanel, "BottomPanel");
            this.BottomPanel.Name = "BottomPanel";
            // 
            // BorderTop
            // 
            this.BorderTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            resources.ApplyResources(this.BorderTop, "BorderTop");
            this.BorderTop.Name = "BorderTop";
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.SystemColors.Window;
            this.HeaderPanel.Controls.Add(this.AppIconPictureBox);
            this.HeaderPanel.Controls.Add(this.UpdateLabel);
            this.HeaderPanel.Controls.Add(this.AppNameLabel);
            resources.ApplyResources(this.HeaderPanel, "HeaderPanel");
            this.HeaderPanel.Name = "HeaderPanel";
            // 
            // AppIconPictureBox
            // 
            resources.ApplyResources(this.AppIconPictureBox, "AppIconPictureBox");
            this.AppIconPictureBox.Name = "AppIconPictureBox";
            this.AppIconPictureBox.TabStop = false;
            // 
            // UpdateLabel
            // 
            resources.ApplyResources(this.UpdateLabel, "UpdateLabel");
            this.UpdateLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UpdateLabel.Name = "UpdateLabel";
            this.UpdateLabel.Click += new System.EventHandler(this.UpdateLabel_Click);
            // 
            // AppNameLabel
            // 
            resources.ApplyResources(this.AppNameLabel, "AppNameLabel");
            this.AppNameLabel.Name = "AppNameLabel";
            // 
            // ContentPanel
            // 
            this.ContentPanel.Controls.Add(this.RichTextBox);
            resources.ApplyResources(this.ContentPanel, "ContentPanel");
            this.ContentPanel.Name = "ContentPanel";
            // 
            // RichTextBox
            // 
            this.RichTextBox.BackColor = System.Drawing.Color.White;
            this.RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.RichTextBox, "RichTextBox");
            this.RichTextBox.Name = "RichTextBox";
            this.RichTextBox.ReadOnly = true;
            this.RichTextBox.ShortcutsEnabled = false;
            this.RichTextBox.TabStop = false;
            this.RichTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.RichTextBox_LinkClicked);
            this.RichTextBox.TextChanged += new System.EventHandler(this.RichTextBox_TextChanged);
            // 
            // AboutForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ContentPanel);
            this.Controls.Add(this.BorderTop);
            this.Controls.Add(this.BorderBottom);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.HeaderPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AboutForm";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.BottomPanel.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AppIconPictureBox)).EndInit();
            this.ContentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button LicenceButton;
        private System.Windows.Forms.Label BorderBottom;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Label BorderTop;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.PictureBox AppIconPictureBox;
        private System.Windows.Forms.Label AppNameLabel;
        private System.Windows.Forms.Panel ContentPanel;
        private System.Windows.Forms.RichTextBox RichTextBox;
        private System.Windows.Forms.Label UpdateLabel;
    }
}

