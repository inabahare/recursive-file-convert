using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Terminal;

namespace VideoConvert
{
  public partial class Ffmpeg : IFfmpeg
  {
    /// <summary>
    /// This catches the entire ffmpeg output string
    /// </summary>

    Regex nbFrames = new Regex(@"(?<=(nb_frames\=))\d+");

    public Action<double> OnProgress { get; set; }

    string FfmpegParams
    {
      get => $"{_codec} {_crf} {_preset} {_fastStart}";
    }

    double GetFrameCount(string videoPath)
    {
      return double.Parse("1");
    }

    public void Convert(string input, string output)
    {
      var frameCount = GetFrameCount(input);
      var outputFormatter = new OutputFormatter();

      var command = new Command
      {
        OnStdErr = data =>
        {
          // var ffmpegOutput = FullFfmpegOutput.Match(data);
          var output = outputFormatter.Format(data);

          Console.WriteLine($"\tFrame={output.Frame}");

          OnProgress(100);
        }
      };

      command.Run("ffmpeg", $"-i \"{input}\" {FfmpegParams} \"{output}\"");
    }
  }
}