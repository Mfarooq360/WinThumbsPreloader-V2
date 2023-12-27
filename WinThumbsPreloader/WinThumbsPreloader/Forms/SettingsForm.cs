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
using WinThumbsPreloader.Forms;
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
            ExtensionsTextBox.DragEnter += new DragEventHandler(ExtensionsTextBox_DragEnter);
            ExtensionsTextBox.DragDrop += new DragEventHandler(ExtensionsTextBox_DragDrop);
            ExtensionsTextBox.LostFocus += new EventHandler(ExtensionsTextBox_LostFocus);
            this.MouseDown += new MouseEventHandler(SettingsForm_MouseDown);
            OptionsGroupBox.MouseDown += new MouseEventHandler(GroupBox_MouseDown);
            ExtensionsGroupBox.MouseDown += new MouseEventHandler(GroupBox_MouseDown);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.MainIcon;
            MultithreadedCheckBox.Checked = Settings.Default.Multithreaded;
            AdminCheckBox.Checked = Settings.Default.Admin;
            ThreadsNumericUpDown.Value = Settings.Default.ThreadCount;
            ExtensionsTextBox.Text = Settings.Default.ExtensionsText;
            FolderIconsCheckBox.Checked = Settings.Default.PreloadFolderIcons;
            UpdateExtensionsTextBoxDisplay();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                Environment.Exit(0); // Exit the entire application
            }
        }

        private void SettingsForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (ExtensionsTextBox.Focused)
            {
                UpdateExtensionsTextBoxDisplay();
            }
        }

        private void GroupBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (ExtensionsTextBox.Focused)
            {
                UpdateExtensionsTextBoxDisplay();
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

        private void DefaultExtensionsButton_Click(object sender, EventArgs e)
        {
            Settings.Default.ExtensionsText = "avif, bmp, gif, heic, heif, jpg, jpeg, mkv, mov, mp4, png, svg, tif, tiff, webp";
            ExtensionsTextBox.Text = Settings.Default.ExtensionsText;
            UpdateExtensionsTextBoxDisplay();
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
                // Save the user's raw input
                Settings.Default.ExtensionsText = textBox.Text;
                Settings.Default.Save();
            }
        }

        private void ExtensionsTextBox_LostFocus(object sender, EventArgs e)
        {
            UpdateExtensionsTextBoxDisplay();
        }

        public void UpdateExtensionsTextBoxDisplay()
        {
            // Prevent recursive updates
            ExtensionsTextBox.LostFocus -= ExtensionsTextBox_LostFocus;

            // Get the current sorting method from settings
            SortingMethod method = (SortingMethod)Enum.Parse(typeof(SortingMethod), Settings.Default.ExtensionsAutoFormatting);
            ExtensionsTextBox.Text = OrganizeExtensions(ExtensionsTextBox.Text, method);

            // Reattach the LostFocus event handler
            ExtensionsTextBox.LostFocus += ExtensionsTextBox_LostFocus;
        }

        public enum SortingMethod
        {
            Disabled,
            Vertically,
            Commas,
            Spaces,
            CommasAndSpaces
        }

        private string OrganizeExtensions(string text, SortingMethod method)
        {
            var extensions = text.Split(new[] { ',', ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            switch (method)
            {
                case SortingMethod.Disabled:
                    // No sorting, return the original text
                    return text;
                case SortingMethod.Vertically:
                    // Join with new lines, vertically
                    return string.Join(Environment.NewLine, extensions);
                case SortingMethod.Commas:
                    // Join with commas
                    return string.Join(",", extensions);
                case SortingMethod.Spaces:
                    // Join with spaces
                    return string.Join(" ", extensions);
                case SortingMethod.CommasAndSpaces:
                    // Join with commas and spaces
                    return string.Join(", ", extensions);
                default:
                    return text; // Default case returns the original text
            }
        }

        private void ExtensionsTextBox_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the data being dragged is a file
            if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private async void ExtensionsTextBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    // Handle file drop
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files != null && files.Length > 0)
                    {
                        string fileContent = await File.ReadAllTextAsync(files[0]);
                        ExtensionsTextBox.Text = fileContent.Length > 10000 ? fileContent.Substring(0, 10000) : fileContent;
                    }
                }
                else if (e.Data.GetDataPresent(DataFormats.Text))
                {
                    // Handle text drop
                    string text = (string)e.Data.GetData(DataFormats.Text);
                    ExtensionsTextBox.Text = text;
                }

                // Save the new content to settings
                Settings.Default.ExtensionsText = ExtensionsTextBox.Text;
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing text: " + ex.Message);
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

        private void ExportButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set properties of SaveFileDialog
                saveFileDialog.FileName = "ThumbnailExtensions.txt";
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv";
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.AddExtension = true;
                saveFileDialog.Title = "Save Extensions";

                // Show the dialog and check if the user clicked the save button
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file name and save the contents of ExtensionsTextBox
                    File.WriteAllText(saveFileDialog.FileName, ExtensionsTextBox.Text);
                }
            }
        }

        private void AdvancedButton_Click(object sender, EventArgs e)
        {
            AdvancedSettingsForm advancedSettingsForm = new AdvancedSettingsForm();
            this.OpenSecondaryFormCentered(advancedSettingsForm);
        }
    }
}
