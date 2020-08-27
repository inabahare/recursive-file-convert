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
        Crf = 32,
        FastStart = true
      };

      ffmpeg.Convert("/home/inaba/Videos/test.webm", "/home/inaba/Videos/test.mp4");

      Console.WriteLine(files);
    }
  }
}
