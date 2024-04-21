using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;

namespace WinThumbsPreloader
{
    public static class Logger
    {
        public static LoggingFrequency currentLoggingFrequency = Settings.Default.LoggingFrequency == 0 ? LoggingFrequency.NoLogging : (LoggingFrequency)Settings.Default.LoggingFrequency;
        static bool loggerInitialized = false;
        private static bool logFileExists = false;  // New flag to track if the log file is expected to exist
        private const long MaxLogFileSize = 1L * 1024 * 1024 * 1024; // 1GB
        private static string logFilePath; // Path to the current log file

        public enum LoggingFrequency
        {
            NoLogging,
            PreloaderLogging,
            GUILogging,
            AllLogging,
            DebugLogging
        }

        public static LoggingFrequency LogFrequency { get; set; }

        public static void InitializeLogger()
        {
            LogFrequency = currentLoggingFrequency;
            if (LoggingEnabled() && !loggerInitialized)
            {
                try
                {
                    // Get the directory where the application is running
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    // Specify the folder name for logs
                    string logDirectory = Path.Combine(appDirectory, "WinThumbsPreloader Logs");
                    // Create the directory if it doesn't exist
                    Directory.CreateDirectory(logDirectory);

                    // Format the log file name with the current date to ensure a new file is created each day
                    string logFileName = $"WinThumbsPreloaderLog_{DateTime.Now:yyyyMMdd}.txt";
                    // Combine the log directory and file name to get the full path
                    logFilePath = Path.Combine(logDirectory, logFileName);

                    // Open a FileStream with FileMode.Append to continue writing to the file if it already exists or create a new one for the day
                    FileStream logFileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                    StreamWriter logStreamWriter = new StreamWriter(logFileStream) { AutoFlush = true };
                    Trace.Listeners.Add(new TextWriterTraceListener(logStreamWriter));
                    Trace.AutoFlush = true;
                    logFileExists = true; // Set flag to true as the file is now created or opened
                    loggerInitialized = true;

                    // Log initialization messages
                    WriteLine("Logger initialized - InitializeLogger()", LoggingFrequency.DebugLogging);
                    WriteLine($"Log frequency: {LogFrequency}", LoggingFrequency.DebugLogging);
                    WriteLine($"Logging to file: {logFilePath}", LoggingFrequency.DebugLogging);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Failed to properly initialize logger: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            CheckLogFileSizeAndRotateIfNeeded();
        }

        // Method to write a log entry, accepting the logging frequency directly
        public static void WriteLine(string message, LoggingFrequency frequency)
        {
            // Check if the current log frequency allows logging this message
            if (ShouldLog(frequency))
            {
                CheckLogFileSizeAndRotateIfNeeded();
                try
                {
                    Trace.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {message}");
                }
                catch (Exception) { }
            }
        }

        private static void CheckLogFileSizeAndRotateIfNeeded()
        {
            if (logFileExists)
            {
                var fileInfo = new FileInfo(logFilePath);
                if (fileInfo.Length >= MaxLogFileSize)
                {
                    RotateLogFile();
                }
            }
        }

        // Rotate the log file
        private static void RotateLogFile()
        {
            // Close existing log streams
            Trace.Flush();
            Trace.Listeners.Clear();

            // Move the current log file to a new file
            string archiveFilePath = Path.Combine(Path.GetDirectoryName(logFilePath),
                                                  Path.GetFileNameWithoutExtension(logFilePath) +
                                                  "_archive_" +
                                                  DateTime.Now.ToString("yyyyMMddHHmmss") +
                                                  Path.GetExtension(logFilePath));
            File.Move(logFilePath, archiveFilePath);

            logFileExists = false; // Update flag since the original file is moved
            // Reinitialize the logger to start a new log file
            InitializeLogger();
        }

        // Helper method to check if the message should be logged based on the frequency
        private static bool ShouldLog(LoggingFrequency frequency)
        {
            return LogFrequency == frequency || (frequency != LoggingFrequency.DebugLogging && LogFrequency == LoggingFrequency.AllLogging) || (frequency == LoggingFrequency.DebugLogging && LogFrequency == LoggingFrequency.DebugLogging) || (frequency == LoggingFrequency.AllLogging && (LogFrequency == LoggingFrequency.PreloaderLogging || LogFrequency == LoggingFrequency.GUILogging)) || ((frequency == LoggingFrequency.PreloaderLogging && LogFrequency == LoggingFrequency.PreloaderLogging) || (frequency == LoggingFrequency.GUILogging && LogFrequency == LoggingFrequency.GUILogging)) || (LogFrequency == LoggingFrequency.DebugLogging);
        }

        public static bool LoggingEnabled()
        {
            if (LogFrequency != LoggingFrequency.NoLogging) return true;
            else return false;
        }
    }
}
