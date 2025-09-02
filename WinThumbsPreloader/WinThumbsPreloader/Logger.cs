using System;
using System.Diagnostics;
using System.IO;
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
        private static StreamWriter logStreamWriter;
        private static FileStream logFileStream;

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
                    string baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string logDirectory = Path.Combine(baseDirectory, "WinThumbsPreloader", "Logs");

                    Directory.CreateDirectory(logDirectory); // Ensure folder exists

                    string logFileName = $"WinThumbsPreloaderLog_{DateTime.Now:yyyyMMdd}.txt";
                    logFilePath = Path.Combine(logDirectory, logFileName);

                    logFileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                    logStreamWriter = new StreamWriter(logFileStream) { AutoFlush = true };
                    Trace.Listeners.Add(new TextWriterTraceListener(logStreamWriter));
                    Trace.AutoFlush = true;
                    logFileExists = true;
                    loggerInitialized = true;

                    WriteLine("Logger initialized - InitializeLogger()", LoggingFrequency.DebugLogging);
                    WriteLine($"Log frequency: {LogFrequency}", LoggingFrequency.DebugLogging);
                    WriteLine($"Logging to file: {logFilePath}", LoggingFrequency.DebugLogging);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Failed to properly initialize logger: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (!LoggingEnabled() && loggerInitialized)
            {
                CloseLogger();
            }

            CheckLogFileSizeAndRotateIfNeeded();
        }

        public static void WriteLine(string message, LoggingFrequency frequency)
        {
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

        private static void RotateLogFile()
        {
            Trace.Flush();
            Trace.Listeners.Clear();

            string archiveFilePath = Path.Combine(Path.GetDirectoryName(logFilePath),
                                                  Path.GetFileNameWithoutExtension(logFilePath) +
                                                  "_archive_" +
                                                  DateTime.Now.ToString("yyyyMMddHHmmss") +
                                                  Path.GetExtension(logFilePath));
            File.Move(logFilePath, archiveFilePath);

            logFileExists = false;
            InitializeLogger(); // Reinitialize the logger to start a new log file
        }

        // Helper method to check if the message should be logged based on the frequency
        private static bool ShouldLog(LoggingFrequency frequency)
        {
            return LogFrequency == frequency || (frequency != LoggingFrequency.DebugLogging && LogFrequency == LoggingFrequency.AllLogging) || (frequency == LoggingFrequency.DebugLogging && LogFrequency == LoggingFrequency.DebugLogging) || (frequency == LoggingFrequency.AllLogging && (LogFrequency == LoggingFrequency.PreloaderLogging || LogFrequency == LoggingFrequency.GUILogging)) || ((frequency == LoggingFrequency.PreloaderLogging && LogFrequency == LoggingFrequency.PreloaderLogging) || (frequency == LoggingFrequency.GUILogging && LogFrequency == LoggingFrequency.GUILogging)) || (LogFrequency == LoggingFrequency.DebugLogging);
        }

        public static bool LoggingEnabled()
        {
            return LogFrequency != LoggingFrequency.NoLogging;
        }

        public static void CloseLogger()
        {
            if (loggerInitialized)
            {
                try
                {
                    Trace.Flush();
                    Trace.Listeners.Clear();
                    Trace.Close();

                    logStreamWriter?.Flush();
                    logStreamWriter?.Close();
                    logStreamWriter?.Dispose();
                    logStreamWriter = null;

                    logFileStream?.Close();
                    logFileStream?.Dispose();
                    logFileStream = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while closing logger: {ex.Message}", "Logger Close Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                loggerInitialized = false;
                logFileExists = false;
            }
        }
    }
}
