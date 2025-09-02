using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using WinThumbsPreloader.Properties;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader
{
    class DirectoryScanner
    {
        public bool cancelled = false;
        public int totalItemsCount = 0;

        private List<string> filesList = new List<string>();
        private bool includeNestedDirectories;
        private bool multiThreaded;
        private string path;
        private bool preloadAllFolders;
        private bool preloadFolderIcons;
        private int threadCount;
        private string[] thumbnailExtensions;

        public DirectoryScanner(string path, bool includeNestedDirectories, bool multiThreaded, int threadCount)
        {
            WriteLine("Initializing Directory Scanner - DirectoryScanner(string, bool)", LoggingFrequency.PreloaderLogging);
            thumbnailExtensions = ThumbnailExtensions();
            this.path = path;
            this.includeNestedDirectories = includeNestedDirectories;
            this.multiThreaded = multiThreaded;
            this.threadCount = threadCount > 0 ? threadCount : Environment.ProcessorCount;
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
                    .Split(new[] { ',', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ext => ext.Trim().TrimStart('.').ToLowerInvariant())
                    .Where(ext => !string.IsNullOrWhiteSpace(ext))
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();
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

        public List<string> GetItems()
        {
            return includeNestedDirectories
                ? (multiThreaded ? GetItemsNestedParallel() : GetItemsNested())
                : GetItemsOnlyFirstLevel();
        }

        private bool ShouldIncludeFile(string file)
        {
            string ext = Path.GetExtension(file).TrimStart('.').ToLowerInvariant();
            return thumbnailExtensions.Length == 0 || thumbnailExtensions.Contains(ext);
        }

        private static bool IsFile(string path)
        {
            try { return (File.GetAttributes(path) & FileAttributes.Directory) == 0; }
            catch { return false; } // Path doesn't exist or isn't accessible
        }

        private static bool IsDirectory(string path)
        {
            try { return (File.GetAttributes(path) & FileAttributes.Directory) != 0; }
            catch { return false; } // Path doesn't exist or isn't accessible
        }

        private List<string> GetItemsOnlyFirstLevel()
        {
            WriteLine("Getting items count for only first level", LoggingFrequency.PreloaderLogging);

            var enumOptions = new EnumerationOptions { IgnoreInaccessible = true, AttributesToSkip = 0 };
            try
            {
                foreach (string entry in Directory.EnumerateFileSystemEntries(path, "*", enumOptions))
                {
                    if (preloadFolderIcons && IsDirectory(entry)) // If the entry is a directory and we are to preload folder icons with thumb extension files...
                    {
                        foreach (string subFile in Directory.EnumerateFiles(entry, "*", enumOptions)) // Scan each file in the subdirectory
                        {
                            if (preloadAllFolders || ShouldIncludeFile(subFile)) // If the file contains a select extension or we are to preload all folder icons...
                            {
                                filesList.Add(entry); // Add the directory to the filesList if a file with a thumbnail extension is found
                                totalItemsCount++;
                                break; // No need to check further files in this directory
                            }
                            if (cancelled) break;
                        }
                    }
                    else if (IsFile(entry) && ShouldIncludeFile(entry)) // If the entry is a file and has a thumbnail extension...
                    {
                        filesList.Add(entry); // Add the file to the fileslist
                        totalItemsCount++;
                    }
                    if (cancelled) break;
                }
            }
            catch (Exception e)
            {
                WriteLine("Exception thrown while scanning directory: " + e.Message, LoggingFrequency.DebugLogging);
            }
            return filesList;
        }

        private List<string> GetItemsNested()
        {
            WriteLine("Getting items count for nested directories", LoggingFrequency.PreloaderLogging);

            var enumOptions = new EnumerationOptions { IgnoreInaccessible = true, AttributesToSkip = 0 };
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            string currentPath;

            while (queue.Count > 0 && !cancelled)
            {
                currentPath = queue.Dequeue();
                bool directoryContainsThumbnail = false;

                try
                {
                    foreach (string file in Directory.EnumerateFiles(currentPath, "*", enumOptions))
                    {
                        if (ShouldIncludeFile(file))
                        {
                            filesList.Add(file);
                            totalItemsCount++;
                            directoryContainsThumbnail = true;
                        }
                        if (cancelled) break;
                    }

                    if (preloadFolderIcons && (directoryContainsThumbnail || preloadAllFolders))
                    {
                        filesList.Add(currentPath);
                        totalItemsCount++;
                    }

                    foreach (string subDir in Directory.EnumerateDirectories(currentPath, "*", enumOptions))
                    {
                        queue.Enqueue(subDir);
                        if (cancelled) break;
                    }

                }
                catch (Exception e)
                {
                    WriteLine("Exception thrown while scanning directory: " + e.Message, LoggingFrequency.DebugLogging);
                }
                if (cancelled) break;
            }
            return filesList;
        }

        private List<string> GetItemsNestedParallel()
        {
            WriteLine("Getting items count for nested directories in parallel", LoggingFrequency.PreloaderLogging);

            var enumOptions = new EnumerationOptions { IgnoreInaccessible = true, AttributesToSkip = 0 };
            var scannedFiles = new ConcurrentBag<string>();
            var directoriesToProcess = new BlockingCollection<string> { path };
            int activeThreads = 0;

            Parallel.ForEach(
                directoriesToProcess.GetConsumingEnumerable(),
                new ParallelOptions { MaxDegreeOfParallelism = threadCount },
                (currentPath, loopState) =>
                {
                    if (cancelled || loopState.ShouldExitCurrentIteration)
                    {
                        loopState.Stop();
                        return;
                    }

                    Interlocked.Increment(ref activeThreads);
                    bool directoryContainsThumbnail = false;

                    try
                    {
                        foreach (string subDir in Directory.EnumerateDirectories(currentPath, "*", enumOptions))
                        {
                            if (cancelled || loopState.ShouldExitCurrentIteration)
                            {
                                directoriesToProcess.CompleteAdding();
                                foreach (string dir in directoriesToProcess.GetConsumingEnumerable()) { }
                                loopState.Stop();
                                return;
                            }

                            directoriesToProcess.Add(subDir);
                        }
                    }
                    catch (Exception e)
                    {
                        WriteLine("Exception thrown while scanning directory: " + e.Message, LoggingFrequency.DebugLogging);
                    }

                    try
                    {
                        foreach (string file in Directory.EnumerateFiles(currentPath, "*", enumOptions))
                        {
                            if (cancelled || loopState.ShouldExitCurrentIteration)
                            {
                                directoriesToProcess.CompleteAdding();
                                foreach (string dir in directoriesToProcess.GetConsumingEnumerable()) { }
                                loopState.Stop();
                                return;
                            }

                            if (ShouldIncludeFile(file))
                            {
                                scannedFiles.Add(file);
                                Interlocked.Increment(ref totalItemsCount);
                                directoryContainsThumbnail = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        WriteLine("Exception thrown while scanning directory: " + e.Message, LoggingFrequency.DebugLogging);
                    }

                    if (preloadFolderIcons && (directoryContainsThumbnail || preloadAllFolders))
                    {
                        scannedFiles.Add(currentPath);
                        Interlocked.Increment(ref totalItemsCount);
                    }

                    if (Interlocked.Decrement(ref activeThreads) == 0 && directoriesToProcess.Count == 0)
                    {
                        directoriesToProcess.CompleteAdding();
                    }
                });

            return scannedFiles.OrderBy(file => file, StringComparer.OrdinalIgnoreCase).ToList();
        }
    }
}
