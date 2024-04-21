using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        private bool isShiftPressed = false;

        private void LicenseForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                isShiftPressed = true;
                OpenLicenseButton.Text = "Open Folder";
                CloseButton.Text = "Exit";
            }
        }

        private void LicenseForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                isShiftPressed = false;
                OpenLicenseButton.Text = "Open License";
                CloseButton.Text = "Close";
            }
        }

        private void LicenseForm_Activated(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                // Shift key is currently pressed, so change the button text
                OpenLicenseButton.Text = "Open Folder";
                CloseButton.Text = "Exit";
            }
            else
            {
                // Shift key is not pressed, so set the button text to its original value
                OpenLicenseButton.Text = "Open License";
                CloseButton.Text = "Close";
            }
        }

        private void OpenLicenseButton_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "LICENSE.txt");
            WriteLine("License file path: " + path, LoggingFrequency.DebugLogging);

            // The license content
            string licenseContent = @"MIT License

Copyright (c) 2018 Dmitry Bruhov
Copyright (c) 2024 Mutahar Farooq

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

            // Check if the file exists
            if (File.Exists(path))
            {
                // Read the current content of the file
                string currentContent = File.ReadAllText(path);

                // Compare the current content with the license content
                if (currentContent != licenseContent)
                {
                    // The contents are different, rewrite the file
                    File.WriteAllText(path, licenseContent);
                }
            }
            else
            {
                // If the file does not exist, create it with the license content
                File.WriteAllText(path, licenseContent);
            }

            try
            {
                if (isShiftPressed)
                {
                    WriteLine("Opening license folder", LoggingFrequency.GUILogging);
                    Process.Start("explorer.exe", $"/select,\"{path}\"");
                }
                else
                {
                    WriteLine("Opening license file", LoggingFrequency.GUILogging);
                    Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                WriteLine("Failed to open the license file: " + ex.Message, LoggingFrequency.GUILogging);
                MessageBox.Show("Failed to open the license file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
