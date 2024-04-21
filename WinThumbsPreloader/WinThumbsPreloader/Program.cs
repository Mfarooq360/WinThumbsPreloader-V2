using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Windows.Automation;
using System.Windows.Forms;
using WinThumbsPreloader.Forms;
using static WinThumbsPreloader.Logger;
using WinThumbsPreloader.Properties;

namespace WinThumbsPreloader
{
    static class Program
    {
        public static Options AppOptions { get; private set; }

        public static int activeInstances = 0;
        public static bool formOpen = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            InitializeLogger();
            WriteLine("Arguments: " + string.Join(" ", arguments), LoggingFrequency.PreloaderLogging);

            Options options = new Options(arguments);
            AppOptions = options;

            WriteLine($"Options: Bad or no arguments = {options.badOrNoArguments}, Include nested directories = {options.includeNestedDirectories}, Multithreaded = {options.multiThreaded}, Silent mode = {options.silentMode}, Start minimized = {options.startMinimized}, Thread count = {options.threadCount}", LoggingFrequency.PreloaderLogging);
            WriteLine($"Paths: { string.Join(Environment.NewLine, options.paths) }", LoggingFrequency.PreloaderLogging);

            if (options.startMinimized)
            {
                WriteLine("Starting GUI cache form minimized", LoggingFrequency.GUILogging);

                StartMinimized();
            }
            else if (options.badOrNoArguments || options.paths.Count == 0 || options.paths == null)
            {
                WriteLine("Starting GUI", LoggingFrequency.AllLogging);

                OpenAboutForm();
            }
            else if (options.paths.Count >= 1)
            {
                WriteLine($"Active Instances: {activeInstances}", LoggingFrequency.DebugLogging);
                WriteLine("Starting preloader", LoggingFrequency.PreloaderLogging);

                StartPreloader(options);
            }
        }

        private static void StartMinimized()
        {
            formOpen = true;
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CacheForm());
        }

        private static void OpenAboutForm()
        {
            CheckForAdminRequirement();

            formOpen = true;
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AboutForm());
        }

        public static void StartPreloader(Options options)
        {
            foreach (string path in options.paths)
            {
                WriteLine($"exePath: {path}", LoggingFrequency.PreloaderLogging);

                ThumbnailsPreloader preloader = new ThumbnailsPreloader(path, options.includeNestedDirectories, options.silentMode, options.multiThreaded, options.threadCount);
                activeInstances++;
                WriteLine($"Active Instances: {activeInstances}", LoggingFrequency.DebugLogging);
                preloader.PreloaderCompleted += (sender) =>
                {
                    if (activeInstances == 0 && !formOpen)
                    {
                        Application.Exit();
                    }
                };
            }
            if (!formOpen) Application.Run();
        }

        private static void CheckForAdminRequirement()
        {
            WriteLine("Checking for admin requirement - CheckForAdminRequirement()", LoggingFrequency.GUILogging);

            bool adminRequired = Settings.Default.Admin;
            WriteLine($"Admin required: {adminRequired}", LoggingFrequency.GUILogging);

            if (!SettingsForm.IsRunningAsAdministrator() && adminRequired)
            {
                WriteLine("Restarting as admin", LoggingFrequency.GUILogging);

                SettingsForm.SetRunAsAdmin();
                RestartAsAdmin();
            }
        }

        public static void RestartAsAdmin()
        {
            WriteLine("Restarting as admin - RestartAsAdmin()", LoggingFrequency.GUILogging);

            var exePath = Application.ExecutablePath;
            WriteLine($"exePath: {exePath}", LoggingFrequency.DebugLogging);

            var startInfo = new ProcessStartInfo(exePath)
            {
                Verb = "runas",
                UseShellExecute = true
            };
            WriteLine($"StartInfo: ExecutablePath = {startInfo.FileName}, Verb = {startInfo.Verb}, UseShellExecute = {startInfo.UseShellExecute}", LoggingFrequency.DebugLogging);

            try
            {
                Process.Start(startInfo);
                Application.Exit();
            }
            catch (Exception ex)
            {
                WriteLine($"Failed to start as administrator: {ex.Message}", LoggingFrequency.GUILogging);
                MessageBox.Show("Failed to start as administrator. The application will continue without elevated privileges.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void OpenFormCentered(this Form currentForm, Form newForm)
        {
            WriteLine("Opening form centered", LoggingFrequency.GUILogging);
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

        public static void OpenSecondaryFormCentered(this Form currentForm, Form newForm)
        {
            WriteLine("Opening secondary form centered", LoggingFrequency.GUILogging);
            newForm.FormClosed += (s, args) =>
            {
                currentForm.Focus();
            };

            newForm.StartPosition = FormStartPosition.Manual;
            newForm.Location = new Point(currentForm.Location.X + (currentForm.Width - newForm.Width) / 2,
                                         currentForm.Location.Y + (currentForm.Height - newForm.Height) / 2);
            newForm.Owner = currentForm;
            newForm.Show();
        }
    }
}
