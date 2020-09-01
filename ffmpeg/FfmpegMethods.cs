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

    string FfmpegParams
    {
      get => $"{_codec} {_crf} {_preset} {_fastStart}";
    }

    int GetFrameCount(string videoPath)
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

      return int.Parse(actualFrameCount);
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
          var currentFrame = int.Parse(frameNumber);

          Console.Clear();
          Console.WriteLine($"{currentFrame}/{frameCount} {input}");
        }
      };

      command.Run("ffmpeg", $"-i \"{input}\" {FfmpegParams} \"{output}\"");
    }
  }
}