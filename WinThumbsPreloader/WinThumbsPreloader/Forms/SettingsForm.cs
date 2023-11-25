using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;

namespace WinThumbsPreloader
{
    [SupportedOSPlatform("windows")]
    public partial class SettingsForm : Form //Add specification of thumbnail cache size. 
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.KeyDown += SettingsForm_KeyDown;
            this.KeyUp += SettingsForm_KeyUp;
            this.Activated += SettingsForm_Activated;
            this.KeyPreview = true;
            this.FormClosing += SettingsForm_FormClosing;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.MainIcon;
            MultithreadedCheckBox.Checked = Settings.Default.Multithreaded;
            AdminCheckBox.Checked = Settings.Default.Admin;
            ThreadsNumericUpDown.Value = Settings.Default.ThreadCount;
            ExtensionsTextBox.Text = Settings.Default.ExtensionsText;
            FolderIconsCheckBox.Checked = Settings.Default.PreloadFolderIcons;
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                Environment.Exit(0); // Exit the entire application
            }
        }

        private void AdminCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Admin = AdminCheckBox.Checked;
            Settings.Default.Save();
            SetRunAsAdmin();
            // If the application isn't running as administrator, restart it as administrator
            if (!IsRunningAsAdministrator() && AdminCheckBox.Checked)
            {
                Program.RestartAsAdmin();
            }
        }

        public static void SetRunAsAdmin()
        {
            string key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers";
            string applicationPath = Path.Combine(AppContext.BaseDirectory, $"{AppDomain.CurrentDomain.FriendlyName}.exe");

            if (Settings.Default.Admin)
            {
                // Add run as administrator flag
                Microsoft.Win32.Registry.SetValue(key, applicationPath, "~ RUNASADMIN");
            }
            else
            {
                // Remove run as administrator flag
                var keyObj = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers", true);
                keyObj.DeleteValue(applicationPath, false);
            }
        }

        public static bool IsRunningAsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void OptionsGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void DefaultExtensionsButton_Click(object sender, EventArgs e)
        {
            Settings.Default.ExtensionsText = "avif, bmp, gif, heic, jpg, jpeg, mkv, mov, mp4, png, svg, tif, tiff, webp";
            ExtensionsTextBox.Text = Settings.Default.ExtensionsText;
        }
        private void ClearExtensionsButton_Click(object sender, EventArgs e)
        {
            Settings.Default.ExtensionsText = "";
            ExtensionsTextBox.Text = Settings.Default.ExtensionsText;
        }

        private void ExtensionsTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                Settings.Default.ExtensionsText = textBox.Text;
                Settings.Default.Save();
            }
        }

        private void DefaultThreadsButton_Click(object sender, EventArgs e)
        {
            ThreadsNumericUpDown.Value = 0;
            Settings.Default.ThreadCount = 0;
            MultithreadedCheckBox.Checked = true;
            Settings.Default.Multithreaded = true;
            Settings.Default.Save();
        }

        private void MultithreadedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Multithreaded = MultithreadedCheckBox.Checked;
            Settings.Default.Save();
        }

        private void ThreadsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (numericUpDown != null)
            {
                Settings.Default.ThreadCount = (int)numericUpDown.Value;
                Settings.Default.Save();
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (CloseButton.Text == "Close")
            {
                Close();
            }
            else if (CloseButton.Text == "Exit")
            {
                Environment.Exit(0);
            }
        }

        private void ScheduleButton_Click(object sender, EventArgs e)
        {
            ScheduleForm scheduleForm = new ScheduleForm();
            this.OpenFormCentered(scheduleForm);
        }

        private void FolderIconsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.PreloadFolderIcons = FolderIconsCheckBox.Checked;
            Settings.Default.Save();
        }

        private void CacheButton_Click(object sender, EventArgs e)
        {
            CacheForm cacheForm = new CacheForm();
            this.OpenFormCentered(cacheForm);
        }

        private void SettingsForm_Activated(object sender, EventArgs e)
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

        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text when Shift is pressed
                CloseButton.Text = "Exit";
            }
        }

        private void SettingsForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text back when Shift is released
                CloseButton.Text = "Close";
            }
        }
    }
}
