namespace WinThumbsPreloader
{
    partial class LicenseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));
            licenseRichTextBox = new System.Windows.Forms.RichTextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            CloseButton = new System.Windows.Forms.Button();
            OpenLicenseButton = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // licenseRichTextBox
            // 
            licenseRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            licenseRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(licenseRichTextBox, "licenseRichTextBox");
            licenseRichTextBox.Name = "licenseRichTextBox";
            licenseRichTextBox.ReadOnly = true;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.SystemColors.Window;
            groupBox1.Controls.Add(licenseRichTextBox);
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // CloseButton
            // 
            resources.ApplyResources(CloseButton, "CloseButton");
            CloseButton.Name = "CloseButton";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // OpenLicenseButton
            // 
            resources.ApplyResources(OpenLicenseButton, "OpenLicenseButton");
            OpenLicenseButton.Name = "OpenLicenseButton";
            toolTip1.SetToolTip(OpenLicenseButton, resources.GetString("OpenLicenseButton.ToolTip"));
            OpenLicenseButton.UseVisualStyleBackColor = true;
            OpenLicenseButton.Click += OpenLicenseButton_Click;
            // 
            // LicenseForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(OpenLicenseButton);
            Controls.Add(CloseButton);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "LicenseForm";
            Load += LicenseForm_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        public System.Windows.Forms.RichTextBox licenseRichTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button OpenLicenseButton;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}