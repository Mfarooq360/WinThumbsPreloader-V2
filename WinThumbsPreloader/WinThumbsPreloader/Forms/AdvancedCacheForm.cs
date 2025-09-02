using Microsoft.Win32;
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
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader.Forms
{
    public partial class AdvancedCacheForm : Form
    {
        public AdvancedCacheForm()
        {
            InitializeComponent();
        }

        private void AdvancedCacheForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.MainIcon;
            CacheSizeUpdateIntervalNumericUpDown.Value = Properties.Settings.Default.CacheUpdateInterval;
            AutoBackupIntervalNumericUpDown.Value = Properties.Settings.Default.AutoBackupInterval / 1000;
            AutoRestoreIntervalNumericUpDown.Value = Properties.Settings.Default.AutoRestoreInterval / 1000;
            CacheSizeFormatComboBox.SelectedItem = Properties.Settings.Default.CacheSizeFormat;
            ExplorerCacheDeletionMethodComboBox.SelectedItem = Properties.Settings.Default.ExplorerCacheDeletionMethod;
            ManualDeletionFrequencyComboBox.SelectedItem = Properties.Settings.Default.ManualDeletionFrequency;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            WriteLine("Closing Advanced Cache Form", LoggingFrequency.GUILogging);
            Close();
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            WriteLine("Saving Advanced Cache Form settings", LoggingFrequency.GUILogging);
            Properties.Settings.Default.CacheUpdateInterval = tempCacheUpdateInterval;
            WriteLine("Cache Update Interval saved: " + tempCacheUpdateInterval, LoggingFrequency.DebugLogging);
            Properties.Settings.Default.AutoBackupInterval = tempAutoBackupInterval;
            WriteLine("Auto Backup Interval saved: " + tempAutoBackupInterval, LoggingFrequency.DebugLogging);
            Properties.Settings.Default.AutoRestoreInterval = tempAutoRestoreInterval;
            WriteLine("Auto Restore Interval saved: " + tempAutoRestoreInterval, LoggingFrequency.DebugLogging);
            Properties.Settings.Default.CacheSizeFormat = CacheSizeFormatComboBox.SelectedItem.ToString();
            WriteLine("Cache Size Format saved: " + CacheSizeFormatComboBox.SelectedItem.ToString(), LoggingFrequency.DebugLogging);
            Properties.Settings.Default.ExplorerCacheDeletionMethod = ExplorerCacheDeletionMethodComboBox.SelectedItem.ToString();
            WriteLine("Explorer Cache Deletion Method saved: " + ExplorerCacheDeletionMethodComboBox.SelectedItem.ToString(), LoggingFrequency.DebugLogging);
            Properties.Settings.Default.ManualDeletionFrequency = ManualDeletionFrequencyComboBox.SelectedItem.ToString();
            WriteLine("Manual Deletion Frequency saved: " + ManualDeletionFrequencyComboBox.SelectedItem.ToString(), LoggingFrequency.DebugLogging);
            Properties.Settings.Default.Save();

            CacheForm cacheForm = Owner as CacheForm;
            if (cacheForm != null)
            {
                cacheForm.UpdateCacheSizeUpdateInterval(tempCacheUpdateInterval);
                cacheForm.UpdateAutoBackupInterval(tempAutoBackupInterval);
                cacheForm.UpdateAutoRestoreInterval(tempAutoRestoreInterval);
                cacheForm.format = Properties.Settings.Default.CacheSizeFormat;
                cacheForm.UpdateCacheSizeLabels();
                if (Properties.Settings.Default.ExplorerCacheDeletionMethod == "Disk Cleanup")
                {
                    cacheForm.ConfigureDiskCleanupSageset();
                }
            }
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            WriteLine("Setting Advanced Cache Form settings to default", LoggingFrequency.GUILogging);
            CacheSizeUpdateIntervalNumericUpDown.Value = 250;
            AutoBackupIntervalNumericUpDown.Value = 5;
            AutoRestoreIntervalNumericUpDown.Value = 5;
            CacheSizeFormatComboBox.SelectedItem = "MB";
            ExplorerCacheDeletionMethodComboBox.SelectedItem = "Manual Deletion";
            ManualDeletionFrequencyComboBox.SelectedItem = "On Deletion Start";
            SaveButton_Click(sender, e);
        }

        private void CacheSizeFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void CacheSizeFormatLabel_Click(object sender, EventArgs e)
        {

        }

        private void ManualDeletionFrequencyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
