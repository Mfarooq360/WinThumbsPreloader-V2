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
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private int tempCacheUpdateInterval = 250;
        private int tempAutoBackupInterval = 5000;
        private int tempAutoRestoreInterval = 5000;

        private void CacheSizeUpdateIntervalNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            tempCacheUpdateInterval = (int)CacheSizeUpdateIntervalNumericUpDown.Value;
        }

        private void AutoBackupIntervalNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            tempAutoBackupInterval = (int)AutoBackupIntervalNumericUpDown.Value * 1000;
        }

        private void AutoRestoreIntervalNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            tempAutoRestoreInterval = (int)AutoRestoreIntervalNumericUpDown.Value * 1000;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CacheUpdateInterval = tempCacheUpdateInterval;
            Properties.Settings.Default.AutoBackupInterval = tempAutoBackupInterval;
            Properties.Settings.Default.AutoRestoreInterval = tempAutoRestoreInterval;
            Properties.Settings.Default.CacheSizeFormat = CacheSizeFormatComboBox.SelectedItem.ToString();
            Properties.Settings.Default.ExplorerCacheDeletionMethod = ExplorerCacheDeletionMethodComboBox.SelectedItem.ToString();
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
            CacheSizeUpdateIntervalNumericUpDown.Value = 250;
            AutoBackupIntervalNumericUpDown.Value = 5;
            AutoRestoreIntervalNumericUpDown.Value = 5;
            CacheSizeFormatComboBox.SelectedItem = "MB";
            ExplorerCacheDeletionMethodComboBox.SelectedItem = "Manual Deletion";
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
    }
}
