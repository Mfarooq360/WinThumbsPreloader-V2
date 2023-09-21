using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;

namespace WinThumbsPreloader
{
    [SupportedOSPlatform("windows")]
    public partial class ScheduleForm : Form //TODO: add a statistics form
    {
        private const string TaskName = "WinThumbsPreloaderTask";

        public ScheduleForm()
        {
            InitializeComponent();
            OutputTextBox_Initialize(null, null);
            this.KeyDown += ScheduleForm_KeyDown;
            this.KeyUp += ScheduleForm_KeyUp;
            this.Activated += ScheduleForm_Activated;
            this.KeyPreview = true;
            this.FormClosing += ScheduleForm_FormClosing;
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.MainIcon;
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
            }
            else
            {
                // Shift key is not pressed, so set the button text to its original value
                CloseButton.Text = "Close";
            }
        }

        private void ScheduleForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text when Shift is pressed
                CloseButton.Text = "Exit";
            }
        }

        private void ScheduleForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                // Change the button text back when Shift is released
                CloseButton.Text = "Close";
            }
        }

        private void LoadTaskDetails()
        {
            using (TaskService ts = new TaskService())
            {
                Task task = ts.FindTask(TaskName);
                if (task != null)
                {
                    // Populate form fields with task details
                    // This can be expanded based on task's definition and settings

                    // Check if the task has an "Idle" trigger
                    OnIdleCheckBox.Checked = task.Definition.Triggers.Any(t => t.TriggerType == TaskTriggerType.Idle);

                    // Extract the arguments from the task
                    string arguments = (task.Definition.Actions[0] as ExecAction)?.Arguments;

                    if (arguments != null)
                    {
                        // Extract multithreading argument
                        MultithreadedCheckBox.Checked = arguments.Contains("-m");
                        var match = System.Text.RegularExpressions.Regex.Match(arguments, @"-m:(\d+)");
                        if (match.Success)
                        {
                            ThreadsNumericUpDown.Value = int.Parse(match.Groups[1].Value);
                        }
                        else if (arguments.Contains("-m"))
                        {
                            ThreadsNumericUpDown.Value = 0;  // -m without a specific thread count
                        }

                        // Extract drive letters
                        var items = CheckedListBox1.Items.Cast<string>().ToList();
                        foreach (string driveLetter in items)
                        {
                            if (arguments.Contains($"{driveLetter}:/"))
                            {
                                CheckedListBox1.SetItemChecked(CheckedListBox1.Items.IndexOf(driveLetter), true);
                            }
                        }
                    }

                    if (RunEveryComboBox.SelectedItem == null) { RunEveryComboBox.SelectedItem = "Week"; }
                    if (TimeDigitsComboBox.SelectedItem == null) { TimeDigitsComboBox.SelectedItem = "12:00"; }
                    if (TimeOfDayComboBox.SelectedItem == null) { TimeOfDayComboBox.SelectedItem = "PM"; }
                    // Set the EnabledCheckBox according to the task's enabled status
                    EnabledCheckBox.Checked = task.Enabled;

                    // Set the RunEveryComboBox based on task's triggers
                    UpdateRunEveryComboBox(task);

                    // TODO: Extract other arguments and update the corresponding form fields
                }
            }
        }
        private void UpdateRunEveryComboBox(Task task)
        {
            if (task.Definition.Triggers.OfType<DailyTrigger>().Any())
            {
                RunEveryComboBox.SelectedItem = "Day";
            }
            else if (task.Definition.Triggers.OfType<WeeklyTrigger>().Any())
            {
                RunEveryComboBox.SelectedItem = "Week";
            }
            else if (task.Definition.Triggers.OfType<MonthlyTrigger>().Any())
            {
                RunEveryComboBox.SelectedItem = "Month";
            }
            else
            {
                RunEveryComboBox.SelectedItem = null; // or set a default value if necessary
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

        string currentExePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

        private void ScheduleSaveButton_Click(object sender, EventArgs e)
        {
            //Saves the scheduled task
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Runs WinThumbsPreloader based on user's schedule";

                // Set trigger based on user's selection
                DateTime selectedTime = GetSelectedTime();
                if (RunEveryComboBox.SelectedItem.ToString() == "Day")
                {
                    var dailyTrigger = new DailyTrigger();
                    dailyTrigger.StartBoundary = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, selectedTime.Hour, selectedTime.Minute, 0);
                    td.Triggers.Add(dailyTrigger);
                }
                else if (RunEveryComboBox.SelectedItem.ToString() == "Week")
                {
                    var weeklyTrigger = new WeeklyTrigger();
                    weeklyTrigger.StartBoundary = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, selectedTime.Hour, selectedTime.Minute, 0);
                    td.Triggers.Add(weeklyTrigger);
                }
                else if (RunEveryComboBox.SelectedItem.ToString() == "Month")
                {
                    var monthlyTrigger = new MonthlyTrigger();
                    monthlyTrigger.StartBoundary = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, selectedTime.Hour, selectedTime.Minute, 0);
                    td.Triggers.Add(monthlyTrigger);
                }

                if (OnIdleCheckBox.Checked)
                {
                    var idleTrigger = new IdleTrigger();
                    td.Triggers.Add(idleTrigger);
                }

                // Construct the arguments for the task action
                string arguments = ConstructArguments();

                td.Actions.Add(new ExecAction(currentExePath, arguments, null));
                Task task = ts.RootFolder.RegisterTaskDefinition(TaskName, td);
                task.Enabled = EnabledCheckBox.Checked;

                OutputTextBox.Text = "Task saved successfully.";
            }
        }

        private DateTime GetSelectedTime()
        {
            string timeStr = TimeDigitsComboBox.SelectedItem.ToString() + " " + TimeOfDayComboBox.SelectedItem.ToString();
            DateTime selectedTime;
            if (!DateTime.TryParseExact(timeStr, "h:mm tt", null, System.Globalization.DateTimeStyles.None, out selectedTime))
            {
                // Provide a default time if parsing fails, or handle the error appropriately
                selectedTime = DateTime.Now;
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

            // Assuming CheckedListBox1 contains drive letters
            foreach (string driveLetter in CheckedListBox1.CheckedItems)
            {
                arguments += $"{driveLetter}:/ ";
            }

            return arguments.Trim();
        }

        private void CheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //A box of all 26 Drive letters to pass as paths to the program if they're checked
            this.CheckedListBox1.CheckOnClick = true;
        }

        private void EnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            using (TaskService ts = new TaskService())
            {
                Task task = ts.FindTask(TaskName);
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

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_MAXIMIZE = 3;

        private void OpenTaskSchedulerButton_Click(object sender, EventArgs e)
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
                    FileName = "cmd.exe",
                    Arguments = "/c taskschd.msc",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            }
        }

        private void OutputTextBox_Initialize(object sender, EventArgs e)
        {
            using (TaskService ts = new TaskService())
            {
                Task task = ts.FindTask(TaskName);
                if (task != null)
                {
                    DateTime lastRunTime = task.LastRunTime;
                    if (lastRunTime != DateTime.MinValue) // DateTime.MinValue is the default when it hasn't been run
                    {
                        OutputTextBox.Text = $"Last run time: {lastRunTime}";
                    }
                    else
                    {
                        OutputTextBox.Text = "The task has not run yet.";
                    }
                }
                else
                {
                    OutputTextBox.Text = "Task not found.";
                }
            }
        }

        private void OutputTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
