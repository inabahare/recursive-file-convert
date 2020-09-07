using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using RecursiveFileConvert;
using Terminal;

namespace VideoConvert
{
  public partial class Ffmpeg : IFfmpeg
  {
    /// <summary>
    /// This catches the entire ffmpeg output string
    /// </summary>

    Regex nbFrames = new Regex(@"(?<=(nb_frames\=))\d+");

    public Action<FfmpegOutput> OnProgress { get; set; }

    string FfmpegParams
    {
      get => $"{_codec} {_crf} {_preset} {_fastStart}";
    }

    double GetFrameCount(string videoPath)
    {
      return double.Parse("1");
    }

    public void Convert(FileName input, FileName output)
    {
      var frameCount = GetFrameCount(input.ToString());
      var outputFormatter = new OutputFormatter(output);

      var command = new Command
      {
        OnStdErr = data =>
        {
          var output = outputFormatter.Format(data);
          OnProgress(output);
        }
      };

      command.Run("ffmpeg", $"-i \"{input}\" {FfmpegParams} \"{output}\"");
    }
  }
}