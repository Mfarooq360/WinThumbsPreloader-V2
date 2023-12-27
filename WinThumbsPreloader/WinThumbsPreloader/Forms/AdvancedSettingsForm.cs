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
            PreloaderThumbnailSizesCheckedListBox.ItemCheck += PreloaderThumbnailSizesCheckedListBox_ItemCheck;
            LoadCheckedItemsFromSettings();

            if (Settings.Default.PreloaderThumbnailSizes == "768,256" || Settings.Default.PreloaderThumbnailSizes == "256,768")
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
            Close();
        }

        private void PreloadFolderIconsForComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PreloadFolderIconsForComboBox.SelectedIndex == 1)
            {
                Settings.Default.PreloadAllFolders = true;
            }
            else
            {
                Settings.Default.PreloadAllFolders = false;
            }
            Settings.Default.Save();
        }

        private void ExtensionsAutoFormattingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            ExtensionsAutoFormattingComboBox.SelectedItem = "Disabled";
            PreloadFolderIconsForComboBox.SelectedIndex = 0;
            Settings.Default.ExtensionsAutoFormatting = "Disabled";
            Settings.Default.PreloadAllFolders = false;
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

        private void PreloaderThumbnailSizesCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

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

            if (checkedSizes == "768,256" || checkedSizes == "256,768")
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
            { "Photos App Large/Medium", new List<string> { "768", "256" } },
        };

        private void PresetsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
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
    }
}
