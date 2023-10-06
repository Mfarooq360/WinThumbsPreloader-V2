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
using WinThumbsPreloader.Properties;

namespace WinThumbsPreloader
{
    [SupportedOSPlatform("windows")]
    public partial class CacheForm : Form
    {
        /*TODO: Add Tooltips, update backup tooltip, add error correction, 
         * fix auto backup and auto restore (make auto restore occur when auto backing up after preloading)
         * allow interval change
         * create new exe for cacheform
         * prevent thumbs cache backup from occurring when explorer cache size is being updated
         */
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
            UpdateCacheSizeTimer();
            AutoBackupAndRestoreThumbsCache();
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

        private System.Windows.Forms.Timer updateTimer = new System.Windows.Forms.Timer();

        private void UpdateCacheSizeTimer()
        {
            updateTimer.Interval = 250; // 250 milliseconds
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private System.Windows.Forms.Timer autoTimer = new System.Windows.Forms.Timer();

        private void AutoBackupAndRestoreThumbsCache()
        {
            autoTimer.Interval = 5000; // 5 second
            autoTimer.Tick += AutoBackupAndRestoreThumbsCache_Tick;
            autoTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateCacheSizeLabels();
        }

        private async void AutoBackupAndRestoreThumbsCache_Tick(object sender, EventArgs e)
        {
            if (AutoRestoreCheckBox.Checked == true && BackupCacheSize() > ExplorerCacheSize())
            {
                await RestoreThumbsCache();
            }
            if (AutoBackupCheckBox.Checked == true && ExplorerCacheSize() > BackupCacheSize() && ExplorerCacheSize() == ExplorerCacheSize())
            {
                await BackupThumbsCache();
            }
        }

        private void UpdateCacheSizeLabels()
        {
            // Get the sizes in bytes
            long explorerSize = ExplorerCacheSize();
            long backupSize = BackupCacheSize();

            // Convert bytes to megabytes
            double explorerSizeInMB = explorerSize / (1024.0 * 1024.0);
            double backupSizeInMB = backupSize / (1024.0 * 1024.0);

            // Update the labels
            CacheSizeLabel.Text = $"Cache Size: {explorerSizeInMB:N0} MB";
            BackupSizeLabel.Text = $"Backup Size: {backupSizeInMB:N0} MB";
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
            if (Program.AppOptions.startMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }

        static string explorerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\Explorer");
        static string backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Explorer Backup"); //Add code to handle backup folder not existing

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
            //long backupCacheSize = backupFiles.Sum(file => new FileInfo(file).Length);
            //long explorerCacheSize = explorerFiles.Sum(file => new FileInfo(file).Length);
            long backupCacheSize = BackupCacheSize();
            long explorerCacheSize = ExplorerCacheSize();

            bool isCacheSizeLargerThanBackup = false;
            if (explorerCacheSize >= backupCacheSize) { isCacheSizeLargerThanBackup = true; }
            else if (explorerCacheSize < backupCacheSize) { isCacheSizeLargerThanBackup = false; }
            return isCacheSizeLargerThanBackup;
        }

        public static async Task BackupThumbsCache()
        {
            await Task.Run(() =>
            {
                string destinationPath = backupPath;
                Directory.CreateDirectory(destinationPath);

                foreach (string file in Directory.GetFiles(explorerPath, "*.db"))
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(backupPath, fileName);
                    try { File.Copy(file, destFile, true); }
                    catch
                    {
                        CacheForm cacheForm = new CacheForm();
                        cacheForm.OutputTextBox.Text = "Failed to backup cache";
                    }
                }
            });
        }

        public static async Task RestoreThumbsCache()
        {
            await Task.Run(() =>
            {
                DeleteExplorerThumbsCache();

                do
                {
                    // Copy .db files to the destination path
                    foreach (string file in Directory.GetFiles(backupPath, "*.db"))
                    {
                        string fileName = Path.GetFileName(file);
                        string sourceFile = Path.Combine(explorerPath, fileName);
                        try { File.Copy(file, sourceFile, true); }
                        catch
                        {
                            CacheForm cacheForm = new CacheForm();
                            cacheForm.OutputTextBox.Text = "Failed to restore cache, try again.";
                        }
                    }
                }
                while (ExplorerCacheSize() < BackupCacheSize());
            });
        }

        public static async void DeleteExplorerThumbsCache()
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
            }
            OutputTextBox.Text = "Backing up thumbnail cache...";
            await BackupThumbsCache();
            OutputTextBox.Text = "Thumbnail cache backup complete.";
        }

        private void OutputTextBox_Initialize(object sender, EventArgs e)
        {
            if (BackupCacheSize() == 0) { OutputTextBox.Text = "Backup doesn't exist or is empty."; }
            if (ExplorerCacheSize() < BackupCacheSize() && Settings.Default.ResetRecognized == false) { OutputTextBox.Text = "Explorer cache has been reset."; Settings.Default.ResetRecognized = true; }
            if (ExplorerCacheSize() < BackupCacheSize() && Settings.Default.ResetRecognized == true) { if (OutputTextBox.Text != "Explorer cache cleared.") { OutputTextBox.Text = "Backup cache larger than explorer cache."; } }
            if (ExplorerCacheSize() >= BackupCacheSize() && Settings.Default.ResetRecognized == true) { Settings.Default.ResetRecognized = false; }
        }

        private void OutputTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private async void RestoreButton_Click(object sender, EventArgs e)
        {
            // Ensure the source directory exists
            if (!Directory.Exists(explorerPath))
            {
                OutputTextBox.Text = "Backup not found.";
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to restore from backup?", "Confirm Restore", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                OutputTextBox.Text = "Restore cancelled.";
                return;
            }
            await RestoreThumbsCache();
            OutputTextBox.Text = "Thumbnail cache restored.";
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
                // Open the backup directory
                Process.Start(new ProcessStartInfo
                {
                    FileName = explorerPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
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
                DeleteExplorerThumbsCache();

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
            }
            else
            {
                // Shift key is not pressed, so set the button text to its original value
                ClearButton.Text = "Open Backup";
                ClearCacheButton.Text = "Open Cache";
                CloseButton.Text = "Close";
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
                    runKey.SetValue(RunKeyName, Application.ExecutablePath + " -startminimized");
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
    }
}
