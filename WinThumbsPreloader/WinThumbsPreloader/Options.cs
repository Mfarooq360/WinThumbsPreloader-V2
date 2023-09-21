using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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
                        if (result >= 0) { threadCount = result + 1; }
                    }
                    continue;
                }
                else if (argu.Contains("-m"))
                {
                    multiThreaded = true;
                    string count = argu.Substring(2);
                    if (int.TryParse(count, out int result))
                    {
                        if (result >= 0) { threadCount = result + 1; }
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
                        if (!Directory.Exists(path) && !File.Exists(path))
                        {
                            badArguments = true; // if the path doesn't exist, set badArguments to true
                            return;
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
            List<string> parsedArguments = new List<string>();

            // Match anything inside quotes or any non-space sequences
            var matches = Regex.Matches(args, @"[\""].+?[\""]|\S+");

            foreach (Match match in matches)
            {
                string arg = match.Value.Trim('"'); // Remove quotes if any

                // If the argument starts with a hyphen, just add it.
                if (arg.StartsWith("-"))
                {
                    parsedArguments.Add(arg);
                }
                else
                {
                    // If it's not starting with hyphen, assume it's drive paths. 
                    // Split on the basis of a pattern that looks for drive letters and colons
                    var drivePaths = Regex.Split(arg, @"(?=\w:)");

                    foreach (string path in drivePaths)
                    {
                        if (!string.IsNullOrWhiteSpace(path))
                        {
                            parsedArguments.Add(path);
                        }
                    }
                }
            }

            return parsedArguments.ToArray();
        }
    }
}