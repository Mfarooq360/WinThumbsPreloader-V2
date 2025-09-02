using System;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader
{
    public partial class LicenseForm : Form
    {
        public LicenseForm()
        {
            InitializeComponent();
            this.KeyDown += LicenseForm_KeyDown;
            this.KeyUp += LicenseForm_KeyUp;
            this.Activated += LicenseForm_Activated;
            this.KeyPreview = true;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (CloseButton.Text == "Close")
            {
                WriteLine("Closing License Form", LoggingFrequency.GUILogging);
                Close();
            }
            else if (CloseButton.Text == "Exit")
            {
                WriteLine("Exiting application from license form", LoggingFrequency.GUILogging);
                Environment.Exit(0);
            }
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.MainIcon;
        }

        private void LicenseForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                CloseButton.Text = "Exit";
            }
        }

        private void LicenseForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                CloseButton.Text = "Close";
            }
        }

        private void LicenseForm_Activated(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                // Shift key is currently pressed, so change the button text
                CloseButton.Text = "Exit";
            }
            else
            {
                // Shift key is not pressed, so set the button text to its original value
                CloseButton.Text = "Close";
            }
        }
    }
}
