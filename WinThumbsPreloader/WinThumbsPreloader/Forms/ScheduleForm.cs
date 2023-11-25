using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using WinThumbsPreloader.Forms;
using WinThumbsPreloader.Properties;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;
using Task = System.Threading.Tasks.Task;
using TaskScheduler = Microsoft.Win32.TaskScheduler.Task;
using Trigger = Microsoft.Win32.TaskScheduler.Trigger;
using TriggerCollection = Microsoft.Win32.TaskScheduler.TriggerCollection;

namespace WinThumbsPreloader
{
    [SupportedOSPlatform("windows")]
    public partial class ScheduleForm : Form //TODO: add a statistics form to show the speed of the preloader along with other things
    {
        private const string TaskName = "WinThumbsPreloaderTask";

        public ScheduleForm()
        {
            InitializeComponent();
            this.KeyDown += ScheduleForm_KeyDown;
            this.KeyUp += ScheduleForm_KeyUp;
            this.Activated += ScheduleForm_Activated;
            this.KeyPreview = true;
            this.FormClosing += ScheduleForm_FormClosing;
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            bool enableDrivePaths = Settings.Default.DriveSelectionEnabled;
            this.Icon = Resources.MainIcon;
            ToggleDriveVisibility(enableDrivePaths);
            ChangeDriveLetterSortingOrder();
            Set24HourMode();
            SetDetailedTimeMode();
            LoadTaskDetails();
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
        private void ScheduleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                Environment.Exit(0); // Exit the entire application
            }
        }

        private void ScheduleForm_Activated(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                // Shift key is currently pressed, so change the button text
                CloseButton.Text = "Exit";
                ScheduleSaveButton.Text = "Delete Task";
            }
            else
            {
                // Shift key is not pressed, so set the button text to its original value
                CloseButton.Text = "Close";
                ScheduleSaveButton.Text = "Save Task";
            }
        }

        private void ScheduleForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text when Shift is pressed
                CloseButton.Text = "Exit";
                ScheduleSaveButton.Text = "Delete Task";
            }
        }

        private void ScheduleForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text back when Shift is released
                CloseButton.Text = "Close";
                ScheduleSaveButton.Text = "Save Task";
            }
        }

        private void MultithreadedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Toggles the -m argument in the scheduler command
            ThreadsNumericUpDown.Enabled = MultithreadedCheckBox.Checked;
        }

        private void ThreadsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //Counts how many threads to use for the -m argument such as "-m:4" or "-m4"
        }

        private void RunEveryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Drop Down menu which inputs whether to run every day, week, or month in the task scheduler
        }

        private void OnIdleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //Toggles whether to run on idle in the task scheduler
        }

        private void ScheduleSaveButton_Click(object sender, EventArgs e)
        {
            if (ScheduleSaveButton.Text == "Save Task")
            {
                SaveTask();
            }
            else if (ScheduleSaveButton.Text == "Delete Task")
            {
                DeleteTask();
            }
        }

        private void DrivesCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //A box of all 26 Drive letters to pass as paths to the program if they're checked
            this.DrivesCheckedListBox.CheckOnClick = true;
        }

        private void EnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            using (TaskService ts = new TaskService())
            {
                TaskScheduler task = ts.FindTask(TaskName);
                if (task != null)
                {
                    task.Enabled = EnabledCheckBox.Checked;
                }
            }
        }

        private void TimeDigitsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Sets time ranges from 1:00 to 12:00
        }

        private void TimeOfDayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Sets whether the time is AM or PM
        }

        private void OutputTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void PathsTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void OutputTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void AdvancedButton_Click(object sender, EventArgs e)
        {
            AdvancedScheduleForm advancedScheduleForm = new AdvancedScheduleForm();
            this.OpenSecondaryFormCentered(advancedScheduleForm);
        }

        private void PathsLabel_Click(object sender, EventArgs e)
        {

        }

        private void OutputLabel_Click(object sender, EventArgs e)
        {

        }

        private void DrivesLabel_Click(object sender, EventArgs e)
        {

        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_MAXIMIZE = 3;

        public void OpenTaskSchedulerButton_Click(object sender, EventArgs e)
        {
            // Check if taskschd.msc is already running
            var processes = System.Diagnostics.Process.GetProcessesByName("mmc");

            bool taskSchedulerFound = false;

            IntPtr foregroundWindow = GetForegroundWindow();

            foreach (var process in processes)
            {
                if (process.MainWindowTitle.Contains("Task Scheduler"))
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
                    taskSchedulerFound = true;
                    break;
                }
            }

            // If not found, start a new instance
            if (!taskSchedulerFound)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "taskschd.msc",
                    UseShellExecute = true
                });
            }
        }

        private void OutputTextBox_Initialize(TaskScheduler task)
        {
            if (task != null)
            {
                DateTime lastRunTime = task.LastRunTime;
                DateTime nextRunTime = task.NextRunTime;

                if (OutputTextBox.Text.Length == 0)
                {
                    if (task.LastTaskResult != 0x41303)
                    {
                        OutputTextBox.Text = $"Last run time: {lastRunTime}";
                    }
                    else if (!EnabledCheckBox.Checked && task.LastTaskResult == 0x41303)
                    {
                        OutputTextBox.Text = "The task has not been enabled yet.";
                    }
                    else
                    {
                        OutputTextBox.Text = "The task has not run yet.";
                    }
                }
                OutputTextBox1.Text = $"Next run time: {nextRunTime}";
            }
            else
            {
                OutputTextBox.Text = "Task not found.";
            }
        }

        private async void LoadTaskDetails()
        {
            await Task.Run(() =>
            {
                bool isIdleTrigger = false;
                bool isEnabled = false;
                string arguments = string.Empty;
                string triggerType = ""; // For storing the type of trigger

                using (TaskService ts = new TaskService())
                {
                    TaskScheduler task = ts.GetTask(TaskName);
                    if (task != null)
                    {
                        isIdleTrigger = task.Definition.Triggers.Any(t => t.TriggerType == TaskTriggerType.Idle);
                        isEnabled = task.Enabled;
                        arguments = (task.Definition.Actions[0] as ExecAction)?.Arguments;

                        // Process the triggers here
                        var triggers = task.Definition.Triggers;
                        if (triggers.OfType<DailyTrigger>().Any(t => t.Repetition != null && t.Repetition.Interval.TotalHours == 1))
                        {
                            triggerType = "Hour";
                        }
                        else if (triggers.OfType<DailyTrigger>().Any())
                        {
                            triggerType = "Day";
                        }
                        else if (triggers.OfType<WeeklyTrigger>().Any())
                        {
                            triggerType = "Week";
                        }
                        else if (triggers.OfType<MonthlyTrigger>().Any())
                        {
                            triggerType = "Month";
                        }

                        // Marshal the updates to the UI thread
                        this.Invoke((MethodInvoker)delegate
                        {
                            OnIdleCheckBox.Checked = isIdleTrigger;
                            EnabledCheckBox.Checked = isEnabled;
                            UpdateUIFromArguments(arguments);
                            UpdateRunEveryComboBox(triggerType);
                            OutputTextBox_Initialize(task);
                        });
                        InitializeTimeValues(triggers);

                    }
                }
            });
        }

        private void UpdateUIFromArguments(string arguments)
        {
            if (arguments != null)
            {
                // Extract multithreading argument
                MultithreadedCheckBox.Checked = arguments.Contains("-m");
                var match = Regex.Match(arguments, @"-m:(\d+)");
                if (match.Success)
                {
                    ThreadsNumericUpDown.Value = int.Parse(match.Groups[1].Value);
                }
                else if (arguments.Contains("-m"))
                {
                    ThreadsNumericUpDown.Value = 0;  // -m without a specific thread count
                }

                // Extract and update other arguments-related UI elements...
                var rawDrivePaths = Regex.Split(arguments, @"(?=(\w:\\)|(\\\\[\w-]+\\?))")
                                         .Where(path => !string.IsNullOrWhiteSpace(path))
                                         .ToList();

                var pathsSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var rawPath in rawDrivePaths)
                {
                    var cleanedPath = rawPath.Trim().Replace("\n", string.Empty);
                    if (cleanedPath.Length == 3) // e.g., "C:\\"
                    {
                        if (DrivesCheckedListBox.Visible)
                        {
                            DrivesCheckedListBox.SetItemChecked(DrivesCheckedListBox.Items.IndexOf(cleanedPath[0].ToString()), true);
                        }
                        else
                        {
                            pathsSet.Add(cleanedPath);
                        }
                    }
                    else if (cleanedPath.Length > 3 && !cleanedPath.StartsWith("-"))
                    {
                        pathsSet.Add(cleanedPath);
                    }
                }
                PathsTextBox.Text = string.Join(Environment.NewLine, pathsSet);
            }
        }

        private void UpdateRunEveryComboBox(string triggerType)
        {
            switch (triggerType)
            {
                case "Hour":
                    RunEveryComboBox.SelectedItem = "Hour";
                    break;
                case "Day":
                    RunEveryComboBox.SelectedItem = "Day";
                    break;
                case "Week":
                    RunEveryComboBox.SelectedItem = "Week";
                    break;
                case "Month":
                    RunEveryComboBox.SelectedItem = "Month";
                    break;
                default:
                    RunEveryComboBox.SelectedItem = null; // No matching trigger found
                    break;
            }
        }

        public void InitializeTimeValues(TriggerCollection triggers = null)
        {
            // If triggers are not provided, fetch them
            if (triggers == null)
            {
                using (TaskService ts = new TaskService())
                {
                    TaskScheduler task = ts.FindTask(TaskName);
                    if (task != null)
                    {
                        triggers = task.Definition.Triggers;
                    }
                }
            }

            // If triggers are still null, we can't proceed
            if (triggers == null) return;

            var timeTrigger = triggers.FirstOrDefault(t => t is DailyTrigger || t is WeeklyTrigger || t is MonthlyTrigger);

            if (timeTrigger != null)
            {
                // Extract the start time from the trigger and convert it to DateTime
                var triggerTime = DateTime.Today + timeTrigger.StartBoundary.TimeOfDay;
                this.Invoke((MethodInvoker)delegate
                {
                    UpdateTimeComboBoxes(triggerTime, Settings.Default.TwentyFourHourMode);
                });
            }
        }

        private void UpdateTimeComboBoxes(DateTime triggerTime, bool is24HourMode)
        {
            // Round the time down to the nearest hour for the ComboBoxes
            DateTime roundedTime = new DateTime(triggerTime.Year, triggerTime.Month, triggerTime.Day, triggerTime.Hour, 0, 0);

            if (!is24HourMode)
            {
                // Select the rounded time in 12-hour format for the ComboBoxes
                TimeDigitsComboBox.SelectedItem = roundedTime.ToString("h:mm");
                TimeOfDayComboBox.SelectedItem = roundedTime.ToString("tt");
            }
            else
            {
                // Select the rounded time in 24-hour format for the ComboBoxes
                TimeDigitsComboBox.SelectedItem = roundedTime.ToString("HH:mm");
                TimeOfDayComboBox.SelectedItem = null;
            }

            // Set the DateTimePicker's value to the exact trigger time
            dateTimePicker1.Value = triggerTime;
        }

        string currentExePath = Environment.ProcessPath;

        private void SaveTask()
        {
            try
            {
                DateTime? selectedTime = GetSelectedTime();
                if (!selectedTime.HasValue)
                {
                    return; // Exit if the selected time is not valid
                }

                DateTime triggerStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                     selectedTime.Value.Hour, selectedTime.Value.Minute, 0);

                // Adjust for next day if the selected time is before the current time
                if (triggerStart < DateTime.Now)
                {
                    triggerStart = triggerStart.AddDays(1);
                }

                using (TaskService ts = new TaskService())
                {
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Runs WinThumbsPreloader based on user's schedule";

                    // Set trigger based on user's selection
                    if (RunEveryComboBox.SelectedItem.ToString() == "Hour") // Special handling for hourly trigger
                    {
                        var dailyTrigger = new DailyTrigger();
                        dailyTrigger.StartBoundary = triggerStart;
                        dailyTrigger.Repetition.Duration = TimeSpan.FromDays(1);
                        dailyTrigger.Repetition.Interval = TimeSpan.FromHours(1);
                        td.Triggers.Add(dailyTrigger);
                    }
                    else
                    {
                        // Handling for Day, Week, Month triggers
                        Trigger trigger = null;
                        switch (RunEveryComboBox.SelectedItem.ToString())
                        {
                            case "Day":
                                trigger = new DailyTrigger();
                                break;
                            case "Week":
                                trigger = new WeeklyTrigger();
                                break;
                            case "Month":
                                trigger = new MonthlyTrigger();
                                break;
                        }

                        if (trigger != null)
                        {
                            trigger.StartBoundary = triggerStart;
                            td.Triggers.Add(trigger);
                        }
                    }

                    if (OnIdleCheckBox.Checked)
                    {
                        td.Triggers.Add(new IdleTrigger());
                    }

                    string arguments = ConstructArguments();
                    if (arguments != null)
                    {
                        td.Actions.Add(new ExecAction(currentExePath, arguments, null));
                        TaskScheduler task = ts.RootFolder.RegisterTaskDefinition(TaskName, td);
                        task.Enabled = EnabledCheckBox.Checked;
                    }
                    else
                    {
                        return; // Exit if arguments are null
                    }

                    // Display next run time
                    TaskScheduler task1 = ts.FindTask(TaskName);
                    OutputTextBox1.Text = $"Next run time: {task1.NextRunTime}";

                    OutputTextBox.Text = "Task saved successfully.";
                }
                LoadTaskDetails();
            }
            catch
            {
                OutputTextBox.Text = "Task failed to save, try restarting as admin.";
            }
        }

        private void DeleteTask()
        {
            using (TaskService ts = new TaskService())
            {
                try
                {
                    ts.RootFolder.DeleteTask(TaskName);
                    EnabledCheckBox.Checked = false;
                    OutputTextBox.Text = "Task deleted successfully.";
                }
                catch (Exception)
                {
                    // Handle specific exceptions as needed
                    OutputTextBox.Text = "Failed to delete task, try restarting as admin.";
                }
            }
        }

        private DateTime? GetSelectedTime()
        {
            // Use DateTimePicker's value if DetailedTime checkbox is checked
            if (Settings.Default.DetailedTimeMode)
            {
                return dateTimePicker1.Value;
            }

            if (TimeDigitsComboBox.SelectedItem == null || TimeOfDayComboBox.SelectedItem == null || RunEveryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select the time to be scheduled before saving the task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string timeStr = TimeDigitsComboBox.SelectedItem.ToString() + " " + TimeOfDayComboBox.SelectedItem.ToString();
            if (!DateTime.TryParseExact(timeStr, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedTime))
            {
                // Provide a default time if parsing fails, or handle the error appropriately
                MessageBox.Show("The selected time is invalid. Please correct it before saving the task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return selectedTime;
        }

        private string ConstructArguments()
        {
            string arguments = "-r -s ";

            if (MultithreadedCheckBox.Checked)
            {
                if (ThreadsNumericUpDown.Value > 0) arguments += $"-m:{ThreadsNumericUpDown.Value} ";
                else if (ThreadsNumericUpDown.Value == 0) arguments += $"-m ";
            }

            // Use a HashSet to store unique paths
            var uniquePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Assuming CheckedListBox1 contains drive letters
            foreach (string driveLetter in DrivesCheckedListBox.CheckedItems)
            {
                uniquePaths.Add($"{driveLetter}:\\");
            }

            if (!string.IsNullOrWhiteSpace(PathsTextBox.Text))
            {
                // Append each line from PathsTextBox.Text as a separate path
                var paths = PathsTextBox.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var path in paths)
                {
                    var correctedPath = path.Replace('/', '\\').Trim(); // Convert forward slashes to backslashes and trim
                    uniquePaths.Add(correctedPath);
                }
            }

            if (uniquePaths.Count == 0)
            {
                MessageBox.Show("Please select at least one drive letter or provide a valid path before saving the task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Join all unique paths with spaces
            arguments += string.Join(" ", uniquePaths);

            return arguments.Trim();
        }

        public void ToggleDriveVisibility(bool showDrives)
        {
            DrivesLabel.Visible = showDrives;
            DrivesCheckedListBox.Visible = showDrives;

            if (showDrives)
            {
                // Enable DrivesCheckedListBox and transfer paths from PathsTextBox
                TransferPathsToDrives();
                // Reset positions if drives are shown
                PathsLabel.Location = new Point(7, 125);
                PathsTextBox.Location = new Point(7, 143);
                PathsTextBox.Height = 52;  // Original height
            }
            else
            {
                // Disable DrivesCheckedListBox and transfer checked drives to PathsTextBox
                TransferDrivesToPaths();
                // Change positions if drives are hidden
                PathsLabel.Location = new Point(7, 46);
                PathsTextBox.Location = new Point(7, 64);
                PathsTextBox.Height = 131; // Increased height to fill space
            }
        }

        private void TransferDrivesToPaths()
        {
            var paths = PathsTextBox.Text.Split(
                new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Add checked drive letters to the paths list
            foreach (var item in DrivesCheckedListBox.CheckedItems)
            {
                string driveLetter = item.ToString();
                string drivePath = $"{driveLetter}:\\";
                if (!paths.Contains(drivePath))
                {
                    paths.Add(drivePath);
                }
            }

            // Clear the DrivesCheckedListBox selections
            for (int i = 0; i < DrivesCheckedListBox.Items.Count; i++)
            {
                DrivesCheckedListBox.SetItemChecked(i, false);
            }

            // Update the PathsTextBox with the new paths list
            PathsTextBox.Text = string.Join(Environment.NewLine, paths);
        }

        private void TransferPathsToDrives()
        {
            var paths = PathsTextBox.Text.Split(
                new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var path in paths)
            {
                if (path.EndsWith("\\") && path.Length == 3) // e.g., "C:\"
                {
                    // If the path is a drive letter, check it in DrivesCheckedListBox
                    int index = DrivesCheckedListBox.Items.IndexOf(path[0].ToString());
                    if (index != -1)
                    {
                        DrivesCheckedListBox.SetItemChecked(index, true);
                    }
                }
            }

            // Remove drive paths from PathsTextBox
            var nonDrivePaths = paths.Where(p => p.Length > 3 || !p.EndsWith("\\")).ToList();
            PathsTextBox.Text = string.Join(Environment.NewLine, nonDrivePaths);
        }

        public void ChangeDriveLetterSortingOrder()
        {
            // Keep track of the checked items
            var checkedItems = DrivesCheckedListBox.CheckedItems
                                .Cast<string>()
                                .ToList();

            // Clear the items from the CheckedListBox
            DrivesCheckedListBox.Items.Clear();

            List<char> alphabet = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
                           'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
                           'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            List<char> reorderedAlphabet = Settings.Default.DriveSortChoice == "Horizontal"
                                           ? SortDriveLettersHorizontally(alphabet)
                                           : SortDriveLettersVertically(alphabet);

            // Add sorted items to the CheckedListBox
            foreach (char letter in reorderedAlphabet)
            {
                DrivesCheckedListBox.Items.Add(letter.ToString());
            }

            // Restore the checked state to the corresponding sorted items
            foreach (var item in checkedItems)
            {
                int index = DrivesCheckedListBox.Items.IndexOf(item);
                if (index != -1)
                {
                    DrivesCheckedListBox.SetItemChecked(index, true);
                }
            }
        }

        private List<char> SortDriveLettersHorizontally(List<char> alphabet)
        {
            // Calculate the number of items per column
            int itemsPerColumn = (int)Math.Ceiling((double)alphabet.Count / 3);

            // Reorder the list for horizontal sorting
            List<char> reorderedAlphabet = new List<char>();
            for (int i = 0; i < itemsPerColumn; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int index = i + j * itemsPerColumn;
                    if (index < alphabet.Count)
                    {
                        reorderedAlphabet.Add(alphabet[index]);
                    }
                }
            }
            return reorderedAlphabet;
        }

        private List<char> SortDriveLettersVertically(List<char> alphabet)
        {
            // Vertical sorting is essentially the default order
            return alphabet;
        }

        private void PopulateTimeDigitsComboBox(bool is24HourMode)
        {
            string[] twelveHourTime = { "1:00", "2:00", "3:00", "4:00", "5:00", "6:00", "7:00", "8:00", "9:00", "10:00", "11:00", "12:00" };
            string[] twentyFourHourTime = { "00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00",
            "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00"};

            TimeDigitsComboBox.Items.Clear(); // Clear existing items
            if (!is24HourMode)
            {
                TimeDigitsComboBox.Items.AddRange(twelveHourTime);
            }
            else if (is24HourMode)
            {
                TimeDigitsComboBox.Items.AddRange(twentyFourHourTime);
            }
        }

        public void Set24HourMode()
        {
            if (Settings.Default.TwentyFourHourMode == false)
            {
                TimeOfDayComboBox.Enabled = true;
                PopulateTimeDigitsComboBox(false);
                dateTimePicker1.CustomFormat = "hh:mm tt";
            }
            else if (Settings.Default.TwentyFourHourMode == true)
            {
                TimeOfDayComboBox.Enabled = false;
                PopulateTimeDigitsComboBox(true);
                dateTimePicker1.CustomFormat = "HH:mm";
            }
        }

        public void SetDetailedTimeMode()
        {
            if (Settings.Default.DetailedTimeMode == false)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker1.Visible = false;
            }
            else if (Settings.Default.DetailedTimeMode == true)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker1.Visible = true;
            }
        }
    }
}
