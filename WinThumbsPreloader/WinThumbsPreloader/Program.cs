using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Windows.Automation;
using System.Windows.Forms;

namespace WinThumbsPreloader
{
    [SupportedOSPlatform("windows")]
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения. (The main entry point for the application.)
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            /*
            //Test culture
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            */
            Options options = new Options(arguments);
            AppOptions = options;
            if (options.startMinimized == true)
            {
                Application.EnableVisualStyles();
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new CacheForm());
            }
            else if (options.badArguments || options.paths.Count == 0 || options.paths == null)
            { 
                Application.EnableVisualStyles();
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new AboutForm());
            }
            else if (options.paths.Count == 1)
            {
                foreach (string path in options.paths)
                {
                    new ThumbnailsPreloader(path, options.includeNestedDirectories, options.silentMode, options.multiThreaded, options.threadCount);
                }
                Application.Run();
            }
            else if (options.paths.Count > 1) 
            {
                RunNewInstanceForMultiplePaths(options);
                Application.Run();
            }
        }

        public static Options AppOptions { get; private set; }

        private static void RunNewInstanceForMultiplePaths(Options options)
        {
            string includeNestedDirectories = string.Empty;
            string multiThreaded = string.Empty;
            string threadCount = string.Empty;
            string silentMode = string.Empty;
            if (options.includeNestedDirectories == true) { includeNestedDirectories = " -r"; }
            if (options.multiThreaded == true) { multiThreaded = "-m"; }
            if (options.silentMode == true) { silentMode = " -s"; }
            if (options.threadCount > 0) { threadCount = (options.threadCount - 1).ToString(); }
            foreach (string path in options.paths)
            {
                string fixedPath = $"\"{path}\"";
                string trimmedPath = fixedPath.Replace("\\\"", "/").Replace("\"", "");
                // Create a new process info
                var processInfo = new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    Arguments = multiThreaded + threadCount + includeNestedDirectories + silentMode + " " + trimmedPath, // Pass the path as an argument
                    UseShellExecute = false
                };

                // Start the new process
                Process.Start(processInfo);
            }
            Environment.Exit(0);
        }

        public static void OpenFormCentered(this Form currentForm, Form newForm)
        {
            currentForm.Hide();
            newForm.FormClosed += (s, args) =>
            {
                currentForm.Location = new Point(newForm.Location.X + (newForm.Width - currentForm.Width) / 2,
                                                 newForm.Location.Y + (newForm.Height - currentForm.Height) / 2);
                currentForm.Show();
            };

            newForm.StartPosition = FormStartPosition.Manual;
            newForm.Location = new Point(currentForm.Location.X + (currentForm.Width - newForm.Width) / 2,
                                         currentForm.Location.Y + (currentForm.Height - newForm.Height) / 2);
            newForm.Owner = currentForm;
            newForm.ShowDialog();
        }
    }
}
