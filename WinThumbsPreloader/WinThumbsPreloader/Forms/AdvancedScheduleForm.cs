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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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
    }
}
