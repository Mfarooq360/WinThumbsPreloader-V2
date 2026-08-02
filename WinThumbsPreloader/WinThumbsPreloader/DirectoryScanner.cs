using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WinThumbsPreloader.Properties;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader
{
    sealed class DirectoryScanner
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
        private HashSet<string> thumbnailExtensions;

        public DirectoryScanner(string path, bool includeNestedDirectories, bool multiThreaded, int threadCount)
        {
            WriteLine("Initializing Directory Scanner - DirectoryScanner(string, bool)", LoggingFrequency.PreloaderLogging);
            thumbnailExtensions = LoadExtensions();
            this.path = path;
            this.includeNestedDirectories = includeNestedDirectories;
            this.multiThreaded = multiThreaded;
            this.threadCount = threadCount;
            this.preloadFolderIcons = Settings.Default.PreloadFolderIcons;
            this.preloadAllFolders = Settings.Default.PreloadAllFolders;
            WriteLine("IncludeNestedDirectories: " + includeNestedDirectories, LoggingFrequency.PreloaderLogging);
            WriteLine("preloadFolderIcons: " + preloadFolderIcons, LoggingFrequency.PreloaderLogging);
            WriteLine("preloadAllFolders: " + preloadAllFolders, LoggingFrequency.PreloaderLogging);
        }

        private HashSet<string> LoadExtensions()
        {
            try
            {
                var exts = Settings.Default.ExtensionsText
                    .Split([',', ' ', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
                    .Select(ext => ext.Trim())
                    .Where(ext => ext.Length > 0)
                    .Select(ext =>
                    {
                        ext = ext.TrimStart('.');
                        return "." + ext.ToLowerInvariant();
                    })
                    .Distinct();

                return new HashSet<string>(exts, StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                WriteLine("Failed to load extensions, using defaults: " + ex.Message, LoggingFrequency.PreloaderLogging);

                return new HashSet<string>(
                [
                    ".avi",".avif",".bmp",".gif",".heic",".heif",".jpg",".jpeg",
                    ".mkv",".mov",".mp4",".png",".svg",".tif",".tiff",".webp"
                ], StringComparer.OrdinalIgnoreCase);
            }
        }

        public string[] GetItems()
        {
            string[] result = includeNestedDirectories
                ? (multiThreaded ? GetItemsNestedParallel() : GetItemsNested())
                : GetItemsOnlyFirstLevel();

            filesList.Clear();
            filesList.Capacity = 0;

            return result;
        }

        private bool ShouldIncludeFile(FileSystemInfo file)
        {
            string ext = file.Extension;

            return !string.IsNullOrEmpty(ext) &&
                   (thumbnailExtensions.Count == 0 || thumbnailExtensions.Contains(ext));
        }

        private string[] GetItemsOnlyFirstLevel()
        {
            WriteLine("Getting items count for only first level", LoggingFrequency.PreloaderLogging);

            var enumOptions = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                AttributesToSkip = 0,
                RecurseSubdirectories = false
            };

            try
            {
                var rootInfo = new DirectoryInfo(path);

                foreach (FileSystemInfo entry in rootInfo.EnumerateFileSystemInfos("*", enumOptions))
                {
                    if (cancelled)
                        break;

                    bool isDirectory = (entry.Attributes & FileAttributes.Directory) != 0;

                    if (isDirectory)
                    {
                        if (!preloadFolderIcons)
                            continue;

                        var dirInfo = (DirectoryInfo)entry;

                        if (preloadAllFolders)
                        {
                            filesList.Add(dirInfo.FullName);
                            totalItemsCount++;
                            continue;
                        }

                        try
                        {
                            foreach (FileSystemInfo subFile in dirInfo.EnumerateFiles("*", enumOptions))
                            {
                                if (cancelled)
                                    break;

                                if (ShouldIncludeFile(subFile))
                                {
                                    filesList.Add(dirInfo.FullName);
                                    totalItemsCount++;
                                    break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            WriteLine("Exception thrown while scanning subdirectory: " + e.Message, LoggingFrequency.DebugLogging);
                        }
                    }
                    else
                    {
                        if (ShouldIncludeFile(entry))
                        {
                            filesList.Add(entry.FullName);
                            totalItemsCount++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine("Exception thrown while scanning directory: " + e.Message, LoggingFrequency.DebugLogging);
            }

            return filesList.ToArray();
        }

        private string[] GetItemsNested()
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
                    var dirInfo = new DirectoryInfo(currentPath);
                    foreach (FileSystemInfo entry in dirInfo.EnumerateFileSystemInfos("*", enumOptions))
                    {
                        if (cancelled) break;

                        if ((entry.Attributes & FileAttributes.Directory) != 0)
                        {
                            queue.Enqueue(entry.FullName);
                        }
                        else if (ShouldIncludeFile(entry))
                        {
                            filesList.Add(entry.FullName);
                            totalItemsCount++;
                            directoryContainsThumbnail = true;
                        }
                    }

                    if (!cancelled && preloadFolderIcons && (directoryContainsThumbnail || preloadAllFolders))
                    {
                        filesList.Add(currentPath);
                        totalItemsCount++;
                    }

                }
                catch (Exception e)
                {
                    WriteLine("Exception thrown while scanning directory: " + e.Message, LoggingFrequency.DebugLogging);
                }
            }
            return filesList.ToArray();
        }

        private string[] GetItemsNestedParallel()
        {
            WriteLine("Getting items count for nested directories in parallel", LoggingFrequency.PreloaderLogging);

            var enumOptions = new EnumerationOptions { IgnoreInaccessible = true, AttributesToSkip = 0 };
            var scannedFiles = new ConcurrentQueue<string>();
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
                        var dirInfo = new DirectoryInfo(currentPath);
                        foreach (FileSystemInfo entry in dirInfo.EnumerateFileSystemInfos("*", enumOptions))
                        {
                            if (cancelled || loopState.ShouldExitCurrentIteration)
                            {
                                directoriesToProcess.CompleteAdding();
                                foreach (string dir in directoriesToProcess.GetConsumingEnumerable()) { }
                                loopState.Stop();
                                return;
                            }

                            if ((entry.Attributes & FileAttributes.Directory) != 0)
                            {
                                directoriesToProcess.Add(entry.FullName);
                            }
                            else if (ShouldIncludeFile(entry))
                            {
                                scannedFiles.Enqueue(entry.FullName);
                                Interlocked.Increment(ref totalItemsCount);
                                directoryContainsThumbnail = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        WriteLine("Exception thrown while scanning directory: " + e.Message, LoggingFrequency.DebugLogging);
                    }

                    if (!cancelled && preloadFolderIcons && (directoryContainsThumbnail || preloadAllFolders))
                    {
                        scannedFiles.Enqueue(currentPath);
                        Interlocked.Increment(ref totalItemsCount);
                    }

                    if (Interlocked.Decrement(ref activeThreads) == 0 && directoriesToProcess.Count == 0)
                    {
                        directoriesToProcess.CompleteAdding();
                    }
                });

            string[] result = scannedFiles.ToArray();
            Array.Sort(result, StringComparer.OrdinalIgnoreCase); // Sorted due to parallel scanning adding items out of order
            return result;
        }
    }
}