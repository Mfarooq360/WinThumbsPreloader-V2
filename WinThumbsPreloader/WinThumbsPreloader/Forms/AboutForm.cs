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
using WinThumbsPreloader.Properties;
using static System.Windows.Forms.Design.AxImporter;

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
        }

        private async void AboutForm_Shown(object sender, EventArgs e)
        {
            await CheckForCacheReset();
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
            UpdateState updateState = await Task.Run(async () =>
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.UserAgent.ParseAdd("WinThumbPreloader");
                        string GitHubApiResponse = await client.GetStringAsync("https://api.github.com/repos/Mfarooq360/WinThumbsPreloader-V2/releases");
                        string latestVersionString = Regex.Match(GitHubApiResponse, @"""tag_name"":\s*""v([\d\.]+(-beta\d+)?)""").Groups[1].Captures[0].ToString();

                        Version currentNumericVersion = new Version(Regex.Match(Application.ProductVersion, @"(\d+\.\d+\.\d+)").Value);
                        Version latestNumericVersion = new Version(Regex.Match(latestVersionString, @"(\d+\.\d+\.\d+)").Value);

                        var currentBetaMatch = Regex.Match(Application.ProductVersion, @"-beta(\d+)");
                        var latestBetaMatch = Regex.Match(latestVersionString, @"-beta(\d+)");
                        bool isCurrentBeta = currentBetaMatch.Success;
                        bool isLatestBeta = latestBetaMatch.Success;
                        int currentBetaNumber = isCurrentBeta ? int.Parse(currentBetaMatch.Groups[1].Value) : 0;
                        int latestBetaNumber = isLatestBeta ? int.Parse(latestBetaMatch.Groups[1].Value) : 0;

                        // If latest version is NOT a beta and current version IS a beta or is older, then update is available
                        if (!isLatestBeta && (isCurrentBeta && currentNumericVersion == latestNumericVersion))
                        {
                            return UpdateState.UpdateAvailable;
                        }
                        // If both current and latest versions are betas, check their beta numbers
                        else if (isCurrentBeta && isLatestBeta)
                        {
                            if (currentBetaNumber < latestBetaNumber)
                            {
                                return UpdateState.BetaAvailable;
                            }
                            else
                            {
                                return UpdateState.Updated;
                            }
                        }
                        else if (currentNumericVersion < latestNumericVersion)
                        {
                            return UpdateState.UpdateAvailable;
                        }
                        else
                        {
                            return UpdateState.Updated;
                        }
                    }
                }
                catch (Exception)
                {
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
            Environment.Exit(0);
        }

        private void AppNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void LicenceButton_Click(object sender, EventArgs e)
        {
            LicenseForm licenseForm = new LicenseForm();
            this.OpenSecondaryFormCentered(licenseForm);
        }

        private void RichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    ArgumentList = { e.LinkText }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateLabel_Click(object sender, EventArgs e)
        {
            if (UpdateLabel.Text == Resources.AboutForm_WinThumbsPreloader_NewVersionAvailable) Process.Start("https://github.com/Mfarooq360/WinThumbsPreloader-V2");
            else if (UpdateLabel.Text == Resources.AboutForm_WinThumbsPreloader_BetaVersionAvailable) Process.Start("https://github.com/Mfarooq360/WinThumbsPreloader-V2");
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            this.OpenFormCentered(settingsForm);
        }

        private void PreloadButton_Click(object sender, EventArgs e)
        {
            string preloadMode = PreloadButton.Text;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            string folderName = folderBrowserDialog.SelectedPath;
            if (folderName == null || folderName == string.Empty) return;
            else
            {
                try
                {
                    // Create an Options object with the appropriate settings
                    Options options = new Options(new string[] { folderName })
                    {
                        includeNestedDirectories = (preloadMode == "Recursively"),
                        silentMode = false, // Assuming silent mode is false in this context
                        multiThreaded = Settings.Default.Multithreaded,
                        threadCount = Settings.Default.ThreadCount
                    };

                    // Start the preloader with the constructed Options object
                    Program.StartPreloader(options);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to start WinThumbsPreloader: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AboutForm_Activated(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                // Shift key is currently pressed, so change the button text
                PreloadButton.Text = "Recursively";
            }
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
        }

        private void AboutForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text back when Shift is released
                PreloadButton.Text = "Preload";
            }
        }

        public static async Task CheckForCacheReset()
        {
            await Task.Run(async () =>
            {
                if (Settings.Default.ThumbsResetAlert == true && CacheForm.CompareThumbsCacheSize() == false && Settings.Default.ResetRecognized == false)
                {
                    DialogResult dialogResult = MessageBox.Show("The Explorer cache has reset. Would you like to restore the backup?", "Restore Backup", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        await CacheForm.RestoreThumbsCache(null, false);
                        if (CacheForm.CompareThumbsCacheSize() == true)
                        {
                            MessageBox.Show("Explorer cache successfully restored.", "Restore Successful", MessageBoxButtons.OK);
                        }
                        Settings.Default.ResetRecognized = true;
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        Settings.Default.ResetRecognized = true;
                    }
                }
            });
        }
    }
}