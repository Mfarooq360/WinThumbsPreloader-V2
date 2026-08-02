using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using WinThumbsPreloader.Properties;

namespace WinThumbsPreloader
{
    public static class Logger
    {
        private const string LogFilePrefix = "WinThumbsPreloaderLog_";
        private const string LogFileSearchPattern = LogFilePrefix + "*.txt";
        private const string AppDataFolderName = "WinThumbsPreloader";
        private const string LogFolderName = "WinThumbsPreloader Logs";
        private const long BytesPerMegabyte = 1024L * 1024L;

        private static readonly object SyncRoot = new object();
        private static readonly TimeSpan MaintenanceInterval = TimeSpan.FromMinutes(10);
        private static readonly Lazy<string> RecommendedDefaultLogPath =
            new Lazy<string>(DetermineRecommendedDefaultLogPath);

        private static bool loggerInitialized;
        private static string logFilePath;
        private static string activeLogDirectory;
        private static StreamWriter logStreamWriter;
        private static FileStream logFileStream;
        private static DateTime lastMaintenanceUtc = DateTime.MinValue;

        public static LoggingFrequency currentLoggingFrequency = ReadLoggingFrequency();

        public enum LoggingFrequency
        {
            NoLogging,
            PreloaderLogging,
            GUILogging,
            AllLogging,
            DebugLogging
        }

        public enum LogFolderStatus
        {
            NotSelected,
            DefaultFolder,
            Writable,
            WritableAdmin,
            WillBeCreated,
            WillBeCreatedAdmin,
            NotWritable,
            InvalidPath
        }

        public sealed class LogFolderStatusInfo
        {
            public LogFolderStatus Status { get; init; }
            public string Message { get; init; }
            public string NormalizedPath { get; init; }
            public bool IsRunningAsAdmin { get; init; }
            public bool UsesDefaultPath { get; init; }

            public bool CanUseForLogging =>
                Status == LogFolderStatus.DefaultFolder ||
                Status == LogFolderStatus.Writable ||
                Status == LogFolderStatus.WritableAdmin ||
                Status == LogFolderStatus.WillBeCreated ||
                Status == LogFolderStatus.WillBeCreatedAdmin;
        }

        public sealed class LogCleanupResult
        {
            public int DeletedByAge { get; internal set; }
            public int FailedDeletes { get; internal set; }
            public long BytesFreed { get; internal set; }

            public int TotalDeleted => DeletedByAge;

        }

        public static LoggingFrequency LogFrequency { get; set; } = currentLoggingFrequency;

        public static string LastError { get; private set; } = string.Empty;

        private static string PortableLogPath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFolderName);

        private static string LocalAppDataLogPath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                AppDataFolderName,
                LogFolderName
            );

        public static void InitializeLogger()
        {
            lock (SyncRoot)
            {
                currentLoggingFrequency = ReadLoggingFrequency();
                LogFrequency = currentLoggingFrequency;
                LastError = string.Empty;

                string requestedDirectory = GetConfiguredLogDirectoryNoLock();

                if (!LoggingEnabledNoLock())
                {
                    CloseWriterNoLock();
                    return;
                }

                if (string.IsNullOrWhiteSpace(requestedDirectory))
                {
                    CloseWriterNoLock();
                    LastError = "Logging is enabled, but no logger folder has been selected.";
                    return;
                }

                RunMaintenanceNoLock(requestedDirectory);

                if (loggerInitialized && logStreamWriter != null && PathsEqual(activeLogDirectory, requestedDirectory))
                {
                    return;
                }

                CloseWriterNoLock();

                if (!TryOpenNewLogFileNoLock(requestedDirectory, out Exception openException))
                {
                    LastError = $"Failed to open the selected log folder '{requestedDirectory}': " + openException.Message;

                    Debug.WriteLine(LastError);
                    return;
                }

                WriteRawNoLock("Logger initialized - InitializeLogger()");
                WriteRawNoLock($"Logging frequency: {LogFrequency}");
                WriteRawNoLock($"Logging to file: {logFilePath}");
            }
        }

        public static void WriteLine(string message, LoggingFrequency frequency)
        {
            if (!ShouldLog(frequency))
                return;

            lock (SyncRoot)
            {
                if (!ShouldLogNoLock(frequency))
                    return;

                EnsureLoggerReadyNoLock();

                if (!loggerInitialized || logStreamWriter == null)
                    return;

                RotateLogFileIfNeededNoLock();

                if (!loggerInitialized || logStreamWriter == null)
                    return;

                WriteRawNoLock($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {message}");

                if (DateTime.UtcNow - lastMaintenanceUtc >= MaintenanceInterval)
                    RunMaintenanceNoLock(activeLogDirectory);
            }
        }

        public static string GetRecommendedDefaultLogPath()
        {
            return RecommendedDefaultLogPath.Value;
        }

        public static bool EnsureNoLoggingWithoutConfiguredPath()
        {
            lock (SyncRoot)
            {
                bool pathMissing = string.IsNullOrWhiteSpace(Settings.Default.LoggerFolderPath);

                bool settingChanged = PersistNoLoggingWhenPathMissing();

                if (pathMissing)
                {
                    currentLoggingFrequency = LoggingFrequency.NoLogging;

                    LogFrequency = LoggingFrequency.NoLogging;

                    LastError = string.Empty;
                    CloseWriterNoLock();
                }

                return settingChanged;
            }
        }

        private static string DetermineRecommendedDefaultLogPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (!IsRestrictedPath(baseDirectory) && CanWriteToExistingDirectory(baseDirectory, out _))
            {
                return PortableLogPath;
            }

            return LocalAppDataLogPath;
        }

        public static bool TryGetLogDirectory(out string logDirectory)
        {
            lock (SyncRoot)
            {
                logDirectory = GetLogDirectoryNoLock();
                return !string.IsNullOrWhiteSpace(logDirectory);
            }
        }

        public static string GetLogFolderPathForDisplay()
        {
            lock (SyncRoot)
            {
                string configuredPath =
                    GetConfiguredLogDirectoryNoLock();

                return string.IsNullOrWhiteSpace(configuredPath)
                    ? "N/A"
                    : configuredPath;
            }
        }

        public static LogFolderStatusInfo GetLogFolderStatusInfo(string selectedPath)
        {
            bool runningAsAdmin = IsRunningAsAdministrator();

            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                return new LogFolderStatusInfo
                {
                    Status = LogFolderStatus.NotSelected,
                    Message = "Status: N/A",
                    NormalizedPath = string.Empty,
                    IsRunningAsAdmin = runningAsAdmin,
                    UsesDefaultPath = false
                };
            }

            string normalizedPath;

            try
            {
                normalizedPath = NormalizeFolderPath(selectedPath);
            }
            catch
            {
                return new LogFolderStatusInfo
                {
                    Status = LogFolderStatus.InvalidPath,
                    Message = "Status: Invalid logger folder path.",
                    NormalizedPath = selectedPath,
                    IsRunningAsAdmin = runningAsAdmin,
                    UsesDefaultPath = false
                };
            }

            bool restricted = IsRestrictedPath(normalizedPath);

            try
            {
                if (Directory.Exists(normalizedPath))
                {
                    if (CanWriteToExistingDirectory(normalizedPath, out _))
                    {
                        if (runningAsAdmin && restricted)
                        {
                            return new LogFolderStatusInfo
                            {
                                Status = LogFolderStatus.WritableAdmin,
                                Message = "Status: Writable (admin).",
                                NormalizedPath = normalizedPath,
                                IsRunningAsAdmin = true,
                                UsesDefaultPath = false
                            };
                        }

                        return new LogFolderStatusInfo
                        {
                            Status = LogFolderStatus.Writable,
                            Message = "Status: Writable.",
                            NormalizedPath = normalizedPath,
                            IsRunningAsAdmin = runningAsAdmin,
                            UsesDefaultPath = false
                        };
                    }

                    return new LogFolderStatusInfo
                    {
                        Status = LogFolderStatus.NotWritable,
                        Message = "Status: Not writable.",
                        NormalizedPath = normalizedPath,
                        IsRunningAsAdmin = runningAsAdmin,
                        UsesDefaultPath = false
                    };
                }

                string existingParent = FindExistingParentDirectory(normalizedPath);

                if (string.IsNullOrWhiteSpace(existingParent))
                {
                    return new LogFolderStatusInfo
                    {
                        Status = LogFolderStatus.InvalidPath,
                        Message = "Status: Invalid path.",
                        NormalizedPath = normalizedPath,
                        IsRunningAsAdmin = runningAsAdmin,
                        UsesDefaultPath = false
                    };
                }

                bool parentRestricted = IsRestrictedPath(existingParent);

                if (CanWriteToExistingDirectory(existingParent, out _))
                {
                    if (runningAsAdmin && (restricted || parentRestricted))
                    {
                        return new LogFolderStatusInfo
                        {
                            Status = LogFolderStatus.WillBeCreatedAdmin,
                            Message = "Status: Folder will be created (admin).",
                            NormalizedPath = normalizedPath,
                            IsRunningAsAdmin = true,
                            UsesDefaultPath = false
                        };
                    }

                    return new LogFolderStatusInfo
                    {
                        Status = LogFolderStatus.WillBeCreated,
                        Message = "Status: Folder will be created.",
                        NormalizedPath = normalizedPath,
                        IsRunningAsAdmin = runningAsAdmin,
                        UsesDefaultPath = false
                    };
                }

                return new LogFolderStatusInfo
                {
                    Status = LogFolderStatus.NotWritable,
                    Message = "Status: Not writable.",
                    NormalizedPath = normalizedPath,
                    IsRunningAsAdmin = runningAsAdmin,
                    UsesDefaultPath = false
                };
            }
            catch
            {
                return new LogFolderStatusInfo
                {
                    Status = LogFolderStatus.NotWritable,
                    Message = "Status: Not writable.",
                    NormalizedPath = normalizedPath,
                    IsRunningAsAdmin = runningAsAdmin,
                    UsesDefaultPath = false
                };
            }
        }

        public static bool TryValidateLogFolderPath(string selectedPath, IWin32Window owner, out string normalizedPath, out string message)
        {
            normalizedPath = string.Empty;
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                message = "No logger folder path was selected.";
                return false;
            }

            LogFolderStatusInfo statusInfo = GetLogFolderStatusInfo(selectedPath);

            if (!statusInfo.CanUseForLogging)
            {
                message = statusInfo.Message;
                return false;
            }

            if (statusInfo.Status == LogFolderStatus.WritableAdmin || statusInfo.Status == LogFolderStatus.WillBeCreatedAdmin)
            {
                DialogResult choice = MessageBox.Show(
                    owner,
                    statusInfo.Message +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Logging may fail when WinThumbsPreloader is launched " +
                    "normally without administrator privileges." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Do you still want to use this folder?",
                    "Logger Folder",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (choice != DialogResult.Yes)
                {
                    message = "Logger folder selection cancelled.";
                    return false;
                }
            }

            try
            {
                Directory.CreateDirectory(statusInfo.NormalizedPath);
            }
            catch (Exception ex)
            {
                message = "Failed to create logger folder: " + ex.Message;
                return false;
            }

            normalizedPath = statusInfo.NormalizedPath;
            message = "Logger folder set to: " + normalizedPath + ".";

            return true;
        }

        public static long GetTotalLogsSize()
        {
            lock (SyncRoot)
            {
                string directory = GetLogDirectoryNoLock();

                if (string.IsNullOrWhiteSpace(directory))
                    return 0;

                return GetLogFilesNoLock(directory)
                    .Sum(file => SafeGetLength(file));
            }
        }

        public static LogCleanupResult RunMaintenanceNow()
        {
            lock (SyncRoot)
            {
                string directory = GetLogDirectoryNoLock();

                return string.IsNullOrWhiteSpace(directory)
                    ? new LogCleanupResult()
                    : RunMaintenanceNoLock(directory);
            }
        }

        public static bool TryClearAllLogs(out int deletedCount, out int failedCount, out string message)
        {
            lock (SyncRoot)
            {
                deletedCount = 0;
                failedCount = 0;

                string directory = GetLogDirectoryNoLock();

                if (string.IsNullOrWhiteSpace(directory))
                {
                    message = "No logger folder has been selected.";
                    return false;
                }

                bool reopenLogger = LoggingEnabledNoLock();

                CloseWriterNoLock();

                foreach (FileInfo file in GetLogFilesNoLock(directory))
                {
                    try
                    {
                        file.Delete();
                        deletedCount++;
                    }
                    catch
                    {
                        failedCount++;
                    }
                }

                if (reopenLogger) TryOpenNewLogFileNoLock(directory, out _);

                if (failedCount == 0)
                {
                    message = reopenLogger
                        ? $"Cleared {deletedCount} log file(s). " +
                          "A new active log file was created."
                        : $"Cleared {deletedCount} log file(s).";

                    return true;
                }

                message =
                    $"Deleted {deletedCount} log file(s), but " +
                    $"{failedCount} file(s) could not be deleted " +
                    "because they were in use or inaccessible.";

                return false;
            }
        }

        private static bool PersistNoLoggingWhenPathMissing()
        {
            if (!string.IsNullOrWhiteSpace(Settings.Default.LoggerFolderPath))
            {
                return false;
            }

            int noLoggingValue = (int)LoggingFrequency.NoLogging;

            if (Settings.Default.LoggingFrequency == noLoggingValue)
                return false;

            try
            {
                Settings.Default.LoggingFrequency = noLoggingValue;
                Settings.Default.Save();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save NoLogging for a missing log path: " + ex.Message
                );

                return false;
            }
        }

        private static LoggingFrequency ReadLoggingFrequency()
        {
            if (string.IsNullOrWhiteSpace(Settings.Default.LoggerFolderPath))
            {
                PersistNoLoggingWhenPathMissing();
                return LoggingFrequency.NoLogging;
            }

            int value = Settings.Default.LoggingFrequency;

            return Enum.IsDefined(typeof(LoggingFrequency), value)
                ? (LoggingFrequency)value
                : LoggingFrequency.NoLogging;
        }

        private static bool LoggingEnabledNoLock()
        {
            return LogFrequency != LoggingFrequency.NoLogging;
        }

        private static bool ShouldLog(LoggingFrequency frequency)
        {
            lock (SyncRoot)
            {
                return ShouldLogNoLock(frequency);
            }
        }

        private static bool ShouldLogNoLock(LoggingFrequency frequency)
        {
            return LogFrequency switch
            {
                LoggingFrequency.NoLogging => false,

                LoggingFrequency.PreloaderLogging =>
                    frequency == LoggingFrequency.PreloaderLogging ||
                    frequency == LoggingFrequency.AllLogging,

                LoggingFrequency.GUILogging =>
                    frequency == LoggingFrequency.GUILogging ||
                    frequency == LoggingFrequency.AllLogging,

                LoggingFrequency.AllLogging =>
                    frequency == LoggingFrequency.PreloaderLogging ||
                    frequency == LoggingFrequency.GUILogging ||
                    frequency == LoggingFrequency.AllLogging,

                LoggingFrequency.DebugLogging =>
                    frequency != LoggingFrequency.NoLogging,

                _ => false
            };
        }

        private static void EnsureLoggerReadyNoLock()
        {
            string desiredDirectory = GetConfiguredLogDirectoryNoLock();

            if (string.IsNullOrWhiteSpace(desiredDirectory))
            {
                CloseWriterNoLock();
                LastError = "Logging is enabled, but no logger folder has been selected.";
                return;
            }

            if (loggerInitialized && logStreamWriter != null && PathsEqual(activeLogDirectory, desiredDirectory))
            {
                return;
            }

            InitializeLogger();
        }

        private static bool TryOpenNewLogFileNoLock(string directory, out Exception exception)
        {
            exception = null;

            if (string.IsNullOrWhiteSpace(directory))
            {
                exception = new InvalidOperationException("No logger folder has been selected.");

                CloseWriterNoLock();
                return false;
            }

            try
            {
                Directory.CreateDirectory(directory);

                int processId = Environment.ProcessId;
                DateTime now = DateTime.Now;

                for (int sequence = 0; sequence < 100; sequence++)
                {
                    // The PID distinguishes simultaneously running processes.
                    // A numeric suffix is used only if this same process needs
                    // another log file during the same second.
                    string collisionSuffix =
                        sequence == 0
                            ? string.Empty
                            : $"_{sequence}";

                    string fileName = $"{LogFilePrefix}{now:yyyyMMdd_HHmmss}_" + $"{processId}{collisionSuffix}.txt";

                    string candidatePath = Path.Combine(directory, fileName);

                    try
                    {
                        logFileStream = new FileStream(candidatePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);

                        logStreamWriter = new StreamWriter(logFileStream)
                        {
                            AutoFlush = true
                        };

                        logFilePath = candidatePath;
                        activeLogDirectory = directory;
                        loggerInitialized = true;
                        return true;
                    }
                    catch (IOException) when (File.Exists(candidatePath))
                    {
                        // The same PID already created a file during this
                        // second. Try the next collision suffix.
                    }
                }

                throw new IOException(
                    "Could not create a unique log file name."
                );
            }
            catch (Exception ex)
            {
                exception = ex;
                CloseWriterNoLock();
                return false;
            }
        }

        private static void RotateLogFileIfNeededNoLock()
        {
            if (!loggerInitialized || logFileStream == null || logFileStream.Length < GetMaxLogFileSizeBytesNoLock())
            {
                return;
            }

            string directory = activeLogDirectory;

            WriteRawNoLock($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: " + "Maximum log-file size reached. Rotating to a new file.");

            CloseWriterNoLock();

            if (!TryOpenNewLogFileNoLock(directory, out Exception exception))
            {
                LastError = "Failed to rotate log file: " + exception.Message;
                Debug.WriteLine(LastError);
                return;
            }

            WriteRawNoLock($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: " + "Logger continued in a new rotated log file.");

            RunMaintenanceNoLock(directory);
        }

        private static void WriteRawNoLock(string message)
        {
            try
            {
                logStreamWriter?.WriteLine(message);
            }
            catch (Exception ex)
            {
                LastError = "Failed to write to the log file: " + ex.Message;
                Debug.WriteLine(LastError);
            }
        }

        private static void CloseWriterNoLock()
        {
            try
            {
                logStreamWriter?.Flush();
            }
            catch { }

            try
            {
                logStreamWriter?.Dispose();
            }
            catch { }

            try
            {
                logFileStream?.Dispose();
            }
            catch { }

            logStreamWriter = null;
            logFileStream = null;
            logFilePath = null;
            activeLogDirectory = null;
            loggerInitialized = false;
        }

        private static LogCleanupResult RunMaintenanceNoLock(string directory)
        {
            LogCleanupResult result = new LogCleanupResult();
            lastMaintenanceUtc = DateTime.UtcNow;

            if (!Settings.Default.AutoDeleteLogsByAge || string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                return result;
            }

            int retentionDays = Math.Max(1, Settings.Default.LogRetentionDays);
            DateTime cutoffUtc = DateTime.UtcNow.AddDays(-retentionDays);

            foreach (FileInfo file in GetLogFilesNoLock(directory)
                .Where(file => !IsCurrentLogFileNoLock(file.FullName))
                .Where(file => SafeGetLastWriteTimeUtc(file) < cutoffUtc)
                .OrderBy(file => SafeGetLastWriteTimeUtc(file)))
            {
                long length = SafeGetLength(file);

                if (TryDeleteFile(file))
                {
                    result.DeletedByAge++;
                    result.BytesFreed += length;
                }
                else
                {
                    result.FailedDeletes++;
                }
            }

            return result;
        }

        private static long GetMaxLogFileSizeBytesNoLock()
        {
            return MegabytesToBytes(Math.Max(1, Settings.Default.MaxLogFileSizeMB));
        }

        private static long MegabytesToBytes(long megabytes)
        {
            if (megabytes <= 0)
                return BytesPerMegabyte;

            if (megabytes > long.MaxValue / BytesPerMegabyte)
                return long.MaxValue;

            return megabytes * BytesPerMegabyte;
        }

        private static string GetConfiguredLogDirectoryNoLock()
        {
            string savedPath = Settings.Default.LoggerFolderPath;

            if (string.IsNullOrWhiteSpace(savedPath))
                return null;

            try
            {
                return NormalizeFolderPath(savedPath);
            }
            catch
            {
                return null;
            }
        }

        private static string GetLogDirectoryNoLock()
        {
            return !string.IsNullOrWhiteSpace(activeLogDirectory)
                ? activeLogDirectory
                : GetConfiguredLogDirectoryNoLock();
        }

        private static List<FileInfo> GetLogFilesNoLock(string directory)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
                {
                    return new List<FileInfo>();
                }

                return new DirectoryInfo(directory)
                    .EnumerateFiles(LogFileSearchPattern, SearchOption.TopDirectoryOnly)
                    .ToList();
            }
            catch
            {
                return new List<FileInfo>();
            }
        }

        private static bool IsCurrentLogFileNoLock(string path)
        {
            return !string.IsNullOrWhiteSpace(logFilePath) && PathsEqual(path, logFilePath);
        }

        private static bool TryDeleteFile(FileInfo file)
        {
            try
            {
                file.Delete();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Could not delete old log file '{file.FullName}': {ex.Message}");
                return false;
            }
        }

        private static long SafeGetLength(FileInfo file)
        {
            try
            {
                file.Refresh();
                return file.Exists ? file.Length : 0;
            }
            catch
            {
                return 0;
            }
        }

        private static DateTime SafeGetLastWriteTimeUtc(FileInfo file)
        {
            try
            {
                file.Refresh();
                return file.Exists ? file.LastWriteTimeUtc : DateTime.MaxValue;
            }
            catch
            {
                return DateTime.MaxValue;
            }
        }

        private static string NormalizeFolderPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            string expanded = Environment.ExpandEnvironmentVariables(path.Trim().Trim('"'));

            return Path.GetFullPath(expanded).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private static string FindExistingParentDirectory(string path)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(path);

                while (directory != null)
                {
                    if (directory.Exists)
                        return directory.FullName;

                    directory = directory.Parent;
                }
            }
            catch { }

            return null;
        }

        private static bool CanWriteToExistingDirectory(string directory, out string message)
        {
            message = string.Empty;

            try
            {
                if (!Directory.Exists(directory))
                {
                    message = "Folder does not exist.";
                    return false;
                }

                string testFile = Path.Combine(
                    directory,
                    ".WinThumbsPreloader_LogWriteTest_" +
                    Guid.NewGuid().ToString("N") +
                    ".tmp"
                );

                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        private static bool IsRestrictedPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

            return IsSameOrUnder(path, programFiles) ||
                   IsSameOrUnder(path, programFilesX86) ||
                   IsSameOrUnder(path, windows);
        }

        private static bool IsSameOrUnder(string path, string root)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(root))
            {
                return false;
            }

            try
            {
                string normalizedPath = NormalizeFolderPath(path);
                string normalizedRoot = NormalizeFolderPath(root);

                return normalizedPath.Equals(normalizedRoot, StringComparison.OrdinalIgnoreCase) || normalizedPath.StartsWith(normalizedRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static bool PathsEqual(string first, string second)
        {
            if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(second))
            {
                return false;
            }

            try
            {
                return NormalizeFolderPath(first).Equals(NormalizeFolderPath(second), StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return string.Equals(first.Trim(), second.Trim(), StringComparison.OrdinalIgnoreCase);
            }
        }

        private static bool IsRunningAsAdministrator()
        {
            try
            {
                using WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }
    }
}