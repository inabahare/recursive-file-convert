using System;
using System.IO;
using System.Linq;
using VideoConvert;

namespace RecursiveFileConvert
{
  class Program
  {
    static void Main(string[] args)
    {
      var path = "/home/inaba/Videos/Test";

      var fileManager = new FileManager
      {
        Path = path
      };

      var files = fileManager.GetVideoFiles();

      var ffmpeg = new Ffmpeg
      {
        Preset = Preset.Medium,
        Codec = "libx265",
        Crf = 30,
        FastStart = true
      };

      foreach (var file in files)
      {
        // Move to a new name in case app crashes
        var tmpFile = file.Move($"{file.Name}.tmp");

        ffmpeg.Convert(tmpFile.ToString(),
                       $"{file.FullPath}");

        Console.WriteLine(file.ToString());

      }

      Console.WriteLine(files);
    }
  }
}
