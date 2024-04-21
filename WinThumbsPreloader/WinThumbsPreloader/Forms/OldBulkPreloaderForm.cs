using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;

namespace WinThumbsPreloader.Forms
{
    public partial class OldBulkPreloaderForm : Form
    {
        bool presetsVisible = false;
        public OldBulkPreloaderForm()
        {
            InitializeComponent();
            this.KeyDown += BulkPreloaderForm_KeyDown;
            this.KeyUp += BulkPreloaderForm_KeyUp;
            this.Activated += BulkPreloaderForm_Activated;
            this.KeyPreview = true;
            TogglePresetsVisibility();
        }

        private void BulkPreloaderForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.MainIcon;
        }

        private void BulkPreloaderForm_Activated(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                CloseButton.Text = "Exit";
            }
            else
            {
                CloseButton.Text = "Close";
            }
        }

        private void BulkPreloaderForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                CloseButton.Text = "Exit";
            }
            else
            {
                CloseButton.Text = "Close";
            }
        }

        private void BulkPreloaderForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                CloseButton.Text = "Close";
            }
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

        private void PresetsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PathsTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void MultithreadedCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ThreadsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void MaximumInstancesNumericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void BulkPreloaderMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ExportPresetsButton_Click(object sender, EventArgs e)
        {

        }

        private void PresetsTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SavePresetsButton_Click(object sender, EventArgs e)
        {

        }

        private void PresetsGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void TogglePresetsVisibility()
        {
            if (!presetsVisible)
            {
                PresetsGroupBox.Visible = false;
                PresetsGroupBox.Enabled = false;
                Size = new Size(450, 450);
                CloseButton.Location = new Point(334, 371);
                StartButton.Location = new Point(12, 371);
            }
            else
            {
                PresetsGroupBox.Visible = true;
                PresetsGroupBox.Enabled = true;
                Size = new Size(750, 450);
                CloseButton.Location = new Point(634, 371);
                StartButton.Location = new Point(12, 371);
            }
        }

        private void PresetsButton_Click(object sender, EventArgs e)
        {
            if (presetsVisible)
            {
                presetsVisible = false;
                TogglePresetsVisibility();
            }
            else
            {
                presetsVisible = true;
                TogglePresetsVisibility();
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {

        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {

        }
    }
}
