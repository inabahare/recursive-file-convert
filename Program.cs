using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Enums;

namespace recursive_file_convert
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var path = "/home/inaba/Videos/TV Shorts";
            
            var files = new DirectoryInfo(path).GetFiles();

            foreach (var file in files) {
                var name = Path.GetFileNameWithoutExtension(file.Name);
                var directoryName = file.DirectoryName;
                var extension = file.Extension;

                var tmpName = $"{directoryName}/{name}.temp{extension}";

                await Ffmpeg.Convert(
                    file.FullName, 
                    tmpName,
                    progress => {
                        Console.Clear();
                        Console.WriteLine(progress);
                    }
                );
            }

            Console.WriteLine("Hello World!");
        }


    }
}
