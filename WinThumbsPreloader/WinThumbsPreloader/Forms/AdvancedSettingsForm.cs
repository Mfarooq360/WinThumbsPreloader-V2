using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader.Forms
{
    public partial class AdvancedSettingsForm : Form
    {
        public AdvancedSettingsForm()
        {
            InitializeComponent();
            this.KeyDown += AdvancedSettingsForm_KeyDown;
            this.KeyUp += AdvancedSettingsForm_KeyUp;
            this.Activated += AdvancedSettingsForm_Activated;
            this.KeyPreview = true;
            this.Shown += AdvancedSettingsForm_Shown;
        }

        private void AdvancedSettingsForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.MainIcon;
            PreloadFolderIconsForComboBox.SelectedIndex = Settings.Default.PreloadAllFolders ? 1 : 0;
            string extensionsAutoFormatting = Settings.Default.ExtensionsAutoFormatting;
            if (extensionsAutoFormatting == "CommasAndSpaces")
            {
                extensionsAutoFormatting = "Commas and Spaces";
            }
            ExtensionsAutoFormattingComboBox.SelectedItem = extensionsAutoFormatting;
            LoadLoggerSettingsControls();
            PreloaderThumbnailSizesCheckedListBox.ItemCheck += PreloaderThumbnailSizesCheckedListBox_ItemCheck;
            LoadCheckedItemsFromSettings();
            StartLogsSizeTimer();

            if (Settings.Default.PreloaderThumbnailSizes == "96,256")
            {
                PresetsComboBox.SelectedItem = "Most Common Sizes";
            }
            else if (Settings.Default.PreloaderThumbnailSizes == "768")
            {
                PresetsComboBox.SelectedItem = "Photos App Large/Medium";
            }
            else if (Settings.Default.PreloaderThumbnailSizes == "256")
            {
                PresetsComboBox.SelectedItem = "Explorer Size/Photos App Small";
            }
            else
            {
                PresetsComboBox.SelectedItem = null;
            }

            WaitAfterPreloadingCompletionCheckBox.Checked = Settings.Default.WaitAfterPreloading;
            WaitAfterCacheBackupCheckBox.Checked = Settings.Default.WaitAfterCacheBackup;
            WaitCacheNumericUpDown.Value = Settings.Default.WaitTimeAfterCacheBackup;
            WaitPreloadNumericUpDown.Value = Settings.Default.WaitTimeAfterPreloading;
            ProgressDialogUpdateSpeedNumericUpDown.Value = Settings.Default.ProgressDialogUpdateSpeed;
            PreloaderProcessPriorityComboBox.SelectedItem = Settings.Default.PreloaderProcessPriority;
        }

        private async void AdvancedSettingsForm_Shown(object sender, EventArgs e)
        {
            _ = UpdateLogsSizeAsync();
        }

        private void AdvancedSettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                LogButton.Text = "Clear Logs";
            }
        }


        private void AdvancedSettingsForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                LogButton.Text = "Open Logs";
            }
        }


        private void AdvancedSettingsForm_Activated(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                LogButton.Text = "Clear Logs";
            }
            else
            {
                LogButton.Text = "Open Logs";
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            WriteLine("Closing Advanced Settings Form", LoggingFrequency.GUILogging);
            Close();
        }

        private void PreloadFolderIconsForComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool preloadAllFolders = PreloadFolderIconsForComboBox.SelectedIndex == 1;

            WriteLine("Pending PreloadFolderIconsForComboBox.SelectedItem: " + PreloadFolderIconsForComboBox.SelectedItem, LoggingFrequency.DebugLogging);

            if (preloadAllFolders)
            {
                WriteLine("Warning: Preloading all folders may cause freezes when adding certain system folders", LoggingFrequency.DebugLogging);

                PreloadFolderIconsForLabel.ForeColor = Color.Red;

                toolTip1.SetToolTip(
                    PreloadFolderIconsForLabel,
                    @"Selecting ""Folders Containing Set Extensions"" will ensure that
only folders that contain files that have the specified extensions
will have their icons included to be preloaded.

Selecting ""All Folders"" will add all folders to the preloader
which may cause freezes when adding certain system folders.

WARNING: Selecting ""All Folders"" may cause the preloader to freeze."
                );
            }
            else
            {
                PreloadFolderIconsForLabel.ForeColor = SystemColors.ControlText;

                toolTip1.SetToolTip(
                    PreloadFolderIconsForLabel,
                    @"Selecting ""Folders Containing Set Extensions"" will ensure that
only folders that contain files that have the specified extensions
will have their icons included to be preloaded.

Selecting ""All Folders"" will add all folders to the preloader
which may cause freezes when adding certain system folders."
                );
            }

        }

        bool automaticUnchecked = false;

        private const int GeneralTabIndex = 0;
        private const int PreloaderTabIndex = 1;
        private const int LogsTabIndex = 2;

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case GeneralTabIndex:
                    ResetGeneralTabToDefaults();
                    break;

                case PreloaderTabIndex:
                    ResetPreloaderTabToDefaults();
                    break;

                case LogsTabIndex:
                    ResetLogsTabToDefaults();
                    break;
            }
        }

        private void ResetGeneralTabToDefaults()
        {
            WriteLine("Resetting General tab settings to default", LoggingFrequency.DebugLogging);

            ExtensionsAutoFormattingComboBox.SelectedItem = "Disabled";
            PreloadFolderIconsForComboBox.SelectedIndex = 0;

            automaticUnchecked = true;

            try
            {
                for (int i = 0; i < PreloaderThumbnailSizesCheckedListBox.Items.Count; i++)
                {
                    PreloaderThumbnailSizesCheckedListBox.SetItemChecked(i, false);
                }

                SetThumbnailSizeChecked("96", true);
                SetThumbnailSizeChecked("256", true);
            }
            finally
            {
                automaticUnchecked = false;
            }

            PresetsComboBox.SelectedItem = "Most Common Sizes";

            Settings.Default.ExtensionsAutoFormatting = "Disabled";
            Settings.Default.PreloadAllFolders = false;
            Settings.Default.PreloaderThumbnailSizes = "96,256";
            Settings.Default.Save();
        }

        private void SetThumbnailSizeChecked(string size, bool isChecked)
        {
            int index = PreloaderThumbnailSizesCheckedListBox.Items.IndexOf(size);

            if (index >= 0)
                PreloaderThumbnailSizesCheckedListBox.SetItemChecked(index, isChecked);
        }

        private void ResetPreloaderTabToDefaults()
        {
            WriteLine("Resetting Preloader tab settings to default", LoggingFrequency.DebugLogging);

            WaitAfterPreloadingCompletionCheckBox.Checked = true;
            WaitPreloadNumericUpDown.Value = 10;
            WaitPreloadComboBox.SelectedItem = "Seconds";

            WaitAfterCacheBackupCheckBox.Checked = true;
            WaitCacheNumericUpDown.Value = 10;
            WaitCacheComboBox.SelectedItem = "Seconds";

            ProgressDialogUpdateSpeedNumericUpDown.Value = 250;
            PreloaderProcessPriorityComboBox.SelectedItem = "Below Normal";

            Settings.Default.WaitAfterPreloading = true;
            Settings.Default.WaitTimeAfterPreloading = 10;
            Settings.Default.WaitAfterPreloadingUnit = "Seconds";

            Settings.Default.WaitAfterCacheBackup = true;
            Settings.Default.WaitTimeAfterCacheBackup = 10;
            Settings.Default.WaitAfterCacheUnit = "Seconds";

            Settings.Default.ProgressDialogUpdateSpeed = 250;
            Settings.Default.PreloaderProcessPriority = "Below Normal";
            Settings.Default.Save();
        }

        private void ResetLogsTabToDefaults()
        {
            WriteLine("Resetting Logs tab settings to default", LoggingFrequency.DebugLogging);

            loggerSettingsLoading = true;

            try
            {
                LoggingFrequencyComboBox.SelectedIndex = (int)LoggingFrequency.NoLogging;

                AutoDeleteLogsByAgeCheckBox.Checked = false;

                SetNumericValue(LogRetentionDaysNumericUpDown, savedValue: 30, fallbackValue: 30);
            }
            finally
            {
                loggerSettingsLoading = false;
            }

            currentLoggingFrequency = LoggingFrequency.NoLogging;
            LogFrequency = LoggingFrequency.NoLogging;

            Settings.Default.LoggingFrequency = (int)LoggingFrequency.NoLogging;
            Settings.Default.AutoDeleteLogsByAge = false;
            Settings.Default.LogRetentionDays = 30;
            Settings.Default.Save();

            UpdateLoggingFrequencyAppearance();
            UpdateLoggerRetentionControlState();
            UpdateLoggerFolderStatus();
            Logger.InitializeLogger();

            _ = UpdateLogsSizeAsync();
            SetLoggerOutput("Log defaults saved.");
        }

        private async void PreloaderThumbnailSizesCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Unchecked && PreloaderThumbnailSizesCheckedListBox.CheckedItems.Count <= 1 && !automaticUnchecked)
            {
                e.NewValue = CheckState.Checked;
                return;
            }

            await Task.Yield();

            string checkedSizes = string.Join(",", PreloaderThumbnailSizesCheckedListBox.CheckedItems.Cast<string>());

            if (checkedSizes == "96,256")
            {
                PresetsComboBox.SelectedItem = "Most Common Sizes";
            }
            else if (checkedSizes == "256")
            {
                PresetsComboBox.SelectedItem = "Explorer Size/Photos App Small";
            }
            else if (checkedSizes == "768")
            {
                PresetsComboBox.SelectedItem = "Photos App Large/Medium";
            }
            else
            {
                PresetsComboBox.SelectedItem = null;
            }

            WriteLine("Pending requested thumbnail sizes: " + checkedSizes, LoggingFrequency.DebugLogging);
        }

        private void LoadCheckedItemsFromSettings()
        {
            string savedSizes = Settings.Default.PreloaderThumbnailSizes;
            if (!string.IsNullOrEmpty(savedSizes))
            {
                var sizes = savedSizes.Split([','], StringSplitOptions.RemoveEmptyEntries);
                foreach (var size in sizes)
                {
                    for (int i = 0; i < PreloaderThumbnailSizesCheckedListBox.Items.Count; i++)
                    {
                        if (PreloaderThumbnailSizesCheckedListBox.Items[i].ToString() == size.Trim())
                        {
                            PreloaderThumbnailSizesCheckedListBox.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }

        private Dictionary<string, List<string>> presets = new Dictionary<string, List<string>>
        {
            { "Most Common Sizes", new List<string> { "96", "256" } },
            { "Explorer Size/Photos App Small", new List<string> { "256" } },
            { "Photos App Large/Medium", new List<string> { "768" } },
        };

        private void PresetsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteLine("PresetsComboBox.SelectedItem: " + PresetsComboBox.SelectedItem, LoggingFrequency.DebugLogging);
            if (PresetsComboBox.SelectedItem is string selectedPreset && presets.ContainsKey(selectedPreset))
            {
                var sizesToCheck = presets[selectedPreset];

                automaticUnchecked = true;

                for (int i = 0; i < PreloaderThumbnailSizesCheckedListBox.Items.Count; i++)
                {
                    PreloaderThumbnailSizesCheckedListBox.SetItemChecked(i, false);
                }

                foreach (var size in sizesToCheck)
                {
                    int index = PreloaderThumbnailSizesCheckedListBox.Items.IndexOf(size);
                    if (index != -1)
                    {
                        PreloaderThumbnailSizesCheckedListBox.SetItemChecked(index, true);
                    }
                }

                automaticUnchecked = false;
            }
        }

        private void LoggingFrequencyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLoggingFrequencyAppearance();

            if (loggerSettingsLoading || LoggingFrequencyComboBox.SelectedIndex < 0)
            {
                return;
            }

            LoggingFrequency pendingFrequency = (LoggingFrequency)LoggingFrequencyComboBox.SelectedIndex;

            WriteLine("Pending LoggingFrequencyComboBox.SelectedItem: " + LoggingFrequencyComboBox.SelectedItem, LoggingFrequency.DebugLogging);

            if (pendingFrequency != LoggingFrequency.NoLogging && string.IsNullOrWhiteSpace(LoggerFolderPathTextBox.Text))
            {
                SetLoggerOutput("Choose a log folder, then Save.");
            }
            else
            {
                SetLoggerOutput("Frequency changed. Click Save.");
            }
        }

        private void UpdateLoggingFrequencyAppearance()
        {
            bool debugLogging = LoggingFrequencyComboBox.SelectedIndex == (int)LoggingFrequency.DebugLogging;

            LoggingFrequencyLabel.ForeColor = debugLogging ? Color.Red : SystemColors.ControlText;

            string warningText = debugLogging
                ? "WARNING: Enabling Debug Logging may cause the application to run\n" +
                  "slower, especially while preloading, and will use more storage."
                : "Enabling Debug Logging may cause the application to run\n" +
                  "slower, especially while preloading, and will use more storage.";

            toolTip1.SetToolTip(LoggingFrequencyLabel,
                @"Enables various tiers of logging for various purposes.

No Logging: Disables all logging.

Preloader Logging: Enables logging for the preloader
which includes basic logging of argument parsing and
directory scanning along with certain errors.

GUI Logging: Enables logging for the GUI forms which
includes basic logging of interactions and certain errors.

All Logging: Enables logging for both the preloader and
the GUI which includes basic logging of actions and errors.

Debug Logging: Enables logging for both the preloader
and the GUI which includes logging for almost every action,
many variables, and every error that occurs, big or small.
(More detailed logging is available in single-threaded mode)

" + warningText
            );
        }

        private DateTime clearLogsConfirmationExpiresUtc = DateTime.MinValue;

        private void SetLoggerOutput(string message)
        {
            string output = (message ?? string.Empty)
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Trim();

            OutputTextBox.Text = output;
        }

        private async void LogButton_Click(object sender, EventArgs e)
        {
            try
            {
                bool clearLogsRequested = Control.ModifierKeys == Keys.Shift;

                if (!Logger.TryGetLogDirectory(out string logFolder))
                {
                    clearLogsConfirmationExpiresUtc = DateTime.MinValue;

                    if (clearLogsRequested)
                    {
                        tabControl1.SelectedIndex = LogsTabIndex;
                        SetLoggerOutput("Select and save a log folder before clearing logs.");
                        LoggerFolderPathTextBox.Focus();
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(LoggerFolderPathTextBox.Text))
                    {
                        tabControl1.SelectedIndex = LogsTabIndex;
                        SetLoggerOutput("Select a log folder to open logs.");
                    }
                    else
                    {
                        SetLoggerOutput("Save the selected log folder before opening logs.");
                        LoggerFolderPathTextBox.Focus();
                    }

                    return;
                }

                if (clearLogsRequested)
                {
                    if (DateTime.UtcNow > clearLogsConfirmationExpiresUtc)
                    {
                        clearLogsConfirmationExpiresUtc = DateTime.UtcNow.AddSeconds(5);
                        SetLoggerOutput("Shift-click again to clear logs.");
                        return;
                    }

                    clearLogsConfirmationExpiresUtc = DateTime.MinValue;

                    bool cleared = Logger.TryClearAllLogs(out int deletedCount, out int failedCount, out string detailedMessage);

                    await UpdateLogsSizeAsync();

                    WriteLine($"Clear logs result: deleted={deletedCount}, failed={failedCount}, message={detailedMessage}", LoggingFrequency.GUILogging);

                    SetLoggerOutput(cleared ? $"Cleared {deletedCount} log(s)." : $"Cleared {deletedCount}; {failedCount} failed.");
                }
                else
                {
                    clearLogsConfirmationExpiresUtc = DateTime.MinValue;

                    Directory.CreateDirectory(logFolder);

                    WriteLine("Opening logs folder: " + logFolder, LoggingFrequency.GUILogging);

                    Process.Start(new ProcessStartInfo { FileName = logFolder, UseShellExecute = true });

                    SetLoggerOutput("Logs folder opened.");
                }
            }
            catch (Exception ex)
            {
                WriteLine("Failed to open or clear logs folder: " + ex.Message, LoggingFrequency.GUILogging);
                SetLoggerOutput("Log action failed.");
            }
        }

        private System.Windows.Forms.Timer logsSizeUpdateTimer = new System.Windows.Forms.Timer();

        private bool logsSizeUpdateInProgress;

        private string FormatSize(long bytes)
        {
            string format = "Auto";

            if (format == "Auto")
            {
                if (bytes < 1024 * 1024)
                    format = "KB";
                else if (bytes < 1024 * 1024 * 1024)
                    format = "MB";
                else
                    format = "GB";
            }

            switch (format)
            {
                case "KB":
                    double kb = bytes / 1024.0;
                    string kbFormat = kb < 100 ? "N2" : "N0";
                    return $"{kb.ToString(kbFormat)} KB";

                case "GB":
                    double gb = bytes / (1024.0 * 1024.0 * 1024.0);
                    string gbFormat = gb < 10 ? "N2" : gb < 100 ? "N1" : "N0";
                    return $"{gb.ToString(gbFormat)} GB";

                default:
                    double mb = bytes / (1024.0 * 1024.0);
                    string mbFormat = mb < 10 ? "N2" : mb < 100 ? "N1" : "N0";
                    return $"{mb.ToString(mbFormat)} MB";
            }
        }

        private async void LogsSizeUpdateTimer_Tick(object sender, EventArgs e)
        {
            await UpdateLogsSizeAsync();
        }

        private async Task UpdateLogsSizeAsync()
        {
            if (logsSizeUpdateInProgress)
                return;

            if (!Logger.TryGetLogDirectory(out _))
            {
                LogsSizeLabel.Text = "Logs Size: N/A";
                return;
            }

            logsSizeUpdateInProgress = true;

            try
            {
                long totalSizeBytes = await Task.Run(Logger.GetTotalLogsSize);

                if (IsDisposed || Disposing)
                    return;

                LogsSizeLabel.Text = "Logs Size: " + (totalSizeBytes > 0 ? FormatSize(totalSizeBytes) : "0.00 KB");
            }
            catch (Exception ex)
            {
                if (!IsDisposed && !Disposing)
                    LogsSizeLabel.Text = "Logs Size: Unavailable";

                WriteLine("Failed to calculate logs size: " + ex.Message, LoggingFrequency.DebugLogging);
            }
            finally
            {
                logsSizeUpdateInProgress = false;
            }
        }

        private void StartLogsSizeTimer()
        {
            logsSizeUpdateTimer.Interval = 1000;
            logsSizeUpdateTimer.Tick -= LogsSizeUpdateTimer_Tick;
            logsSizeUpdateTimer.Tick += LogsSizeUpdateTimer_Tick;
            logsSizeUpdateTimer.Start();
        }

        private void WaitPreloadComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUnit = WaitPreloadComboBox.SelectedItem?.ToString() ?? "Seconds";

            WaitPreloadNumericUpDown.Maximum = selectedUnit == "Hours" ? 24 : 60;

            WriteLine("Pending WaitPreloadComboBox.SelectedItem: " + selectedUnit, LoggingFrequency.DebugLogging);
        }

        private void WaitCacheComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUnit = WaitCacheComboBox.SelectedItem?.ToString() ?? "Seconds";

            WaitCacheNumericUpDown.Maximum = selectedUnit == "Hours" ? 24 : 60;

            WriteLine("Pending WaitCacheComboBox.SelectedItem: " + selectedUnit, LoggingFrequency.DebugLogging);
        }

        private void PreloaderProcessPriorityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPriority = PreloaderProcessPriorityComboBox.SelectedItem?.ToString() ?? "Below Normal";

            WriteLine("Pending PreloaderProcessPriorityComboBox.SelectedItem: " + selectedPriority, LoggingFrequency.DebugLogging);

            if (selectedPriority == "Realtime" || selectedPriority == "High")
            {
                PreloaderProcessPriorityLabel.ForeColor = Color.Red;

                toolTip1.SetToolTip(
                    PreloaderProcessPriorityLabel,
                    @"Determines the process priority used by the thumbnail preloader.

The recommended default is ""Below Normal"" for best system
responsiveness while preloading thumbnails in the background.

WARNING: Setting the process priority to ""Realtime"" or ""High"" may negatively impact system performance."
                );
            }
            else if (selectedPriority == "Idle")
            {
                PreloaderProcessPriorityLabel.ForeColor = Color.Orange;

                toolTip1.SetToolTip(
                    PreloaderProcessPriorityLabel,
                    @"Determines the process priority used by the thumbnail preloader.

The recommended default is ""Below Normal"" for best system
responsiveness while preloading thumbnails in the background.

WARNING: Setting the process priority to ""Idle"" may negatively impact preloader performance."
                );
            }
            else
            {
                PreloaderProcessPriorityLabel.ForeColor = SystemColors.ControlText;

                toolTip1.SetToolTip(
                    PreloaderProcessPriorityLabel,
                    @"Determines the process priority used by the thumbnail preloader.

The recommended default is ""Below Normal"" for best system
responsiveness while preloading thumbnails in the background."
                );
            }
        }

        private bool loggerSettingsLoading;
        private bool loggerSettingsEventsAttached;

        private void LoadLoggerSettingsControls()
        {
            AttachLoggerSettingsEvents();

            Logger.EnsureNoLoggingWithoutConfiguredPath();

            loggerSettingsLoading = true;

            try
            {
                int savedFrequency = Settings.Default.LoggingFrequency;

                if (!Enum.IsDefined(typeof(LoggingFrequency), savedFrequency))
                    savedFrequency = (int)LoggingFrequency.NoLogging;

                LoggingFrequencyComboBox.SelectedIndex = savedFrequency;

                string displayedPath = Logger.GetLogFolderPathForDisplay();

                LoggerFolderPathTextBox.Text = string.Equals(displayedPath, "N/A", StringComparison.OrdinalIgnoreCase) ? string.Empty : displayedPath;

                AutoDeleteLogsByAgeCheckBox.Checked = Settings.Default.AutoDeleteLogsByAge;

                SetNumericValue(LogRetentionDaysNumericUpDown, Settings.Default.LogRetentionDays, 30);
            }
            finally
            {
                loggerSettingsLoading = false;
            }

            UpdateLoggingFrequencyAppearance();
            UpdateLoggerRetentionControlState();
            UpdateLoggerFolderStatus();
            SetLoggerOutput(string.Empty);
        }

        private void AttachLoggerSettingsEvents()
        {
            if (loggerSettingsEventsAttached)
                return;

            BrowseLoggerFolderButton.Click += BrowseLoggerFolderButton_Click;
            DefaultLoggerFolderButton.Click += DefaultLoggerFolderButton_Click;
            ClearLoggerFolderButton.Click += ClearLoggerFolderButton_Click;

            LoggerFolderPathTextBox.TextChanged += LoggerFolderPathTextBox_TextChanged;
            AutoDeleteLogsByAgeCheckBox.CheckedChanged += AutoDeleteLogsByAgeCheckBox_CheckedChanged;

            loggerSettingsEventsAttached = true;
        }

        private static void SetNumericValue(NumericUpDown control, int savedValue, int fallbackValue)
        {
            decimal value = savedValue > 0 ? savedValue : fallbackValue;

            if (value < control.Minimum)
                value = control.Minimum;
            else if (value > control.Maximum)
                value = control.Maximum;

            control.Value = value;
        }

        private void BrowseLoggerFolderButton_Click(object sender, EventArgs e)
        {
            BrowseForPendingLoggerFolder();
        }

        private bool BrowseForPendingLoggerFolder()
        {
            using FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                UseDescriptionForTitle = true,
                Description = "Select Logger Folder",
                ShowNewFolderButton = true
            };

            string currentText = LoggerFolderPathTextBox.Text.Trim();

            if (Directory.Exists(currentText))
            {
                dialog.SelectedPath = currentText;
            }
            else
            {
                string defaultPath = Logger.GetRecommendedDefaultLogPath();
                string defaultParent = Path.GetDirectoryName(defaultPath);

                if (!string.IsNullOrWhiteSpace(defaultParent) && Directory.Exists(defaultParent))
                {
                    dialog.SelectedPath = defaultParent;
                }
            }

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                SetLoggerOutput("Folder selection cancelled.");
                return false;
            }

            SetLoggerFolderText(dialog.SelectedPath);
            SetLoggerOutput("Folder selected. Click Save.");
            return true;
        }

        private void DefaultLoggerFolderButton_Click(object sender, EventArgs e)
        {
            SetLoggerFolderText(Logger.GetRecommendedDefaultLogPath());
            SetLoggerOutput("Default folder selected. Click Save.");
        }

        private void ClearLoggerFolderButton_Click(object sender, EventArgs e)
        {
            SetLoggerFolderText(string.Empty);
            SetLoggerOutput("Path cleared. Click Save.");
        }

        private void SetLoggerFolderText(string text)
        {
            loggerSettingsLoading = true;

            try
            {
                LoggerFolderPathTextBox.Text = text ?? string.Empty;
            }
            finally
            {
                loggerSettingsLoading = false;
            }

            UpdateLoggerFolderStatus();
        }

        private void LoggerFolderPathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (loggerSettingsLoading)
                return;

            UpdateLoggerFolderStatus();
            SetLoggerOutput("Path changed. Click Save.");
        }

        private void UpdateLoggerFolderStatus()
        {
            Logger.LogFolderStatusInfo info = Logger.GetLogFolderStatusInfo(LoggerFolderPathTextBox.Text.Trim());

            LoggerFolderStatusLabel.Text = info.Message;

            LoggerFolderStatusLabel.ForeColor = info.Status switch
            {
                Logger.LogFolderStatus.NotSelected => SystemColors.GrayText,
                Logger.LogFolderStatus.Writable => Color.LimeGreen,
                Logger.LogFolderStatus.WritableAdmin => Color.DarkOrange,
                Logger.LogFolderStatus.WillBeCreated => Color.DarkGreen,
                Logger.LogFolderStatus.WillBeCreatedAdmin => Color.DarkOrange,
                Logger.LogFolderStatus.NotWritable => Color.Red,
                Logger.LogFolderStatus.InvalidPath => Color.Firebrick,
                _ => SystemColors.GrayText
            };
        }

        private void AutoDeleteLogsByAgeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loggerSettingsLoading)
                UpdateLoggerRetentionControlState();
        }

        private void UpdateLoggerRetentionControlState()
        {
            LogRetentionDaysNumericUpDown.Enabled = AutoDeleteLogsByAgeCheckBox.Checked;
            LogRetentionDaysLabel.Enabled = AutoDeleteLogsByAgeCheckBox.Checked;
        }

        private bool SaveLogsTab()
        {
            if (LoggingFrequencyComboBox.SelectedIndex < 0)
                return false;

            LoggingFrequency selectedFrequency = (LoggingFrequency)LoggingFrequencyComboBox.SelectedIndex;

            if (selectedFrequency != LoggingFrequency.NoLogging && string.IsNullOrWhiteSpace(LoggerFolderPathTextBox.Text))
            {
                SetLoggerOutput("Choose a log folder or disable logging before saving.");
                return false;
            }

            string selectedPath = LoggerFolderPathTextBox.Text.Trim();

            string normalizedPath = string.Empty;
            string pathMessage = "Logger folder cleared.";

            if (!string.IsNullOrWhiteSpace(selectedPath))
            {
                if (!Logger.TryValidateLogFolderPath(selectedPath, this, out normalizedPath, out pathMessage))
                {
                    UpdateLoggerFolderStatus();

                    WriteLine("Logger folder validation failed: " + pathMessage, LoggingFrequency.GUILogging);

                    SetLoggerOutput("Log folder is unavailable.");
                    return false;
                }
            }

            Settings.Default.LoggerFolderPath = normalizedPath;
            Settings.Default.LoggingFrequency = (int)selectedFrequency;
            Settings.Default.AutoDeleteLogsByAge = AutoDeleteLogsByAgeCheckBox.Checked;
            Settings.Default.LogRetentionDays = (int)LogRetentionDaysNumericUpDown.Value;
            Settings.Default.Save();

            currentLoggingFrequency = selectedFrequency;
            LogFrequency = selectedFrequency;

            Logger.InitializeLogger();

            Logger.LogCleanupResult cleanupResult = Logger.RunMaintenanceNow();

            SetLoggerFolderText(normalizedPath);
            UpdateLoggerRetentionControlState();
            _ = UpdateLogsSizeAsync();

            if (!string.IsNullOrWhiteSpace(Logger.LastError))
            {
                WriteLine("Logger settings saved with warning: " + Logger.LastError, LoggingFrequency.GUILogging);
                SetLoggerOutput("Saved with logger warning.");
            }
            else if (cleanupResult.TotalDeleted > 0)
            {
                SetLoggerOutput($"Saved. Deleted {cleanupResult.TotalDeleted} old log(s).");
            }
            else
            {
                SetLoggerOutput("Log settings saved.");
            }

            return true;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            logsSizeUpdateTimer?.Stop();
            logsSizeUpdateTimer?.Dispose();
            base.OnFormClosed(e);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case GeneralTabIndex:
                    SaveGeneralTab();
                    break;

                case PreloaderTabIndex:
                    SavePreloaderTab();
                    break;

                case LogsTabIndex:
                    SaveLogsTab();
                    break;

                default:
                    WriteLine("Could not save Advanced Settings because no known tab was selected.", LoggingFrequency.DebugLogging);
                    break;
            }
        }

        private void SaveGeneralTab()
        {
            string extensionsAutoFormatting = ExtensionsAutoFormattingComboBox.SelectedItem?.ToString() ?? "Disabled";

            if (extensionsAutoFormatting == "Commas and Spaces")
                extensionsAutoFormatting = "CommasAndSpaces";

            string checkedSizes = string.Join(",", PreloaderThumbnailSizesCheckedListBox.CheckedItems.Cast<string>());

            if (string.IsNullOrWhiteSpace(checkedSizes))
            {
                MessageBox.Show(this, "Select at least one requested thumbnail size.", "General Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            Settings.Default.ExtensionsAutoFormatting = extensionsAutoFormatting;
            Settings.Default.PreloadAllFolders = PreloadFolderIconsForComboBox.SelectedIndex == 1;
            Settings.Default.PreloaderThumbnailSizes = checkedSizes;
            Settings.Default.Save();

            if (Owner is SettingsForm settingsForm)
                settingsForm.UpdateExtensionsTextBoxDisplay();

            WriteLine("General tab settings saved. " + "ExtensionsAutoFormatting=" + extensionsAutoFormatting + ", PreloadAllFolders=" + Settings.Default.PreloadAllFolders + ", PreloaderThumbnailSizes=" + checkedSizes, LoggingFrequency.GUILogging);
        }

        private void SavePreloaderTab()
        {
            string waitPreloadUnit = WaitPreloadComboBox.SelectedItem?.ToString() ?? "Seconds";
            string waitCacheUnit = WaitCacheComboBox.SelectedItem?.ToString() ?? "Seconds";
            string processPriority = PreloaderProcessPriorityComboBox.SelectedItem?.ToString() ?? "Below Normal";

            Settings.Default.WaitAfterPreloading = WaitAfterPreloadingCompletionCheckBox.Checked;
            Settings.Default.WaitTimeAfterPreloading = (int)WaitPreloadNumericUpDown.Value;
            Settings.Default.WaitAfterPreloadingUnit = waitPreloadUnit;

            Settings.Default.WaitAfterCacheBackup = WaitAfterCacheBackupCheckBox.Checked;
            Settings.Default.WaitTimeAfterCacheBackup = (int)WaitCacheNumericUpDown.Value;
            Settings.Default.WaitAfterCacheUnit = waitCacheUnit;

            Settings.Default.ProgressDialogUpdateSpeed = (int)ProgressDialogUpdateSpeedNumericUpDown.Value;
            Settings.Default.PreloaderProcessPriority = processPriority;

            Settings.Default.Save();

            WriteLine(
                "Preloader tab settings saved. " +
                "WaitAfterPreloading=" +
                Settings.Default.WaitAfterPreloading +
                ", WaitTimeAfterPreloading=" +
                Settings.Default.WaitTimeAfterPreloading +
                " " + waitPreloadUnit +
                ", WaitAfterCacheBackup=" +
                Settings.Default.WaitAfterCacheBackup +
                ", WaitTimeAfterCacheBackup=" +
                Settings.Default.WaitTimeAfterCacheBackup +
                " " + waitCacheUnit +
                ", ProgressDialogUpdateSpeed=" +
                Settings.Default.ProgressDialogUpdateSpeed +
                ", PreloaderProcessPriority=" +
                processPriority,
                LoggingFrequency.GUILogging
            );
        }
    }
}