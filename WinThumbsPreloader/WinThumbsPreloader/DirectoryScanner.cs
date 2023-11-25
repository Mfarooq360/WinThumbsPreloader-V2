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

        public DirectoryScanner(string path, bool includeNestedDirectories)
        {
            this.path = path;
            this.includeNestedDirectories = includeNestedDirectories;
            this.preloadFolderIcons = Settings.Default.PreloadFolderIcons; // Read once if value doesn't change often
        }

        public static string[] ThumbnailExtensions()
        {
            string[] defaultExtensions = { "avif", "bmp", "gif", "heic", "jpg", "jpeg", "mkv", "mov", "mp4", "png", "svg", "tif", "tiff", "webp" };
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
            try
            {
                foreach (string file in Directory.GetFileSystemEntries(path))
                {
                    if (thumbnailExtensions.Contains(new FileInfo(file).Extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase))
                    {
                        filesList.Add(file);
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
                try
                {
                    if (preloadFolderIcons)
                    {
                        foreach (string subDir in Directory.GetDirectories(currentPath))
                        {
                            queue.Enqueue(subDir);
                            try
                            {   // Check if the subDir contains any files with the specified extensions
                                if (Directory.EnumerateFiles(subDir).Any(file => thumbnailExtensions.Contains(new FileInfo(file).Extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase) || thumbnailExtensions.Length == 0))
                                {
                                    filesList.Add(subDir);
                                }
                            }
                            catch (Exception) { } // Do nothing
                        }
                    }
                    else
                    {
                        foreach (string subDir in Directory.GetDirectories(currentPath))
                        {
                            queue.Enqueue(subDir);
                        }
                    }
                    foreach (string subFiles in Directory.GetFiles(currentPath))
                    {
                        if (thumbnailExtensions.Contains(new FileInfo(subFiles).Extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase) || thumbnailExtensions.Length == 0)
                        {
                            filesList.Add(subFiles);
                        }
                    }
                }
                catch (Exception) { } // Do nothing
                if (filesList.Count > 0) yield return new Tuple<int, List<string>>(filesList.Count, filesList);
            }
        }
    }
}
