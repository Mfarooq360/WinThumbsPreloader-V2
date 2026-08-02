using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinThumbsPreloader.Forms;
using WinThumbsPreloader.Properties;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader
{
    public partial class CacheForm : Form
    {
        /*TODO:
         * add list of folders to preload as batch, add saving of list of folders
         * prevent thumbs cache backup from occurring when explorer cache size is being updated, 
         * add advanced preloader button to add list of paths to preload, watch out for admin manifest requirements 
         * add setting to toggle whether preload button automatically starts a folder selector or if it opens the advanced preload form
         * add a setting to minimize the application to tray no matter what form it's on and whether to open cacheform when its reopened
         * fix schedule form bugging out settings form when opened and closed while preloading
         * maybe check for high resolution timer when using stopwatch and log it
         * fix schedule form lag/stuttering when opening
         * allow changing of startup window such as starting on cache form
         * Make minimize button send to taskbar and exit button send to tray icon
         */
        public int cacheUpdateInterval;
        public int autoBackupInterval;
        public int autoRestoreInterval;

        public CacheForm()
        {
            InitializeComponent();
            notifyIcon1.Visible = false;
            CheckForStartMinimized(null, null);
            OutputTextBox_Initialize(null, null);
            this.KeyDown += CacheForm_KeyDown;
            this.KeyUp += CacheForm_KeyUp;
            this.Activated += CacheForm_Activated;
            this.KeyPreview = true;
            this.FormClosing += CacheForm_FormClosing;
            this.Resize += CacheForm_Resize;
            this.Shown += CacheForm_Shown;
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            notifyIcon1.Icon = Resources.MainIcon;
            CacheForm.BackupInfoChanged += CacheForm_BackupInfoChanged;

            SetIntervals();
            UpdateCacheSizeTimer();
            AutoBackupThumbsCache();
            AutoRestoreThumbsCache();
        }

        private async void CacheForm_Shown(object sender, EventArgs e)
        {
            await AboutForm.CheckForCacheReset(true, false);
        }

        private void CacheForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.MainIcon;
            AlertCheckBox.Checked = Settings.Default.ThumbsResetAlert;
            AutoBackupCheckBox.Checked = Settings.Default.AutoBackupThumbs;
            AutoRestoreCheckBox.Checked = Settings.Default.AutoRestoreThumbs;
            StartWithWindowsCheckBox.Checked = Settings.Default.StartWithWindows;
            toggleAutoBackupToolStripMenuItem.Checked = AutoBackupCheckBox.Checked;
            toggleAutoRestoreToolStripMenuItem.Checked = AutoRestoreCheckBox.Checked;
            toggleCacheResetAlertToolStripMenuItem.Checked = AlertCheckBox.Checked;
            startWithWindowsToolStripMenuItem.Checked = StartWithWindowsCheckBox.Checked;

            UpdateAutoBackupRestoreAvailability();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CacheForm.BackupInfoChanged -= CacheForm_BackupInfoChanged;
            base.OnFormClosed(e);
        }

        private void CacheForm_BackupInfoChanged(object sender, EventArgs e)
        {
            if (IsDisposed || Disposing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateAutoBackupRestoreAvailability));
                return;
            }

            UpdateAutoBackupRestoreAvailability();
        }

        private void UpdateAutoBackupRestoreAvailability()
        {
            bool hasBackupPath = TryGetBackupPath(out _);

            if (!hasBackupPath)
            {
                if (AutoBackupCheckBox.Checked)
                    AutoBackupCheckBox.Checked = false;

                if (AutoRestoreCheckBox.Checked)
                    AutoRestoreCheckBox.Checked = false;
            }

            AutoBackupCheckBox.Enabled = hasBackupPath;
            AutoRestoreCheckBox.Enabled = hasBackupPath;

            toggleAutoBackupToolStripMenuItem.Enabled = hasBackupPath;
            toggleAutoRestoreToolStripMenuItem.Enabled = hasBackupPath;

            toggleAutoBackupToolStripMenuItem.Checked = AutoBackupCheckBox.Checked;
            toggleAutoRestoreToolStripMenuItem.Checked = AutoRestoreCheckBox.Checked;
        }


        public void UpdateAutoBackupRestoreCheckboxes()
        {
            AutoBackupCheckBox.Checked = Settings.Default.AutoBackupThumbs;
            AutoRestoreCheckBox.Checked = Settings.Default.AutoRestoreThumbs;
            UpdateAutoBackupRestoreAvailability();
        }

        private void OutputTextBox_Initialize(object sender, EventArgs e)
        {
            long explorerSize = ExplorerCacheSize();
            long? backupSize = BackupCacheSizeNullable();

            if (!backupSize.HasValue)
            {
                OutputTextBox.Text = "No backup folder selected.";
                return;
            }

            if (backupSize.Value == 0)
            {
                OutputTextBox.Text = "Backup doesn't exist or is empty.";
            }

            if (explorerSize < backupSize.Value && Settings.Default.ResetRecognized == false)
            {
                OutputTextBox.Text = "Explorer cache has been reset.";
                Settings.Default.ResetRecognized = true;
            }

            if (explorerSize < backupSize.Value && Settings.Default.ResetRecognized == true)
            {
                if (OutputTextBox.Text != "Explorer cache cleared.")
                    OutputTextBox.Text = "Backup cache larger than explorer cache.";
            }

            if (explorerSize >= backupSize.Value && Settings.Default.ResetRecognized == true)
            {
                Settings.Default.ResetRecognized = false;
            }
        }

        private void SetIntervals()
        {
            bool settingsUpdated = false;

            if (Settings.Default.CacheUpdateInterval == 0)
            {
                Settings.Default.CacheUpdateInterval = 250;
                settingsUpdated = true;
            }
            cacheUpdateInterval = Settings.Default.CacheUpdateInterval;
            WriteLine("Cache Update Interval: " + cacheUpdateInterval, LoggingFrequency.DebugLogging);

            if (Settings.Default.AutoBackupInterval == 0)
            {
                Settings.Default.AutoBackupInterval = 5000;
                settingsUpdated = true;
            }
            autoBackupInterval = Settings.Default.AutoBackupInterval;
            WriteLine("Auto Backup Interval: " + autoBackupInterval, LoggingFrequency.DebugLogging);

            if (Settings.Default.AutoRestoreInterval == 0)
            {
                Settings.Default.AutoRestoreInterval = 5000;
                settingsUpdated = true;
            }
            autoRestoreInterval = Settings.Default.AutoRestoreInterval;
            WriteLine("Auto Restore Interval: " + autoRestoreInterval, LoggingFrequency.DebugLogging);

            if (settingsUpdated)
            {
                Settings.Default.Save();
            }
        }

        private System.Windows.Forms.Timer updateTimer = new System.Windows.Forms.Timer();

        private void UpdateCacheSizeTimer()
        {
            updateTimer.Interval = cacheUpdateInterval;
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private System.Windows.Forms.Timer autoBackupTimer = new System.Windows.Forms.Timer();

        private void AutoBackupThumbsCache()
        {
            autoBackupTimer.Interval = autoBackupInterval;
            autoBackupTimer.Tick += AutoBackupThumbsCache_Tick;
            autoBackupTimer.Start();
        }

        private System.Windows.Forms.Timer autoRestoreTimer = new System.Windows.Forms.Timer();

        private void AutoRestoreThumbsCache()
        {
            autoRestoreTimer.Interval = autoRestoreInterval;
            autoRestoreTimer.Tick += AutoRestoreThumbsCache_Tick;
            autoRestoreTimer.Start();
        }

        public void UpdateCacheSizeUpdateInterval(int newInterval)
        {
            cacheUpdateInterval = newInterval;
            WriteLine("Cache Update Interval set to: " + cacheUpdateInterval, LoggingFrequency.DebugLogging);
            if (updateTimer != null)
                updateTimer.Interval = newInterval;
        }

        public void UpdateAutoBackupInterval(int newInterval)
        {
            autoBackupInterval = newInterval;
            WriteLine("Auto Backup Interval set to: " + autoBackupInterval, LoggingFrequency.DebugLogging);
            if (autoBackupTimer != null)
                autoBackupTimer.Interval = newInterval;
        }

        public void UpdateAutoRestoreInterval(int newInterval)
        {
            autoRestoreInterval = newInterval;
            WriteLine("Auto Restore Interval set to: " + autoRestoreInterval, LoggingFrequency.DebugLogging);
            if (autoRestoreTimer != null)
                autoRestoreTimer.Interval = newInterval;
        }

        private async void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateCacheSizeLabels();
            if (AlertCheckBox.Checked == true)
            {
                updateTimer.Stop();
                await AboutForm.CheckForCacheReset(true, false);
                updateTimer.Start();
            }
        }

        private async void AutoBackupThumbsCache_Tick(object sender, EventArgs e)
        {
            autoBackupTimer.Stop();
            if (AutoBackupCheckBox.Checked && TryGetBackupPath(out _) && ExplorerCacheSize() > BackupCacheSize())
            {
                WriteLine("Auto-backup triggered", LoggingFrequency.DebugLogging);
                await BackupThumbsCacheAsync(null);
            }
            autoBackupTimer.Start();
        }

        private async void AutoRestoreThumbsCache_Tick(object sender, EventArgs e)
        {
            autoRestoreTimer.Stop();
            if (AutoRestoreCheckBox.Checked && BackupCacheSize() > ExplorerCacheSize())
            {
                WriteLine("Auto-restore triggered", LoggingFrequency.DebugLogging);
                await RestoreThumbsCache(progressBarRestore, false);
            }
            autoRestoreTimer.Start();
        }

        public static event EventHandler BackupInfoChanged;
        public static event EventHandler BackupPathChanged;

        public static void NotifyBackupInfoChanged()
        {
            BackupInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void NotifyBackupPathChanged()
        {
            BackupPathChanged?.Invoke(null, EventArgs.Empty);
            NotifyBackupInfoChanged();
        }

        long oldExplorerSize = 0;
        long? oldBackupSize = null;

        public void UpdateCacheSizeLabels()
        {
            long explorerSize = ExplorerCacheSize();

            if (explorerSize != oldExplorerSize)
            {
                WriteLine("Explorer cache size: " + explorerSize, LoggingFrequency.DebugLogging);
            }

            oldExplorerSize = explorerSize;

            long? backupSize = BackupCacheSizeNullable();

            if (backupSize != oldBackupSize)
            {
                WriteLine(backupSize.HasValue ? "Backup cache size: " + backupSize.Value : "Backup cache size: N/A", LoggingFrequency.DebugLogging);
            }

            oldBackupSize = backupSize;

            CacheSizeLabel.Text = $"Cache Size: {FormatSize(explorerSize)}";

            BackupSizeLabel.Text = backupSize.HasValue
                ? $"Backup Size: {FormatSize(backupSize.Value)}"
                : "Backup Size: N/A";
        }

        public string format = Properties.Settings.Default.CacheSizeFormat;

        private string FormatSize(long bytes)
        {
            string individualFormat = format;
            // Choose the format based on the size if format is set to "auto"
            if (individualFormat == "Auto")
            {
                if (bytes < 1024 * 1024) // Less than 1 MB
                {
                    individualFormat = "KB";
                }
                else if (bytes < 1024 * 1024 * 1024) // Less than 1 GB
                {
                    individualFormat = "MB";
                }
                else
                {
                    individualFormat = "GB";
                }
            }

            switch (individualFormat)
            {
                case "KB":
                    double kilobytes = bytes / 1024.0;
                    string kbFormat = kilobytes < 10 ? "N2" : kilobytes < 100 ? "N2" : "N0";
                    return $"{kilobytes.ToString(kbFormat)} KB";

                case "GB":
                    double gigabytes = bytes / (1024.0 * 1024.0 * 1024.0);
                    string gbFormat = gigabytes < 10 ? "N2" : gigabytes < 100 ? "N1" : "N0";
                    return $"{gigabytes.ToString(gbFormat)} GB";

                default: // "MB"
                    double megabytes = bytes / (1024.0 * 1024.0);
                    string mbFormat = megabytes < 10 ? "N2" : megabytes < 100 ? "N1" : "N0";
                    return $"{megabytes.ToString(mbFormat)} MB";
            }
        }

        public enum BackupFolderStatus
        {
            NotSelected,
            Writable,
            WritableAdmin,
            WillBeCreated,
            WillBeCreatedAdmin,
            NotWritable,
            InvalidPath
        }

        public sealed class BackupFolderStatusInfo
        {
            public BackupFolderStatus Status { get; init; }
            public string Message { get; init; }
            public string NormalizedPath { get; init; }
            public bool IsRunningAsAdmin { get; init; }
            public bool CanUseForBackup =>
                Status == BackupFolderStatus.Writable ||
                Status == BackupFolderStatus.WritableAdmin ||
                Status == BackupFolderStatus.WillBeCreated ||
                Status == BackupFolderStatus.WillBeCreatedAdmin;
        }

        public static bool IsRunningAsAdministrator()
        {
            try
            {
                using WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        public static BackupFolderStatusInfo GetBackupFolderStatusInfo(string selectedPath)
        {
            bool runningAsAdmin = IsRunningAsAdministrator();

            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                return new BackupFolderStatusInfo
                {
                    Status = BackupFolderStatus.NotSelected,
                    Message = "Status: No backup folder selected.",
                    NormalizedPath = string.Empty,
                    IsRunningAsAdmin = runningAsAdmin
                };
            }

            string normalizedPath;

            try
            {
                normalizedPath = NormalizeFolderPath(selectedPath);
            }
            catch
            {
                return new BackupFolderStatusInfo
                {
                    Status = BackupFolderStatus.InvalidPath,
                    Message = "Status: Invalid path.",
                    NormalizedPath = selectedPath,
                    IsRunningAsAdmin = runningAsAdmin
                };
            }

            if (IsSameOrUnder(normalizedPath, explorerPath))
            {
                return new BackupFolderStatusInfo
                {
                    Status = BackupFolderStatus.InvalidPath,
                    Message = "Status: Cannot use the Explorer cache folder.",
                    NormalizedPath = normalizedPath,
                    IsRunningAsAdmin = runningAsAdmin
                };
            }

            bool restricted = IsRestrictedPath(normalizedPath);

            try
            {
                if (Directory.Exists(normalizedPath))
                {
                    if (CanWriteToExistingDirectory(normalizedPath, out string writeMessage))
                    {
                        if (runningAsAdmin && restricted)
                        {
                            return new BackupFolderStatusInfo
                            {
                                Status = BackupFolderStatus.WritableAdmin,
                                Message = "Status: Writable (admin).",
                                NormalizedPath = normalizedPath,
                                IsRunningAsAdmin = runningAsAdmin
                            };
                        }

                        return new BackupFolderStatusInfo
                        {
                            Status = BackupFolderStatus.Writable,
                            Message = "Status: Writable.",
                            NormalizedPath = normalizedPath,
                            IsRunningAsAdmin = runningAsAdmin
                        };
                    }

                    return new BackupFolderStatusInfo
                    {
                        Status = BackupFolderStatus.NotWritable,
                        Message = "Status: Not writable.",
                        NormalizedPath = normalizedPath,
                        IsRunningAsAdmin = runningAsAdmin
                    };
                }

                string parent = FindExistingParentDirectory(normalizedPath);

                if (string.IsNullOrWhiteSpace(parent))
                {
                    return new BackupFolderStatusInfo
                    {
                        Status = BackupFolderStatus.InvalidPath,
                        Message = "Status: Invalid path.",
                        NormalizedPath = normalizedPath,
                        IsRunningAsAdmin = runningAsAdmin
                    };
                }

                bool parentRestricted = IsRestrictedPath(parent);

                if (CanWriteToExistingDirectory(parent, out string parentWriteMessage))
                {
                    if (runningAsAdmin && (restricted || parentRestricted))
                    {
                        return new BackupFolderStatusInfo
                        {
                            Status = BackupFolderStatus.WillBeCreatedAdmin,
                            Message = "Status: Folder will be created (admin).",
                            NormalizedPath = normalizedPath,
                            IsRunningAsAdmin = runningAsAdmin
                        };
                    }

                    return new BackupFolderStatusInfo
                    {
                        Status = BackupFolderStatus.WillBeCreated,
                        Message = "Status: Folder will be created.",
                        NormalizedPath = normalizedPath,
                        IsRunningAsAdmin = runningAsAdmin
                    };
                }

                return new BackupFolderStatusInfo
                {
                    Status = BackupFolderStatus.NotWritable,
                    Message = "Status: Not writable.",
                    NormalizedPath = normalizedPath,
                    IsRunningAsAdmin = runningAsAdmin
                };
            }
            catch
            {
                return new BackupFolderStatusInfo
                {
                    Status = BackupFolderStatus.NotWritable,
                    Message = "Status: Not writable.",
                    NormalizedPath = normalizedPath,
                    IsRunningAsAdmin = runningAsAdmin
                };
            }
        }

        private static string FindExistingParentDirectory(string path)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(path);

                while (directory != null)
                {
                    if (directory.Exists)
                        return directory.FullName;

                    directory = directory.Parent;
                }
            }
            catch { }

            return null;
        }

        private const string BackupFolderName = "Explorer Backup";
        private const string AppDataFolderName = "WinThumbsPreloader";
        private const string BackupInfoFileName = ".WinThumbsPreloader_BackupInfo.txt";

        static readonly string explorerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\Explorer");

        private static string PortableBackupPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BackupFolderName);

        private static string LocalAppDataBackupPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataFolderName, BackupFolderName);

        private static string GetBackupInfoFilePath(string backupFolder)
        {
            return Path.Combine(backupFolder, BackupInfoFileName);
        }

        private static void SaveLastBackupDateTimeForFolder(string backupFolder)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(backupFolder))
                    return;

                Directory.CreateDirectory(backupFolder);

                string infoFile = GetBackupInfoFilePath(backupFolder);

                File.WriteAllText(infoFile, DateTimeOffset.Now.ToString("O"));

                WriteLine("Last backup date saved for folder: " + backupFolder, LoggingFrequency.GUILogging);
            }
            catch (Exception ex)
            {
                WriteLine("Failed to save last backup date: " + ex.Message, LoggingFrequency.GUILogging);
            }
        }

        private static void DeleteLastBackupDateTimeForFolder(string backupFolder)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(backupFolder))
                    return;

                string infoFile = GetBackupInfoFilePath(backupFolder);

                if (File.Exists(infoFile))
                    File.Delete(infoFile);
            }
            catch (Exception ex)
            {
                WriteLine("Failed to delete last backup date file: " + ex.Message, LoggingFrequency.DebugLogging);
            }
        }

        public static DateTimeOffset? GetLastBackupDateTimeForFolder(string backupFolder)
        {
            if (string.IsNullOrWhiteSpace(backupFolder))
                return null;

            string normalizedFolder;

            try
            {
                normalizedFolder = NormalizeFolderPath(backupFolder);
            }
            catch
            {
                return null;
            }

            if (!Directory.Exists(normalizedFolder))
                return null;

            string[] backupFiles;

            try
            {
                backupFiles = Directory.GetFiles(normalizedFolder, "*.db", SearchOption.TopDirectoryOnly);
            }
            catch
            {
                return null;
            }

            if (backupFiles.Length == 0)
                return null;

            string infoFile = GetBackupInfoFilePath(normalizedFolder);

            try
            {
                if (File.Exists(infoFile))
                {
                    string text = File.ReadAllText(infoFile).Trim();

                    if (DateTimeOffset.TryParse(text, out DateTimeOffset parsed))
                        return parsed.ToLocalTime();
                }
            }
            catch (Exception ex)
            {
                WriteLine("Failed to read last backup date file: " + ex.Message, LoggingFrequency.DebugLogging);
            }

            try
            {
                DateTime newestWriteTimeUtc = backupFiles
                    .Select(File.GetLastWriteTimeUtc)
                    .OrderByDescending(x => x)
                    .FirstOrDefault();

                if (newestWriteTimeUtc != default)
                    return new DateTimeOffset(newestWriteTimeUtc, TimeSpan.Zero).ToLocalTime();
            }
            catch (Exception ex)
            {
                WriteLine("Failed to infer last backup date from backup files: " + ex.Message, LoggingFrequency.DebugLogging);
            }

            return null;
        }

        public static DateTimeOffset? GetLastBackupDateTimeForSelectedBackupFolder()
        {
            if (!TryGetBackupPath(out string currentBackupPath))
                return null;

            return GetLastBackupDateTimeForFolder(currentBackupPath);
        }

        public static string GetBackupPathForDisplay()
        {
            return TryGetBackupPath(out string path) ? path : "N/A";
        }

        public static bool TryGetBackupPath(out string backupFolder)
        {
            backupFolder = null;

            string savedPath = Settings.Default.BackupCacheFolderPath;

            if (string.IsNullOrWhiteSpace(savedPath))
                return false;

            try
            {
                backupFolder = NormalizeFolderPath(savedPath);
                return true;
            }
            catch
            {
                backupFolder = null;
                return false;
            }
        }

        public static string GetRecommendedDefaultBackupPath()
        {
            string baseDirectory =
                AppDomain.CurrentDomain.BaseDirectory;

            if (!IsRestrictedPath(baseDirectory) &&
                CanWriteToExistingDirectory(baseDirectory, out _))
            {
                return PortableBackupPath;
            }

            return LocalAppDataBackupPath;
        }

        public static bool TrySetBackupFolderPath(string selectedPath, IWin32Window owner, out string message)
        {
            message = string.Empty;

            string normalizedPath;

            try
            {
                normalizedPath = NormalizeFolderPath(selectedPath);
            }
            catch (Exception ex)
            {
                message = "Invalid backup folder path: " + ex.Message;
                return false;
            }

            if (string.IsNullOrWhiteSpace(normalizedPath))
            {
                message = "No backup folder path was selected.";
                return false;
            }

            if (IsSameOrUnder(normalizedPath, explorerPath))
            {
                message = "The backup folder cannot be inside the Explorer thumbnail cache folder.";
                return false;
            }

            BackupFolderStatusInfo statusInfo = GetBackupFolderStatusInfo(normalizedPath);

            if (!statusInfo.CanUseForBackup)
            {
                message = statusInfo.Message;
                return false;
            }

            if (statusInfo.Status == BackupFolderStatus.WritableAdmin ||
                statusInfo.Status == BackupFolderStatus.WillBeCreatedAdmin)
            {
                DialogResult choice = MessageBox.Show(
                    owner,
                    statusInfo.Message + "\n\n" +
                    "This folder works while the app is running as administrator, but backups may fail when the app is launched normally.\n\n" +
                    "Do you still want to use this folder?",
                    "Backup Folder",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (choice != DialogResult.Yes)
                {
                    message = "Backup folder selection cancelled.";
                    return false;
                }
            }

            try
            {
                Directory.CreateDirectory(statusInfo.NormalizedPath);
            }
            catch (Exception ex)
            {
                message = "Failed to create backup folder: " + ex.Message;
                return false;
            }

            Settings.Default.BackupCacheFolderPath = statusInfo.NormalizedPath;
            Settings.Default.Save();

            NotifyBackupPathChanged();

            message = "Backup folder set to: " + statusInfo.NormalizedPath + ".";
            WriteLine(message, LoggingFrequency.GUILogging);

            return true;
        }

        private bool EnsureBackupFolderForManualBackup()
        {
            if (TryGetBackupPath(out _))
                return true;

            string suggestedPath = GetRecommendedDefaultBackupPath();

            using FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                UseDescriptionForTitle = true,
                Description = "Select Backup Folder",
                ShowNewFolderButton = true
            };

            if (Directory.Exists(suggestedPath))
            {
                dialog.SelectedPath = suggestedPath;
            }
            else
            {
                string suggestedParent = Path.GetDirectoryName(suggestedPath);

                if (!string.IsNullOrWhiteSpace(suggestedParent) && Directory.Exists(suggestedParent))
                {
                    dialog.SelectedPath = suggestedParent;
                }
            }

            SetCacheOutput("Choose a backup folder.");

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                SetCacheOutput("Folder selection cancelled.");
                return false;
            }

            if (!TrySetBackupFolderPath(dialog.SelectedPath, this, out string selectedMessage))
            {
                WriteLine("Backup folder selection failed: " + selectedMessage, LoggingFrequency.GUILogging);

                SetCacheOutput("Backup folder unavailable.");
                return false;
            }

            SetCacheOutput("Backup folder selected.");
            UpdateCacheSizeLabels();

            return true;
        }

        private static string NormalizeFolderPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            string expanded = Environment.ExpandEnvironmentVariables(path.Trim().Trim('"'));
            return Path.GetFullPath(expanded);
        }

        private static bool TryEnsureWritableBackupFolder(string folder, out string message)
        {
            message = string.Empty;

            try
            {
                Directory.CreateDirectory(folder);

                string testFile = Path.Combine(folder, ".WinThumbsPreloader_WriteTest_" + Guid.NewGuid().ToString("N") + ".tmp");

                File.WriteAllText(testFile, "test");
                File.Delete(testFile);

                return true;
            }
            catch (Exception ex)
            {
                if (IsLikelyPermissionDenied(ex))
                {
                    message =
                        "Windows denied permission to write to the selected backup folder. \n\n" +
                        "Choose a different folder, such as your AppData folder, Documents folder, or another writable location. \n\n" +
                        "Folder:\n" + folder;
                }
                else
                {
                    message =
                        "The selected backup folder could not be created or written to.\n\n" +
                        "Folder:\n" + folder + "\n\n" +
                        "Error:\n" + ex.Message;
                }

                WriteLine("Backup folder write test failed: " + ex.Message, LoggingFrequency.GUILogging);
                return false;
            }
        }

        private static bool CanWriteToExistingDirectory(string folder, out string message)
        {
            message = string.Empty;

            try
            {
                if (!Directory.Exists(folder))
                {
                    message = "Folder does not exist.";
                    return false;
                }

                string testFile = Path.Combine(folder, ".WinThumbsPreloader_WriteTest_" + Guid.NewGuid().ToString("N") + ".tmp");

                File.WriteAllText(testFile, "test");
                File.Delete(testFile);

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        private static bool IsRestrictedPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

            return IsSameOrUnder(path, programFiles) ||
                   IsSameOrUnder(path, programFilesX86) ||
                   IsSameOrUnder(path, windows);
        }

        private static bool IsSameOrUnder(string path, string root)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(root))
                return false;

            try
            {
                string normalizedPath = NormalizeForCompare(path);
                string normalizedRoot = NormalizeForCompare(root);

                return normalizedPath.Equals(normalizedRoot, StringComparison.OrdinalIgnoreCase) ||
                       normalizedPath.StartsWith(normalizedRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static string NormalizeForCompare(string path)
        {
            return Path.GetFullPath(path)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private static bool IsLikelyPermissionDenied(Exception ex)
        {
            return ex is UnauthorizedAccessException ||
                   ex is SecurityException ||
                   ((uint)ex.HResult == 0x80070005);
        }

        public static long ExplorerCacheSize()
        {
            long explorerCacheSize = 0;
            try
            {
                string[] sourceFiles = Directory.GetFiles(explorerPath, "*.db");
                explorerCacheSize = sourceFiles.Sum(file => new FileInfo(file).Length);
            }
            catch (Exception e)
            {
                WriteLine("Failed to get explorer cache size: " + e.Message, LoggingFrequency.GUILogging);
            }
            return explorerCacheSize;
        }

        public static long? BackupCacheSizeNullable()
        {
            if (!TryGetBackupPath(out string currentBackupPath))
                return null;

            long backupCacheSize = 0;

            if (!Directory.Exists(currentBackupPath))
                return 0;

            try
            {
                string[] destinationFiles = Directory.GetFiles(currentBackupPath, "*.db");
                backupCacheSize = destinationFiles.Sum(file => new FileInfo(file).Length);
            }
            catch (Exception e)
            {
                WriteLine("Failed to get backup cache size: " + e.Message, LoggingFrequency.GUILogging);
            }

            return backupCacheSize;
        }

        public static long BackupCacheSize()
        {
            return BackupCacheSizeNullable() ?? 0;
        }

        private static bool resetLogged = false;

        public static bool CompareThumbsCacheSize()
        {
            long? backupCacheSizeNullable = BackupCacheSizeNullable();

            if (!backupCacheSizeNullable.HasValue)
            {
                resetLogged = false;
                return false;
            }

            long backupCacheSize = backupCacheSizeNullable.Value;
            long explorerCacheSize = ExplorerCacheSize();

            bool isCacheSizeLargerThanBackup = false;

            if (explorerCacheSize >= backupCacheSize)
            {
                isCacheSizeLargerThanBackup = true;
                resetLogged = false;
            }
            else if (explorerCacheSize < backupCacheSize)
            {
                isCacheSizeLargerThanBackup = false;

                if (!resetLogged)
                {
                    WriteLine("Backup Cache Size: " + backupCacheSize, LoggingFrequency.DebugLogging);
                    WriteLine("Explorer Cache Size: " + explorerCacheSize, LoggingFrequency.DebugLogging);
                    WriteLine("Cache Size Larger Than Backup: " + isCacheSizeLargerThanBackup, LoggingFrequency.DebugLogging);
                    resetLogged = true;
                }
            }

            return isCacheSizeLargerThanBackup;
        }

        public enum Success
        {
            Success,
            PartialSuccess,
            Failure
        }

        public enum CacheOperationFailureReason
        {
            None,
            BackupPathNotConfigured,
            InvalidBackupPath,
            PermissionDenied,
            NoSourceFiles,
            NoFilesCopied,
            AlreadyInProgress,
            Unknown
        }

        public sealed class CacheOperationResult
        {
            public Success Status { get; private set; }
            public CacheOperationFailureReason FailureReason { get; private set; }
            public string Message { get; private set; }
            public Exception Exception { get; private set; }

            public bool Succeeded => Status == Success.Success;
            public bool PartiallySucceeded => Status == Success.PartialSuccess;
            public bool Failed => Status == Success.Failure;

            public static CacheOperationResult Complete(string message)
            {
                return new CacheOperationResult
                {
                    Status = Success.Success,
                    FailureReason = CacheOperationFailureReason.None,
                    Message = message
                };
            }

            public static CacheOperationResult Partial(CacheOperationFailureReason reason, string message, Exception exception = null)
            {
                return new CacheOperationResult
                {
                    Status = Success.PartialSuccess,
                    FailureReason = reason,
                    Message = message,
                    Exception = exception
                };
            }

            public static CacheOperationResult Fail(CacheOperationFailureReason reason, string message, Exception exception = null)
            {
                return new CacheOperationResult
                {
                    Status = Success.Failure,
                    FailureReason = reason,
                    Message = message,
                    Exception = exception
                };
            }
        }

        private static async Task UpdateProgressBarAsync(ProgressBar progressBar, int value)
        {
            if (progressBar == null) return;

            if (!progressBar.IsHandleCreated)
            {
                var form = progressBar.FindForm();

                if (form != null)
                {
                    if (form.InvokeRequired)
                    {
                        await form.InvokeAsync(() => _ = progressBar.Handle);
                    }
                    else
                    {
                        _ = progressBar.Handle;
                    }
                }
            }

            value = Math.Clamp(value, 0, 100);

            await progressBar.InvokeAsync(() =>
            {
                if (!progressBar.Visible)
                    progressBar.Visible = true;

                progressBar.Value = value;
            });

            if (value == 100)
            {
                await progressBar.InvokeAsync(() =>
                {
                    progressBar.Value = 99;
                    progressBar.Value = 100;
                });
                await Task.Delay(33);
            }
        }

        private static void HideProgressBar(ProgressBar progressBar)
        {
            if (progressBar == null) return;

            progressBar.Invoke((MethodInvoker)(() =>
            {
                progressBar.Visible = false;
            }));
        }

        private async void BackupButton_Click(object sender, EventArgs e)
        {
            if (!EnsureBackupFolderForManualBackup())
                return;

            long src = ExplorerCacheSize();
            long dst = BackupCacheSize();

            if (src <= dst)
            {

                string message = src < dst
                    ? "The current thumbcache size is smaller than the backup size. Overwrite the backup?"
                    : "The current thumbcache size is equal to the backup size. Overwrite the backup?";

                DialogResult overwriteChoice = MessageBox.Show(this, message, "Confirm Backup Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (overwriteChoice != DialogResult.Yes)
                {
                    SetCacheOutput("Backup cancelled.");
                    return;
                }
            }

            SetActionButtonsEnabled(false);

            try
            {
                SetCacheOutput("Backing up thumbnail cache...");

                autoBackupTimer.Stop();

                CacheOperationResult result = await BackupThumbsCacheDetailedAsync(progressBarRestore);

                autoBackupTimer.Start();

                OutputBackupSuccessText(result);
                UpdateCacheSizeLabels();
            }
            finally
            {
                SetActionButtonsEnabled(true);
            }
        }

        private void OutputBackupSuccessText(CacheOperationResult result)
        {
            string message = result?.Message ?? "Thumbnail cache backup failed.";

            WriteLine("Backup result: " + message, LoggingFrequency.GUILogging);

            SetCacheOutput(message);
        }

        public static async Task<Success> BackupThumbsCacheAsync(ProgressBar progressBar)
        {
            CacheOperationResult result = await BackupThumbsCacheDetailedAsync(progressBar);
            return result.Status;
        }

        public static async Task<CacheOperationResult> BackupThumbsCacheDetailedAsync(ProgressBar progressBar)
        {
            if (!TryGetBackupPath(out string currentBackupPath))
            {
                HideProgressBar(progressBar);

                return CacheOperationResult.Fail(CacheOperationFailureReason.BackupPathNotConfigured, "No backup folder selected. Choose a backup folder before backing up.");
            }

            CacheBackupLock.AcquireResult lockResult = CacheBackupLock.TryAcquire(currentBackupPath, out Exception lockException);

            if (lockResult != CacheBackupLock.AcquireResult.Acquired)
            {
                HideProgressBar(progressBar);

                return lockResult switch
                {
                    CacheBackupLock.AcquireResult.AlreadyHeldByThisProcess =>
                        CacheOperationResult.Fail(
                            CacheOperationFailureReason.AlreadyInProgress,
                            "Skipped: a thumbnail cache backup is already running in this process."
                        ),

                    CacheBackupLock.AcquireResult.AlreadyHeldByAnotherProcess =>
                        CacheOperationResult.Fail(
                            CacheOperationFailureReason.AlreadyInProgress,
                            "Skipped: another WinThumbsPreloader instance is already backing up the thumbnail cache."
                        ),

                    CacheBackupLock.AcquireResult.PermissionDenied =>
                        CacheOperationResult.Fail(
                            CacheOperationFailureReason.PermissionDenied,
                            "Backup failed because Windows denied permission to create or open the backup lock file. Choose a writable backup folder or run as administrator.",
                            lockException
                        ),

                    CacheBackupLock.AcquireResult.InvalidPath =>
                        CacheOperationResult.Fail(
                            CacheOperationFailureReason.InvalidBackupPath,
                            "Backup failed because the selected backup folder path is invalid or unavailable.",
                            lockException
                        ),

                    _ =>
                        CacheOperationResult.Fail(
                            CacheOperationFailureReason.Unknown,
                            "Backup failed because the backup lock file could not be created or opened: " +
                            (lockException?.Message ?? "Unknown lock error."),
                            lockException
                        )
                };
            }

            try
            {
                return await Task.Run(async () =>
                {
                    Success result = Success.Success;
                    bool allowPartial = Settings.Default.BackupRestoreClearSafety == "Allow Partial Copy";
                    bool sawPermissionFailure = false;

                    var movedOldBackupFileNames = new List<string>();
                    var newBackupFileNamesWritten = new List<string>();

                    try
                    {
                        if (!TryEnsureWritableBackupFolder(currentBackupPath, out string writableMessage))
                        {
                            HideProgressBar(progressBar);

                            return CacheOperationResult.Fail(CacheOperationFailureReason.PermissionDenied, writableMessage);
                        }

                        string tempPath = Path.Combine(currentBackupPath, "_staging_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));

                        string restorePath = Path.Combine(currentBackupPath, "_restore_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));

                        Directory.CreateDirectory(tempPath);

                        string[] sourceFiles = Directory.GetFiles(explorerPath, "*.db");

                        string[] existingFiles = Directory
                            .EnumerateFiles(currentBackupPath, "*.db", SearchOption.TopDirectoryOnly)
                            .ToArray();

                        if (sourceFiles.Length == 0)
                        {
                            DeleteDirectory(tempPath);
                            HideProgressBar(progressBar);

                            return CacheOperationResult.Fail(CacheOperationFailureReason.NoSourceFiles, "No Explorer thumbnail cache files were found to back up.");
                        }

                        int totalSteps = sourceFiles.Length + existingFiles.Length + sourceFiles.Length;
                        int completed = 0;

                        await UpdateProgressBarAsync(progressBar, 0);

                        int copyFailures = 0;

                        foreach (string src in sourceFiles)
                        {
                            string dest = Path.Combine(tempPath, Path.GetFileName(src));

                            try
                            {
                                File.Copy(src, dest, true);
                            }
                            catch (Exception ex)
                            {
                                WriteLine("Failed to copy to staging: " + ex.Message, LoggingFrequency.DebugLogging);

                                if (IsLikelyPermissionDenied(ex))
                                    sawPermissionFailure = true;

                                copyFailures++;

                                if (!allowPartial)
                                {
                                    DeleteDirectory(tempPath);
                                    HideProgressBar(progressBar);

                                    return CacheOperationResult.Fail(
                                        sawPermissionFailure
                                            ? CacheOperationFailureReason.PermissionDenied
                                            : CacheOperationFailureReason.Unknown,
                                        sawPermissionFailure
                                            ? "Backup failed because Windows denied permission to copy one or more cache files."
                                            : "Backup failed while copying cache files to the staging folder.",
                                        ex
                                    );
                                }
                            }

                            completed++;
                            await UpdateProgressBarAsync(progressBar, completed * 100 / totalSteps);
                        }

                        int totalFiles = sourceFiles.Length;
                        int successfulCopies = totalFiles - copyFailures;

                        if (allowPartial)
                        {
                            if (successfulCopies == 0)
                            {
                                WriteLine("No files could be copied. Backup failed.", LoggingFrequency.GUILogging);

                                DeleteDirectory(tempPath);
                                DeleteDirectory(restorePath);
                                HideProgressBar(progressBar);

                                return CacheOperationResult.Fail(
                                    sawPermissionFailure
                                        ? CacheOperationFailureReason.PermissionDenied
                                        : CacheOperationFailureReason.NoFilesCopied,
                                    sawPermissionFailure
                                        ? "Backup failed because Windows denied permission to copy the cache files."
                                        : "Backup failed because no cache files could be copied."
                                );
                            }

                            if (copyFailures > 0)
                                result = Success.PartialSuccess;
                        }

                        if (existingFiles.Length > 0)
                            Directory.CreateDirectory(restorePath);

                        foreach (string oldFile in existingFiles)
                        {
                            string fileName = Path.GetFileName(oldFile);

                            try
                            {
                                File.Move(oldFile, Path.Combine(restorePath, fileName), true);
                                movedOldBackupFileNames.Add(fileName);
                            }
                            catch (Exception ex)
                            {
                                WriteLine("Failed to move old backup to restore folder: " + ex.Message, LoggingFrequency.DebugLogging);

                                if (IsLikelyPermissionDenied(ex))
                                    sawPermissionFailure = true;

                                if (!allowPartial)
                                {
                                    RollbackBackup(currentBackupPath, restorePath, movedOldBackupFileNames, newBackupFileNamesWritten);

                                    DeleteDirectory(tempPath);
                                    HideProgressBar(progressBar);

                                    return CacheOperationResult.Fail(
                                        sawPermissionFailure
                                            ? CacheOperationFailureReason.PermissionDenied
                                            : CacheOperationFailureReason.Unknown,
                                        sawPermissionFailure
                                            ? "Backup failed because Windows denied permission to replace the old backup."
                                            : "Backup failed while moving the old backup into the restore folder.",
                                        ex
                                    );
                                }

                                result = Success.PartialSuccess;
                            }

                            completed++;
                            await UpdateProgressBarAsync(progressBar, completed * 100 / totalSteps);
                        }

                        foreach (string tempCache in Directory.GetFiles(tempPath, "*.db"))
                        {
                            string fileName = Path.GetFileName(tempCache);
                            string dest = Path.Combine(currentBackupPath, fileName);

                            try
                            {
                                File.Move(tempCache, dest, true);
                                newBackupFileNamesWritten.Add(fileName);
                            }
                            catch (Exception ex)
                            {
                                WriteLine("Failed final move: " + ex.Message, LoggingFrequency.DebugLogging);

                                if (IsLikelyPermissionDenied(ex))
                                    sawPermissionFailure = true;

                                if (!allowPartial)
                                {
                                    RollbackBackup(currentBackupPath, restorePath, movedOldBackupFileNames, newBackupFileNamesWritten);

                                    DeleteDirectory(tempPath);
                                    HideProgressBar(progressBar);

                                    return CacheOperationResult.Fail(
                                        sawPermissionFailure
                                            ? CacheOperationFailureReason.PermissionDenied
                                            : CacheOperationFailureReason.Unknown,
                                        sawPermissionFailure
                                            ? "Backup failed because Windows denied permission to write to the backup folder."
                                            : "Backup failed while moving staged files into the backup folder.",
                                        ex
                                    );
                                }

                                result = Success.PartialSuccess;
                            }

                            completed++;
                            await UpdateProgressBarAsync(progressBar, completed * 100 / totalSteps);
                        }

                        DeleteDirectory(tempPath);
                        DeleteDirectory(restorePath);

                        await UpdateProgressBarAsync(progressBar, 100);
                        HideProgressBar(progressBar);

                        if (result == Success.PartialSuccess)
                        {
                            SaveLastBackupDateTimeForFolder(currentBackupPath);
                            NotifyBackupInfoChanged();
                            return CacheOperationResult.Partial(
                                sawPermissionFailure
                                    ? CacheOperationFailureReason.PermissionDenied
                                    : CacheOperationFailureReason.Unknown,
                                sawPermissionFailure
                                    ? "Thumbnail cache backup partially complete. Some files could not be copied because Windows denied permission."
                                    : "Thumbnail cache backup partially complete."
                            );
                        }
                        SaveLastBackupDateTimeForFolder(currentBackupPath);
                        NotifyBackupInfoChanged();
                        return CacheOperationResult.Complete("Thumbnail cache backup complete.");
                    }
                    catch (Exception ex)
                    {
                        WriteLine("Backup exception: " + ex.Message, LoggingFrequency.GUILogging);
                        HideProgressBar(progressBar);

                        return CacheOperationResult.Fail(
                            IsLikelyPermissionDenied(ex)
                                ? CacheOperationFailureReason.PermissionDenied
                                : CacheOperationFailureReason.Unknown,
                            IsLikelyPermissionDenied(ex)
                                ? "Thumbnail cache backup failed because Windows denied permission to the backup folder. Choose a different folder or run as administrator."
                                : "Thumbnail cache backup failed: " + ex.Message,
                            ex
                        );
                    }
                });
            }
            finally
            {
                CacheBackupLock.Release();
            }
        }

        private static void DeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch { }
        }

        private static void RollbackBackup(string backupPath, string restorePath, IEnumerable<string> movedOldBackupFileNames, IEnumerable<string> newBackupFileNamesWritten)
        {
            WriteLine("Rolling back backup...", LoggingFrequency.GUILogging);

            try
            {
                Directory.CreateDirectory(backupPath);

                foreach (string fileName in newBackupFileNamesWritten)
                {
                    string backupFile = Path.Combine(backupPath, fileName);

                    try
                    {
                        if (File.Exists(backupFile))
                        {
                            File.Delete(backupFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLine("Rollback failed to delete new backup file: " + ex.Message, LoggingFrequency.DebugLogging);
                    }
                }

                foreach (string fileName in movedOldBackupFileNames)
                {
                    string restoreFile = Path.Combine(restorePath, fileName);
                    string backupFile = Path.Combine(backupPath, fileName);

                    try
                    {
                        if (File.Exists(restoreFile))
                        {
                            File.Move(restoreFile, backupFile, overwrite: true);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLine("Rollback failed to restore file: " + ex.Message, LoggingFrequency.DebugLogging);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine("RollbackBackup exception: " + ex.Message, LoggingFrequency.DebugLogging);
            }
        }

        public const int sagesetNumber = 100;
        private const string ThumbnailCacheKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache";

        public void ConfigureDiskCleanupSageset()
        {
            WriteLine("Configuring disk cleanup sageset...", LoggingFrequency.DebugLogging);
            try
            {
                using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(ThumbnailCacheKey, writable: true))
                {
                    if (regKey != null)
                    {
                        WriteLine("ThumbnailCacheKey: " + ThumbnailCacheKey, LoggingFrequency.DebugLogging);
                        WriteLine("SagesetNumber: " + sagesetNumber, LoggingFrequency.DebugLogging);
                        regKey.SetValue($"StateFlags{sagesetNumber:D4}", 2, RegistryValueKind.DWord);
                    }
                    else
                    {
                        throw new Exception("Failed to open registry key.");
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine("Failed to configure disk cleanup: " + e.Message, LoggingFrequency.GUILogging);
                OutputTextBox.Text = "Failed to configure disk cleanup. Retry as Admin.";
            }
        }

        public static async Task<Success> RunThumbnailDiskCleanupAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    WriteLine("Running disk cleanup...", LoggingFrequency.GUILogging);

                    var info = new ProcessStartInfo()
                    {
                        FileName = "cleanmgr.exe",
                        Arguments = $"/sagerun:{sagesetNumber}",
                        UseShellExecute = false
                    };

                    using (var p = Process.Start(info))
                    {
                        p?.WaitForExit();
                    }

                    WriteLine("Disk cleanup complete.", LoggingFrequency.GUILogging);
                    return Success.Success;
                }
                catch (Exception ex)
                {
                    WriteLine("Disk cleanup failed: " + ex.Message, LoggingFrequency.GUILogging);
                    return Success.Failure;
                }
            });
        }

        private async void RestoreButton_Click(object sender, EventArgs e)
        {
            if (!TryGetBackupPath(out string currentBackupPath))
            {
                SetCacheOutput("No backup folder selected.");
                return;
            }

            if (!Directory.Exists(currentBackupPath))
            {
                SetCacheOutput("Backup not found.");
                return;
            }

            if (!Directory.Exists(explorerPath))
            {
                SetCacheOutput("Explorer cache folder not found.");
                return;
            }

            bool force = RestoreButton.Text == "Force Restore";

            string question = force
                ? "Are you sure you want to force restore from backup?" +
                  Environment.NewLine +
                  Environment.NewLine +
                  "Explorer will be closed."
                : "Are you sure you want to restore from backup?";

            DialogResult restoreChoice = MessageBox.Show(this, question, "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (restoreChoice != DialogResult.Yes)
            {
                SetCacheOutput(force ? "Force restore cancelled." : "Restore cancelled.");

                return;
            }

            SetActionButtonsEnabled(false);

            try
            {
                autoRestoreTimer.Stop();

                Success result = await RestoreThumbsCache(progressBarRestore, force);

                autoRestoreTimer.Start();

                string resultMessage = result switch
                {
                    Success.Success => force
                        ? "Thumbnail cache force-restored."
                        : "Thumbnail cache restored.",

                    Success.PartialSuccess => force
                        ? "Thumbnail cache partially force-restored."
                        : "Thumbnail cache partially restored.",

                    _ => force
                        ? "Force restore failed. Retry as admin."
                        : "Restore failed. Try force restore or admin."
                };

                SetCacheOutput(resultMessage);
            }
            finally
            {
                SetActionButtonsEnabled(true);
            }
        }

        public static async Task<Success> RestoreThumbsCache(ProgressBar progressBar, bool forceRestore)
        {
            return await Task.Run(async () =>
            {
                WriteLine("Restoring thumbnail cache...", LoggingFrequency.GUILogging);

                if (!TryGetBackupPath(out string currentBackupPath))
                {
                    WriteLine("No backup folder selected.", LoggingFrequency.GUILogging);
                    return Success.Failure;
                }

                Success result = Success.Success;
                bool allowPartial = (Properties.Settings.Default.BackupRestoreClearSafety == "Allow Partial Copy");

                string rollbackPath = Path.Combine(Path.GetTempPath(), "WinThumbsPreloader_RestoreRollback_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));

                var restoredFileNames = new List<string>();

                try
                {
                    long backupSize = BackupCacheSize();
                    if (backupSize == 0)
                    {
                        WriteLine("No backup found.", LoggingFrequency.GUILogging);
                        return Success.Failure;
                    }

                    string[] backupFiles = Directory.GetFiles(currentBackupPath, "*.db");
                    if (backupFiles.Length == 0)
                    {
                        WriteLine("No backup files found.", LoggingFrequency.GUILogging);
                        return Success.Failure;
                    }

                    if (forceRestore)
                    {
                        await CloseExplorerAsync();
                    }

                    // Copy current explorer cache to temporary restore folder
                    if (!allowPartial)
                    {
                        Directory.CreateDirectory(rollbackPath);

                        foreach (string explorerFile in Directory.GetFiles(explorerPath, "*.db"))
                        {
                            string fileName = Path.GetFileName(explorerFile);
                            string rollbackFile = Path.Combine(rollbackPath, fileName);

                            try
                            {
                                File.Copy(explorerFile, rollbackFile, true);
                            }
                            catch (Exception ex)
                            {
                                WriteLine("Failed to create restore rollback copy: " + ex.Message, LoggingFrequency.GUILogging);
                                DeleteDirectory(rollbackPath);
                                return Success.Failure;
                            }
                        }
                    }

                    // Delete current explorer cache using selected method
                    try
                    {
                        string mode = Properties.Settings.Default.ExplorerCacheDeletionMethod;
                        Success deleteResult;

                        if (mode == "Disk Cleanup")
                        {
                            WriteLine("Using Disk Cleanup deletion method.", LoggingFrequency.GUILogging);
                            deleteResult = await RunThumbnailDiskCleanupAsync();
                        }
                        else // Manual Deletion
                        {
                            WriteLine("Using Manual Deletion method.", LoggingFrequency.GUILogging);
                            deleteResult = await DeleteExplorerThumbsCacheAsync(forceRestore);
                        }

                        if (!allowPartial)
                        {
                            if (deleteResult != Success.Success)
                            {
                                WriteLine("Strict restore aborted because cache deletion was not fully successful.", LoggingFrequency.GUILogging);

                                RollbackRestore(explorerPath, rollbackPath, restoredFileNames);

                                if (forceRestore)
                                {
                                    await Task.Delay(1000);
                                    await RestartExplorerAsync();
                                }

                                DeleteDirectory(rollbackPath);
                                return Success.Failure;
                            }
                        }
                        else
                        {
                            if (deleteResult == Success.Failure)
                            {
                                result = Success.Failure;
                            }
                            else if (deleteResult == Success.PartialSuccess && result != Success.Failure)
                            {
                                result = Success.PartialSuccess;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLine($"Error deleting existing cache: {ex.Message}", LoggingFrequency.GUILogging);

                        if (!allowPartial)
                        {
                            RollbackRestore(explorerPath, rollbackPath, restoredFileNames);

                            if (forceRestore)
                            {
                                await Task.Delay(1000);
                                await RestartExplorerAsync();
                            }

                            DeleteDirectory(rollbackPath);
                            return Success.Failure;
                        }

                        result = Success.Failure;
                    }

                    // Ensure explorer cache folder exists
                    Directory.CreateDirectory(explorerPath);

                    // Restore files
                    int totalFiles = backupFiles.Length;
                    int failureCount = 0;

                    await UpdateProgressBarAsync(progressBar, 0);

                    foreach (string file in backupFiles)
                    {
                        bool copied = false;
                        int retryCount = 0;
                        string fileName = Path.GetFileName(file);
                        string destination = Path.Combine(explorerPath, fileName);

                        while (!copied && retryCount < 3)
                        {
                            try
                            {
                                if (forceRestore && Properties.Settings.Default.ExplorerCloseFrequency == "On Every File")
                                {
                                    await CloseExplorerAsync();
                                }

                                File.Copy(file, destination, true);
                                copied = true;
                                restoredFileNames.Add(fileName);
                            }
                            catch (Exception ex)
                            {
                                retryCount++;
                                WriteLine($"Retry {retryCount} - Failed to copy {fileName}: {ex.Message}", LoggingFrequency.DebugLogging);
                                await Task.Delay(500);
                            }
                        }

                        if (!copied)
                        {
                            WriteLine($"Failed to restore {fileName} after 3 retries.", LoggingFrequency.GUILogging);
                            failureCount++;

                            if (!allowPartial)
                            {
                                RollbackRestore(explorerPath, rollbackPath, restoredFileNames);
                                HideProgressBar(progressBar);

                                if (forceRestore)
                                {
                                    await Task.Delay(1000);
                                    await RestartExplorerAsync();
                                }

                                DeleteDirectory(rollbackPath);
                                return Success.Failure;
                            }
                        }

                        long currentSize = ExplorerCacheSize();
                        int progress = (int)((currentSize * 100) / backupSize);
                        await UpdateProgressBarAsync(progressBar, Math.Min(progress, 100));
                    }

                    await UpdateProgressBarAsync(progressBar, 100);
                    HideProgressBar(progressBar);

                    // Restart Explorer
                    if (forceRestore)
                    {
                        await Task.Delay(1000);
                        await RestartExplorerAsync();
                    }

                    DeleteDirectory(rollbackPath);

                    // Set result based on failures
                    if (failureCount == totalFiles)
                    {
                        result = Success.Failure;
                    }
                    else if (failureCount > 0)
                    {
                        if (result != Success.Failure)
                            result = Success.PartialSuccess;
                    }
                    else
                    {
                        if (result != Success.Failure && result != Success.PartialSuccess)
                            result = Success.Success;
                    }

                    WriteLine($"Restore complete. Status: {result}", LoggingFrequency.GUILogging);
                    return result;
                }
                catch (Exception ex)
                {
                    WriteLine("Restore exception: " + ex.Message, LoggingFrequency.GUILogging);

                    if (!allowPartial)
                    {
                        RollbackRestore(explorerPath, rollbackPath, restoredFileNames);
                    }

                    HideProgressBar(progressBar);

                    if (forceRestore)
                    {
                        try
                        {
                            await Task.Delay(1000);
                            await RestartExplorerAsync();
                        }
                        catch { }
                    }

                    DeleteDirectory(rollbackPath);
                    return Success.Failure;
                }
            });
        }

        private static void RollbackRestore(string explorerPath, string rollbackPath, IEnumerable<string> restoredFileNames)
        {
            WriteLine("Rolling back restore...", LoggingFrequency.GUILogging);

            try
            {
                Directory.CreateDirectory(explorerPath);

                // Remove only the files that were restored during this attempt
                foreach (string fileName in restoredFileNames)
                {
                    string explorerFile = Path.Combine(explorerPath, fileName);

                    try
                    {
                        if (File.Exists(explorerFile))
                        {
                            File.Delete(explorerFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLine("Rollback restore failed to delete restored file: " + ex.Message, LoggingFrequency.DebugLogging);
                    }
                }

                // Restore original Explorer cache files from rollback folder
                if (Directory.Exists(rollbackPath))
                {
                    foreach (string rollbackFile in Directory.GetFiles(rollbackPath, "*.db"))
                    {
                        string fileName = Path.GetFileName(rollbackFile);
                        string explorerFile = Path.Combine(explorerPath, fileName);

                        try
                        {
                            File.Copy(rollbackFile, explorerFile, true);
                        }
                        catch (Exception ex)
                        {
                            WriteLine("Rollback restore failed to restore original file: " + ex.Message, LoggingFrequency.DebugLogging);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine("RollbackRestore exception: " + ex.Message, LoggingFrequency.DebugLogging);
            }
        }

        private static async Task<Success> DeleteExplorerThumbsCacheAsync(bool forceRestore)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    if (!Directory.Exists(explorerPath))
                        return Success.Failure;

                    string[] files = Directory.GetFiles(explorerPath, "*.db");
                    if (files.Length == 0)
                        return Success.Success;

                    int failed = 0;

                    WriteLine("Deleting Explorer cache manually...", LoggingFrequency.GUILogging);

                    foreach (string file in files)
                    {
                        try
                        {
                            if (forceRestore && Properties.Settings.Default.ExplorerCloseFrequency == "On Every File")
                            {
                                await CloseExplorerAsync();
                            }

                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            failed++;
                            WriteLine($"Failed to delete {file}: {ex.Message}", LoggingFrequency.DebugLogging);
                        }
                    }

                    if (failed == 0)
                        return Success.Success;

                    if (failed < files.Length)
                        return Success.PartialSuccess;

                    return Success.Failure;
                }
                catch (Exception ex)
                {
                    WriteLine("DeleteExplorerThumbsCacheAsync exception: " + ex.Message, LoggingFrequency.GUILogging);
                    return Success.Failure;
                }
            });
        }

        private static async Task CloseExplorerAsync()
        {
            WriteLine("Closing Explorer...", LoggingFrequency.GUILogging);

            var processes = Process.GetProcessesByName("explorer");

            if (processes.Length == 0)
            {
                return;
            }

            await Task.Run(() =>
            {
                foreach (var process in processes)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception ex)
                    {
                        WriteLine("Failed to kill explorer: " + ex.Message, LoggingFrequency.GUILogging);
                    }
                }

                // Wait for all explorer processes to exit
                foreach (var process in processes)
                {
                    try
                    {
                        process.WaitForExit(5000);
                    }
                    catch { }
                }
            });

            // Allow OS to clean up COM shell components
            await Task.Delay(300);
        }

        private static async Task RestartExplorerAsync()
        {
            try
            {
                // Check if explorer is already running
                bool explorerRunning = Process.GetProcessesByName("explorer").Length != 0;

                if (!explorerRunning)
                {
                    WriteLine("Restarting Explorer...", LoggingFrequency.GUILogging);

                    await Task.Run(() =>
                    {
                        Process.Start("explorer.exe");
                    });
                }
                else
                {
                    WriteLine("Explorer already running, restarting cancelled.", LoggingFrequency.GUILogging);
                }
            }
            catch (Exception e)
            {
                WriteLine("Failed to restart explorer: " + e.Message, LoggingFrequency.GUILogging);
            }
        }

        private void AutoBackupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AutoBackupThumbs = AutoBackupCheckBox.Checked;
            Settings.Default.Save();
            toggleAutoBackupToolStripMenuItem.Checked = AutoBackupCheckBox.Checked;
        }

        private void AlertCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ThumbsResetAlert = AlertCheckBox.Checked;
            Settings.Default.Save();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_MAXIMIZE = 3;

        private async void ClearButton_Click(object sender, EventArgs e)
        {
            if (ClearButton.Text == "Open Backup")
            {
                if (OpenBackupFolder(null, null))
                    SetCacheOutput("Backup folder opened.");

                return;
            }

            DialogResult clearBackupChoice = MessageBox.Show(this, "Are you sure you want to clear the backup?", "Confirm Clear Backup", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (clearBackupChoice != DialogResult.Yes)
            {
                SetCacheOutput("Clear backup cancelled.");
                return;
            }

            SetActionButtonsEnabled(false);

            Success result;

            try
            {
                result = await ClearBackupCacheAsync();
            }
            finally
            {
                SetActionButtonsEnabled(true);
            }

            ClearButton.Enabled = true;
            ClearCacheButton.Enabled = true;

            SetCacheOutput(result switch
            {
                Success.Success => "Backup cleared.",
                Success.PartialSuccess => "Backup partially cleared.",
                _ => "Failed to clear backup."
            });
        }

        private bool OpenBackupFolder(object sender, EventArgs e)
        {
            WriteLine("Opening backup folder...", LoggingFrequency.GUILogging);

            if (!TryGetBackupPath(out string currentBackupPath))
            {
                SetCacheOutput("No backup folder selected.");
                return false;
            }

            try
            {
                // Check if the Explorer backup folder is open and maximize it if it is
                var processes = Process.GetProcessesByName("explorer");

                bool backupFolderWindowFound = false;

                IntPtr foregroundWindow = GetForegroundWindow();

                foreach (var process in processes)
                {
                    if (process.MainWindowTitle == "Explorer Backup")
                    {
                        if (process.MainWindowHandle != foregroundWindow || IsIconic(process.MainWindowHandle))
                        {
                            ShowWindowAsync(process.MainWindowHandle, SW_MAXIMIZE);
                            SetForegroundWindow(process.MainWindowHandle);
                        }

                        backupFolderWindowFound = true;
                        break;
                    }
                }

                // If not found, start a new instance.
                if (!backupFolderWindowFound)
                {
                    if (!Directory.Exists(currentBackupPath))
                        Directory.CreateDirectory(currentBackupPath);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = currentBackupPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }

                return true;
            }
            catch (Exception ex)
            {
                WriteLine("Failed to open backup folder: " + ex.Message, LoggingFrequency.GUILogging);

                SetCacheOutput("Failed to open backup folder.");
                return false;
            }
        }

        public static async Task<Success> ClearBackupCacheAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!TryGetBackupPath(out string currentBackupPath))
                        return Success.Failure;

                    if (!Directory.Exists(currentBackupPath))
                        return Success.Failure;

                    var files = Directory.GetFiles(currentBackupPath, "*.db");
                    int failed = 0;

                    foreach (var file in files)
                    {
                        try { File.Delete(file); }
                        catch (Exception ex)
                        {
                            failed++;
                            WriteLine($"Failed to delete backup file: {ex.Message}", LoggingFrequency.DebugLogging);
                        }
                    }

                    if (files.Length == 0)
                        return Success.Failure;

                    if (failed == 0)
                    {
                        DeleteLastBackupDateTimeForFolder(currentBackupPath);
                        NotifyBackupInfoChanged();
                        return Success.Success;
                    }

                    if (failed < files.Length)
                    {
                        DeleteLastBackupDateTimeForFolder(currentBackupPath);
                        NotifyBackupInfoChanged();
                        return Success.PartialSuccess;
                    }

                    return Success.Failure;
                }
                catch (Exception ex)
                {
                    WriteLine("ClearBackupCacheAsync exception: " + ex.Message, LoggingFrequency.GUILogging);
                    return Success.Failure;
                }
            });
        }

        private async void ClearCacheButton_Click(object sender, EventArgs e)
        {
            if (ClearCacheButton.Text == "Open Cache")
            {
                OpenCacheFolder(null, null);
                SetCacheOutput("Explorer cache opened.");
                return;
            }

            DialogResult clearCacheChoice = MessageBox.Show(this, "Are you sure you want to clear the Explorer cache?", "Confirm Clear Cache", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (clearCacheChoice != DialogResult.Yes)
            {
                SetCacheOutput("Clear cache cancelled.");
                return;
            }

            Success result;

            SetActionButtonsEnabled(false);

            try
            {
                if (Settings.Default.ExplorerCacheDeletionMethod == "Disk Cleanup")
                {
                    result = await RunThumbnailDiskCleanupAsync();
                }
                else
                {
                    result = await ClearExplorerCacheAsync();
                }
            }
            finally
            {
                SetActionButtonsEnabled(true);
            }

            SetCacheOutput(result switch
            {
                Success.Success => "Explorer cache cleared.",
                Success.PartialSuccess => "Explorer cache partially cleared.",
                _ => "Failed to clear explorer cache."
            });
        }

        private void OpenCacheFolder(object sender, EventArgs e)
        {
            WriteLine("Opening cache folder...", LoggingFrequency.GUILogging);
            var processes = Process.GetProcessesByName("explorer");

            bool cacheFolderWindowFound = false;

            IntPtr foregroundWindow = GetForegroundWindow();

            foreach (var process in processes)
            {
                if (process.MainWindowTitle == "Explorer")
                {
                    if (process.MainWindowHandle != foregroundWindow)
                    {
                        if (process.MainWindowHandle != foregroundWindow || IsIconic(process.MainWindowHandle))
                        {
                            ShowWindowAsync(process.MainWindowHandle, SW_MAXIMIZE);

                            SetForegroundWindow(process.MainWindowHandle);
                        }
                    }
                    cacheFolderWindowFound = true;
                    break;
                }
            }

            if (!cacheFolderWindowFound)
            {
                if (!Directory.Exists(explorerPath))
                {
                    WriteLine("Cache folder not found", LoggingFrequency.GUILogging);
                    OutputTextBox.Text = "Cache folder not found";
                    return;
                }

                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = explorerPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
                catch (Exception ex)
                {
                    WriteLine("Failed to open cache folder: " + ex.Message, LoggingFrequency.GUILogging);
                    OutputTextBox.Text = "Failed to open cache folder";
                }
            }
        }

        public static async Task<Success> ClearExplorerCacheAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(explorerPath))
                        return Success.Failure;

                    var files = Directory.GetFiles(explorerPath, "*.db");
                    int failed = 0;

                    foreach (var file in files)
                    {
                        try { File.Delete(file); }
                        catch (Exception ex)
                        {
                            failed++;
                            WriteLine($"Failed to delete explorer cache file: {ex.Message}", LoggingFrequency.DebugLogging);
                        }
                    }

                    if (files.Length == 0)
                        return Success.Failure;

                    if (failed == 0) return Success.Success;
                    if (failed < files.Length) return Success.PartialSuccess;
                    return Success.Failure;
                }
                catch (Exception ex)
                {
                    WriteLine("ClearExplorerCacheAsync exception: " + ex.Message, LoggingFrequency.GUILogging);
                    return Success.Failure;
                }
            });
        }

        private void SetActionButtonsEnabled(bool enabled)
        {
            BackupButton.Enabled = enabled;
            RestoreButton.Enabled = enabled;
            ClearButton.Enabled = enabled;
            ClearCacheButton.Enabled = enabled;
        }

        private void CacheForm_Activated(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                // Shift key is currently pressed, so change the button text
                ClearButton.Text = "Clear Backup";
                ClearCacheButton.Text = "Clear Cache";
                CloseButton.Text = "Exit";
                RestoreButton.Text = "Force Restore";
            }
            else
            {
                // Shift key is not pressed, so set the button text to its original value
                ClearButton.Text = "Open Backup";
                ClearCacheButton.Text = "Open Cache";
                CloseButton.Text = "Close";
                RestoreButton.Text = "Restore";
            }
        }

        private void CacheForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text when Shift is pressed
                ClearButton.Text = "Clear Backup";
                ClearCacheButton.Text = "Clear Cache";
                CloseButton.Text = "Exit";
                RestoreButton.Text = "Force Restore";
            }
        }

        private void CacheForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text back when Shift is released
                ClearButton.Text = "Open Backup";
                ClearCacheButton.Text = "Open Cache";
                CloseButton.Text = "Close";
                RestoreButton.Text = "Restore";
            }
        }

        private void AutoRestoreCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AutoRestoreThumbs = AutoRestoreCheckBox.Checked;
            Settings.Default.Save();
            toggleAutoRestoreToolStripMenuItem.Checked = AutoRestoreCheckBox.Checked;
        }

        private void toggleAutoBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoBackupCheckBox.Checked = !AutoBackupCheckBox.Checked;
            toggleAutoBackupToolStripMenuItem.Checked = AutoBackupCheckBox.Checked;
        }

        private void toggleAutoRestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoRestoreCheckBox.Checked = !AutoRestoreCheckBox.Checked;
            toggleAutoRestoreToolStripMenuItem.Checked = AutoRestoreCheckBox.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteLine("Exiting application from icon tray", LoggingFrequency.DebugLogging);
            Environment.Exit(0);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1_DoubleClick(this, EventArgs.Empty);
        }

        private void StartWithWindowsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            StartWithWindowsCheckBox.CheckedChanged -= StartWithWindowsCheckBox_CheckedChanged;

            try
            {
                if (SetStartup(StartWithWindowsCheckBox.Checked))
                {
                    WriteLine("Startup setting updated successfully", LoggingFrequency.GUILogging);
                    Settings.Default.StartWithWindows = StartWithWindowsCheckBox.Checked;
                    Settings.Default.Save();
                }
                else
                {
                    WriteLine("Failed to update startup setting", LoggingFrequency.GUILogging);
                    OutputTextBox.Text = "Failed to update startup setting. Retry as admin.";
                    StartWithWindowsCheckBox.Checked = !StartWithWindowsCheckBox.Checked;
                }
            }
            finally
            {
                StartWithWindowsCheckBox.CheckedChanged += StartWithWindowsCheckBox_CheckedChanged;
            }
        }

        private const string RunKeyName = "WinThumbsPreloader";
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static bool SetStartup(bool startWithWindows)
        {
            using (RegistryKey runKey = Registry.CurrentUser.OpenSubKey(RunKeyPath, true))
            {
                if (runKey == null)
                {
                    WriteLine("Failed to open registry key for startup: " + RunKeyPath, LoggingFrequency.GUILogging);
                    return false;
                }

                if (startWithWindows)
                {
                    WriteLine("Setting startup", LoggingFrequency.DebugLogging);
                    try
                    {
                        string executablePath = $"\"{Application.ExecutablePath}\" -startminimized";
                        WriteLine("Executable Path: " + executablePath, LoggingFrequency.DebugLogging);
                        runKey.SetValue(RunKeyName, executablePath);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        WriteLine("Failed to set startup: " + ex.Message, LoggingFrequency.GUILogging);
                        return false;
                    }
                }
                else
                {
                    WriteLine("Removing startup", LoggingFrequency.DebugLogging);
                    try
                    {
                        runKey.DeleteValue(RunKeyName, false);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        WriteLine("Failed to remove startup: " + ex.Message, LoggingFrequency.GUILogging);
                        return false;
                    }
                }
            }
        }

        private void toggleCacheResetAlertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlertCheckBox.Checked = !AlertCheckBox.Checked;
            toggleCacheResetAlertToolStripMenuItem.Checked = AlertCheckBox.Checked;
        }

        private void AdvancedButton_Click(object sender, EventArgs e)
        {
            WriteLine("Opening advanced cache settings form", LoggingFrequency.DebugLogging);
            AdvancedCacheForm advancedCacheForm = new AdvancedCacheForm();
            this.OpenSecondaryFormCentered(advancedCacheForm);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (CloseButton.Text == "Close") Close();
            else if (CloseButton.Text == "Exit") Environment.Exit(0);
        }

        private void CacheForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                ShowInTaskbar = true;
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            Activate(); // Ensures correct focus
            notifyIcon1.Visible = false;
        }

        private void CacheForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                WriteLine("Exiting application from cache form", LoggingFrequency.GUILogging);
                Environment.Exit(0); // Exit the entire application
            }
        }

        private void CheckForStartMinimized(object sender, EventArgs e)
        {
            if (Program.AppOptions.startMinimized) { StartMinimized(null, null); }
        }

        private void StartMinimized(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            notifyIcon1.Visible = true;
        }

        private void SetCacheOutput(string message)
        {
            string output = (message ?? string.Empty)
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Trim();

            while (output.Contains("  "))
                output = output.Replace("  ", " ");

            OutputTextBox.Text = output;
        }

    }

    internal static class CacheBackupLock
    {
        private const string LockFileName = ".WinThumbsPreloader_BackupLock.lock";

        private const uint ErrorAccessDenied = 0x80070005;
        private const uint ErrorSharingViolation = 0x80070020;
        private const uint ErrorLockViolation = 0x80070021;

        private static FileStream _handle;
        private static readonly object _sync = new();

        internal enum AcquireResult
        {
            Acquired,
            AlreadyHeldByThisProcess,
            AlreadyHeldByAnotherProcess,
            PermissionDenied,
            InvalidPath,
            Failed
        }

        public static AcquireResult TryAcquire(string backupFolder, out Exception exception)
        {
            lock (_sync)
            {
                exception = null;

                if (_handle != null)
                {
                    WriteLine("Backup lock is already held by this process.", LoggingFrequency.DebugLogging);

                    return AcquireResult.AlreadyHeldByThisProcess;
                }

                if (string.IsNullOrWhiteSpace(backupFolder))
                {
                    exception = new ArgumentException("No backup folder was provided.", nameof(backupFolder));

                    return AcquireResult.InvalidPath;
                }

                try
                {
                    Directory.CreateDirectory(backupFolder);

                    string lockFilePath = Path.Combine(backupFolder, LockFileName);

                    _handle = new FileStream(
                        lockFilePath,
                        FileMode.OpenOrCreate,
                        FileAccess.ReadWrite,
                        FileShare.None,
                        bufferSize: 4096,
                        FileOptions.DeleteOnClose
                    );

                    return AcquireResult.Acquired;
                }
                catch (IOException ex) when (IsSharingViolation(ex))
                {
                    exception = ex;

                    WriteLine("Backup lock is already held by another process.", LoggingFrequency.DebugLogging);

                    return AcquireResult.AlreadyHeldByAnotherProcess;
                }
                catch (UnauthorizedAccessException ex)
                {
                    exception = ex;
                    LogAcquireFailure("Permission denied", ex);
                    return AcquireResult.PermissionDenied;
                }
                catch (SecurityException ex)
                {
                    exception = ex;
                    LogAcquireFailure("Security permission denied", ex);
                    return AcquireResult.PermissionDenied;
                }
                catch (Exception ex) when (IsAccessDenied(ex))
                {
                    exception = ex;
                    LogAcquireFailure("Access denied", ex);
                    return AcquireResult.PermissionDenied;
                }
                catch (ArgumentException ex)
                {
                    exception = ex;
                    LogAcquireFailure("Invalid backup lock path", ex);
                    return AcquireResult.InvalidPath;
                }
                catch (NotSupportedException ex)
                {
                    exception = ex;
                    LogAcquireFailure("Unsupported backup lock path", ex);
                    return AcquireResult.InvalidPath;
                }
                catch (PathTooLongException ex)
                {
                    exception = ex;
                    LogAcquireFailure("Backup lock path is too long", ex);
                    return AcquireResult.InvalidPath;
                }
                catch (DirectoryNotFoundException ex)
                {
                    exception = ex;
                    LogAcquireFailure("Backup lock directory was not found", ex);
                    return AcquireResult.InvalidPath;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    LogAcquireFailure("Backup lock acquisition failed", ex);
                    return AcquireResult.Failed;
                }
            }
        }

        private static bool IsSharingViolation(IOException exception)
        {
            uint hResult = unchecked((uint)exception.HResult);

            return hResult == ErrorSharingViolation ||
                   hResult == ErrorLockViolation;
        }

        private static bool IsAccessDenied(Exception exception)
        {
            return unchecked((uint)exception.HResult) == ErrorAccessDenied;
        }

        private static void LogAcquireFailure(string category, Exception exception)
        {
            WriteLine($"{category}: {exception.GetType().Name}: " + exception.Message, LoggingFrequency.GUILogging);
        }

        public static void Release()
        {
            lock (_sync)
            {
                try
                {
                    _handle?.Dispose();
                }
                catch (Exception ex)
                {
                    WriteLine("Failed to release backup lock: " + ex.Message, LoggingFrequency.DebugLogging);
                }
                finally
                {
                    _handle = null;
                }
            }
        }
    }
}