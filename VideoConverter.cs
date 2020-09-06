using System;
using System.IO;
using System.Linq;
using VideoConvert;

namespace RecursiveFileConvert
{
  public class VideoConverter
  {
    public string FilePath { get; set; }

    IFfmpeg ffmpeg;
    IFileManager fileManager;


    public VideoConverter Configure()
    {
      ffmpeg = new Ffmpeg
      {
        Preset = Preset.Medium,
        Codec = "libx265",
        Crf = 30,
        FastStart = true,
        OnProgress = percentage =>
        {
          Console.Clear();
          Console.WriteLine($"{percentage.ToString("#.##")}%");
        }
      };

      fileManager = new FileManager
      {
        Path = FilePath
      };

      return this;
    }

    public void Start()
    {
      var files = fileManager.GetVideoFiles();

      foreach (var file in files)
      {
        // Move to a new name in case app crashes
        var tmpFile = file.Move($"{file.Name}.tmp");
        file.Extension = ".mp4";
        ffmpeg.Convert(tmpFile.ToString(),
                       file.ToString());

        tmpFile.Remove();
        fileManager.SaveFile(file.ToString());
        Console.WriteLine($"Finished: {file}");
      }
    }
  }
}