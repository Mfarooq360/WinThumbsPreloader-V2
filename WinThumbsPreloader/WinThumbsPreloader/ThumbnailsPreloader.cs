using System;
using System.Windows.Forms;
using System.IO;
using WinThumbsPreloader.Properties;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using static WinThumbsPreloader.Logger;
using System.Linq;

namespace WinThumbsPreloader
{
    public enum ThumbnailsPreloaderState
    {
        New,
        GettingNumberOfItems,
        Processing,
        Canceled,
        Done
    }

    //Preload all thumbnails, show progress dialog
    class ThumbnailsPreloader
    {
        private DirectoryScanner directoryScanner;
        private ProgressDialog progressDialog;
        private Timer progressDialogUpdateTimer;
        private Timer cacheCheckTimer;
        private bool includeNestedDirectories;
        private bool multiThreaded;
        private string path;
        private int threadCount;

        public ThumbnailsPreloaderState state = ThumbnailsPreloaderState.GettingNumberOfItems;
        public ThumbnailsPreloaderState prevState = ThumbnailsPreloaderState.New;
        public event Action<ThumbnailsPreloader> PreloaderCompleted;
        private bool hasDecrementedActiveInstances = false;
        public int totalItemsCount = 0;
        public int processedItemsCount = 0;
        public string currentFile = "";
        public int[] thumbnailSizes;

        public ThumbnailsPreloader(string path, bool includeNestedDirectories, bool silentMode, bool multiThreaded, int threadCount)
        {
            WriteLine("Initializing Preloader - ThumbnailsPreloader(string, bool, bool, bool, int)", LoggingFrequency.PreloaderLogging);
            thumbnailSizes = ParseThumbnailSizes();
            WriteLine("thumbnailSizes: " + string.Join(", ", thumbnailSizes), LoggingFrequency.PreloaderLogging);
            // Single File Mode for when passing a file through the command line

            FileAttributes fAt = File.GetAttributes(path);
            WriteLine("Path is a file: " + fAt.HasFlag(FileAttributes.Directory), LoggingFrequency.PreloaderLogging);
            // Path is file and not a directory, so the application had to be run in single file mode:
            if (!fAt.HasFlag(FileAttributes.Directory))
            {
                WriteLine("Preloading single file: " + path, LoggingFrequency.PreloaderLogging);
                ThumbnailPreloader.PreloadThumbnail(path, thumbnailSizes); // Generating thumbnail
                WriteLine("Preloading single file done: " + path, LoggingFrequency.PreloaderLogging);
                state = ThumbnailsPreloaderState.Done;
                WriteLine("Preloader state: " + state, LoggingFrequency.PreloaderLogging);
                if (InstanceCompleted()) 
                { 
                    WriteLine("Instance completed, ending instance.", LoggingFrequency.PreloaderLogging);
                    EndInstance(); 
                    return; 
                }
            }
            // Normal mode for when passing a directory through the command line, context menu, or GUI

            if (!silentMode)
            {
                InitProgressDialog();
                InitProgressDialogUpdateTimer();
                if (Settings.Default.ThumbsResetAlert == true && currentLoggingFrequency != LoggingFrequency.DebugLogging && currentLoggingFrequency != LoggingFrequency.NoLogging) { InitializeCacheCheckTimer(); }
            }
            this.includeNestedDirectories = includeNestedDirectories;
            WriteLine("IncludeNestedDirectories: " + includeNestedDirectories, LoggingFrequency.PreloaderLogging);
            this.multiThreaded = multiThreaded;
            WriteLine("MultiThreaded: " + multiThreaded, LoggingFrequency.PreloaderLogging);
            this.threadCount = threadCount;
            WriteLine("ThreadCount: " + threadCount, LoggingFrequency.PreloaderLogging);
            this.path = path;
            WriteLine("Path: " + path, LoggingFrequency.PreloaderLogging);
            Run();
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
            progressDialogUpdateTimer = new Timer();
            progressDialogUpdateTimer.Interval = 250;
            progressDialogUpdateTimer.Tick += new EventHandler(UpdateProgressDialog);
            progressDialogUpdateTimer.Start();
            WriteLine("Progress dialog update timer initialized", LoggingFrequency.PreloaderLogging);
        }
        bool statusLogged = false;
        bool statusLogged2 = false;

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
                state = ThumbnailsPreloaderState.Canceled;
                progressDialog.Close();
                progressDialog?.Dispose();
                progressDialogUpdateTimer.Stop();
                progressDialogUpdateTimer?.Dispose();
                if (InstanceCompleted())
                {
                    progressDialog.Close();
                    progressDialog?.Dispose();
                    progressDialogUpdateTimer.Stop();
                    progressDialogUpdateTimer?.Dispose();
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
                    progressDialog.Line1 = Resources.ThumbnailsPreloader_PreloadingThumbnails;
                    progressDialog.Line3 = Resources.ThumbnailsPreloader_CalculatingNumberOfItems;
                    progressDialog.Marquee = true;
                }
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
                if (prevState != state)
                {
                    WriteLine("Finalizing progress dialog", LoggingFrequency.DebugLogging);
                    if (Settings.Default.AutoBackupThumbs == true && CacheForm.CompareThumbsCacheSize() == true && Program.activeInstances <= 1)
                    {
                        WriteLine("AutoBackupThumbs is enabled and the cache size has changed, backing up thumbs cache", LoggingFrequency.PreloaderLogging);
                        prevState = state;
                        progressDialog.Line3 = Resources.ThumbnailsPreloader_BackingUpThumbsCache;
                        await CacheForm.BackupThumbsCache(null);
                        WriteLine("Thumbs cache backed up", LoggingFrequency.PreloaderLogging);
                    }
                    if (InstanceCompleted() == true) 
                    {
                        WriteLine("Instance completed, ending progress dialog", LoggingFrequency.PreloaderLogging);
                        progressDialog.Close();
                        progressDialog?.Dispose();
                        progressDialogUpdateTimer.Stop();
                        progressDialogUpdateTimer?.Dispose();
                        EndInstance();
                        return; 
                    }
                }
            }
        }

        private void InitializeCacheCheckTimer()
        {
            WriteLine("Initializing cache check timer", LoggingFrequency.PreloaderLogging);
            cacheCheckTimer = new Timer();
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
        public long initialCacheSize = 0;
        public bool cacheReset = false;

        private async void Run()
        {
            WriteLine("Running Preloader - Run()", LoggingFrequency.PreloaderLogging);
            await Task.Run(() =>
            {
                state = ThumbnailsPreloaderState.GettingNumberOfItems;
                WriteLine("Preloader state: " + state, LoggingFrequency.PreloaderLogging);

                directoryScanner = new DirectoryScanner(path, includeNestedDirectories);
                List<string> items = new List<string>();
                foreach (Tuple<int, List<string>> itemsCount in directoryScanner.GetItemsCount()) //Get items and items count
                {
                    totalItemsCount = itemsCount.Item1;
                    items = itemsCount.Item2;
                }
                if (totalItemsCount == 0)
                {
                    WriteLine("No items found", LoggingFrequency.PreloaderLogging);
                    state = ThumbnailsPreloaderState.Done;
                    WriteLine("Preloader state: " + state, LoggingFrequency.PreloaderLogging);
                }
                WriteLine("Total items count: " + totalItemsCount, LoggingFrequency.PreloaderLogging);

                state = ThumbnailsPreloaderState.Processing; //Start processing
                WriteLine("Preloader state: " + state, LoggingFrequency.PreloaderLogging);

                //Set the process priority to Below Normal to prevent system unresponsiveness
                using (Process p = Process.GetCurrentProcess())
                    p.PriorityClass = ProcessPriorityClass.BelowNormal;
                WriteLine("Process priority set to Below Normal", LoggingFrequency.PreloaderLogging);

                //Set the thread count
                int threads = DetermineThreadCount();
                WriteLine("Thread count: " + threads, LoggingFrequency.PreloaderLogging);
                if (currentLoggingFrequency == LoggingFrequency.DebugLogging)
                {
                    WriteLine("Preloading thumbnails", LoggingFrequency.DebugLogging);
                }
                else
                {
                    WriteLine("Preloading thumbnails \nUse Debug Logging for more detailed information (Slows thumbnail generation)", LoggingFrequency.PreloaderLogging);

                }
                if (Settings.Default.ThumbsResetAlert == true && currentLoggingFrequency != LoggingFrequency.DebugLogging && currentLoggingFrequency != LoggingFrequency.NoLogging) 
                {
                    cacheCheckTimer?.Start();
                    WriteLine("Cache check timer started", LoggingFrequency.PreloaderLogging);
                }

                if (!multiThreaded)
                {
                    initialCacheSize = CacheForm.ExplorerCacheSize();
                    foreach (string item in items)
                    {
                        /*WriteLine("Preloading thumbnail for: " + item, LoggingFrequency.DebugLogging);*/
                        try
                        {
                            currentFile = item;
                            ThumbnailPreloader.PreloadThumbnail(item, thumbnailSizes);
                            /*WriteLine("Preloading thumbnail done for: " + item, LoggingFrequency.DebugLogging);*/
                            CheckCacheReset(item);
                        }
                        catch (Exception e)
                        {
                            WriteLine($"Exception thrown while preloading thumbnail '{item}': " + e.Message, LoggingFrequency.PreloaderLogging);
                        }
                        processedItemsCount++;
                        if (InstanceCompleted()) { return; }
                    }
                    state = ThumbnailsPreloaderState.Done;
                }
                else
                {
                    Parallel.ForEach(
                        items,
                        new ParallelOptions { MaxDegreeOfParallelism = threads },
                        item =>
                        {
                            /*WriteLine("Preloading thumbnail for: " + item, LoggingFrequency.DebugLogging);*/
                            try
                            {
                                currentFile = item;
                                ThumbnailPreloader.PreloadThumbnail(item, thumbnailSizes);
                                /*WriteLine("Preloading thumbnail done for: " + item, LoggingFrequency.DebugLogging);*/
                            }
                            catch (Exception e)
                            {
                                WriteLine($"Exception thrown while preloading thumbnail '{item}': " + e.Message, LoggingFrequency.PreloaderLogging);
                            }
                            processedItemsCount++;
                            if (InstanceCompleted()) { return; }
                        });
                    state = ThumbnailsPreloaderState.Done;
                }
                if (InstanceCompleted()) 
                {
                    cacheCheckTimer?.Stop();
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
                    WriteLine("Instance completed, ending instance.", LoggingFrequency.PreloaderLogging);
                    EndInstance();
                    return;
                }
            });
        }

        private void CheckCacheReset(string item)
        {
            if (currentLoggingFrequency == LoggingFrequency.DebugLogging && Settings.Default.ThumbsResetAlert)
            {
                long currentCacheSize = CacheForm.ExplorerCacheSize();
                if (initialCacheSize > currentCacheSize)
                {
                    if (cacheReset == false)
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

        private int DetermineThreadCount()
        {
            if (threadCount == 0) //If the threadCount is 0, use the settings default unless it isn't set, if so, then use the system thread count instead. This is when threadCount is not specified in the command prompt.
            {
                return Settings.Default.ThreadCount == 0 ? Environment.ProcessorCount : Settings.Default.ThreadCount;
            }
            if (threadCount == 1) //If the threadCount is 1, use system thread count. This is when threadCount is specified as 0.
            {
                return Environment.ProcessorCount;
            }
            return threadCount <= 257 ? threadCount - 1 : Environment.ProcessorCount; //For other cases, use threadCount - 1
        }

        private static int[] ParseThumbnailSizes()
        {
            try
            {
                return Settings.Default.PreloaderThumbnailSizes
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
            }
            catch (Exception e)
            {
                WriteLine("Error parsing thumbnail sizes: " + e.Message, LoggingFrequency.PreloaderLogging);
                return new int[] { 256 }; // Default size
            }
        }

        //This was done to track how many preloading instances are open due to multiple instances now occurring in the same program, which helps to properly end the application when there are no more preloading instances.
        private bool InstanceCompleted()
        {
            if (state == ThumbnailsPreloaderState.Canceled || state == ThumbnailsPreloaderState.Done) { return true; } 
            else { return false; }
        }
        
        private void EndInstance()
        {
            if (!hasDecrementedActiveInstances)
            {
                Program.activeInstances--;
                hasDecrementedActiveInstances = true;
            }
            PreloaderCompleted?.Invoke(this);
        }
    }
}
