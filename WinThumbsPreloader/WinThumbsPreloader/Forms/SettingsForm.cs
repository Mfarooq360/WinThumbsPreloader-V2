using System;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Forms;
using WinThumbsPreloader.Forms;
using WinThumbsPreloader.Properties;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader
{
    [SupportedOSPlatform("windows")]
    public partial class SettingsForm : Form // TODO: Add swapping of preload button options between recursive and bulk so they can swap between shift and ctrl (bulk form not implemented yet)
    {
        private bool adminCheckboxInitialized = false;
        private bool adminCheckboxUserAction = false;

        public SettingsForm()
        {
            InitializeComponent();
            AdminCheckBox.MouseDown += (_, __) => adminCheckboxUserAction = true;
            AdminCheckBox.KeyDown += (_, e) => { if (e.KeyCode == Keys.Space) adminCheckboxUserAction = true; };
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
            adminCheckboxInitialized = false; // prevent prompts
            AdminCheckBox.Checked = Settings.Default.Admin;
            AdminCheckBoxUpdate();
            adminCheckboxInitialized = true; // now user actions count
            ThreadsNumericUpDown.Value = Settings.Default.ThreadCount;
            ExtensionsTextBox.Text = Settings.Default.ExtensionsText;
            FolderIconsCheckBox.Checked = Settings.Default.PreloadFolderIcons;
            UpdateExtensionsTextBoxDisplay();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                WriteLine("Exiting application from SettingsForm", LoggingFrequency.DebugLogging);
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

        private void AdminCheckBoxUpdate()
        {
            bool runningasAdmin = Program.IsRunningAsAdministrator();

            if (runningasAdmin)
            {
                AdminCheckBox.ForeColor = Color.LimeGreen;
                SettingsToolTips.SetToolTip(AdminCheckBox, "Toggles whether the preloader is run as Admin or not.\r\nEnabling this will scan more files on the disk, but may break\r\ncompatibility with some thumbnail generators like PowerToys.\r\nIf possible, use SVGSee as it is much faster and compatible with Admin.\r\n\r\nWinThumbsPreloader is currently running as Administrator.");
            }
            else
            {
                if (Settings.Default.Admin)
                {
                    AdminCheckBox.ForeColor = Color.Red;
                    SettingsToolTips.SetToolTip(AdminCheckBox, "Toggles whether the preloa" +
                        "der is run as Admin or not.\r\nEnabling this will scan more files on the disk, but may break\r\ncompatibility with some thumbnail generators like PowerToys.\r\nIf possible, use SVGSee as it is much faster and compatible with Admin.\r\n\r\nWinThumbsPreloader is currently NOT running as Administrator.");
                }
                else
                {
                    AdminCheckBox.ForeColor = SystemColors.ControlText;
                    SettingsToolTips.SetToolTip(AdminCheckBox, "Toggles whether the preloader is run as Admin or not.\r\nEnabling this will scan more files on the disk, but may break\r\ncompatibility with some thumbnail generators like PowerToys.\r\nIf possible, use SVGSee as it is much faster and compatible with Admin.");
                }
            }
        }

        private void AdminCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Ignore automatic/form-driven changes
            if (!adminCheckboxInitialized || !adminCheckboxUserAction)
                return;

            adminCheckboxUserAction = false; // consume the flag

            bool wantAdmin = AdminCheckBox.Checked;
            bool isAdmin = Program.IsRunningAsAdministrator();

            Settings.Default.Admin = wantAdmin;
            Settings.Default.Save();

            if (wantAdmin && !isAdmin)
            {
                var r = MessageBox.Show(
                    "Restart as administrator?",
                    "Requires Elevation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (r == DialogResult.Yes)
                {
                    if (Program.RestartAsAdmin(["-reopensettings"])) { return; }

                }
                AdminCheckBoxUpdate();
                AdminCheckBox.Text = "Run As Admin (relaunch required)";
                AdminCheckBox.ForeColor = Color.Orange;
                return;
            }
            else if (!wantAdmin && isAdmin)
            {
                AdminCheckBoxUpdate();
                AdminCheckBox.Text = "Run As Admin (relaunch required)";
                return;
            }
            AdminCheckBoxUpdate();
            AdminCheckBox.Text = "Run As Admin";
        }

        private void DefaultExtensionsButton_Click(object sender, EventArgs e)
        {
            Settings.Default.ExtensionsText = "avi, avif, bmp, gif, heic, heif, jpg, jpeg, mkv, mov, mp4, png, svg, tif, tiff, webp";
            ExtensionsTextBox.Text = Settings.Default.ExtensionsText;
            WriteLine("Default extensions applied", LoggingFrequency.DebugLogging);
            UpdateExtensionsTextBoxDisplay();
        }

        private void ClearExtensionsButton_Click(object sender, EventArgs e)
        {
            Settings.Default.ExtensionsText = "";
            ExtensionsTextBox.Text = Settings.Default.ExtensionsText;
            WriteLine("Extensions text cleared", LoggingFrequency.DebugLogging);
        }

        private void ExtensionsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                Settings.Default.ExtensionsText = textBox.Text;
                Settings.Default.Save();
                WriteLine("Extensions text changed", LoggingFrequency.DebugLogging);
                WriteLine("Extensions text: " + textBox.Text, LoggingFrequency.DebugLogging);
            }
        }

        private void ExtensionsTextBox_LostFocus(object sender, EventArgs e)
        {
            UpdateExtensionsTextBoxDisplay();
        }

        public void UpdateExtensionsTextBoxDisplay()
        {
            WriteLine("Updating extensions text box display", LoggingFrequency.DebugLogging);

            ExtensionsTextBox.LostFocus -= ExtensionsTextBox_LostFocus;

            SortingMethod method = Enum.Parse<SortingMethod>(Settings.Default.ExtensionsAutoFormatting);
            ExtensionsTextBox.Text = OrganizeExtensions(ExtensionsTextBox.Text, method);
            WriteLine("ExtensionsTextBox.Text: " + ExtensionsTextBox.Text, LoggingFrequency.DebugLogging);

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

        private static string OrganizeExtensions(string text, SortingMethod method)
        {
            WriteLine("Organizing extensions", LoggingFrequency.DebugLogging);
            var extensions = text.Split([',', ' ', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

            switch (method)
            {
                case SortingMethod.Disabled:
                    return text;
                case SortingMethod.Vertically:
                    return string.Join(Environment.NewLine, extensions);
                case SortingMethod.Commas:
                    return string.Join(",", extensions);
                case SortingMethod.Spaces:
                    return string.Join(" ", extensions);
                case SortingMethod.CommasAndSpaces:
                    return string.Join(", ", extensions);
                default:
                    return text;
            }
        }

        private void ExtensionsTextBox_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Text))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                WriteLine("Error during drag enter: " + ex.Message, LoggingFrequency.GUILogging);
            }
        }

        private async void ExtensionsTextBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files != null && files.Length > 0)
                    {
                        string fileContent = await File.ReadAllTextAsync(files[0]);
                        WriteLine("File content: " + fileContent, LoggingFrequency.DebugLogging);
                        ExtensionsTextBox.Text = fileContent.Length > 10000 ? fileContent.Substring(0, 10000) : fileContent;
                        WriteLine("ExtensionsTextBox.Text: " + ExtensionsTextBox.Text, LoggingFrequency.DebugLogging);
                    }
                }
                else if (e.Data.GetDataPresent(DataFormats.Text))
                {
                    string text = (string)e.Data.GetData(DataFormats.Text);
                    WriteLine("Text: " + text, LoggingFrequency.DebugLogging);
                    ExtensionsTextBox.Text = text.Length > 10000 ? text.Substring(0, 10000) : text;
                    WriteLine("ExtensionsTextBox.Text: " + ExtensionsTextBox.Text, LoggingFrequency.DebugLogging);
                }

                Settings.Default.ExtensionsText = ExtensionsTextBox.Text;
                Settings.Default.Save();
                WriteLine("Extensions text updated from drag and drop", LoggingFrequency.DebugLogging);
            }
            catch (Exception ex)
            {
                WriteLine("Error processing text: " + ex.Message, LoggingFrequency.GUILogging);
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
            WriteLine("Default thread count and multithreaded settings applied", LoggingFrequency.DebugLogging);
        }

        private void MultithreadedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Multithreaded = MultithreadedCheckBox.Checked;
            Settings.Default.Save();
            WriteLine("Multithreaded: " + MultithreadedCheckBox.Checked, LoggingFrequency.DebugLogging);
        }

        private void ThreadsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (sender is NumericUpDown numericUpDown)
            {
                Settings.Default.ThreadCount = (int)numericUpDown.Value;
                Settings.Default.Save();
                WriteLine("Thread count: " + (int)numericUpDown.Value, LoggingFrequency.DebugLogging);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (CloseButton.Text == "Close")
            {
                WriteLine("Closing settings form", LoggingFrequency.DebugLogging);
                Close();
            }
            else if (CloseButton.Text == "Exit")
            {
                WriteLine("Exiting application", LoggingFrequency.DebugLogging);
                Environment.Exit(0);
            }
        }

        private void ScheduleButton_Click(object sender, EventArgs e)
        {
            WriteLine("Opening schedule form", LoggingFrequency.DebugLogging);
            ScheduleForm scheduleForm = new ScheduleForm();
            this.OpenFormCentered(scheduleForm);
        }

        private void FolderIconsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.PreloadFolderIcons = FolderIconsCheckBox.Checked;
            Settings.Default.Save();
            WriteLine("Preload folder icons: " + FolderIconsCheckBox.Checked, LoggingFrequency.DebugLogging);
        }

        private void CacheButton_Click(object sender, EventArgs e)
        {
            WriteLine("Opening cache form", LoggingFrequency.DebugLogging);
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
            WriteLine("Exporting extensions", LoggingFrequency.DebugLogging);
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = "ThumbnailExtensions.txt";
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv";
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.AddExtension = true;
                saveFileDialog.Title = "Save Extensions";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, ExtensionsTextBox.Text);
                }
            }
        }

        private void AdvancedButton_Click(object sender, EventArgs e)
        {
            WriteLine("Opening advanced settings form", LoggingFrequency.DebugLogging);
            AdvancedSettingsForm advancedSettingsForm = new AdvancedSettingsForm();
            this.OpenSecondaryFormCentered(advancedSettingsForm);
        }
    }
}
