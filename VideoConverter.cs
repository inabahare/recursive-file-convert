using System;
using System.IO;
using System.Linq;
using VideoConvert;
using VideoConvert.Ui;

namespace RecursiveFileConvert
{
  public class VideoConverter
  {
    public string FilePath { get; set; }

    IFfmpeg ffmpeg;
    IFileManager fileManager;
    IUserInterface userInterface;

    int Count { get; set; } = 0;

    void OnProgress(FfmpegOutput output)
    {
      // Don't print 0%;
      if (output.Percentage.CompareTo(0) == 0)
        return;

      userInterface.PrintTmp($"{Count++} {output.File.Name} {output.Percentage.ToString("N2")}%");
    }

    public VideoConverter Configure()
    {
      ffmpeg = new Ffmpeg
      {
        Preset = Preset.Medium,
        Codec = "libx265",
        Crf = 28,
        FastStart = true,
        OnProgress = OnProgress
      };

      fileManager = new FileManager
      {
        Path = FilePath
      };

      userInterface = new UserInterface();

      return this;
    }

    public void Start()
    {
      var files = fileManager.GetVideoFilesToConvert();

      userInterface.PrintPermanent($"Converting {files.Count} files");

      foreach (var file in files)
      {
        // Move to a new name in case app crashes
        var tmpFile = file.Move($"{file.Name}.tmp");
        file.Extension = ".mp4";
        ffmpeg.Convert(tmpFile,
                       file);

        tmpFile.Remove();
        fileManager.SaveFile(file.ToString());
        userInterface.PrintPermanent($"Finished: {file}");
      }

      userInterface.PrintPermanent("Done!");
    }
  }
}