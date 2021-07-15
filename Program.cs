using System;
using System.Linq;
using System.Collections.Generic;
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
      var converted = new List<string>();
      var path = "/home/inaba/Videos/TV Shorts";
      var files = new DirectoryInfo(path).GetFiles();


      foreach (var file in files)
      {
        var name = Path.GetFileNameWithoutExtension(file.Name);
        var directoryName = file.DirectoryName;
        var extension = file.Extension;

        var tmpName = $"{directoryName}/{name}.temp{extension}";

        await Ffmpeg.Convert(
          file.FullName,
          tmpName,
          (progress, totalTime) =>
          {
            Console.Clear();
            var alreadyConverted = "";
            for (var i = 0; i < converted.Count; i++)
              alreadyConverted += $"{i + 1} - {converted[i]}\n";

            var convertedPercentage = (progress / totalTime) * 100;

            var converting = $"{file.FullName} - {convertedPercentage.ToString("0.00")}% - {progress}";

            var result = $"Converted:\n{alreadyConverted}\n\nConverting:\n{converting}";

            Console.WriteLine(result);
          }
        );

        converted.Add(file.FullName);
      }

      Console.WriteLine("Hello World!");
    }
  }
}
