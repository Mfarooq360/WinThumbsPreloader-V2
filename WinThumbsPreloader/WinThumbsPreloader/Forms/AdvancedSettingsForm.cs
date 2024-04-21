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
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader.Forms
{
    public partial class AdvancedSettingsForm : Form
    {
        public AdvancedSettingsForm()
        {
            InitializeComponent();
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
            LoggingFrequencyComboBox.SelectedIndex = Settings.Default.LoggingFrequency;
            PreloaderThumbnailSizesCheckedListBox.ItemCheck += PreloaderThumbnailSizesCheckedListBox_ItemCheck;
            LoadCheckedItemsFromSettings();

            if (Settings.Default.PreloaderThumbnailSizes == "768")
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
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            WriteLine("Closing Advanced Settings Form", LoggingFrequency.GUILogging);
            Close();
        }

        private void PreloadFolderIconsForComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteLine("PreloadFolderIconsForComboBox.SelectedItem: " + PreloadFolderIconsForComboBox.SelectedItem, LoggingFrequency.DebugLogging);
            if (PreloadFolderIconsForComboBox.SelectedIndex == 1)
            {
                WriteLine("Warning: Preloading all folders may cause freezes when adding certain system folders", LoggingFrequency.DebugLogging);
                Settings.Default.PreloadAllFolders = true;
                PreloadFolderIconsForLabel.ForeColor = Color.Red;
                toolTip1.SetToolTip(PreloadFolderIconsForLabel, @"Selecting ""Folders Containing Set Extensions"" will ensure that
only folders that contain files that have the specified extensions
will have their icons included to be preloaded.

Selecting ""All Folders"" will add all folders to the preloader
which may cause freezes when adding certain system folders.

WARNING: Selecting ""All Folders"" may cause the preloader to freeze.");
            }
            else
            {
                Settings.Default.PreloadAllFolders = false;
                PreloadFolderIconsForLabel.ForeColor = SystemColors.ControlText;
                toolTip1.SetToolTip(PreloadFolderIconsForLabel, @"Selecting ""Folders Containing Set Extensions"" will ensure that
only folders that contain files that have the specified extensions
will have their icons included to be preloaded.

Selecting ""All Folders"" will add all folders to the preloader
which may cause freezes when adding certain system folders.");
            }
            Settings.Default.Save();
        }

        private void ExtensionsAutoFormattingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteLine("ExtensionsAutoFormattingComboBox.SelectedItem: " + ExtensionsAutoFormattingComboBox.SelectedItem, LoggingFrequency.DebugLogging);
            if (ExtensionsAutoFormattingComboBox.SelectedItem != null)
            {
                string extensionsAutoFormatting = ExtensionsAutoFormattingComboBox.SelectedItem.ToString();
                if (extensionsAutoFormatting == "Commas and Spaces")
                {
                    extensionsAutoFormatting = "CommasAndSpaces";
                }
                Settings.Default.ExtensionsAutoFormatting = extensionsAutoFormatting;
                Settings.Default.Save();

                // If SettingsForm is open, update the display
                var settingsForm = (SettingsForm)this.Owner;
                settingsForm?.UpdateExtensionsTextBoxDisplay();
            }
        }

        bool automaticUnchecked = false;

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            WriteLine("Resetting settings to default", LoggingFrequency.DebugLogging);
            ExtensionsAutoFormattingComboBox.SelectedItem = "Disabled";
            PreloadFolderIconsForComboBox.SelectedIndex = 0;
            LoggingFrequencyComboBox.SelectedIndex = 0;
            Settings.Default.ExtensionsAutoFormatting = "Disabled";
            Settings.Default.PreloadAllFolders = false;
            Settings.Default.LoggingFrequency = 0;
            PresetsComboBox.SelectedItem = "File Explorer Size";

            automaticUnchecked = true;

            // Uncheck all items in the CheckedListBox
            for (int i = 0; i < PreloaderThumbnailSizesCheckedListBox.Items.Count; i++)
            {
                PreloaderThumbnailSizesCheckedListBox.SetItemChecked(i, false);
            }

            // Find and check the default size (256)
            int defaultSizeIndex = PreloaderThumbnailSizesCheckedListBox.Items.IndexOf("256");
            if (defaultSizeIndex != -1)
            {
                PreloaderThumbnailSizesCheckedListBox.SetItemChecked(defaultSizeIndex, true);
            }

            automaticUnchecked = false;

            // Save the new settings
            Settings.Default.PreloaderThumbnailSizes = "256";
            Settings.Default.Save();
        }

        private async void PreloaderThumbnailSizesCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Prevent unchecking if this is the last checked item
            if (e.NewValue == CheckState.Unchecked && PreloaderThumbnailSizesCheckedListBox.CheckedItems.Count <= 1 && !automaticUnchecked)
            {
                e.NewValue = CheckState.Checked; // Keep the item checked
                return; // Skip further processing
            }

            // Wait for the checked state to be updated
            await Task.Delay(10);

            // Concatenate the checked items into a comma-separated string
            var checkedSizes = PreloaderThumbnailSizesCheckedListBox.CheckedItems
                                  .Cast<string>()
                                  .Aggregate((current, next) => current + "," + next);

            if (checkedSizes == "768")
            {
                PresetsComboBox.SelectedItem = "Photos App Large/Medium";
            }
            else if (checkedSizes == "256")
            {
                PresetsComboBox.SelectedItem = "Explorer Size/Photos App Small";
            }
            else
            {
                PresetsComboBox.SelectedItem = null;
            }

            // Update and save the setting
            Settings.Default.PreloaderThumbnailSizes = checkedSizes;
            Settings.Default.Save();
        }

        private void LoadCheckedItemsFromSettings()
        {
            string savedSizes = Settings.Default.PreloaderThumbnailSizes;
            if (!string.IsNullOrEmpty(savedSizes))
            {
                var sizes = savedSizes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

                // Uncheck all items first
                for (int i = 0; i < PreloaderThumbnailSizesCheckedListBox.Items.Count; i++)
                {
                    PreloaderThumbnailSizesCheckedListBox.SetItemChecked(i, false);
                }

                // Check the items based on the selected preset
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
            WriteLine("LoggingFrequencyComboBox.SelectedItem: " + LoggingFrequencyComboBox.SelectedItem, LoggingFrequency.DebugLogging);
            currentLoggingFrequency = (LoggingFrequency)LoggingFrequencyComboBox.SelectedIndex;
            Settings.Default.LoggingFrequency = (int)currentLoggingFrequency;
            Settings.Default.Save();
            LogFrequency = currentLoggingFrequency;
            if (LoggingFrequencyComboBox.SelectedIndex < 4)
            {
                LoggingFrequencyLabel.ForeColor = SystemColors.ControlText;
                toolTip1.SetToolTip(LoggingFrequencyLabel, @"Enables various tiers of logging for various purposes.

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
(Only works in single-threaded mode)

Enabling Debug Logging may cause the application to run
slower, especially while preloading, and will use more storage.");
            }
            else if (LoggingFrequencyComboBox.SelectedIndex == 4)
            {
                LoggingFrequencyLabel.ForeColor = Color.Red;
                toolTip1.SetToolTip(LoggingFrequencyLabel, @"Enables various tiers of logging for various purposes.

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
(Only works in single-threaded mode)

WARNING: Enabling Debug Logging may cause the application to run
slower, especially while preloading, and will use more storage.");
            }
            InitializeLogger();
        }
    }
}
