using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Terminal;

namespace VideoConvert
{
  public partial class Ffmpeg : IFfmpeg
  {
    Regex nbFrames = new Regex(@"(?<=(nb_frames\=))\d+");
    Regex frameCountPattern = new Regex(@"frame=\s*\d+");
    Regex numberPattern = new Regex(@"\d+");
    Regex currentFramePattern = new Regex(@"frame=\s*\d+(?=(\sfps))");

    public Action<double> OnPercentage { get; set; }

    string FfmpegParams
    {
      get => $"{_codec} {_crf} {_preset} {_fastStart}";
    }

    double GetFrameCount(string videoPath)
    {
      var stdOut = "";
      var command = new Command
      {
        OnStdErr =
          data => stdOut += data
      };

      command.Run("ffmpeg", $"-i \"{videoPath}\" -map 0:v:0 -c copy -f null -");

      // Can't use a negative lookbehind for this since space count keeps changing
      var frameCount = frameCountPattern.Match(stdOut).Value;
      var actualFrameCount = numberPattern.Match(frameCount).Value;

      return double.Parse(actualFrameCount);
    }

    public void Convert(string input, string output)
    {
      var frameCount = GetFrameCount(input);
      var command = new Command
      {
        OnStdErr = data =>
        {
          var frame = currentFramePattern.Match(data).Value;
          if (frame.Length == 0)
            return;
          var frameNumber = numberPattern.Match(frame).Value;
          var currentFrame = double.Parse(frameNumber);

          var percentage = (currentFrame / frameCount) * 100;

          OnPercentage(percentage);
        }
      };

      command.Run("ffmpeg", $"-i \"{input}\" {FfmpegParams} \"{output}\"");
    }
  }
}