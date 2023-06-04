using System.IO;

namespace WinThumbsPreloader
{
    class Options
    {
        public bool badArguments;
        public bool includeNestedDirectories;
        public bool silentMode;
        public bool multithreaded;
        public string path;

        public Options(string[] arguments)
        {
            //Check if we have more arguments than we support
            badArguments = (arguments.Length == 0 || arguments.Length > 4);
            if (badArguments) return;

            //Set default options
            includeNestedDirectories = false;
            silentMode = false;
            multithreaded = false;
            //Set the options the user wants from the arguments
            foreach (string argu in arguments) {
                switch (argu) {
                    case "-r":
                        includeNestedDirectories = true;
                        break;
                    case "-s":
                        silentMode = true;
                        break;
                    case "-m":
                        multithreaded = true;
                        break;
                    default:
                        path = argu;
                        break;
                }
            }

            //Check if the path we grabbed is real
            badArguments = !(Directory.Exists(path) || File.Exists(path));
            if (badArguments) return;
        }
    }
}
