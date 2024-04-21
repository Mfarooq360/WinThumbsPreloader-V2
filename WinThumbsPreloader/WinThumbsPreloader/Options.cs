using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Shapes;
using static WinThumbsPreloader.Logger;

namespace WinThumbsPreloader
{
    class Options
    {
        public bool badOrNoArguments;
        public bool includeNestedDirectories;
        public bool silentMode;
        public bool multiThreaded;
        public bool startMinimized;
        public int threadCount;
        public List<string> paths = new List<string>();

        public Options(string[] arguments) //Add a message box to show the user that the arguments are invalid
        {
            WriteLine("Starting to parse command-line arguments. - Options(string[] arguments)", LoggingFrequency.AllLogging);

            //Check if we have any arguments
            badOrNoArguments = (arguments.Length == 0);
            if (badOrNoArguments)
            {
                WriteLine("No command-line arguments provided.", LoggingFrequency.AllLogging);
                return;
            }
            //Set default options
            includeNestedDirectories = false;
            silentMode = false;
            multiThreaded = Properties.Settings.Default.Multithreaded;
            WriteLine($"Multi-threading default: {multiThreaded}", LoggingFrequency.PreloaderLogging);
            //Set the options the user wants from the arguments
            string argus = string.Join(" ", arguments);
            WriteLine($"string argus: {argus}", LoggingFrequency.DebugLogging);
            string combinedArguments = argus.Replace("\"", "");
            WriteLine($"string combinedArguments (removed backslashes): {combinedArguments}", LoggingFrequency.DebugLogging);
            string[] parsedArguments = SplitArgs(combinedArguments);
            WriteLine("SplitArgs completed", LoggingFrequency.DebugLogging);
            WriteLine($"string[] parsedArguments: {String.Join(", ", parsedArguments)}", LoggingFrequency.DebugLogging);
            WriteLine($"parsedArguments.Length: {parsedArguments.Length}", LoggingFrequency.DebugLogging);

            for (int i = 0; i < parsedArguments.Length; i++)
            {
                string argu = parsedArguments[i];
                WriteLine($"argu: {argu}", LoggingFrequency.DebugLogging);
                // Remove special characters that may be entered by the user when following the argument template inappropiately
                argu = (i != parsedArguments.Length - 1) ? argu.Replace("[", "").Replace("]", "").Replace("<", "").Replace(">", "") : argu;
                WriteLine($"argu (removed '[', ']', '<', '>'): {argu}", LoggingFrequency.DebugLogging);

                if (argu.Contains("-m:")) // Check if argument starts with -m:
                {
                    multiThreaded = true;
                    string count = argu.Substring(3); // Get everything after -m:
                    WriteLine($"string count: {count}", LoggingFrequency.DebugLogging);
                    if (int.TryParse(count, out int result)) // Try to parse it as int
                    {
                        if (result >= 0 && result <= 256) { threadCount = result + 1; }
                        WriteLine($"threadCount result (-m:): {threadCount}", LoggingFrequency.DebugLogging);
                    }
                    WriteLine($"Multi-threading enabled", LoggingFrequency.PreloaderLogging);
                    continue;
                }
                else if (argu.Contains("-m"))
                {
                    multiThreaded = true;
                    string count = argu.Substring(2);
                    WriteLine($"string count: {count}", LoggingFrequency.DebugLogging);
                    if (int.TryParse(count, out int result))
                    {
                        if (result >= 0 && result <= 256) { threadCount = result + 1; }
                        WriteLine($"threadCount result (-m): {threadCount}", LoggingFrequency.DebugLogging);
                    }
                    WriteLine($"Multi-threading enabled", LoggingFrequency.PreloaderLogging);
                    continue;
                }
                switch (argu)
                {
                    case "-r":
                        includeNestedDirectories = true;
                        WriteLine("Including nested directories.", LoggingFrequency.PreloaderLogging);
                        break;
                    case "-s":
                        silentMode = true;
                        WriteLine("Silent mode enabled.", LoggingFrequency.AllLogging);
                        break;
                    case "-m":
                        multiThreaded = true;
                        WriteLine("Multi-threading enabled.", LoggingFrequency.PreloaderLogging);
                        break;
                    case "-startminimized":
                        WriteLine("Start minimized enabled.", LoggingFrequency.AllLogging);
                        startMinimized = true;
                        break;
                    default:
                        // if it's not a switch argument, then it should be a path
                        string path = argu.Replace("\"", ""); // remove quotation marks
                        /*path = path.Replace("\\\\", "\\");*/
                        if (!Directory.Exists(path) && !File.Exists(path))
                        {
                            if (path.EndsWith(",") || path.EndsWith(" "))
                            {
                                path = path.TrimEnd(new char[] { ' ', ',' }); // If the path doesn't exist, remove any trailing spaces or commas to ensure that multiple paths that are separated by commas aren't being considered invalid
                                if (!Directory.Exists(path) && !File.Exists(path))
                                {
                                    WriteLine("Invalid or no path: " + path, LoggingFrequency.AllLogging);
                                    Console.WriteLine("Invalid or no path: " + path);
                                    badOrNoArguments = true; // if the path doesn't exist, set badOrNoArguments to true
                                    return;
                                }
                                else
                                {
                                    WriteLine("Valid path: " + path, LoggingFrequency.PreloaderLogging);
                                    paths.Add(path);
                                }
                            }
                            else
                            {
                                WriteLine("Invalid or no path: " + path, LoggingFrequency.AllLogging);
                                Console.WriteLine("Invalid or no path: " + path);
                                badOrNoArguments = true; // if the path doesn't exist, set badOrNoArguments to true
                                return;
                            }

                        }
                        else
                        {
                            WriteLine("Valid path: " + path, LoggingFrequency.PreloaderLogging);
                            paths.Add(path);
                        }
                        break;
                }
            }
        }

        public static string[] SplitArgs(string args)
        {
            WriteLine("Starting to split arguments. - SplitArgs(string args)", LoggingFrequency.DebugLogging);
            var parsedArguments = new List<string>();
            var currentArg = new StringBuilder();
            var inPath = false;
            var hyphenArg = false;

            WriteLine($"args.Length: {args.Length}", LoggingFrequency.DebugLogging);
            for (int i = 0; i < args.Length; i++)
            {
                char c = args[i];
                WriteLine($"char c: {c}", LoggingFrequency.DebugLogging);

                // Check for start of a path
                if (i < args.Length - 1 && Regex.IsMatch(args.Substring(i, 2), @"\w:"))
                {
                    string foundPath = args.Substring(i, 2);
                    WriteLine($"Found drive letter in argument: {foundPath}", LoggingFrequency.DebugLogging);
                    WriteLine($"currentArg.Length: {currentArg.Length}", LoggingFrequency.DebugLogging);
                    if (currentArg.Length > 0)
                    {
                        parsedArguments.Add(currentArg.ToString());
                        WriteLine($"parsedArguments.Add(currentArg.ToString()): {currentArg}", LoggingFrequency.DebugLogging);
                        currentArg.Clear();
                        WriteLine($"currentArg.Clear(): {currentArg}", LoggingFrequency.DebugLogging);
                    }
                    inPath = true;
                    WriteLine($"inPath: {inPath}", LoggingFrequency.DebugLogging);
                }

                // Check for hyphen-prefixed arguments
                if (c == '-' && !inPath)
                {
                    WriteLine($"Found hyphen in argument", LoggingFrequency.DebugLogging);
                    WriteLine($"currentArg.Length: {currentArg.Length}", LoggingFrequency.DebugLogging);
                    if (currentArg.Length > 0)
                    {
                        parsedArguments.Add(currentArg.ToString());
                        WriteLine($"parsedArguments.Add(currentArg.ToString()): {currentArg}", LoggingFrequency.DebugLogging);
                        currentArg.Clear();
                        WriteLine($"currentArg.Clear(): {currentArg}", LoggingFrequency.DebugLogging);
                    }
                    hyphenArg = true;
                    WriteLine($"hyphenArg: {hyphenArg}", LoggingFrequency.DebugLogging);
                }

                // If space and not in path and the current argument is a hyphen-prefixed argument
                if (char.IsWhiteSpace(c) && !inPath && hyphenArg)
                {
                    WriteLine($"Found whitespace in argument", LoggingFrequency.DebugLogging);
                    parsedArguments.Add(currentArg.ToString());
                    WriteLine($"parsedArguments.Add(currentArg.ToString()): {currentArg}", LoggingFrequency.DebugLogging);
                    currentArg.Clear();
                    WriteLine($"currentArg.Clear(): {currentArg}", LoggingFrequency.DebugLogging);
                    hyphenArg = false;
                    WriteLine($"hyphenArg: {hyphenArg}", LoggingFrequency.DebugLogging);
                    continue;
                }

                currentArg.Append(c);
                WriteLine($"currentArg.Append(c): {currentArg}", LoggingFrequency.DebugLogging);
            }

            // Add the last argument if any
            if (currentArg.Length > 0)
            {
                WriteLine($"currentArg.Length > 0", LoggingFrequency.DebugLogging);
                parsedArguments.Add(currentArg.ToString());
                WriteLine($"parsedArguments.Add(currentArg.ToString()): {currentArg}", LoggingFrequency.DebugLogging);
                WriteLine($"parsedArguments: {String.Join(", ", parsedArguments)}", LoggingFrequency.DebugLogging);
            }

            // Now handle the path arguments
            var finalArguments = new List<string>();
            foreach (var arg in parsedArguments)
            {
                WriteLine("foreach (var arg in parsedArguments)", LoggingFrequency.DebugLogging);
                WriteLine($"arg: {arg}", LoggingFrequency.DebugLogging);
                if (arg.StartsWith("-"))
                {
                    WriteLine($"arg.StartsWith('-')", LoggingFrequency.DebugLogging);
                    finalArguments.Add(arg);
                    WriteLine($"finalArguments.Add(arg): {arg}", LoggingFrequency.DebugLogging);
                    WriteLine($"finalArguments: {String.Join(", ", finalArguments)}", LoggingFrequency.DebugLogging);
                }
                else
                {
                    var drivePaths = Regex.Split(arg, @"(?=\w:)");
                    WriteLine($"drivePaths: {String.Join(", ", drivePaths)}", LoggingFrequency.DebugLogging);
                    finalArguments.AddRange(drivePaths
                                  .Where(p => !string.IsNullOrWhiteSpace(p))
                                  .Select(p => p.TrimEnd(new char[] { ' ', ',' })));
                    WriteLine("finalArguments.AddRange(drivePaths.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.TrimEnd(new char[] {{ ' ', ',' }}))): " + String.Join(", ", finalArguments), LoggingFrequency.DebugLogging);
                }
            }
            WriteLine($"finalArguments: {String.Join(", ", finalArguments)}", LoggingFrequency.DebugLogging);
            return finalArguments.ToArray();
        }
    }
}