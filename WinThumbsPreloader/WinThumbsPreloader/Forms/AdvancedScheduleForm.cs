using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;
using Task = System.Threading.Tasks.Task;
using TaskScheduler = Microsoft.Win32.TaskScheduler.Task;
using Trigger = Microsoft.Win32.TaskScheduler.Trigger;
using TriggerCollection = Microsoft.Win32.TaskScheduler.TriggerCollection;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class AdvancedScheduleForm : Form
    {
        public AdvancedScheduleForm()
        {
            InitializeComponent();
        }

        private void ScheduleOptionsForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.MainIcon;
            DriveSelectionCheckBox.Checked = Settings.Default.DriveSelectionEnabled;
            TwentyFourHourCheckBox.Checked = Settings.Default.TwentyFourHourMode;
            DetailedTimeModeCheckBox.Checked = Settings.Default.DetailedTimeMode;

            // Load the saved sorting choice.
            string savedSortChoice = Settings.Default.DriveSortChoice;
            if (!string.IsNullOrEmpty(savedSortChoice))
            {
                DriveSortComboBox.SelectedItem = savedSortChoice;
            }
            /*LoadAdvancedTaskDetails();*/
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DriveSelectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DriveSelectionEnabled = DriveSelectionCheckBox.Checked;
            Settings.Default.Save();

            // Assuming ScheduleForm is the parent or has been otherwise made accessible
            var scheduleForm = (ScheduleForm)this.Owner;
            scheduleForm?.ToggleDriveVisibility(DriveSelectionCheckBox.Checked);
        }

        private void DriveSortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save the selected sort choice to the application settings.
            Settings.Default.DriveSortChoice = DriveSortComboBox.SelectedItem.ToString();
            Settings.Default.Save();
            var scheduleForm = (ScheduleForm)this.Owner;
            scheduleForm?.ChangeDriveLetterSortingOrder();
        }

        private void OpenTaskSchedulerButton_Click(object sender, EventArgs e)
        {
            var scheduleForm = (ScheduleForm)this.Owner;
            scheduleForm?.OpenTaskSchedulerButton_Click(null, null);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TwentyFourHourCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.TwentyFourHourMode = TwentyFourHourCheckBox.Checked;
            Settings.Default.Save();
            var scheduleForm = (ScheduleForm)this.Owner;
            scheduleForm?.Set24HourMode();
            scheduleForm?.InitializeTimeValues();
        }

        private void DetailedTimeModeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DetailedTimeMode = DetailedTimeModeCheckBox.Checked;
            Settings.Default.Save();
            var scheduleForm = (ScheduleForm)this.Owner;
            scheduleForm?.SetDetailedTimeMode();
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            DriveSelectionCheckBox.Checked = true;
            DriveSortComboBox.SelectedItem = "Vertical";
            TwentyFourHourCheckBox.Checked = false;
            DetailedTimeModeCheckBox.Checked = false;
        }

        private void SaveTask()
        {
            /*try
            {
                using (TaskService ts = new TaskService())
                {
                    TaskDefinition td = ts.NewTask();
                    TaskScheduler task = ts.RootFolder.RegisterTaskDefinition(ScheduleForm.TaskName, td);

                }
            }
            catch (Exception e)
            {
                WriteLine("An error occurred while saving the task: " + e.Message, LoggingFrequency.GUILogging);
                MessageBox.Show("An error occurred while saving the task. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }

        private void LoadAdvancedTaskDetails()
        {
            try
            {
                using (TaskService ts = new TaskService())
                {
                    TaskScheduler task = ts.GetTask(ScheduleForm.TaskName);
                    TaskScheduler existingTask = ts.GetTask(ScheduleForm.TaskName);
                    TaskDefinition td = existingTask.Definition;
                    if (task != null)
                    {
                        // General
                        /*GeneralDescriptionTextBox.Text = task.Definition.RegistrationInfo.Description;*/
                        RunOnlyWhenUserIsLoggedOnRadioButton.Checked = task.Definition.Principal.LogonType == TaskLogonType.InteractiveToken;
                        RunWhetherUserIsLoggedOnOrNotRadioButton.Checked = task.Definition.Principal.LogonType == TaskLogonType.ServiceAccount;
                        RunWithHighestPrivilegesCheckBox.Checked = task.Definition.Principal.RunLevel == TaskRunLevel.Highest;
                        GeneralHiddenCheckBox.Checked = task.Definition.Settings.Hidden;

                        // Conditions

                        StartTheTaskOnlyIfTheComputerIsIdleForCheckBox.Checked = task.Definition.Settings.IdleSettings.IdleDuration != TimeSpan.Zero && task.Definition.Settings.RunOnlyIfIdle;

                        task.Definition.Settings.RunOnlyIfIdle = true;
                        ts.RootFolder.RegisterTaskDefinition(ScheduleForm.TaskName, td);

                        double idleMinutes = task.Definition.Settings.IdleSettings.IdleDuration.TotalMinutes;
                        string idleDurationString = ConvertMinutesToIdleDurationString(idleMinutes);
                        StartTheTaskOnlyIfTheComputerIsIdleForComboBox.SelectedItem = idleDurationString;
                        double waitTimeoutMinutes = task.Definition.Settings.IdleSettings.WaitTimeout.TotalMinutes;
                        string waitTimeoutString = ConvertMinutesToWaitTimeoutString(waitTimeoutMinutes);
                        WaitForIdleForComboBox.SelectedItem = waitTimeoutString;
                        StopIfTheComputerCeasesToBeIdleCheckBox.Checked = task.Definition.Settings.IdleSettings.StopOnIdleEnd;
                        if (!StartTheTaskOnlyIfTheComputerIsIdleForCheckBox.Checked)
                        {
                            StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Enabled = false;
                            WaitForIdleForLabel.Enabled = false;
                            WaitForIdleForComboBox.Enabled = false;
                            StopIfTheComputerCeasesToBeIdleCheckBox.Enabled = false;
                        }
                        else if (StartTheTaskOnlyIfTheComputerIsIdleForCheckBox.Checked)
                        {
                            StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Enabled = true;
                            WaitForIdleForLabel.Enabled = true;
                            WaitForIdleForComboBox.Enabled = true;
                            StopIfTheComputerCeasesToBeIdleCheckBox.Enabled = true;
                        }
                        StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox.Checked = task.Definition.Settings.DisallowStartIfOnBatteries;
                        StopIfTheComputerSwitchesToBatteryPowerCheckBox.Checked = task.Definition.Settings.StopIfGoingOnBatteries;
                        WakeTheComputerToRunThisTaskCheckBox.Checked = task.Definition.Settings.WakeToRun;

                        // Settings
                        AllowTaskToBeRunOnDemandCheckBox.Checked = task.Definition.Settings.AllowDemandStart;
                        RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox.Checked = task.Definition.Settings.StartWhenAvailable;
                        IfTheTaskFailsRestartEveryCheckBox.Checked = task.Definition.Settings.RestartInterval != TimeSpan.Zero;
                        double restartIntervalMinutes = task.Definition.Settings.RestartInterval.TotalMinutes;
                        string restartIntervalString = ConvertRestartIntervalToString(restartIntervalMinutes);
                        IfTheTaskFailsRestartEveryComboBox.SelectedItem = restartIntervalString;
                        AttemptToRestartUpToTextBox.Text = task.Definition.Settings.RestartCount.ToString();
                        StopTheTaskIfItRunsLongerThanCheckBox.Checked = task.Definition.Settings.ExecutionTimeLimit != TimeSpan.Zero;
                        var executionLimitMinutes = task.Definition.Settings.ExecutionTimeLimit.TotalMinutes;
                        var executionLimitString = ConvertMinutesToSchedulerFormat(executionLimitMinutes);
                        StopTheTaskIfItRunsLongerThanComboBox.SelectedItem = StopTheTaskIfItRunsLongerThanComboBox.Items.Contains(executionLimitString)
                                                            ? executionLimitString
                                                            : "Custom"; // Ensure "Custom" or similar is an item in your ComboBox for unmatched cases.
                        IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox.Checked = task.Definition.Settings.AllowHardTerminate;
                        IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox.SelectedItem = ConvertMultipleInstancesToString(task.Definition.Settings.MultipleInstances);
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine("An error occurred while loading the task: " + e.Message, LoggingFrequency.GUILogging);
                MessageBox.Show("An error occurred while loading the task. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConvertMinutesToSchedulerFormat(double totalMinutes)
        {
            // Define known durations in minutes and their string representations
            var durations = new Dictionary<double, string>
            {
                { 60, "1 hour" },
                { 120, "2 hours" },
                { 240, "4 hours" },
                { 480, "8 hours" },
                { 720, "12 hours" },
                { 1440, "1 day" },
                { 4320, "3 days" }
            };

            // Attempt to find a matching duration
            if (durations.TryGetValue(totalMinutes, out string formattedDuration))
            {
                return formattedDuration;
            }
            // If no exact match is found, return a default or custom string
            // This could be enhanced to find the closest match or format it differently
            return $"{totalMinutes} minutes";
        }

        private string ConvertMinutesToIdleDurationString(double totalMinutes)
        {
            // Map of known duration values in Task Scheduler to their display strings
            var durationMapping = new Dictionary<double, string>
            {
                { 1, "1 minute" },
                { 5, "5 minutes" },
                { 10, "10 minutes" },
                { 15, "15 minutes" },
                { 30, "30 minutes" },
                { 60, "1 hour" } // 60 minutes
            };

            // Check if the totalMinutes is one of the known values and return the corresponding string
            if (durationMapping.TryGetValue(totalMinutes, out var durationString))
            {
                return durationString;
            }

            // Fallback for any other value not in the mapping
            return totalMinutes < 60 ? $"{totalMinutes} minutes" : $"{totalMinutes / 60} hour(s)";
        }

        private string ConvertMinutesToWaitTimeoutString(double totalMinutes)
        {
            // Special case for "Do not wait"
            if (totalMinutes == 0) return "Do not wait";

            // Map of known duration values in Task Scheduler to their display strings
            var durationMapping = new Dictionary<double, string>
            {
                { 1, "1 minute" },
                { 5, "5 minutes" },
                { 10, "10 minutes" },
                { 15, "15 minutes" },
                { 30, "30 minutes" },
                { 60, "1 hour" }, // 60 minutes
                { 120, "2 hours" } // 120 minutes
            };

            // Check if the totalMinutes is one of the known values and return the corresponding string
            if (durationMapping.TryGetValue(totalMinutes, out var durationString))
            {
                return durationString;
            }

            // Fallback for any other value not in the mapping
            return totalMinutes < 60 ? $"{totalMinutes} minutes" : $"{totalMinutes / 60} hour(s)";
        }

        private string ConvertRestartIntervalToString(double totalMinutes)
        {
            // Map of known restart interval values in Task Scheduler to their display strings
            var intervalMapping = new Dictionary<double, string>
            {
                { 1, "1 minute" },
                { 5, "5 minutes" },
                { 10, "10 minutes" },
                { 15, "15 minutes" },
                { 30, "30 minutes" },
                { 60, "1 hour" }, // 60 minutes
                { 120, "2 hours" } // 120 minutes
            };

            // Check if the totalMinutes is one of the known values and return the corresponding string
            if (intervalMapping.TryGetValue(totalMinutes, out var intervalString))
            {
                return intervalString;
            }

            // Fallback for any other value not in the mapping
            return totalMinutes < 60 ? $"{totalMinutes} minutes" : $"{totalMinutes / 60} hour(s)";
        }

        private string ConvertMultipleInstancesToString(TaskInstancesPolicy policy)
        {
            switch (policy)
            {
                case TaskInstancesPolicy.IgnoreNew:
                    return "Do not start a new instance";
                case TaskInstancesPolicy.Parallel:
                    return "Run a new instance in parallel";
                case TaskInstancesPolicy.Queue:
                    return "Queue a new instance";
                case TaskInstancesPolicy.StopExisting:
                    return "Stop the existing instance";
                default:
                    return "Unknown";
            }
        }

        private void GeneralDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void RunOnlyWhenUserIsLoggedOnRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RunWhetherUserIsLoggedOnOrNotRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RunWithHighestPrivilegesCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void GeneralHiddenCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void StartTheTaskOnlyIfTheComputerIsIdleForCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!StartTheTaskOnlyIfTheComputerIsIdleForCheckBox.Checked)
            {
                StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Enabled = false;
                WaitForIdleForLabel.Enabled = false;
                WaitForIdleForComboBox.Enabled = false;
                StopIfTheComputerCeasesToBeIdleCheckBox.Enabled = false;
            }
            else if (StartTheTaskOnlyIfTheComputerIsIdleForCheckBox.Checked)
            {
                StartTheTaskOnlyIfTheComputerIsIdleForComboBox.Enabled = true;
                WaitForIdleForLabel.Enabled = true;
                WaitForIdleForComboBox.Enabled = true;
                StopIfTheComputerCeasesToBeIdleCheckBox.Enabled = true;
            }
        }

        private void StartTheTaskOnlyIfTheComputerIsIdleForComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void WaitForIdleForLabel_Click(object sender, EventArgs e)
        {

        }

        private void WaitForIdleForComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void StopIfTheComputerCeasesToBeIdleCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RestartIfTheIdleStateResumesCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void StartTheTaskOnlyIfTheComputerIsOnACPowerCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void StopIfTheComputerSwitchesToBatteryPowerCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void WakeTheComputerToRunThisTaskCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void AllowTaskToBeRunOnDemandCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RunTaskAsSoonAsPossibleAfterAScheduledStartIsMissedCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void IfTheTaskFailsRestartEveryCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void IfTheTaskFailsRestartEveryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AttemptToRestartUpToLabel_Click(object sender, EventArgs e)
        {

        }

        private void AttemptToRestartUpToTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void StopTheTaskIfItRunsLongerThanCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void StopTheTaskIfItRunsLongerThanComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void IfTheRunningTaskDoesNotEndWhenRequestedForceItToStopCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesLabel_Click(object sender, EventArgs e)
        {

        }

        private void IfTheTaskIsAlreadyRunningThenTheFollowingRuleAppliesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AdvancedScheduleSaveButton_Click(object sender, EventArgs e)
        {
            /*SaveTask();*/
        }
    }
}
