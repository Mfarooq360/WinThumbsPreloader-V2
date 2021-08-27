using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWTP
{
    class DirectoryScanner
    {

        public static List<string> GetItemsBulk(string path, bool includeNestedDirectories)
        {
            List<string> items = new List<string>();
            if (includeNestedDirectories)
            {
                items = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
            }
            else
            {
                items = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly).ToList();
            }
            return items;
        }
    }
}
