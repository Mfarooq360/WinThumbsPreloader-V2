using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinThumbsPreloader.Forms;
using WinThumbsPreloader.Properties;

namespace WinThumbsPreloader
{
    public partial class CacheForm : Form
    {
        /*TODO: Add Tooltips, add error correction,
         * add list of folders to preload as batch, add saving of list of folders
         * fix auto backup and auto restore (make auto restore occur when auto backing up after preloading)
         * allow interval change, add duration preloaded, add speed preloaded, add statistics form in settings
         * prevent thumbs cache backup from occurring when explorer cache size is being updated, generate UWP photos app thumbnails
         * fix scheduleform and everything portable version related, fix the admin perms
         * add advanced preloader button to add list of paths to preload, watch out for admin manifest requirements 
         * add setting to toggle whether preload button automatically starts a folder selector or if it opens the advanced preload form
         * add a setting to minimize the application to tray no matter what form it's on and whether to open cacheform when its reopened
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
            SetIntervals();
            UpdateCacheSizeTimer();
            AutoBackupThumbsCache();
            AutoRestoreThumbsCache();
        }

        private async void CacheForm_Shown(object sender, EventArgs e)
        {
            await AboutForm.CheckForCacheReset();
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

            if (Settings.Default.AutoBackupInterval == 0)
            {
                Settings.Default.AutoBackupInterval = 5000;
                settingsUpdated = true;
            }
            autoBackupInterval = Settings.Default.AutoBackupInterval;

            if (Settings.Default.AutoRestoreInterval == 0)
            {
                Settings.Default.AutoRestoreInterval = 5000;
                settingsUpdated = true;
            }
            autoRestoreInterval = Settings.Default.AutoRestoreInterval;

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
            autoRestoreTimer.Interval = autoBackupInterval;
            autoRestoreTimer.Tick += AutoRestoreThumbsCache_Tick;
            autoRestoreTimer.Start();
        }

        public void UpdateCacheSizeUpdateInterval(int newInterval)
        {
            cacheUpdateInterval = newInterval;
            if (updateTimer != null)
            {
                updateTimer.Interval = newInterval;
            }
        }

        public void UpdateAutoBackupInterval(int newInterval)
        {
            autoBackupInterval = newInterval;
            if (autoBackupTimer != null)
            {
                autoBackupTimer.Interval = newInterval;
            }
        }

        public void UpdateAutoRestoreInterval(int newInterval)
        {
            autoRestoreInterval = newInterval;
            if (autoRestoreTimer != null)
            {
                autoRestoreTimer.Interval = newInterval;
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateCacheSizeLabels();
        }

        private async void AutoBackupThumbsCache_Tick(object sender, EventArgs e)
        {
            if (AutoBackupCheckBox.Checked == true && ExplorerCacheSize() > BackupCacheSize() && ExplorerCacheSize() == ExplorerCacheSize())
            {
                await BackupThumbsCache(null);
            }
        }

        private async void AutoRestoreThumbsCache_Tick(object sender, EventArgs e)
        {
            if (AutoRestoreCheckBox.Checked == true && BackupCacheSize() > ExplorerCacheSize())
            {
                await RestoreThumbsCache(progressBarRestore, false);
            }
        }
        public string format = Properties.Settings.Default.CacheSizeFormat;

        public void UpdateCacheSizeLabels()
        {
            long explorerSize = ExplorerCacheSize();
            long backupSize = BackupCacheSize();

            CacheSizeLabel.Text = $"Cache Size: {FormatSize(explorerSize)}";
            BackupSizeLabel.Text = $"Backup Size: {FormatSize(backupSize)}";
        }

        private string FormatSize(long bytes)
        {
            string individualFormat = Properties.Settings.Default.CacheSizeFormat;
            // Choose the format based on the size if format is set to "auto"
            if (format == "Auto")
            {
                if (bytes < 1024) // Less than 1 KB
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
                    return $"{bytes / 1024.0:N0} KB";

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

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (CloseButton.Text == "Close") Close();
            else if (CloseButton.Text == "Exit") Environment.Exit(0);
        }

        private void CacheForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
        }

        private void CacheForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
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
            this.Hide();
            notifyIcon1.Visible = true;
        }

        static string explorerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\Explorer");
        static string backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Explorer Backup");

        static long ExplorerCacheSize()
        {
            long explorerCacheSize = 0;
            try
            {
                string[] sourceFiles = Directory.GetFiles(explorerPath, "*.db");
                explorerCacheSize = sourceFiles.Sum(file => new FileInfo(file).Length);
            }
            catch { }
            return explorerCacheSize;
        }

        static long BackupCacheSize()
        {
            long backupCacheSize = 0;

            if (Directory.Exists(backupPath))
            {
                try
                {
                    string[] destinationFiles = Directory.GetFiles(backupPath, "*.db");
                    backupCacheSize = destinationFiles.Sum(file => new FileInfo(file).Length);
                }
                catch { }
            }
            return backupCacheSize;
        }

        public static bool CompareThumbsCacheSize()
        {
            long backupCacheSize = BackupCacheSize();
            long explorerCacheSize = ExplorerCacheSize();

            bool isCacheSizeLargerThanBackup = false;
            if (explorerCacheSize > backupCacheSize) { isCacheSizeLargerThanBackup = true; }
            else if (explorerCacheSize <= backupCacheSize) { isCacheSizeLargerThanBackup = false; }
            return isCacheSizeLargerThanBackup;
        }

        public static async Task BackupThumbsCache(ProgressBar progressBar)
        {
            await Task.Run(() =>
            {
                string destinationPath = backupPath;
                long explorerSize = ExplorerCacheSize();
                try
                {
                    Directory.CreateDirectory(destinationPath);
                }
                catch
                {
                    CacheForm cacheForm = new CacheForm();
                    cacheForm.OutputTextBox.Text = "Failed to create backup path.";
                    return;
                }
                progressBar?.Invoke((MethodInvoker)(() =>
                {
                    progressBar.Visible = true;
                    progressBar.Value = 0;
                }));

                foreach (string file in Directory.GetFiles(explorerPath, "*.db"))
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(backupPath, fileName);
                    try { File.Copy(file, destFile, true); }
                    catch { }

                    // Update progress
                    long currentSize = BackupCacheSize();
                    int progress = (int)((currentSize * 100) / explorerSize);
                    progressBar?.Invoke((MethodInvoker)(() =>
                    {
                        progressBar.Value = progress > 100 ? 100 : progress; // Ensure the value is within valid range
                        if (progressBar.Value == 100) { progressBar.Value--; progressBar.Value++; } // This forces the progress bar's level to instantly hit the max instead of moving towards 100% which would otherwise cause it to disappear before it actually reaches the end
                    }));
                }
                if (BackupCacheSize() == explorerSize) { Thread.Sleep(50); } // This helps to ensure the visual progress bar actually reaches the end by waiting for a certain time to keep the progress bar displayed before getting rid of it

                // Final update to ensure progress bar reaches 100% at the end
                progressBar?.Invoke((MethodInvoker)(() =>
                {
                    progressBar.Value = 100;
                    progressBar.Visible = false;
                }));
            });
        }

        public static async Task RestoreThumbsCache(ProgressBar progressBar, bool forceRestore)
        {
            await Task.Run(() =>
            {
                if (forceRestore) CloseExplorer();

                if (Properties.Settings.Default.ExplorerCacheDeletionMethod == "Disk Cleanup")
                {
                    CacheForm cacheForm = new CacheForm();
                    cacheForm.RunThumbnailDiskCleanup();
                }
                else if (Properties.Settings.Default.ExplorerCacheDeletionMethod == "Manual Deletion")
                {
                    DeleteExplorerThumbsCacheManually();
                }

                long backupSize = BackupCacheSize();
                int retryCount = 0;

                progressBar?.Invoke((MethodInvoker)(() =>
                {
                    progressBar.Visible = true;
                    progressBar.Value = 0;
                }));
                do
                {
                    // Copy .db files to the destination path
                    foreach (string file in Directory.GetFiles(backupPath, "*.db"))
                    {
                        string fileName = Path.GetFileName(file);
                        string sourceFile = Path.Combine(explorerPath, fileName);
                        if (forceRestore) CloseExplorer();
                        try { File.Copy(file, sourceFile, true); }
                        catch { }

                        // Update progress
                        long currentSize = ExplorerCacheSize();
                        int progress = (int)((currentSize * 100) / backupSize);
                        progressBar?.Invoke((MethodInvoker)(() =>
                        {
                            progressBar.Value = progress > 100 ? 100 : progress; // Ensure the value is within valid range
                            if (progressBar.Value == 100) { progressBar.Value--; progressBar.Value++; } // This forces the progress bar's level to instantly hit the max instead of moving towards 100% which would otherwise cause it to disappear before it actually reaches the end
                        }));
                    }
                    if (ExplorerCacheSize() == backupSize) { Thread.Sleep(50); } // This helps to ensure the visual progress bar actually reaches the end by waiting for a certain time to keep the progress bar displayed before getting rid of it

                    if (ExplorerCacheSize() < backupSize)
                    {
                        retryCount++; // Increment retry count
                        if (retryCount >= 3)
                        {
                            break; // Exit the loop after 3 retries
                        }
                    }

                } while (ExplorerCacheSize() < backupSize && retryCount < 3);

                // Final update to ensure progress bar reaches 100% at the end
                progressBar?.Invoke((MethodInvoker)(() =>
                {
                    progressBar.Value = 100;
                    progressBar.Visible = false;
                }));
                if (forceRestore) RestartExplorer();
            });
        }

        private static void CloseExplorer()
        {
            foreach (var process in Process.GetProcessesByName("explorer"))
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                }
                catch { } // Do Nothing
            }
        }

        private static void RestartExplorer()
        {
            try
            {
                Process.Start("explorer.exe");
            }
            catch { } // Do Nothing
        }

        public static async void DeleteExplorerThumbsCacheManually()
        {
            await Task.Run(() =>
            {
                // Delete existing .db files in the explorer directory
                foreach (string file in Directory.GetFiles(explorerPath, "*.db"))
                {
                    try { File.Delete(file); }
                    catch { } // Do Nothing
                }
            });
        }

        public const int sagesetNumber = 100;
        private const string ThumbnailCacheKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches\Thumbnail Cache";

        public void ConfigureDiskCleanupSageset()
        {
            try
            {
                using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(ThumbnailCacheKey, writable: true))
                {
                    if (regKey != null)
                    {
                        regKey.SetValue($"StateFlags{sagesetNumber:D4}", 2, RegistryValueKind.DWord);
                    }
                    else
                    {
                        throw new Exception("Failed to open registry key.");
                    }
                }
            }
            catch (Exception)
            {
                OutputTextBox.Text = "Failed to configure disk cleanup. Retry as Admin.";
            }
        }

        private void RunThumbnailDiskCleanup()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cleanmgr.exe",
                Arguments = $"/sagerun:{sagesetNumber}",
                UseShellExecute = false
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                process.WaitForExit(); // Wait for the clean-up process to complete
            }
        }

        private async void BackupButton_Click(object sender, EventArgs e)
        {
            string destinationPath = backupPath;

            // Ensure the destination directory exists
            Directory.CreateDirectory(destinationPath);

            // If the size of the source files is less than the size of the destination files, prompt for confirmation
            if (ExplorerCacheSize() < BackupCacheSize())
            {
                DialogResult dialogResult = MessageBox.Show("The current thumbcache size is smaller than the current stored backup. Are you sure you want to backup?", "Confirm Backup", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    OutputTextBox.Text = "Backup cancelled.";
                    return;
                }
                else if (dialogResult == DialogResult.Yes)
                {
                    OutputTextBox.Text = "Backing up thumbnail cache...";
                    await BackupThumbsCache(progressBarRestore);
                    OutputTextBox.Text = "Thumbnail cache backup complete.";
                }
            }
            else if (ExplorerCacheSize() == BackupCacheSize())
            {
                DialogResult dialogResult = MessageBox.Show("The current thumbcache size is equal to the current stored backup. Are you sure you want to backup?", "Confirm Backup", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    OutputTextBox.Text = "Backup cancelled.";
                    return;
                }
                else if (dialogResult == DialogResult.Yes)
                {
                    OutputTextBox.Text = "Backing up thumbnail cache...";
                    await BackupThumbsCache(progressBarRestore);
                    OutputTextBox.Text = "Thumbnail cache backup complete.";
                }
            }
            else
            {
                OutputTextBox.Text = "Backing up thumbnail cache...";
                await BackupThumbsCache(progressBarRestore);
                OutputTextBox.Text = "Thumbnail cache backup complete.";
            }
        }

        private void OutputTextBox_Initialize(object sender, EventArgs e)
        {
            if (BackupCacheSize() == 0) { OutputTextBox.Text = "Backup doesn't exist or is empty."; }
            if (ExplorerCacheSize() < BackupCacheSize() && Settings.Default.ResetRecognized == false) { OutputTextBox.Text = "Explorer cache has been reset."; Settings.Default.ResetRecognized = true; }
            if (ExplorerCacheSize() < BackupCacheSize() && Settings.Default.ResetRecognized == true) { if (OutputTextBox.Text != "Explorer cache cleared.") { OutputTextBox.Text = "Backup cache larger than explorer cache."; } }
            if (ExplorerCacheSize() >= BackupCacheSize() && Settings.Default.ResetRecognized == true) { Settings.Default.ResetRecognized = false; }
        }

        private async void RestoreButton_Click(object sender, EventArgs e)
        {
            // Ensure the source directory exists
            if (!Directory.Exists(backupPath))
            {
                OutputTextBox.Text = "Backup not found.";
                return;
            }
            if (!Directory.Exists(explorerPath))
            {
                OutputTextBox.Text = "Cache folder not found.";
                return;
            }
            if (RestoreButton.Text == "Restore")
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to restore from backup?", "Confirm Restore", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    OutputTextBox.Text = "Restore cancelled.";
                    return;
                }
                await RestoreThumbsCache(progressBarRestore, false);
                if (BackupCacheSize() == ExplorerCacheSize()) { OutputTextBox.Text = "Thumbnail cache restored."; }
                else { OutputTextBox.Text = "Restore failed. Retry as admin or force restore."; }
            }
            else if (RestoreButton.Text == "Force Restore")
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to force restore from backup? This will close Explorer.", "Confirm Force Restore", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    OutputTextBox.Text = "Force restore cancelled.";
                    return;
                }
                await RestoreThumbsCache(progressBarRestore, true);
                if (BackupCacheSize() <= ExplorerCacheSize()) { OutputTextBox.Text = "Thumbnail cache restored forcefully."; }
                else { OutputTextBox.Text = "Force restore failed. Retry as admin."; }
            }
        }

        private void AutoBackupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AutoBackupThumbs = AutoBackupCheckBox.Checked;
            Settings.Default.Save();
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

        private void OpenBackupFolder(object sender, EventArgs e)
        {
            // Check if the Explorer backup folder is open and maximize it if it is
            var processes = Process.GetProcessesByName("explorer");

            bool backupFolderWindowFound = false;

            IntPtr foregroundWindow = GetForegroundWindow();

            foreach (var process in processes)
            {
                if (process.MainWindowTitle == "Explorer Backup")
                {
                    if (process.MainWindowHandle != foregroundWindow)
                    {
                        if (process.MainWindowHandle != foregroundWindow || IsIconic(process.MainWindowHandle))
                        {
                            // Maximize the window
                            ShowWindowAsync(process.MainWindowHandle, SW_MAXIMIZE);

                            // Bring it to the front
                            SetForegroundWindow(process.MainWindowHandle);
                        }
                    }
                    backupFolderWindowFound = true;
                    break;
                }
            }

            // If not found, start a new instance
            if (!backupFolderWindowFound)
            {
                // Check if the backup directory exists, if not, create it
                if (!Directory.Exists(backupPath))
                {
                    try
                    {
                        Directory.CreateDirectory(backupPath);
                    }
                    catch (Exception)
                    {
                        OutputTextBox.Text = "Failed to create backup folder.";
                        return;  // Exit the method if folder creation failed
                    }
                }

                // Open the backup directory
                Process.Start(new ProcessStartInfo
                {
                    FileName = backupPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }

        private void OpenCacheFolder(object sender, EventArgs e)
        {
            // Check if the Explorer backup folder is open and maximize it if it is
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
                            // Maximize the window
                            ShowWindowAsync(process.MainWindowHandle, SW_MAXIMIZE);

                            // Bring it to the front
                            SetForegroundWindow(process.MainWindowHandle);
                        }
                    }
                    cacheFolderWindowFound = true;
                    break;
                }
            }

            // If not found, start a new instance
            if (!cacheFolderWindowFound)
            {
                // Check if the cache directory exists
                if (!Directory.Exists(explorerPath))
                {
                    OutputTextBox.Text = "Cache folder not found";
                    return;  // Exit the method if folder doesn't exist
                }

                // Open the cache directory
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = explorerPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
                catch (Exception)
                {
                    OutputTextBox.Text = "Failed to open cache folder";
                }
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            // Ensure the backup directory exists
            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }

            if (ClearButton.Text == "Open Backup")
            {
                // Open the backup directory
                OpenBackupFolder(null, null);

                OutputTextBox.Text = "Backup directory opened.";
            }
            else if (ClearButton.Text == "Clear Backup")
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the backup?", "Confirm Clear", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    OutputTextBox.Text = "Backup clear cancelled.";
                    return;
                }

                // Delete all .db files in the backup directory
                foreach (string file in Directory.GetFiles(backupPath, "*.db"))
                {
                    try { File.Delete(file); }
                    catch { }
                }
                OutputTextBox.Text = "Backup cache cleared.";
            }
        }

        private void ClearCacheButton_Click(object sender, EventArgs e)
        {
            if (ClearCacheButton.Text == "Open Cache")
            {
                // Open the Explorer cache directory
                OpenCacheFolder(null, null);

                OutputTextBox.Text = "Explorer cache directory opened.";
            }
            else if (ClearCacheButton.Text == "Clear Cache")
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the explorer cache?", "Confirm Clear", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    OutputTextBox.Text = "Cache clear cancelled.";
                    return;
                }

                if (Properties.Settings.Default.ExplorerCacheDeletionMethod == "Disk Cleanup")
                {
                    RunThumbnailDiskCleanup();
                }
                else if (Properties.Settings.Default.ExplorerCacheDeletionMethod == "Manual Deletion")
                {
                    DeleteExplorerThumbsCacheManually();
                }

                OutputTextBox.Text = "Explorer cache cleared.";
            } 
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
            Environment.Exit(0);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1_DoubleClick(this, EventArgs.Empty);
        }

        private void StartWithWindowsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.StartWithWindows = StartWithWindowsCheckBox.Checked;
            Settings.Default.Save();
            SetStartup(Settings.Default.StartWithWindows);
        }
        private const string RunKeyName = "WinThumbsPreloader";
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static void SetStartup(bool startWithWindows)
        {
            using (RegistryKey runKey = Registry.CurrentUser.OpenSubKey(RunKeyPath, true))
            {
                if (startWithWindows)
                {
                    string executablePath = $"\"{Application.ExecutablePath}\" -startminimized";
                    runKey.SetValue(RunKeyName, executablePath);
                }
                else
                {
                    runKey.DeleteValue(RunKeyName, false);
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
            AdvancedCacheForm advancedCacheForm = new AdvancedCacheForm();
            this.OpenSecondaryFormCentered(advancedCacheForm);
        }
    }
}
