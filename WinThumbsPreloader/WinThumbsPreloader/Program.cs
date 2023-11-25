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
    static class Program
    {
        public static Options AppOptions { get; private set; }

        public static int activeInstances = 0;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            Options options = new Options(arguments);
            AppOptions = options;

            if (options.startMinimized)
            {
                StartMinimized();
            }
            else if (options.badArguments || options.paths.Count == 0 || options.paths == null)
            {
                OpenAboutForm();
            }
            else if (options.paths.Count >= 1)
            {
                StartPreloader(options);
            }
        }

        private static void StartMinimized()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CacheForm());
        }

        private static void OpenAboutForm()
        {
            CheckForAdminRequirement();

            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AboutForm());
        }

        private static void StartPreloader(Options options)
        {
            foreach (string path in options.paths)
            {
                ThumbnailsPreloader preloader = new ThumbnailsPreloader(path, options.includeNestedDirectories, options.silentMode, options.multiThreaded, options.threadCount);
                activeInstances++;
                preloader.PreloaderCompleted += (sender) =>
                {
                    if (activeInstances == 0)
                    {
                        Application.Exit();
                    }
                };
            }
            Application.Run();
        }

        private static void CheckForAdminRequirement()
        {
            bool adminRequired = Properties.Settings.Default.Admin;
            if (!SettingsForm.IsRunningAsAdministrator() && adminRequired)
            {
                SettingsForm.SetRunAsAdmin();
                RestartAsAdmin();
            }
        }

        public static void RestartAsAdmin()
        {
            var exePath = Application.ExecutablePath;
            var startInfo = new ProcessStartInfo(exePath)
            {
                Verb = "runas",
                UseShellExecute = true
            };

            try
            {
                Process.Start(startInfo);
                Application.Exit();
            }
            catch
            {
                MessageBox.Show("Failed to start as administrator. The application will continue without elevated privileges.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        public static void OpenSecondaryFormCentered(this Form currentForm, Form newForm)
        {
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
