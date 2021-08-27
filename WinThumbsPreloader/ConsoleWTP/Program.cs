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
            //Get the items first before doing work
            List<string> items = DirectoryScanner.GetItemsBulk(args[0], true);
            Parallel.ForEach(
                items,
                new ParallelOptions { MaxDegreeOfParallelism = 2048 },
                item =>
                {
                    ThumbnailPreloader thumbnailPreloader = new ThumbnailPreloader();
                    Console.WriteLine(item);
                    thumbnailPreloader.PreloadThumbnail(item);
                }
            );
        }
    }
}
