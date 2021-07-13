using System;
using System.IO;
using FFMpegCore;
using FFMpegCore.Enums;

namespace recursive_file_convert
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "/home/inaba/Videos/TV Shorts";
            
            var files = new DirectoryInfo(path).GetFiles();

            foreach (var file in files) {
                var name = Path.GetFileNameWithoutExtension(file.Name);
                var directoryName = file.DirectoryName;
                var extension = file.Extension;

                var tmpName = $"{directoryName}/{name}.temp{extension}";

                FFMpegArguments
                    .FromFileInput(file.FullName)
                    .OutputToFile(tmpName, false, options => options
                        .WithVideoCodec("libx265")
                        .WithConstantRateFactor(28)
                        .WithFastStart()
                    )
                    .NotifyOnProgress(onTimeProgress => Console.WriteLine(onTimeProgress))
                    .ProcessSynchronously();
            }

            Console.WriteLine("Hello World!");
        }
    }
}
