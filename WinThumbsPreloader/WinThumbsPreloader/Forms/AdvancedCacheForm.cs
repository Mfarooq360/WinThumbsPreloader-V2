using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader.Forms
{
    public partial class AdvancedCacheForm : Form
    {
        private System.Windows.Forms.Timer lastBackupRefreshTimer;

        public AdvancedCacheForm()
        {
            InitializeComponent();
        }

        private void AdvancedCacheForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.MainIcon;
            CacheForm.BackupInfoChanged += CacheForm_BackupInfoChanged;
            CacheForm.BackupPathChanged += CacheForm_BackupPathChanged;
            LoadSavedBackupPathIntoTextBox();
            CacheSizeUpdateIntervalNumericUpDown.Value = Properties.Settings.Default.CacheUpdateInterval;
            AutoBackupIntervalNumericUpDown.Value = Properties.Settings.Default.AutoBackupInterval / 1000;
            AutoRestoreIntervalNumericUpDown.Value = Properties.Settings.Default.AutoRestoreInterval / 1000;
            CacheSizeFormatComboBox.SelectedItem = Properties.Settings.Default.CacheSizeFormat;
            ExplorerCacheDeletionMethodComboBox.SelectedItem = Properties.Settings.Default.ExplorerCacheDeletionMethod;
            ExplorerCloseFrequencyComboBox.SelectedItem = Properties.Settings.Default.ExplorerCloseFrequency;
            BackupRestoreClearSafetyComboBox.SelectedItem = Properties.Settings.Default.BackupRestoreClearSafety;
            AutoBackupAfterPreloadCheckBox.Checked = Properties.Settings.Default.AutoBackupAfterPreload;
            UpdateBackupFolderStatus();
            UpdateLastBackupOutput();

            SetActionOutput(string.Empty);

            lastBackupRefreshTimer = new System.Windows.Forms.Timer();
            lastBackupRefreshTimer.Interval = 1000;
            lastBackupRefreshTimer.Tick += (s, args) => UpdateLastBackupOutput();
            lastBackupRefreshTimer.Start();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            WriteLine("Closing Advanced Cache Form", LoggingFrequency.GUILogging);
            lastBackupRefreshTimer?.Stop();
            lastBackupRefreshTimer?.Dispose();
            lastBackupRefreshTimer = null;
            Close();
        }

        private void CacheForm_BackupInfoChanged(object sender, EventArgs e)
        {
            if (IsDisposed || Disposing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshBackupInfoFromSignal));
                return;
            }

            RefreshBackupInfoFromSignal();
        }

        private void RefreshBackupInfoFromSignal()
        {
            UpdateBackupFolderStatus();
            UpdateLastBackupOutput();
        }

        private void CacheForm_BackupPathChanged(object sender, EventArgs e)
        {
            if (IsDisposed || Disposing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshBackupPathFromSignal));
                return;
            }

            RefreshBackupPathFromSignal();
        }

        private void RefreshBackupPathFromSignal()
        {
            LoadSavedBackupPathIntoTextBox();
            UpdateBackupFolderStatus();
            UpdateLastBackupOutput();

            SetActionOutput(
                string.IsNullOrWhiteSpace(BackupFolderPathTextBox.Text)
                    ? "Backup path cleared."
                    : "Backup path updated."
            );
        }

        private void LoadSavedBackupPathIntoTextBox()
        {
            string displayPath = CacheForm.GetBackupPathForDisplay();

            BackupFolderPathTextBox.Text =
                string.Equals(displayPath, "N/A", StringComparison.OrdinalIgnoreCase)
                    ? string.Empty
                    : displayPath;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CacheForm.BackupInfoChanged -= CacheForm_BackupInfoChanged;
            CacheForm.BackupPathChanged -= CacheForm_BackupPathChanged;
            base.OnFormClosed(e);
        }

        private int tempCacheUpdateInterval = 250;
        private int tempAutoBackupInterval = 5000;
        private int tempAutoRestoreInterval = 5000;

        private void CacheSizeUpdateIntervalNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            tempCacheUpdateInterval = (int)CacheSizeUpdateIntervalNumericUpDown.Value;
            WriteLine("Cache Update Interval set to: " + tempCacheUpdateInterval, LoggingFrequency.DebugLogging);
        }

        private void AutoBackupIntervalNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            tempAutoBackupInterval = (int)AutoBackupIntervalNumericUpDown.Value * 1000;
            WriteLine("Auto Backup Interval set to: " + tempAutoBackupInterval, LoggingFrequency.DebugLogging);
        }

        private void AutoRestoreIntervalNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            tempAutoRestoreInterval = (int)AutoRestoreIntervalNumericUpDown.Value * 1000;
            WriteLine("Auto Restore Interval set to: " + tempAutoRestoreInterval, LoggingFrequency.DebugLogging);
        }

        private const int BackupTabIndex = 0;
        private const int CacheTabIndex = 1;
        private const int GeneralTabIndex = 2;

        private void SaveButton_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case BackupTabIndex:
                    SaveBackupTab();
                    break;

                case CacheTabIndex:
                    SaveCacheTab();
                    break;

                case GeneralTabIndex:
                    SaveGeneralTab();
                    break;

                default:
                    WriteLine(
                        "Could not save Advanced Cache settings because no known tab was selected.",
                        LoggingFrequency.DebugLogging
                    );
                    break;
            }
        }

        private bool SaveBackupTab()
        {
            WriteLine(
                "Saving Advanced Cache Backup tab settings",
                LoggingFrequency.GUILogging
            );

            string oldBackupPath =
                Settings.Default.BackupCacheFolderPath ?? string.Empty;

            string selectedBackupPath =
                BackupFolderPathTextBox.Text.Trim();

            bool backupPathChanged = !string.Equals(
                NormalizeForComparison(oldBackupPath),
                NormalizeForComparison(selectedBackupPath),
                StringComparison.OrdinalIgnoreCase
            );

            string resultMessage;

            if (string.IsNullOrWhiteSpace(selectedBackupPath))
            {
                Settings.Default.BackupCacheFolderPath = string.Empty;
                Settings.Default.Save();

                CacheForm.NotifyBackupPathChanged();

                resultMessage = "Backup path cleared.";

                WriteLine(
                    "Backup cache folder cleared.",
                    LoggingFrequency.DebugLogging
                );
            }
            else
            {
                if (!CacheForm.TrySetBackupFolderPath(
                        selectedBackupPath,
                        this,
                        out string backupPathMessage))
                {
                    WriteLine(
                        "Backup folder save failed: " + backupPathMessage,
                        LoggingFrequency.GUILogging
                    );

                    SetActionOutput("Backup folder unavailable.");
                    UpdateBackupFolderStatus();
                    return false;
                }

                resultMessage = "Backup folder saved.";

                WriteLine(
                    "Backup cache folder saved: " + selectedBackupPath,
                    LoggingFrequency.DebugLogging
                );
            }

            if (backupPathChanged &&
                (Settings.Default.AutoBackupThumbs ||
                 Settings.Default.AutoRestoreThumbs))
            {
                Settings.Default.AutoBackupThumbs = false;
                Settings.Default.AutoRestoreThumbs = false;

                resultMessage = "Saved. Auto backup/restore disabled.";
            }

            Settings.Default.Save();

            SetActionOutput(resultMessage);
            UpdateBackupFolderStatus();
            UpdateLastBackupOutput();

            if (Owner is CacheForm cacheForm)
            {
                cacheForm.UpdateAutoBackupRestoreCheckboxes();
            }

            return true;
        }

        private void SaveGeneralTab()
        {
            WriteLine(
                "Saving Advanced Cache General tab settings",
                LoggingFrequency.GUILogging
            );

            int cacheUpdateInterval =
                (int)CacheSizeUpdateIntervalNumericUpDown.Value;

            int autoBackupInterval =
                checked((int)AutoBackupIntervalNumericUpDown.Value * 1000);

            int autoRestoreInterval =
                checked((int)AutoRestoreIntervalNumericUpDown.Value * 1000);

            string cacheSizeFormat =
                CacheSizeFormatComboBox.SelectedItem?.ToString() ?? "MB";

            tempCacheUpdateInterval = cacheUpdateInterval;
            tempAutoBackupInterval = autoBackupInterval;
            tempAutoRestoreInterval = autoRestoreInterval;

            Settings.Default.CacheUpdateInterval = cacheUpdateInterval;
            Settings.Default.AutoBackupInterval = autoBackupInterval;
            Settings.Default.AutoRestoreInterval = autoRestoreInterval;
            Settings.Default.CacheSizeFormat = cacheSizeFormat;
            Settings.Default.Save();

            WriteLine(
                "Cache Update Interval saved: " + cacheUpdateInterval,
                LoggingFrequency.DebugLogging
            );

            WriteLine(
                "Auto Backup Interval saved: " + autoBackupInterval,
                LoggingFrequency.DebugLogging
            );

            WriteLine(
                "Auto Restore Interval saved: " + autoRestoreInterval,
                LoggingFrequency.DebugLogging
            );

            WriteLine(
                "Cache Size Format saved: " + cacheSizeFormat,
                LoggingFrequency.DebugLogging
            );

            if (Owner is CacheForm cacheForm)
            {
                cacheForm.UpdateCacheSizeUpdateInterval(cacheUpdateInterval);
                cacheForm.UpdateAutoBackupInterval(autoBackupInterval);
                cacheForm.UpdateAutoRestoreInterval(autoRestoreInterval);

                cacheForm.format = cacheSizeFormat;
                cacheForm.UpdateCacheSizeLabels();
            }
        }

        private void SaveCacheTab()
        {
            WriteLine(
                "Saving Advanced Cache Cache tab settings",
                LoggingFrequency.GUILogging
            );

            bool autoBackupAfterPreload = AutoBackupAfterPreloadCheckBox.Checked;

            string deletionMethod =
                ExplorerCacheDeletionMethodComboBox.SelectedItem?.ToString()
                ?? "Manual Deletion";

            string closeFrequency =
                ExplorerCloseFrequencyComboBox.SelectedItem?.ToString()
                ?? "On Every File";

            string safety =
                BackupRestoreClearSafetyComboBox.SelectedItem?.ToString()
                ?? "Allow Partial Copy";

            Settings.Default.AutoBackupAfterPreload = autoBackupAfterPreload;
            Settings.Default.ExplorerCacheDeletionMethod = deletionMethod;
            Settings.Default.ExplorerCloseFrequency = closeFrequency;
            Settings.Default.BackupRestoreClearSafety = safety;
            Settings.Default.Save();

            WriteLine(
                "Explorer Cache Deletion Method saved: " + deletionMethod,
                LoggingFrequency.DebugLogging
            );

            WriteLine(
                "Explorer Close Frequency saved: " + closeFrequency,
                LoggingFrequency.DebugLogging
            );

            WriteLine(
                "Backup/Restore/Clear Safety saved: " + safety,
                LoggingFrequency.DebugLogging
            );

            if (Owner is CacheForm cacheForm &&
                deletionMethod == "Disk Cleanup")
            {
                cacheForm.ConfigureDiskCleanupSageset();
            }
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case BackupTabIndex:
                    ResetBackupTabToDefaults();
                    break;

                case CacheTabIndex:
                    ResetCacheTabToDefaults();
                    break;

                case GeneralTabIndex:
                    ResetGeneralTabToDefaults();
                    break;

                default:
                    WriteLine(
                        "Could not reset Advanced Cache settings because no known tab was selected.",
                        LoggingFrequency.DebugLogging
                    );
                    break;
            }
        }

        private void ResetBackupTabToDefaults()
        {
            WriteLine(
                "Resetting Advanced Cache Backup tab to default",
                LoggingFrequency.GUILogging
            );

            SelectRecommendedBackupFolder();
            SaveBackupTab();
        }

        private void ResetGeneralTabToDefaults()
        {
            WriteLine(
                "Resetting Advanced Cache Cache tab to default",
                LoggingFrequency.GUILogging
            );

            AutoBackupIntervalNumericUpDown.Value = 5;
            AutoRestoreIntervalNumericUpDown.Value = 5;
            CacheSizeFormatComboBox.SelectedItem = "MB";
            CacheSizeUpdateIntervalNumericUpDown.Value = 250;

            SaveGeneralTab();
        }

        private void ResetCacheTabToDefaults()
        {
            WriteLine(
                "Resetting Advanced Cache General tab to default",
                LoggingFrequency.GUILogging
            );

            AutoBackupAfterPreloadCheckBox.Checked = true;

            BackupRestoreClearSafetyComboBox.SelectedItem =
                "Allow Partial Copy";

            ExplorerCacheDeletionMethodComboBox.SelectedItem =
                "Manual Deletion";

            ExplorerCloseFrequencyComboBox.SelectedItem =
                "On Every File";

            SaveCacheTab();
        }

        private void SelectRecommendedBackupFolder()
        {
            string defaultPath =
                CacheForm.GetRecommendedDefaultBackupPath();

            BackupFolderPathTextBox.Text = defaultPath;

            SetActionOutput(
                "Default folder selected. Click Save."
            );

            UpdateBackupFolderStatus();
            UpdateLastBackupOutput();
        }

        private void DefaultBackupFolderButton_Click(object sender, EventArgs e)
        {
            SelectRecommendedBackupFolder();
        }

        private static string NormalizeForComparison(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            try
            {
                return Path.GetFullPath(Environment.ExpandEnvironmentVariables(path.Trim().Trim('"')))
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            catch
            {
                return path.Trim();
            }
        }

        private void BrowseBackupFolderButton_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.UseDescriptionForTitle = true;
            dialog.Description = "Select Backup Folder";
            dialog.ShowNewFolderButton = true;

            string currentText = BackupFolderPathTextBox.Text.Trim();

            if (Directory.Exists(currentText))
            {
                dialog.SelectedPath = currentText;
            }
            else
            {
                string defaultPath = CacheForm.GetRecommendedDefaultBackupPath();
                string defaultParent = Path.GetDirectoryName(defaultPath);

                if (!string.IsNullOrWhiteSpace(defaultParent) && Directory.Exists(defaultParent))
                    dialog.SelectedPath = defaultParent;
            }

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                BackupFolderPathTextBox.Text = dialog.SelectedPath;
                SetActionOutput("Folder selected. Click Save.");
                UpdateBackupFolderStatus();
            }
            else
            {
                SetActionOutput("Selection cancelled.");
            }
        }

        private void ClearBackupFolderButton_Click(object sender, EventArgs e)
        {
            BackupFolderPathTextBox.Clear();
            SetActionOutput("Path cleared. Click Save.");
            UpdateBackupFolderStatus();
            UpdateLastBackupOutput();
        }

        private void UpdateBackupFolderStatus()
        {
            CacheForm.BackupFolderStatusInfo info =
                CacheForm.GetBackupFolderStatusInfo(BackupFolderPathTextBox.Text);

            StatusLabel.Text = info.Message;

            StatusLabel.ForeColor = info.Status switch
            {
                CacheForm.BackupFolderStatus.Writable => Color.LimeGreen,
                CacheForm.BackupFolderStatus.WritableAdmin => Color.DarkOrange,
                CacheForm.BackupFolderStatus.WillBeCreated => Color.DarkGreen,
                CacheForm.BackupFolderStatus.WillBeCreatedAdmin => Color.DarkOrange,
                CacheForm.BackupFolderStatus.NotWritable => Color.Red,
                CacheForm.BackupFolderStatus.InvalidPath => Color.Firebrick,
                _ => SystemColors.GrayText
            };
        }

        private const int MaxBackupOutputLength = 64;

        private void SetActionOutput(string message)
        {
            string output = (message ?? string.Empty)
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Trim();

            while (output.Contains("  "))
                output = output.Replace("  ", " ");

            if (output.Length > MaxBackupOutputLength)
            {
                output =
                    output.Substring(0, MaxBackupOutputLength - 3) + "...";
            }

            OutputTextBox.Text = output;
        }

        private void UpdateLastBackupOutput()
        {
            string selectedPath = BackupFolderPathTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                OutputTextBox1.Clear(); // or: OutputTextBox1.Text = "Last backup: N/A";
                return;
            }

            DateTimeOffset? lastBackup =
                CacheForm.GetLastBackupDateTimeForFolder(selectedPath);

            if (!lastBackup.HasValue)
            {
                OutputTextBox1.Clear(); // or: OutputTextBox1.Text = "Last backup: N/A";
                return;
            }

            OutputTextBox1.Text = $"Last backup: {lastBackup.Value.LocalDateTime:g}";
        }
        private void BackupFolderPathTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateBackupFolderStatus();
            UpdateLastBackupOutput();
        }
    }
}