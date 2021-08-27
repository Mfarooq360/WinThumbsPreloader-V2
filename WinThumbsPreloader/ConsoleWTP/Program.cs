using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWTP
{
    class Program
    {
        static void Main(string[] args)
        {
            //Check args
            if (string.IsNullOrWhiteSpace(args[0])) {
                Console.WriteLine("Please provide a location to scan!");
            }
            //Set multithread
            bool multithread = true;
            if (!string.IsNullOrWhiteSpace(args[1])) {
                if (args[1] == "false") {
                    multithread = false;
                }
            }
            //Get the items first before doing work
            List<string> items = DirectoryScanner.GetItemsBulk(args[0], true);
            if (!multithread)
            {
                ThumbnailPreloader thumbnailPreloader = new ThumbnailPreloader();
                int count = items.Count();
                int i = 0;
                foreach (string item in items)
                {
                    i++;
                    Console.WriteLine(i+"/"+count+": "+item);
                    thumbnailPreloader.PreloadThumbnail(item);
                }
            }
            else
            {
                Parallel.ForEach(
                    items,
                    new ParallelOptions { MaxDegreeOfParallelism = 2048 },
                    item =>
                    {
                        ThumbnailPreloader thumbnailPreloader = new ThumbnailPreloader();
                        Console.WriteLine(item);
                        thumbnailPreloader.PreloadThumbnail(item);
                    });
            }
        }
    }
}
