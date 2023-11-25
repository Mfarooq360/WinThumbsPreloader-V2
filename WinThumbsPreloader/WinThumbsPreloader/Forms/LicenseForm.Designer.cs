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
            licenseRichTextBox.Location = new System.Drawing.Point(6, 22);
            licenseRichTextBox.Name = "licenseRichTextBox";
            licenseRichTextBox.ReadOnly = true;
            licenseRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            licenseRichTextBox.Size = new System.Drawing.Size(298, 224);
            licenseRichTextBox.TabIndex = 0;
            licenseRichTextBox.Text = resources.GetString("licenseRichTextBox.Text");
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.SystemColors.Window;
            groupBox1.Controls.Add(licenseRichTextBox);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(310, 252);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "MIT License";
            // 
            // CloseButton
            // 
            CloseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CloseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            CloseButton.Location = new System.Drawing.Point(233, 270);
            CloseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new System.Drawing.Size(88, 29);
            CloseButton.TabIndex = 1;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // OpenLicenseButton
            // 
            OpenLicenseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OpenLicenseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            OpenLicenseButton.Location = new System.Drawing.Point(13, 270);
            OpenLicenseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            OpenLicenseButton.Name = "OpenLicenseButton";
            OpenLicenseButton.Size = new System.Drawing.Size(100, 29);
            OpenLicenseButton.TabIndex = 5;
            OpenLicenseButton.Text = "Open License";
            toolTip1.SetToolTip(OpenLicenseButton, "Opens the LICENSE.txt file\r\nHolding \"SHIFT\" will open the folder that the license is contained in\r\n\r\n");
            OpenLicenseButton.UseVisualStyleBackColor = true;
            OpenLicenseButton.Click += OpenLicenseButton_Click;
            // 
            // LicenseForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(334, 311);
            Controls.Add(OpenLicenseButton);
            Controls.Add(CloseButton);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "LicenseForm";
            Text = "WinThumbsPreloader - License";
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