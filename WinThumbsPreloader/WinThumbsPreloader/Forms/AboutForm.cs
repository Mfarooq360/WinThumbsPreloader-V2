using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using WinThumbsPreloader.Forms;
using WinThumbsPreloader.Properties;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            this.KeyDown += AboutForm_KeyDown;
            this.KeyUp += AboutForm_KeyUp;
            this.Activated += AboutForm_Activated;
            this.KeyPreview = true;
            this.Shown += AboutForm_Shown;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            AppNameLabel.Text += " " + Application.ProductVersion;
            this.Icon = Resources.MainIcon;
            AppIconPictureBox.Image = new Icon(Resources.MainIcon, 48, 48).ToBitmap();
            CheckForUpdates();

            WriteLine($"AboutForm loaded with product version: {Application.ProductVersion}", LoggingFrequency.GUILogging);
        }

        private async void AboutForm_Shown(object sender, EventArgs e)
        {
            await CheckForCacheReset(true, false);
        }

        private enum UpdateState
        {
            Updated,
            UpdateAvailable,
            BetaAvailable,
            Error
        }

        private async void CheckForUpdates()
        {
            WriteLine("Checking for updates", LoggingFrequency.DebugLogging);
            UpdateState updateState = await Task.Run(async () =>
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.UserAgent.ParseAdd("WinThumbPreloader");
                        string GitHubApiResponse = await client.GetStringAsync("https://api.github.com/repos/Mfarooq360/WinThumbsPreloader-V2/releases");
                        string latestVersionString = Regex.Match(GitHubApiResponse, @"""tag_name"":\s*""v([\d\.]+(-beta\d+)?)""").Groups[1].Captures[0].ToString();
                        WriteLine("LatestVersionString: " + latestVersionString, LoggingFrequency.DebugLogging);

                        Version currentNumericVersion = new Version(Regex.Match(Application.ProductVersion, @"(\d+\.\d+\.\d+)").Value);
                        WriteLine("Current version: " + currentNumericVersion, LoggingFrequency.DebugLogging);
                        Version latestNumericVersion = new Version(Regex.Match(latestVersionString, @"(\d+\.\d+\.\d+)").Value);
                        WriteLine("Latest version: " + latestNumericVersion, LoggingFrequency.DebugLogging);

                        var currentBetaMatch = Regex.Match(Application.ProductVersion, @"-beta(\d+)");
                        WriteLine("CurrentBetaMatch: " + currentBetaMatch, LoggingFrequency.DebugLogging);
                        var latestBetaMatch = Regex.Match(latestVersionString, @"-beta(\d+)");
                        WriteLine("LatestBetaMatch: " + latestBetaMatch, LoggingFrequency.DebugLogging);
                        bool isCurrentBeta = currentBetaMatch.Success;
                        WriteLine("IsCurrentBeta: " + isCurrentBeta, LoggingFrequency.DebugLogging);
                        bool isLatestBeta = latestBetaMatch.Success;
                        WriteLine("IsLatestBeta: " + isLatestBeta, LoggingFrequency.DebugLogging);
                        int currentBetaNumber = isCurrentBeta ? int.Parse(currentBetaMatch.Groups[1].Value) : 0;
                        WriteLine("CurrentBetaNumber: " + currentBetaNumber, LoggingFrequency.DebugLogging);
                        int latestBetaNumber = isLatestBeta ? int.Parse(latestBetaMatch.Groups[1].Value) : 0;
                        WriteLine("LatestBetaNumber: " + latestBetaNumber, LoggingFrequency.DebugLogging);

                        // If latest version is NOT a beta and current version IS a beta or is older, then update is available
                        if (!isLatestBeta && (isCurrentBeta && currentNumericVersion == latestNumericVersion))
                        {
                            WriteLine("Update available", LoggingFrequency.GUILogging);
                            return UpdateState.UpdateAvailable;
                        }
                        // If both current and latest versions are betas, check their beta numbers
                        else if (isCurrentBeta && isLatestBeta)
                        {
                            if (currentBetaNumber < latestBetaNumber)
                            {
                                WriteLine("Beta available", LoggingFrequency.GUILogging);
                                return UpdateState.BetaAvailable;
                            }
                            else
                            {
                                WriteLine("Up to date", LoggingFrequency.GUILogging);
                                return UpdateState.Updated;
                            }
                        }
                        else if (currentNumericVersion < latestNumericVersion)
                        {
                            WriteLine("Update available", LoggingFrequency.GUILogging);
                            return UpdateState.UpdateAvailable;
                        }
                        else
                        {
                            WriteLine("Up to date", LoggingFrequency.GUILogging);
                            return UpdateState.Updated;
                        }
                    }
                }
                catch (Exception e)
                {
                    WriteLine("Failed to check for updates: " + e.Message, LoggingFrequency.GUILogging);
                    return UpdateState.Error;
                }
            });

            switch (updateState)
            {
                case UpdateState.Updated:
                    UpdateLabel.Text = Resources.AboutForm_WinThumbsPreloader_IsUpToDate;
                    break;
                case UpdateState.Error:
                    UpdateLabel.Text = Resources.AboutForm_WinThumbsPreloader_UpdateCheckFailed;
                    break;
                case UpdateState.UpdateAvailable:
                    UpdateLabel.Text = Resources.AboutForm_WinThumbsPreloader_NewVersionAvailable;
                    UpdateLabel.ForeColor = Color.FromArgb(0, 102, 204);
                    UpdateLabel.Font = new Font(UpdateLabel.Font.Name, UpdateLabel.Font.SizeInPoints, FontStyle.Underline);
                    UpdateLabel.Cursor = Cursors.Hand;
                    break;
                case UpdateState.BetaAvailable:
                    UpdateLabel.Text = Resources.AboutForm_WinThumbsPreloader_BetaVersionAvailable;
                    UpdateLabel.ForeColor = Color.FromArgb(0, 102, 204);
                    UpdateLabel.Font = new Font(UpdateLabel.Font.Name, UpdateLabel.Font.SizeInPoints, FontStyle.Underline);
                    UpdateLabel.Cursor = Cursors.Hand;
                    break;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            WriteLine("Exiting application from AboutForm", LoggingFrequency.GUILogging);
            Environment.Exit(0);
        }

        private void LicenceButton_Click(object sender, EventArgs e)
        {
            WriteLine("Opening LicenseForm", LoggingFrequency.GUILogging);
            LicenseForm licenseForm = new LicenseForm();
            this.OpenSecondaryFormCentered(licenseForm);
        }

        private void RichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            WriteLine("Link clicked: " + e.LinkText, LoggingFrequency.GUILogging);
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    ArgumentList = { e.LinkText }
                });
                WriteLine("Opened link: " + e.LinkText, LoggingFrequency.GUILogging);
            }
            catch (Exception ex)
            {
                WriteLine("Failed to open link: " + e.LinkText + " with error: " + ex.Message, LoggingFrequency.GUILogging);
                MessageBox.Show($"Failed to open link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateLabel_Click(object sender, EventArgs e)
        {
            WriteLine("Opening GitHub link from UpdateLabel", LoggingFrequency.GUILogging);
            string url = "";

            if (UpdateLabel.Text == Resources.AboutForm_WinThumbsPreloader_NewVersionAvailable ||
                UpdateLabel.Text == Resources.AboutForm_WinThumbsPreloader_BetaVersionAvailable)
            {
                url = "https://github.com/Mfarooq360/WinThumbsPreloader-V2";
                WriteLine("Opening GitHub link: " + url, LoggingFrequency.GUILogging);
            }

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    WriteLine("Unable to open the link: " + ex.Message, LoggingFrequency.GUILogging);
                    MessageBox.Show("Unable to open the link: " + ex.Message);
                }
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            WriteLine("Opening SettingsForm from AboutForm", LoggingFrequency.GUILogging);
            SettingsForm settingsForm = new SettingsForm();
            this.OpenFormCentered(settingsForm);
        }

        private void PreloadButton_Click(object sender, EventArgs e)
        {
            WriteLine("PreloadButton clicked", LoggingFrequency.GUILogging);
            if (PreloadButton.Text == "Preload" || PreloadButton.Text == "Recursively")
            {
                string preloadMode = PreloadButton.Text;
                WriteLine("Preload mode: " + preloadMode, LoggingFrequency.GUILogging);
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.ShowDialog();
                string folderName = folderBrowserDialog.SelectedPath;
                WriteLine("Selected folder: " + folderName, LoggingFrequency.GUILogging);
                if (folderName == null || folderName == string.Empty) return;
                else
                {
                    try
                    {
                        // Create an Options object with the appropriate settings
                        Options options = new Options(new string[] { folderName })
                        {
                            includeNestedDirectories = (preloadMode == "Recursively"),
                            silentMode = false,
                            multiThreaded = Settings.Default.Multithreaded,
                            threadCount = Settings.Default.ThreadCount
                        };
                        WriteLine("options.includeNestedDirectories: " + options.includeNestedDirectories + "\n" +
                                   "options.silentMode: " + options.silentMode + "\n" +
                                   "options.multiThreaded: " + options.multiThreaded + "\n" +
                                   "options.threadCount: " + options.threadCount, LoggingFrequency.GUILogging);

                        // Start the preloader with the constructed Options object
                        Program.StartPreloader(options);
                    }
                    catch (Exception ex)
                    {
                        WriteLine("Failed to start WinThumbsPreloader: " + ex.Message, LoggingFrequency.GUILogging);
                        MessageBox.Show($"Failed to start WinThumbsPreloader: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (PreloadButton.Text == "Bulk Preload")
            {
                WriteLine("Opening BulkPreloaderForm from AboutForm", LoggingFrequency.GUILogging);
                OpenBulkPreloaderForm();
            }
        }

        private void AboutForm_Activated(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                // Shift key is currently pressed, so change the button text
                PreloadButton.Text = "Recursively";
            }
            /*else if (Control.ModifierKeys == Keys.Control)
            {
                // Control key is currently pressed, so change the button text
                PreloadButton.Text = "Bulk Preload";
            }*/
            else
            {
                // Shift key is not pressed, so set the button text to its original value
                PreloadButton.Text = "Preload";
            }
        }

        private void AboutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text when Shift is pressed
                PreloadButton.Text = "Recursively";
            }
            /*else if (e.KeyCode == Keys.ControlKey)
            {
                // Change the button text when Control is pressed
                PreloadButton.Text = "Bulk Preload";
            }*/
        }

        private void AboutForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text back when Shift is released
                PreloadButton.Text = "Preload";
            }
            /*else if (e.KeyCode == Keys.ControlKey)
            {
                // Change the button text back when Control is released
                PreloadButton.Text = "Preload";
            }*/
        }

        private void OpenBulkPreloaderForm()
        {
            WriteLine("Opening BulkPreloaderForm from AboutForm", LoggingFrequency.GUILogging);
            OldBulkPreloaderForm bulkPreloaderForm = new OldBulkPreloaderForm();
            this.OpenFormCentered(bulkPreloaderForm);
        }
        public static bool recognizedLogged = false;
        public static bool cacheResetLogged = false;

        public static async Task CheckForCacheReset(bool checkResetRecognized, bool logOnly)
        {
            if (checkResetRecognized && !recognizedLogged && Settings.Default.ResetRecognized)
            {
                WriteLine("Cache reset already recognized, skipping check", LoggingFrequency.DebugLogging);
                recognizedLogged = true;
                return;
            }
            await Task.Run(async () =>
            {
                if (Settings.Default.ThumbsResetAlert && !CacheForm.CompareThumbsCacheSize())
                {
                    if (!cacheResetLogged)
                    {
                        WriteLine("WARNING: Cache Reset Detected", LoggingFrequency.DebugLogging);
                        cacheResetLogged = true;
                    }
                    if (!logOnly && !Settings.Default.ResetRecognized)
                    {
                        DialogResult dialogResult = MessageBox.Show("The Explorer cache has reset. Would you like to restore the backup?", "Restore Backup", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            await CacheForm.RestoreThumbsCache(null, false);
                            if (CacheForm.CompareThumbsCacheSize())
                            {
                                WriteLine("Explorer cache successfully restored", LoggingFrequency.DebugLogging);
                                MessageBox.Show("Explorer cache successfully restored.", "Restore Successful", MessageBoxButtons.OK);
                            }
                            else
                            {
                                WriteLine("Failed to restore Explorer cache", LoggingFrequency.DebugLogging);
                                MessageBox.Show("Failed to restore Explorer cache.", "Restore Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (checkResetRecognized)
                        {
                            Settings.Default.ResetRecognized = true;
                            Settings.Default.Save(); // Save settings after modifying them
                            WriteLine("cacheResetRecognized set to true", LoggingFrequency.DebugLogging);
                        }
                    }
                }
            });
        }
    }
}