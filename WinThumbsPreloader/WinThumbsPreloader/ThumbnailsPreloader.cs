using System;
using System.Windows.Forms;
using System.IO;
using WinThumbsPreloader.Properties;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
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

        protected bool _multiThreaded;

        public ThumbnailsPreloaderState state = ThumbnailsPreloaderState.GettingNumberOfItems;
        public ThumbnailsPreloaderState prevState = ThumbnailsPreloaderState.New;
        public int totalItemsCount = 0;
        public int processedItemsCount = 0;
        public string currentFile = "";

        string executablePath = "";

        public ThumbnailsPreloader(string path, bool includeNestedDirectories, bool silentMode, bool multiThreaded)
        {
            //Set the process priority to Below Normal to prevent system unresponsiveness
            using (Process p = Process.GetCurrentProcess())
                p.PriorityClass = ProcessPriorityClass.BelowNormal;

            // Single File Mode for when passing a file through the command line or preloading a .svg file
            executablePath = Process.GetCurrentProcess().MainModule.FileName;
            FileAttributes fAt = File.GetAttributes(path);
            if (!fAt.HasFlag(FileAttributes.Directory)) // path is file and not a directory, so the application had to be run in single file mode:
            {
                ThumbnailPreloader.PreloadThumbnail(path); // generating thumbnail
                Environment.Exit(0); //  done, let's exit this app instance
            }

            // Normal mode for when passing a directory through the command line
            directoryScanner = new DirectoryScanner(path, includeNestedDirectories);
            if (!silentMode)
            {
                InitProgressDialog();
                InitProgressDialogUpdateTimer();
            }
            _multiThreaded = multiThreaded;
            Run();
        }

        private void InitProgressDialog()
        {
            progressDialog = new ProgressDialog();
            progressDialog.AutoClose = false;
            progressDialog.ShowTimeRemaining = false;
            progressDialog.Title = "WinThumbsPreloader";
            progressDialog.CancelMessage = Resources.ThumbnailsPreloader_CancelMessage;
            progressDialog.Maximum = 100;
            progressDialog.Value = 0;
            progressDialog.Show();
            UpdateProgressDialog(null, null);
        }

        private void InitProgressDialogUpdateTimer()
        {
            progressDialogUpdateTimer = new System.Windows.Forms.Timer();
            progressDialogUpdateTimer.Interval = 250;
            progressDialogUpdateTimer.Tick += new EventHandler(UpdateProgressDialog);
            progressDialogUpdateTimer.Start();
        }

        private void UpdateProgressDialog(object sender, EventArgs e)
        {
            if (progressDialog.HasUserCancelled)
            {
                state = ThumbnailsPreloaderState.Canceled;
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
        }

        private async void Run()
        {
            await Task.Run(() =>
            {
                //Set the process priority to Below Normal to prevent system unresponsiveness
                using (Process p = Process.GetCurrentProcess())
                    p.PriorityClass = ProcessPriorityClass.BelowNormal;

                state = ThumbnailsPreloaderState.GettingNumberOfItems;

                List<string> items = new List<string>();
                foreach (Tuple<int, List<string>> itemsCount in directoryScanner.GetItemsCount()) //Get items and items count
                {
                    totalItemsCount = itemsCount.Item1;
                    items = itemsCount.Item2;
                    if (state == ThumbnailsPreloaderState.Canceled) return;
                }
                if (totalItemsCount == 0)
                {
                    state = ThumbnailsPreloaderState.Done;
                    return;
                }

                state = ThumbnailsPreloaderState.Processing; //Start processing
                if (!_multiThreaded)
                {
                    foreach (string item in items)
                    {
                        try
                        {
                            currentFile = item;
                            ThumbnailPreloader.PreloadThumbnail(item);
                            processedItemsCount++;
                            if (processedItemsCount == totalItemsCount) state = ThumbnailsPreloaderState.Done;
                            if (state == ThumbnailsPreloaderState.Canceled) Application.Exit();
                        }
                        catch (Exception) { } // Do nothing
                    }
                }
                else
                {
                    Parallel.ForEach(
                        items,
                        new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                        item =>
                        {
                            try
                            {
                                currentFile = item;
                                ThumbnailPreloader.PreloadThumbnail(item);
                                processedItemsCount++;
                                if (processedItemsCount == totalItemsCount) state = ThumbnailsPreloaderState.Done;
                                if (state == ThumbnailsPreloaderState.Canceled) Application.Exit();
                            }
                            catch (Exception) { } // Do nothing
                        });
                }
            });
            Application.Exit();
        }
    }
}
