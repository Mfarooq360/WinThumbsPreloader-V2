using System;
using System.Windows.Forms;
using System.IO;
using WinThumbsPreloader.Properties;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using static System.Windows.Forms.Design.AxImporter;

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
        
        public ThumbnailsPreloader(string path, bool includeNestedDirectories, bool silentMode, bool multiThreaded, int threadCount)
        {
            // Single File Mode for when passing a file through the command line

            FileAttributes fAt = File.GetAttributes(path);
            // Path is file and not a directory, so the application had to be run in single file mode:
            if (!fAt.HasFlag(FileAttributes.Directory))
            {
                ThumbnailPreloader.PreloadThumbnail(path); // Generating thumbnail
                state = ThumbnailsPreloaderState.Done;
                if (InstanceCompleted() == true) { EndInstance(); return; }
            }

            // Normal mode for when passing a directory through the command line

            if (!silentMode)
            {
                InitProgressDialog();
                InitProgressDialogUpdateTimer();
            }
            this.includeNestedDirectories = includeNestedDirectories;
            this.multiThreaded = multiThreaded;
            this.threadCount = threadCount;
            this.path = path;
            Run();
        }
        
        private void InitProgressDialog()
        {
            progressDialog = new ProgressDialog();
            progressDialog.ShowTimeRemaining = false;
            progressDialog.Title = Resources.ThumbnailsPreloader_Title;
            progressDialog.CancelMessage = Resources.ThumbnailsPreloader_CancelMessage;
            progressDialog.Show();
            UpdateProgressDialog(null, null);
        }
        
        private void InitProgressDialogUpdateTimer()
        {
            progressDialogUpdateTimer = new Timer();
            progressDialogUpdateTimer.Interval = 250;
            progressDialogUpdateTimer.Tick += new EventHandler(UpdateProgressDialog);
            progressDialogUpdateTimer.Start();
        }
        
        private async void UpdateProgressDialog(object sender, EventArgs e)
        {
            if (progressDialog.HasUserCancelled)
            {
                state = ThumbnailsPreloaderState.Canceled;
                progressDialog.Close();
                progressDialog?.Dispose();
                progressDialogUpdateTimer.Stop();
                progressDialogUpdateTimer?.Dispose();
                if (InstanceCompleted() == true) 
                {
                    EndInstance();
                    return; 
                }
            }
            else if (state == ThumbnailsPreloaderState.GettingNumberOfItems)
            {
                if (prevState != state)
                {
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
                    if (Settings.Default.AutoBackupThumbs == true && CacheForm.CompareThumbsCacheSize() == true && Program.activeInstances <= 1)
                    {
                        prevState = state;
                        progressDialog.Line3 = Resources.ThumbnailsPreloader_BackingUpThumbsCache;
                        await CacheForm.BackupThumbsCache(null);
                    }
                    if (InstanceCompleted() == true) 
                    {
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
        
        private async void Run()
        {
            await Task.Run(() =>
            {
                state = ThumbnailsPreloaderState.GettingNumberOfItems;

                directoryScanner = new DirectoryScanner(path, includeNestedDirectories);
                List<string> items = new List<string>();
                foreach (Tuple<int, List<string>> itemsCount in directoryScanner.GetItemsCount()) //Get items and items count
                {
                    totalItemsCount = itemsCount.Item1;
                    items = itemsCount.Item2;
                }
                if (totalItemsCount == 0 )
                {
                    state = ThumbnailsPreloaderState.Done;
                }

                state = ThumbnailsPreloaderState.Processing; //Start processing
                
                //Set the process priority to Below Normal to prevent system unresponsiveness
                using (Process p = Process.GetCurrentProcess())
                    p.PriorityClass = ProcessPriorityClass.BelowNormal;

                //Set the thread count
                int threads = DetermineThreadCount();

                if (!multiThreaded)
                {
                    foreach (string item in items)
                    {
                        try
                        {
                            currentFile = item;
                            ThumbnailPreloader.PreloadThumbnail(item);
                        }
                        catch (Exception) { } // Do nothing
                        processedItemsCount++;
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
                            try
                            {
                                currentFile = item;
                                ThumbnailPreloader.PreloadThumbnail(item);
                            }
                            catch (Exception) { } // Do nothing
                            processedItemsCount++;
                        });
                    state = ThumbnailsPreloaderState.Done;
                }
                if (InstanceCompleted() == true) { EndInstance(); return; }
            });
        }

        private int DetermineThreadCount()
        {
            if (threadCount == 0) //If the threadCount is 0, use the settings default. This is when threadCount is not specified.
            {
                return Settings.Default.ThreadCount == 0 ? Environment.ProcessorCount : Settings.Default.ThreadCount;
            }
            if (threadCount == 1) //If the threadCount is 1, use system thread count
            {
                return Environment.ProcessorCount;
            }
            return threadCount <= 257 ? threadCount - 1 : Environment.ProcessorCount; //For other cases, use threadCount - 1
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
