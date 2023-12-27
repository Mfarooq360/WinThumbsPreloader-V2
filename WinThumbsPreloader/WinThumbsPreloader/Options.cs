using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Shapes;

namespace WinThumbsPreloader
{
    class Options
    {
        public bool badArguments;
        public bool includeNestedDirectories;
        public bool silentMode;
        public bool multiThreaded;
        public bool startMinimized;
        public int threadCount;
        public List<string> paths = new List<string>();

        public Options(string[] arguments)
        {
            //Check if we have any arguments
            badArguments = (arguments.Length == 0);
            if (badArguments) return;

            //Set default options
            includeNestedDirectories = false;
            silentMode = false;
            multiThreaded = Properties.Settings.Default.Multithreaded;
            //Set the options the user wants from the arguments
            string argus = string.Join(" ", arguments);
            string combinedArguments = argus.Replace("\"", "");
            string[] parsedArguments = SplitArgs(combinedArguments);

            for (int i = 0; i < parsedArguments.Length; i++)
            {
                string argu = parsedArguments[i];
                // Remove brackets for non-path arguments
                argu = (i != parsedArguments.Length - 1) ? argu.Replace("[", "").Replace("]", "") : argu;

                if (argu.Contains("-m:")) // Check if argument starts with -m:
                {
                    multiThreaded = true;
                    string count = argu.Substring(3); // Get everything after -m:
                    if (int.TryParse(count, out int result)) // Try to parse it as int
                    {
                        if (result >= 0 && result <= 256) { threadCount = result + 1; }
                    }
                    continue;
                }
                else if (argu.Contains("-m"))
                {
                    multiThreaded = true;
                    string count = argu.Substring(2);
                    if (int.TryParse(count, out int result))
                    {
                        if (result >= 0 && result <= 256) { threadCount = result + 1; }
                    }
                    continue;
                }
                switch (argu)
                {
                    case "-r":
                        includeNestedDirectories = true;
                        break;
                    case "-s":
                        silentMode = true;
                        break;
                    case "-m":
                        multiThreaded = true;
                        break;
                    case "-startminimized":
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
                                    badArguments = true; // if the path doesn't exist, set badArguments to true
                                    return;
                                }
                                else
                                {
                                    paths.Add(path);
                                }
                            }
                            else
                            { 
                                badArguments = true; // if the path doesn't exist, set badArguments to true
                                return;
                            }

                        }
                        else
                        {
                            paths.Add(path);
                        }
                        break;
                }
            }
        }

        public static string[] SplitArgs(string args)
        {
            var parsedArguments = new List<string>();
            var currentArg = new StringBuilder();
            var inPath = false;
            var hyphenArg = false;

            for (int i = 0; i < args.Length; i++)
            {
                char c = args[i];

                // Check for start of a path
                if (i < args.Length - 1 && Regex.IsMatch(args.Substring(i, 2), @"\w:"))
                {
                    if (currentArg.Length > 0)
                    {
                        parsedArguments.Add(currentArg.ToString());
                        currentArg.Clear();
                    }
                    inPath = true;
                }

                // Check for hyphen-prefixed arguments
                if (c == '-' && !inPath)
                {
                    if (currentArg.Length > 0)
                    {
                        parsedArguments.Add(currentArg.ToString());
                        currentArg.Clear();
                    }
                    hyphenArg = true;
                }

                // If space and not in path and the current argument is a hyphen-prefixed argument
                if (char.IsWhiteSpace(c) && !inPath && hyphenArg)
                {
                    parsedArguments.Add(currentArg.ToString());
                    currentArg.Clear();
                    hyphenArg = false;
                    continue;
                }

                currentArg.Append(c);
            }

            // Add the last argument if any
            if (currentArg.Length > 0)
            {
                parsedArguments.Add(currentArg.ToString());
            }

            // Now handle the path arguments
            var finalArguments = new List<string>();
            foreach (var arg in parsedArguments)
            {
                if (arg.StartsWith("-"))
                {
                    finalArguments.Add(arg);
                }
                else
                {
                    var drivePaths = Regex.Split(arg, @"(?=\w:)");
                    finalArguments.AddRange(drivePaths
                                  .Where(p => !string.IsNullOrWhiteSpace(p))
                                  .Select(p => p.TrimEnd(new char[] { ' ', ',' })));
                }
            }

            return finalArguments.ToArray();
        }
    }
}