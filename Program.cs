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
#if DEBUG
      Console.SetCursorPosition(0, Console.CursorTop - 1);
#endif
      Console.Write($"\r{new String(' ', Console.BufferWidth)}\r");
#if DEBUG
      Console.SetCursorPosition(0, Console.CursorTop - 1);
#endif
    }

    static Dictionary<string, string> ParseArgs(string[] args)
    {
      var res = new Dictionary<string, string>();

      // Step through every second 
      for (var i = 0; i < args.Length; i += 2)
      {
        var arg = args[i].Replace("--", "");

        if (i + 1 > args.Length)
          throw new Exception($"Could not find a value for {arg}");

        var value = args[i + 1];
        res.Add(arg, value);
      }

      return res;
    }

    static void PrintHelp()
    {
      var args = new Dictionary<string, string> {
        { "help", "Show this message" },
        { "dir", "The directory for the files to convert"},
        { "list", "Where the list of converted files are stored"},
      };

      foreach (var arg in args)
      {
        Console.WriteLine($"--{arg.Key}");
        Console.WriteLine(arg.Value);
        Console.WriteLine();
      }
    }

    static async Task Main(string[] args)
    {
      if (args.Contains("--help"))
      {
        PrintHelp();
        return;
      }

      var parsedArgs = ParseArgs(args);

      var extensionsToKeep = new List<string> {
        ".mp4",
        ".mkv"
      };

      var videoPath = parsedArgs["dir"];
      var convertedPath = parsedArgs["list"] ?? Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "converted.txt");

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
        // Because if it's not part of the extensions to keep it should be removed
        File.Delete(file.FullName);
        File.Move(tmpName, newName, true);
      }

      await File.WriteAllLinesAsync(convertedPath, converted);
    }
  }
}
