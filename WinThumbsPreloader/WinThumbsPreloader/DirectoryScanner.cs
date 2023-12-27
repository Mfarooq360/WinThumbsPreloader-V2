using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WinThumbsPreloader.Properties;

namespace WinThumbsPreloader
{
    class DirectoryScanner
    {
        private string path;
        private bool includeNestedDirectories;
        List<string> filesList = new List<string>();
        string[] thumbnailExtensions = ThumbnailExtensions();
        private bool preloadFolderIcons;
        private bool preloadAllFolders;

        public DirectoryScanner(string path, bool includeNestedDirectories)
        {
            this.path = path;
            this.includeNestedDirectories = includeNestedDirectories;
            this.preloadFolderIcons = Settings.Default.PreloadFolderIcons;
            this.preloadAllFolders = Settings.Default.PreloadAllFolders;
        }

        public static string[] ThumbnailExtensions()
        {
            string[] defaultExtensions = { "avif", "bmp", "gif", "heic", "heif", "jpg", "jpeg", "mkv", "mov", "mp4", "png", "svg", "tif", "tiff", "webp" };
            string[] thumbnailExtensions;
            try
            {
                thumbnailExtensions = Properties.Settings.Default.ExtensionsText
                                      .Split(new[] { ',', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries) // Ignore commas, spaces and newlines between extensions
                                      .Where(ext => !string.IsNullOrWhiteSpace(ext)) // Ignore blank lines or lines with only spaces
                                      .Select(ext => ext.Trim(' ')).ToArray(); // Ignore spaces after the extension for each line

                if (thumbnailExtensions == null) { thumbnailExtensions = defaultExtensions; }
            }
            catch (Exception) { thumbnailExtensions = defaultExtensions; }
            return thumbnailExtensions;
        }

        public IEnumerable<Tuple<int, List<string>>> GetItemsCount()
        {
            if (includeNestedDirectories)
            {
                foreach (Tuple<int, List<string>> itemsCount in GetItemsCountNested()) yield return itemsCount;
            }
            else
            {
                foreach (Tuple<int, List<string>> itemsCount in GetItemsCountOnlyFirstLevel()) yield return itemsCount;
            }
        }

        private IEnumerable<Tuple<int, List<string>>> GetItemsCountOnlyFirstLevel()
        {
            HashSet<string> addedDirectories = new HashSet<string>(); // To avoid adding the same directory multiple times

            try
            {
                foreach (string entry in Directory.GetFileSystemEntries(path))
                {
                    if (Directory.Exists(entry) && preloadFolderIcons)
                    {
                        // Check each file in the subdirectory
                        foreach (string subFile in Directory.GetFiles(entry))
                        {
                            if (thumbnailExtensions.Contains(new FileInfo(subFile).Extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase) || preloadAllFolders)
                            {
                                if (!addedDirectories.Contains(entry))
                                {
                                    filesList.Add(entry); // Add the directory if a file with a thumbnail extension is found
                                    addedDirectories.Add(entry); // Mark this directory as added
                                }
                            }
                        }
                    }
                    else if (File.Exists(entry) && thumbnailExtensions.Contains(new FileInfo(entry).Extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase))
                    {
                        filesList.Add(entry); // Add the file directly if it has a thumbnail extension
                    }
                }
            }
            catch (Exception) { } // Do nothing
            if (filesList.Count > 0) yield return new Tuple<int, List<string>>(filesList.Count, filesList);
        }

        private IEnumerable<Tuple<int, List<string>>> GetItemsCountNested()
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            string currentPath;

            while (queue.Count > 0)
            {
                currentPath = queue.Dequeue();
                bool directoryContainsThumbnail = false; // Flag to check if directory contains thumbnail

                try
                {
                    foreach (string file in Directory.GetFiles(currentPath)) // Check each file in the current directory
                    {
                        if (thumbnailExtensions.Contains(new FileInfo(file).Extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase) || thumbnailExtensions.Length == 0)
                        {
                            filesList.Add(file);
                            directoryContainsThumbnail = true; // Set flag if a thumbnail file is found
                        }
                    }

                    if (preloadFolderIcons && directoryContainsThumbnail) // If directory contains thumbnail, add the directory to the list
                    {
                        filesList.Add(currentPath);
                    }
                    else if (preloadFolderIcons && preloadAllFolders)
                    {
                        filesList.Add(currentPath);
                    }

                    foreach (string subDir in Directory.GetDirectories(currentPath)) // Add subdirectories to the queue
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception) { } // Do nothing
                if (filesList.Count > 0) yield return new Tuple<int, List<string>>(filesList.Count, filesList);
            }
        }
    }
}
