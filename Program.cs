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
    static void PrintConverted(List<string> converted)
    {
      Console.Clear();

      var alreadyConverted = "";
      for (var i = 0; i < converted.Count; i++)
        alreadyConverted += $"{i + 1} - {converted[i]}\n";

      Console.WriteLine($"Converted:\n{alreadyConverted}\nConverting:\n");
    }

    static void ClearCurrentLine()
    {
      Console.SetCursorPosition(0, Console.CursorTop - 1);
      Console.Write($"\r{new String(' ', Console.BufferWidth)}\r");
      Console.SetCursorPosition(0, Console.CursorTop - 1);
    }

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

        PrintConverted(converted);

        await Ffmpeg.Convert(
          file.FullName,
          tmpName,
          (progress, totalTime) =>
          {
            ClearCurrentLine();

            var convertedPercentage = (progress / totalTime) * 100;
            var converting = $"{file.FullName} - {convertedPercentage.ToString("0.00")}% - {progress}";
            Console.WriteLine(converting);
          }
        );

        converted.Add(file.FullName);
      }


      Console.WriteLine("Hello World!");
    }
  }
}
