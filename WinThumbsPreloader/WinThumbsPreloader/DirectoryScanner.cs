using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WinThumbsPreloader.Properties;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader
{
    class DirectoryScanner
    {
        private string path;
        private bool includeNestedDirectories;
        List<string> filesList = new List<string>();
        string[] thumbnailExtensions;
        private bool preloadFolderIcons;
        private bool preloadAllFolders;

        public DirectoryScanner(string path, bool includeNestedDirectories)
        {
            WriteLine("Initializing Directory Scanner - DirectoryScanner(string, bool)", LoggingFrequency.PreloaderLogging);
            thumbnailExtensions = ThumbnailExtensions();
            this.path = path;
            this.includeNestedDirectories = includeNestedDirectories;
            this.preloadFolderIcons = Settings.Default.PreloadFolderIcons;
            this.preloadAllFolders = Settings.Default.PreloadAllFolders;
            WriteLine("IncludeNestedDirectories: " + includeNestedDirectories, LoggingFrequency.PreloaderLogging);
            WriteLine("preloadFolderIcons: " + preloadFolderIcons, LoggingFrequency.PreloaderLogging);
            WriteLine("preloadAllFolders: " + preloadAllFolders, LoggingFrequency.PreloaderLogging);
        }

        public static string[] ThumbnailExtensions()
        {
            WriteLine("Getting thumbnail extensions - ThumbnailExtensions()", LoggingFrequency.PreloaderLogging);
            string[] defaultExtensions = { "avif", "bmp", "gif", "heic", "heif", "jpg", "jpeg", "mkv", "mov", "mp4", "png", "svg", "tif", "tiff", "webp" };
            WriteLine("Default extensions: " + string.Join(", ", defaultExtensions), LoggingFrequency.DebugLogging);
            string[] thumbnailExtensions;
            try
            {
                thumbnailExtensions = Properties.Settings.Default.ExtensionsText
                                      .Split(new[] { ',', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries) // Ignore commas, spaces and newlines between extensions
                                      .Where(ext => !string.IsNullOrWhiteSpace(ext)) // Ignore blank lines or lines with only spaces
                                      .Select(ext => ext.Trim(' ')).ToArray(); // Ignore spaces after the extension for each line
                WriteLine("Split(new[] { ',', ' ', '\\n', '\\r' }, StringSplitOptions.RemoveEmptyEntries)", LoggingFrequency.DebugLogging);
                WriteLine("Where(ext => !string.IsNullOrWhiteSpace(ext))", LoggingFrequency.DebugLogging);
                WriteLine("Select(ext => ext.Trim(' ')).ToArray();", LoggingFrequency.DebugLogging);

                if (thumbnailExtensions == null) 
                {
                    WriteLine("Thumbnail extensions are null. Using default extensions.", LoggingFrequency.PreloaderLogging);
                    thumbnailExtensions = defaultExtensions; 
                }
                else
                {
                    WriteLine("Thumbnail extensions: " + string.Join(", ", thumbnailExtensions), LoggingFrequency.PreloaderLogging);
                }
            }
            catch (Exception ex) 
            { 
                WriteLine("Exception thrown while getting thumbnail extensions: " + ex.Message, LoggingFrequency.PreloaderLogging);
                WriteLine("Using default extensions: " + string.Join(", ", defaultExtensions), LoggingFrequency.PreloaderLogging);
                thumbnailExtensions = defaultExtensions;
            }
            return thumbnailExtensions;
        }

        public IEnumerable<Tuple<int, List<string>>> GetItemsCount()
        {
            WriteLine("Getting items count - GetItemsCount()", LoggingFrequency.PreloaderLogging);
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
            WriteLine("Getting items count for first level", LoggingFrequency.PreloaderLogging);
            HashSet<string> addedDirectories = new HashSet<string>(); // To avoid adding the same directory multiple times

            try
            {
                foreach (string entry in Directory.GetFileSystemEntries(path))
                {
                    /*WriteLine("Entry: " + entry, LoggingFrequency.DebugLogging);*/
                    if (Directory.Exists(entry) && preloadFolderIcons)
                    {
                        // Check each file in the subdirectory
                        foreach (string subFile in Directory.GetFiles(entry))
                        {
                            /*WriteLine("SubFile: " + subFile, LoggingFrequency.DebugLogging);*/
                            if (thumbnailExtensions.Contains(new FileInfo(subFile).Extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase) || preloadAllFolders)
                            {
                                if (!addedDirectories.Contains(entry))
                                {
                                    /*WriteLine("Adding directory (For Folder Icon Preloading): " + entry, LoggingFrequency.DebugLogging);*/
                                    filesList.Add(entry); // Add the directory if a file with a thumbnail extension is found
                                    addedDirectories.Add(entry); // Mark this directory as added
                                }
                            }
                        }
                    }
                    else if (File.Exists(entry) && thumbnailExtensions.Contains(new FileInfo(entry).Extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase))
                    {
                        /*WriteLine("Adding file: " + entry, LoggingFrequency.DebugLogging);*/
                        filesList.Add(entry); // Add the file directly if it has a thumbnail extension
                    }
                }
                /*WriteLine("filesList.Count: " + filesList.Count, LoggingFrequency.DebugLogging);*/
            }
            catch (Exception e)
            {
                WriteLine("Exception thrown while scanning directory: " + e.Message, LoggingFrequency.PreloaderLogging);
            }
            if (filesList.Count > 0) yield return new Tuple<int, List<string>>(filesList.Count, filesList);
        }

        private IEnumerable<Tuple<int, List<string>>> GetItemsCountNested()
        {
            WriteLine("Getting items count for nested directories", LoggingFrequency.PreloaderLogging);
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            string currentPath;

            while (queue.Count > 0)
            {
                /*WriteLine("Queue count: " + queue.Count, LoggingFrequency.DebugLogging);
                WriteLine("Dequeueing currentPath", LoggingFrequency.DebugLogging);*/
                currentPath = queue.Dequeue();
                bool directoryContainsThumbnail = false; // Flag to check if directory contains thumbnail

                try
                {
                    foreach (string file in Directory.GetFiles(currentPath)) // Check each file in the current directory
                    {
                        /*WriteLine("File: " + file, LoggingFrequency.DebugLogging);*/
                        if (thumbnailExtensions.Contains(new FileInfo(file).Extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase) || thumbnailExtensions.Length == 0)
                        {
                            /*WriteLine("Adding file: " + file, LoggingFrequency.DebugLogging);*/
                            filesList.Add(file);
                            directoryContainsThumbnail = true; // Set flag if a thumbnail file is found
                            /*WriteLine("directoryContainsThumbnail: " + directoryContainsThumbnail, LoggingFrequency.DebugLogging);*/
                        }
                    }

                    if (preloadFolderIcons && (directoryContainsThumbnail || preloadAllFolders)) // If directory contains thumbnail, add the directory to the list
                    {
                        /*WriteLine("Adding directory (For Folder Icon Preloading): " + currentPath, LoggingFrequency.DebugLogging);*/
                        filesList.Add(currentPath);
                    }

                    foreach (string subDir in Directory.GetDirectories(currentPath)) // Add subdirectories to the queue
                    {
                        /*WriteLine("Queueing subdirectory: " + subDir, LoggingFrequency.DebugLogging);*/
                        queue.Enqueue(subDir);
                    }
                    /*WriteLine("filesList.Count: " + filesList.Count, LoggingFrequency.DebugLogging);*/
                }
                catch (Exception e)
                {
                    WriteLine("Exception thrown while scanning directory: " + e.Message, LoggingFrequency.PreloaderLogging);
                }
                if (filesList.Count > 0) yield return new Tuple<int, List<string>>(filesList.Count, filesList);
            }
        }
    }
}
