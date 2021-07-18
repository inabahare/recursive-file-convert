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
      var extensionsToKeep = new List<string> {
        ".mp4",
        ".mkv"
      };

      var convertedPath = "/home/inaba/converted.txt";
      var videoPath = "/home/inaba/Videos/TV Shorts";

      var converted = (await File.ReadAllLinesAsync(convertedPath)).ToList();
      var files =
        new DirectoryInfo(videoPath)
          .GetFiles()
          .Where(file => !file.Name.Contains(".tmp"))
          .Where(file => !converted.Contains(file.FullName));

      foreach (var file in files)
      {
        PrintConverted(converted);

        var name = Path.GetFileNameWithoutExtension(file.Name);
        var directoryName = file.DirectoryName;
        var extension =
          extensionsToKeep.Contains(file.Extension) ? file.Extension : ".mp4";

        var tmpName = $"{directoryName}/{name}.tmp{extension}";
        var newName = $"{directoryName}/{name}{extension}";

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

        File.Move(tmpName, newName, true);
      }

      await File.WriteAllLinesAsync(convertedPath, converted);
    }
  }
}
