using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader
{
    class ThumbnailsPreloader
    {
        public enum ThumbnailsPreloaderState
        {
            New,
            GettingNumberOfItems,
            Processing,
            Canceled,
            Done
        }

        private DirectoryScanner directoryScanner;
        private System.Windows.Forms.Timer cacheCheckTimer;
        private ProgressDialog progressDialog;
        private System.Windows.Forms.Timer progressDialogUpdateTimer;

        private bool hasDecrementedActiveInstances = false;
        private bool includeNestedDirectories;
        private bool multiThreaded;
        private bool silentMode;
        private int threadCount;
        private int threads;
        private string path;
        private readonly TaskCompletionSource<bool> _instanceCompletion =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public ThumbnailsPreloaderState state = ThumbnailsPreloaderState.GettingNumberOfItems;
        public ThumbnailsPreloaderState prevState = ThumbnailsPreloaderState.New;
        public int totalItemsCount = 0;
        public int processedItemsCount = 0;
        public uint[] thumbnailSizes;
        public string currentFile = "";

        public ThumbnailsPreloader(string path, bool includeNestedDirectories, bool silentMode, bool multiThreaded, int threadCount)
        {
            WriteLine("Initializing Preloader - ThumbnailsPreloader(string, bool, bool, bool, int)", LoggingFrequency.PreloaderLogging);

            thumbnailSizes = ParseThumbnailSizes();
            WriteLine("thumbnailSizes: " + string.Join(", ", thumbnailSizes), LoggingFrequency.PreloaderLogging);
                        
            this.includeNestedDirectories = includeNestedDirectories;
            WriteLine("IncludeNestedDirectories: " + includeNestedDirectories, LoggingFrequency.PreloaderLogging);

            this.multiThreaded = multiThreaded;
            WriteLine("MultiThreaded: " + multiThreaded, LoggingFrequency.PreloaderLogging);

            this.threadCount = threadCount;
            WriteLine("ThreadCount: " + threadCount, LoggingFrequency.PreloaderLogging);

            threads = DetermineThreadCount();
            WriteLine("Resolved Thread Count: " + threads, LoggingFrequency.PreloaderLogging);

            this.silentMode = silentMode;
            WriteLine("SilentMode: " + silentMode, LoggingFrequency.PreloaderLogging);

            this.path = path;
            WriteLine("Path: " + path, LoggingFrequency.PreloaderLogging);            
        }

        public async Task StartPreloaderAsync()
        {
            try
            {
                WriteLine("Starting Preloader - StartPreloaderAsync()", LoggingFrequency.PreloaderLogging);

                FileAttributes attributes = File.GetAttributes(path);
                bool isDirectory = attributes.HasFlag(FileAttributes.Directory);

                WriteLine("Path is a directory: " + isDirectory, LoggingFrequency.PreloaderLogging);

                if (!isDirectory)
                {
                    WriteLine("Preloading single file: " + path, LoggingFrequency.PreloaderLogging);

                    ThumbnailPreloader.PreloadThumbnail(path, thumbnailSizes);

                    WriteLine("Preloading single file done: " + path, LoggingFrequency.PreloaderLogging);

                    state = ThumbnailsPreloaderState.Done;
                    FinalizeInstance();
                    EndInstance();
                    return;
                }

                directoryScanner = new DirectoryScanner(path, includeNestedDirectories, multiThreaded, threads);

                if (!silentMode)
                {
                    InitProgressDialog();
                    InitProgressDialogUpdateTimer();

                    if (Settings.Default.ThumbsResetAlert && currentLoggingFrequency != LoggingFrequency.DebugLogging && currentLoggingFrequency != LoggingFrequency.NoLogging)
                    {
                        InitializeCacheCheckTimer();
                    }
                }

                await Run();

                if (silentMode)
                {
                    FinalizeInstance();
                    EndInstance();
                }

                await _instanceCompletion.Task;
            }
            catch (Exception ex)
            {
                WriteLine($"Preloader failed for '{path}': {ex}", LoggingFrequency.PreloaderLogging);

                state = ThumbnailsPreloaderState.Canceled;
                FinalizeInstance();
                EndInstance();

                throw;
            }
        }

        private void InitProgressDialog()
        {
            WriteLine("Initializing progress dialog - InitProgressDialog()", LoggingFrequency.PreloaderLogging);
            progressDialog = new ProgressDialog();
            progressDialog.ShowTimeRemaining = false;
            progressDialog.Title = Resources.ThumbnailsPreloader_Title;
            progressDialog.CancelMessage = Resources.ThumbnailsPreloader_CancelMessage;
            progressDialog.Show();
            WriteLine("Progress dialog initialized", LoggingFrequency.PreloaderLogging);
            UpdateProgressDialog(null, null);
        }

        private void InitProgressDialogUpdateTimer()
        {
            WriteLine("Initializing progress dialog update timer", LoggingFrequency.PreloaderLogging);
            progressDialogUpdateTimer = new System.Windows.Forms.Timer();
            progressDialogUpdateTimer.Interval = Settings.Default.ProgressDialogUpdateSpeed; // Default: 250ms
            progressDialogUpdateTimer.Tick += new EventHandler(UpdateProgressDialog);
            progressDialogUpdateTimer.Start();
            WriteLine("Progress dialog update timer initialized", LoggingFrequency.PreloaderLogging);
        }

        bool statusLogged = false;
        bool statusLogged2 = false;

        private DateTime doneCompletedAt;
        private bool doneCompletedAtSet = false;

        private async void UpdateProgressDialog(object sender, EventArgs e)
        {
            if (!statusLogged)
            {
                WriteLine("Updating progress dialog - UpdateProgressDialog(object, EventArgs)", LoggingFrequency.DebugLogging);
                statusLogged = true;
            }
            if (progressDialog.HasUserCancelled)
            {
                if (!statusLogged2)
                {
                    WriteLine("Cancelling preloader and progress dialog", LoggingFrequency.PreloaderLogging);
                    statusLogged2 = true;
                }
                finishDelayCts.Cancel();
                state = ThumbnailsPreloaderState.Canceled;
                directoryScanner.cancelled = true;

                progressDialog.Close();
                progressDialog?.Dispose();
                progressDialog = null;
                progressDialogUpdateTimer.Stop();
                progressDialogUpdateTimer.Tick -= UpdateProgressDialog;
                progressDialogUpdateTimer?.Dispose();
                progressDialogUpdateTimer = null;

                if (InstanceCompleted())
                {
                    EndInstance();
                    return;
                }
            }
            else if (state == ThumbnailsPreloaderState.GettingNumberOfItems)
            {
                if (prevState != state)
                {
                    WriteLine("Updating number of items in progress dialog", LoggingFrequency.DebugLogging);
                    prevState = state;
                    progressDialog.Line1 = "Scanning directory for items...";
                    progressDialog.Line3 = Resources.ThumbnailsPreloader_CalculatingNumberOfItems;
                    progressDialog.Marquee = true;
                }
                totalItemsCount = directoryScanner.totalItemsCount; // Placed here to only update when needed by the progress dialog
                progressDialog.Line2 = String.Format(Resources.ThumbnailsPreloader_Discovered0Items, totalItemsCount);
            }
            else if (state == ThumbnailsPreloaderState.Processing)
            {
                if (prevState != state)
                {
                    WriteLine("Updating progress dialog for thumbnail processing", LoggingFrequency.DebugLogging);
                    prevState = state;
                    progressDialog.Line1 = String.Format(Resources.ThumbnailsPreloader_PreloadingThumbnailsFor0Items, totalItemsCount);
                    progressDialog.Maximum = totalItemsCount;
                    progressDialog.Marquee = false;
                }
                progressDialog.Title = String.Format(Resources.ThumbnailsPreloader_Processing, (processedItemsCount * 100) / totalItemsCount);
                progressDialog.Line2 = Resources.ThumbnailsPreloader_Name + ": " + Path.GetFileName(currentFile);
                progressDialog.Line3 = String.Format(Resources.ThumbnailsPreloader_ItemsRemaining, totalItemsCount - processedItemsCount);
                progressDialog.Value = processedItemsCount;
            }
            else if (state == ThumbnailsPreloaderState.Done)
            {
                progressDialogUpdateTimer.Stop();
                progressDialogUpdateTimer.Tick -= UpdateProgressDialog;
                progressDialogUpdateTimer?.Dispose();
                progressDialogUpdateTimer = null;

                if (prevState != state)
                {
                    WriteLine("Finalizing progress dialog", LoggingFrequency.DebugLogging);
                    if (!doneCompletedAtSet)
                    {
                        doneCompletedAt = DateTime.Now;
                        doneCompletedAtSet = true;
                    }
                    progressDialog.Title = String.Format(Resources.ThumbnailsPreloader_Processing, 100); // TODO: Check if it flashes the taskbar icon on completion, and if so, allow the user to change that behavior if possible
                    progressDialog.Line1 = $"Preloading completed for {totalItemsCount:N0} items";
                    string displayPath = ShortenPath(path, 56);
                    progressDialog.Line2 = "Path: \"" + displayPath + "\"";
                    progressDialog.Line3 = $"Finished preloading {processedItemsCount:N0} items in {preloaderElapsedSeconds:N2} seconds.";
                    progressDialog.Value = progressDialog.Maximum;

                    bool backupAttempted = false;
                    if (Settings.Default.AutoBackupAfterPreload && Settings.Default.AutoBackupThumbs && CacheForm.CompareThumbsCacheSize())
                    {
                        WriteLine("AutoBackupThumbs is enabled and the cache size has changed, backing up thumbs cache", LoggingFrequency.PreloaderLogging);
                        prevState = state;
                        progressDialog.Line1 = Resources.ThumbnailsPreloader_BackingUpThumbsCache;
                        
                        CacheForm.CacheOperationResult backupResult = await CacheForm.BackupThumbsCacheDetailedAsync(null);

                        if (backupResult.Succeeded)
                        {
                            WriteLine("Thumbs cache backed up successfully", LoggingFrequency.PreloaderLogging);
                            progressDialog.Line1 = "Thumbnail cache backed up successfully";
                            backupAttempted = true;
                        }
                        else if (backupResult.PartiallySucceeded)
                        {
                            WriteLine("Thumbs cache partially backed up", LoggingFrequency.PreloaderLogging);
                            progressDialog.Line1 = "Thumbnail cache partially backed up";
                            backupAttempted = true;
                        }
                        else if (backupResult.FailureReason == CacheForm.CacheOperationFailureReason.AlreadyInProgress) // Preloader will return to regular done logic instead if cache backup is skipped due to another instance already running the backup
                        {
                            WriteLine("Thumbs cache backup skipped, already running in another instance", LoggingFrequency.PreloaderLogging);
                        }
                        else
                        {
                            WriteLine("Thumbs cache backup failed", LoggingFrequency.PreloaderLogging);
                            WriteLine("Failure reason: " + backupResult.FailureReason, LoggingFrequency.PreloaderLogging);
                            progressDialog.Line1 = "Thumbnail cache backup failed";
                            backupAttempted = true;
                        }

                        if (backupAttempted)
                        {
                            TimeSpan delay = GetDelayTimeSpan(Settings.Default.WaitTimeAfterCacheBackup, Settings.Default.WaitAfterCacheUnit);

                            await DelayWithCancellation(delay, updateDoneTitle: true);
                        }
                    }
                    if (InstanceCompleted() == true && !backupAttempted)
                    {
                        if (Settings.Default.WaitAfterPreloading)
                        {
                            if (Settings.Default.WaitTimeAfterPreloading == 0)
                            {
                                while (!progressDialog.HasUserCancelled)
                                {
                                    await DelayWithCancellation(TimeSpan.Zero, updateDoneTitle: true);
                                }
                            }
                            else
                            {
                                WriteLine("Waiting " + Settings.Default.WaitTimeAfterPreloading + " " + Settings.Default.WaitAfterPreloadingUnit.ToLower() + " before closing preloader", LoggingFrequency.PreloaderLogging);
                                TimeSpan delay = GetDelayTimeSpan(Settings.Default.WaitTimeAfterPreloading, Settings.Default.WaitAfterPreloadingUnit);

                                await DelayWithCancellation(delay, updateDoneTitle: true);
                            }
                        }
                    }
                    WriteLine("Instance completed, ending progress dialog", LoggingFrequency.PreloaderLogging);

                    progressDialog.Close();
                    progressDialog?.Dispose();
                    progressDialog = null;

                    finishDelayCts.Cancel();
                    finishDelayCts.Dispose();

                    EndInstance();
                    return;
                }
            }
        }

        private CancellationTokenSource finishDelayCts = new();

        private async Task DelayWithCancellation(TimeSpan delay, bool updateDoneTitle = false)
        {
            const int stepMs = 100;

            if (delay == TimeSpan.Zero) // Never close unless cancelled
            {
                if (updateDoneTitle)
                    progressDialog.Title = $"Finished at {doneCompletedAt:yyyy-MM-dd h:mm:ss tt}";

                while (!finishDelayCts.IsCancellationRequested && !progressDialog.HasUserCancelled)
                {
                    await Task.Delay(stepMs);
                }

                return;
            }

            DateTime closeAt = DateTime.Now + delay;
            int lastSecondsShown = -1;

            while (true)
            {
                if (finishDelayCts.IsCancellationRequested || progressDialog.HasUserCancelled)
                    return;

                TimeSpan remaining = closeAt - DateTime.Now;

                if (remaining <= TimeSpan.Zero)
                    break;

                if (updateDoneTitle)
                {
                    int secondsShown = (int)Math.Ceiling(remaining.TotalSeconds);

                    if (secondsShown != lastSecondsShown)
                    {
                        UpdateDoneTitleForCountdown(closeAt);
                        lastSecondsShown = secondsShown;
                    }
                }

                await Task.Delay(stepMs);
            }

            if (updateDoneTitle)
                progressDialog.Title = "Closing...";
        }

        private static TimeSpan GetDelayTimeSpan(int amount, string unit)
        {
            if (amount <= 0)
                return TimeSpan.Zero;

            if (unit.Equals("Hours", StringComparison.OrdinalIgnoreCase))
                return TimeSpan.FromHours(amount);

            if (unit.Equals("Minutes", StringComparison.OrdinalIgnoreCase))
                return TimeSpan.FromMinutes(amount);

            return TimeSpan.FromSeconds(amount);
        }

        private static string FormatRemainingTime(TimeSpan remaining)
        {
            if (remaining < TimeSpan.Zero)
                remaining = TimeSpan.Zero;

            int hours = (int)Math.Floor(remaining.TotalHours);

            return $"{hours}h {remaining.Minutes:D2}m {remaining.Seconds:D2}s";
        }

        private void UpdateDoneTitleForCountdown(DateTime closeAt)
        {
            TimeSpan remaining = closeAt - DateTime.Now;

            if (remaining <= TimeSpan.Zero)
            {
                progressDialog.Title = "Closing...";
                return;
            }

            progressDialog.Title = $"Closing in {FormatRemainingTime(remaining)}";
        }

        public static string ShortenPath(string path, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(path))
                return path;

            path = Path.GetFullPath(path);
            if (path.Length <= maxLength)
                return path;

            string root = Path.GetPathRoot(path) ?? "";
            string relative = path.Substring(root.Length).Trim(Path.DirectorySeparatorChar);
            string[] segments = relative.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            string sep = Path.DirectorySeparatorChar.ToString();
            const string ellipsis = "...";

            for (int keep = segments.Length; keep >= 1; keep--)
            {
                string tail = string.Join(sep, segments.Skip(segments.Length - keep));
                string candidate = $"{root}{ellipsis}{sep}{tail}";
                if (candidate.Length <= maxLength)
                    return candidate;
            }

            string fileName = segments.Length > 0 ? segments[^1] : relative;
            string prefix = root.Length + ellipsis.Length + sep.Length < maxLength
                ? root + ellipsis + sep
                : "";

            int budget = Math.Max(1, maxLength - prefix.Length);
            string truncatedName = fileName.Length > budget ? fileName[^budget..] : fileName;

            return prefix + truncatedName;
        }

        private void InitializeCacheCheckTimer()
        {
            WriteLine("Initializing cache check timer", LoggingFrequency.PreloaderLogging);
            cacheCheckTimer = new System.Windows.Forms.Timer();
            cacheCheckTimer.Interval = 1000;
            cacheCheckTimer.Tick += CacheCheckTimer_Tick;
            WriteLine("Cache check timer initialized", LoggingFrequency.PreloaderLogging);
        }

        private void CacheCheckTimer_Tick(object sender, EventArgs e)
        {
            cacheCheckTimer.Stop();
            CheckCacheReset(currentFile);
            cacheCheckTimer.Start();
        }

        private void CheckCacheReset(string item)
        {
            if (Settings.Default.ThumbsResetAlert && currentLoggingFrequency == LoggingFrequency.DebugLogging)
            {
                long currentCacheSize = CacheForm.ExplorerCacheSize();
                if (initialCacheSize > currentCacheSize)
                {
                    if (!cacheReset)
                    {
                        WriteLine("WARNING: Thumbnail cache has been reset at file: " + item, LoggingFrequency.DebugLogging);
                    }
                    cacheReset = true;
                }
                else if (currentCacheSize > initialCacheSize)
                {
                    cacheReset = false;
                }
            }
        }

        public long initialCacheSize = 0;
        public bool cacheReset = false;
        public double preloaderElapsedSeconds;

        // TODO: Make the directory scanner output relative paths and use them instead of generating relative paths in the loop to improve performance and reduce memory usage (I ran out of time to implement this before beta 7)
        private async Task Run()
        {
            WriteLine("Running Preloader - Run()", LoggingFrequency.PreloaderLogging);
            await Task.Run(() =>
            {
                state = ThumbnailsPreloaderState.GettingNumberOfItems;
                WriteLine("Preloader state: " + state, LoggingFrequency.PreloaderLogging);

                var directoryStopWatch = new Stopwatch();
                if (currentLoggingFrequency == LoggingFrequency.PreloaderLogging || currentLoggingFrequency >= LoggingFrequency.AllLogging) { directoryStopWatch = Stopwatch.StartNew(); }

                string[] items = directoryScanner.GetItems();
                totalItemsCount = directoryScanner.totalItemsCount; // This is also here in case the program is run in silent mode

                if (currentLoggingFrequency == LoggingFrequency.PreloaderLogging || currentLoggingFrequency >= LoggingFrequency.AllLogging) { directoryStopWatch.Stop(); }
                WriteLine($"Directory scanning completed in {directoryStopWatch.Elapsed.TotalSeconds:F2} seconds.", LoggingFrequency.PreloaderLogging);

                //Debug code for testing directory scanner optimizations
                //MessageBox.Show("Directory scanning completed in " + directoryStopWatch.Elapsed.TotalSeconds.ToString("F2") + " seconds.", "Directory Scanning Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Environment.Exit(0);

                if (totalItemsCount == 0)
                {
                    WriteLine("No items found", LoggingFrequency.PreloaderLogging);

                    if (state != ThumbnailsPreloaderState.Canceled)
                    {
                        state = ThumbnailsPreloaderState.Done;
                    }
                    WriteLine("Preloader state: " + state, LoggingFrequency.PreloaderLogging);

                    preloaderElapsedSeconds = 0;

                    FinalizeInstance();
                    return;
                }
                WriteLine("Total items count: " + totalItemsCount, LoggingFrequency.PreloaderLogging);

                if (state != ThumbnailsPreloaderState.Canceled)
                {
                    state = ThumbnailsPreloaderState.Processing; // Start processing
                }
                WriteLine("Preloader state: " + state, LoggingFrequency.PreloaderLogging);

                using (Process p = Process.GetCurrentProcess())
                {
                    if (Settings.Default.PreloaderProcessPriority.Equals("Realtime")) { p.PriorityClass = ProcessPriorityClass.RealTime; }
                    else if (Settings.Default.PreloaderProcessPriority.Equals("High")) { p.PriorityClass = ProcessPriorityClass.High; }
                    else if (Settings.Default.PreloaderProcessPriority.Equals("Above Normal")) { p.PriorityClass = ProcessPriorityClass.AboveNormal; }
                    else if (Settings.Default.PreloaderProcessPriority.Equals("Normal")) { p.PriorityClass = ProcessPriorityClass.Normal; }
                    else if (Settings.Default.PreloaderProcessPriority.Equals("Below Normal")) { p.PriorityClass = ProcessPriorityClass.BelowNormal; }
                    else if (Settings.Default.PreloaderProcessPriority.Equals("Idle")) { p.PriorityClass = ProcessPriorityClass.Idle; }
                }
                WriteLine("Process priority set to " + Settings.Default.PreloaderProcessPriority, LoggingFrequency.PreloaderLogging);

                if (currentLoggingFrequency == LoggingFrequency.DebugLogging)
                {
                    WriteLine("Preloading thumbnails", LoggingFrequency.DebugLogging);
                }
                else
                {
                    WriteLine("Preloading thumbnails \nUse Debug Logging in single-threaded mode for more detailed information (Slows thumbnail generation)", LoggingFrequency.PreloaderLogging);

                }
                if (Settings.Default.ThumbsResetAlert == true && currentLoggingFrequency != LoggingFrequency.DebugLogging && currentLoggingFrequency != LoggingFrequency.NoLogging) 
                {
                    cacheCheckTimer?.Start();
                    WriteLine("Cache check timer started", LoggingFrequency.PreloaderLogging);
                }

                var PreloaderStopWatch = Stopwatch.StartNew();

                if (!multiThreaded)
                {
                    initialCacheSize = CacheForm.ExplorerCacheSize();
                    foreach (string item in items)
                    {
                        if (state == ThumbnailsPreloaderState.Canceled)
                        {
                            break;
                        }
                        WriteLine("Preloading thumbnail for: " + item, LoggingFrequency.DebugLogging);
                        try
                        {
                            currentFile = item;
                            string relativePath = Path.GetRelativePath(path, item);

                            if (relativePath == "." || string.IsNullOrWhiteSpace(relativePath))
                            {
                                ThumbnailPreloader.PreloadThumbnail(item, thumbnailSizes);
                            }
                            else
                            {
                                ThumbnailPreloader.PreloadThumbnail(path, relativePath, thumbnailSizes);
                            }
                            WriteLine("Preloading thumbnail done for: " + item, LoggingFrequency.DebugLogging);
                            CheckCacheReset(item);
                        }
                        catch (Exception e)
                        {
                            WriteLine($"Exception thrown while preloading thumbnail '{item}': " + e.Message, LoggingFrequency.PreloaderLogging);
                        }
                        processedItemsCount++;
                        if (InstanceCompleted()) 
                        {
                            break; 
                        }
                    }
                    if (state != ThumbnailsPreloaderState.Canceled)
                    {
                        state = ThumbnailsPreloaderState.Done;
                    }
                }
                else
                {
                    WriteLine("(Note: COM exceptions in multithreaded mode may be harmless due to shell handlers)", LoggingFrequency.DebugLogging);

                    int nextIndex = -1;

                    Parallel.For(
                        0,
                        threads,
                        new ParallelOptions { MaxDegreeOfParallelism = threads },
                        workerNumber =>
                        {
                            while (state != ThumbnailsPreloaderState.Canceled)
                            {
                                int index = Interlocked.Increment(ref nextIndex);

                                if (index >= items.Length)
                                    break;

                                string item = items[index];

                                try
                                {
                                    Volatile.Write(ref currentFile, item);

                                    string relativePath = Path.GetRelativePath(path, item);

                                    if (relativePath == "." || string.IsNullOrWhiteSpace(relativePath))
                                    {
                                        ThumbnailPreloader.PreloadThumbnail(item, thumbnailSizes);
                                    }
                                    else
                                    {
                                        ThumbnailPreloader.PreloadThumbnail(path, relativePath, thumbnailSizes);
                                    }
                                }
                                catch (Exception e)
                                {
                                    WriteLine($"Exception thrown while preloading thumbnail " + $"'{item}': {e.Message}", LoggingFrequency.PreloaderLogging);
                                }

                                Interlocked.Increment(ref processedItemsCount);
                            }
                        });
                    if (state != ThumbnailsPreloaderState.Canceled)
                    {
                        state = ThumbnailsPreloaderState.Done;
                    }
                }
                PreloaderStopWatch.Stop();
                preloaderElapsedSeconds = PreloaderStopWatch.Elapsed.TotalSeconds;
                WriteLine($"Thumbnail preloading completed in {preloaderElapsedSeconds:F2} seconds.", LoggingFrequency.PreloaderLogging);

                if (InstanceCompleted()) 
                {
                    Array.Clear(items, 0, items.Length);
                    items = null;
                    FinalizeInstance();
                    return; 
                }
            });
        }

        private int DetermineThreadCount()
        {
            // User did NOT provide an explicit count
            if (threadCount < 0)
            {
                int saved = Settings.Default.ThreadCount;

                if (saved > 0)
                    return Math.Min(saved, 512);

                return Environment.ProcessorCount;
            }

            // User explicitly selected auto (0)
            if (threadCount == 0)
                return Environment.ProcessorCount;

            // User entered an exact number
            return Math.Min(threadCount, 512);
        }

        private static uint[] ParseThumbnailSizes()
        {
            try
            {
                return Settings.Default.PreloaderThumbnailSizes
                    .Split([','], StringSplitOptions.RemoveEmptyEntries)
                    .Select(uint.Parse)
                    .OrderByDescending(x => x)
                    .ToArray();
            }
            catch (Exception e)
            {
                WriteLine("Error parsing thumbnail sizes: " + e.Message, LoggingFrequency.PreloaderLogging);
                return [256];
            }
        }

        private bool InstanceCompleted() =>
            state == ThumbnailsPreloaderState.Canceled || state == ThumbnailsPreloaderState.Done;

        bool isFinalizing = false;

        private void FinalizeInstance()
        {
            if (isFinalizing) return; // Prevent multiple calls when multithreading
            isFinalizing = true;

            cacheCheckTimer?.Stop();
            cacheCheckTimer?.Tick -= CacheCheckTimer_Tick;
            cacheCheckTimer?.Dispose();
            cacheCheckTimer = null;
            WriteLine("Cache check timer stopped", LoggingFrequency.PreloaderLogging);

            if (state == ThumbnailsPreloaderState.Done)
            {
                WriteLine("Preloader state: " + state, LoggingFrequency.PreloaderLogging);
                WriteLine("Preloader has finished", LoggingFrequency.PreloaderLogging);
            }
            else if (state == ThumbnailsPreloaderState.Canceled)
            {
                WriteLine("Preloader state: " + state, LoggingFrequency.PreloaderLogging);
                WriteLine("Preloader has been canceled", LoggingFrequency.PreloaderLogging);
            }
        }

        private void EndInstance()
        {
            if (hasDecrementedActiveInstances)
                return;

            hasDecrementedActiveInstances = true;

            int remainingInstances =
                Interlocked.Decrement(ref Program.activeInstances);

            WriteLine($"Instance completed. Active instances remaining: {remainingInstances}", LoggingFrequency.PreloaderLogging);

            _instanceCompletion.TrySetResult(true);

            if (remainingInstances == 0)
            {
                if (!Program.formOpen)
                {
                    WriteLine("No forms or preloaders remain; exiting application.", LoggingFrequency.GUILogging);

                    Application.Exit();
                }
            }
        }
    }
}