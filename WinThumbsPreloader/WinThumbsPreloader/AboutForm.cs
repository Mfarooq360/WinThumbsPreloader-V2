﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;

namespace WinThumbsPreloader
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            AppNameLabel.Text += " " + Application.ProductVersion;
            this.Icon = Resources.MainIcon;
            AppIconPictureBox.Image = new Icon(Resources.MainIcon, 48, 48).ToBitmap();
            CheckForUpdates();
        }

        private enum UpdateState
        {
            Updated,
            NotUpdated,
            Error
        }

        private async void CheckForUpdates()
        {
            UpdateState updateState = await Task.Run(() =>
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("User-Agent", "WinThumbPreloader");
                        string GitHubApiResponse = client.DownloadString("https://api.github.com/repos/Mfarooq360/WinThumbsPreloader/releases/latest");
                        string latestVersionString = Regex.Match(GitHubApiResponse, @"""tag_name"":\s*""v([\d\.]+)").Groups[1].Captures[0].ToString();
                        Version currentVersion = new Version(Application.ProductVersion);
                        Version latestVersion = new Version(latestVersionString);
                        return (currentVersion >= latestVersion ? UpdateState.Updated : UpdateState.NotUpdated);
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
                case UpdateState.NotUpdated:
                    UpdateLabel.Text = Resources.AboutForm_WinThumbsPreloader_NewVersionAvailable;
                    UpdateLabel.ForeColor = Color.FromArgb(0, 102, 204);
                    UpdateLabel.Font = new Font(UpdateLabel.Font.Name, UpdateLabel.Font.SizeInPoints, FontStyle.Underline);
                    UpdateLabel.Cursor = Cursors.Hand;
                    break;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LicenceButton_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "LICENSE.txt");
            try
            {
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception) { } // Do nothing
        }

        string[] defaultExtensions = new string[] { "avif", "bmp", "gif", "heic", "jpg", "jpeg", "mkv", "mov", "mp4", "png", "svg", "tif", "tiff", "webp" };
        private void ExtensionsButton_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ThumbnailExtensions.txt");
            try
            {
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception)
            {
                File.WriteAllLines(path, defaultExtensions);
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ThumbnailExtensions.txt");
            File.WriteAllLines(path, defaultExtensions);
        }

        private void RichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void UpdateLabel_Click(object sender, EventArgs e)
        {
            if (UpdateLabel.Text == Resources.AboutForm_WinThumbsPreloader_NewVersionAvailable) Process.Start("https://github.com/Mfarooq360/WinThumbsPreloader");
        }
    }
}
