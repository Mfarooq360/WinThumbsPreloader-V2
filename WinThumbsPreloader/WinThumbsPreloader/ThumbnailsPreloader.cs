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

        List<string> NotThreadSafeFileTypes = (new string[] { "heic", "mp4", "mov", "png", "jpg", "jpeg" }).ToList();


        public ThumbnailsPreloader(string path, bool includeNestedDirectories, bool silentMode, bool multiThreaded)
        {

            //
            // Single File Mode (HEIC and other files that cause this app to crash while using Parallel)
            //

            executablePath = Process.GetCurrentProcess().MainModule.FileName;
            FileAttributes fAt = File.GetAttributes(path);
            if(! fAt.HasFlag(FileAttributes.Directory)) // path is file and not a directory, so the application had to be run in single file mode:
            {


                if( NotThreadSafeFileTypes.Contains( new FileInfo(path).Extension.ToLower().TrimStart('.') ))
                {
                    //it'a a heic or other, not threadsafe file , let's process it directly
                    ThumbnailPreloader thumbnailPreloader = new ThumbnailPreloader();
                    thumbnailPreloader.PreloadThumbnail(path); // generating thumbnail
                    Environment.Exit(0); //  done, let's exit this app instance

                }

            }


            //
            // EOF Single File Mode
            //





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
                //Get total items count
                state = ThumbnailsPreloaderState.GettingNumberOfItems;
                foreach (int itemsCount in directoryScanner.GetItemsCount())
                {
                    totalItemsCount += itemsCount;
                    if (state == ThumbnailsPreloaderState.Canceled) return;
                }
                if (totalItemsCount == 0)
                {
                    state = ThumbnailsPreloaderState.Done;
                    return;
                }
                //Start processing
                state = ThumbnailsPreloaderState.Processing;
                ThumbnailPreloader thumbnailPreloader = new ThumbnailPreloader();
                //Get the items first before doing work
                List<string> items = directoryScanner.GetItemsBulk();
                if (!_multiThreaded)
                {
                    foreach (string item in items)
                    {

                        FileAttributes fAt = File.GetAttributes(item);
                        if (fAt.HasFlag(FileAttributes.Directory))
                            continue; ;

                        currentFile = item;
                        if (NotThreadSafeFileTypes.Contains(new FileInfo(item).Extension.ToLower().TrimStart('.'))) // launch a copy of app for each HEIC or other not threadsafe file, aka start in single file mode
                        {
                            Process.Start(executablePath, '"' + item + '"').WaitForExit();
                        }
                        else
                        {
                            thumbnailPreloader.PreloadThumbnail(item);
                        }
                        processedItemsCount++;
                        if (processedItemsCount == totalItemsCount) state = ThumbnailsPreloaderState.Done;
                        if (state == ThumbnailsPreloaderState.Canceled) return;
                    }
                }
                else {
                    Parallel.ForEach(
                        items,
                        new ParallelOptions { MaxDegreeOfParallelism = 2048 },
                        item =>
                        {
                            // let's skip directories
                            FileAttributes fAt = File.GetAttributes(item);
                            if (fAt.HasFlag(FileAttributes.Directory))
                                return;

                            currentFile = item;
                            if (NotThreadSafeFileTypes.Contains( new FileInfo(item).Extension.ToLower().TrimStart('.') )) // launch a copy of app for each HEIC or other not threadsafe file, aka start in single file mode
                            {
                                Process.Start(executablePath, '"'+item + '"').WaitForExit();
                            }
                            else
                            {
                                thumbnailPreloader.PreloadThumbnail(item);
                            }
                            processedItemsCount++;
                            if (processedItemsCount == totalItemsCount) state = ThumbnailsPreloaderState.Done;
                            if (state == ThumbnailsPreloaderState.Canceled) return;
                        });
                }       
            });
            Application.Exit();
        }
    }
}
