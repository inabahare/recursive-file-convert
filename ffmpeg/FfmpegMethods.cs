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

    string FfmpegParams
    {
      get => $"{_codec} {_crf} {_preset} {_fastStart}";
    }

    int GetFrameCount(string videoPath)
    {
      return 0;
    }

    public void Convert(string input, string output)
    {
      var frameCount = GetFrameCount(input);
      var command = new Command
      {
        OnStdout = data => { Console.WriteLine($"StdOut {data}"); },
        OnStdErr = data => { Console.WriteLine($"StdErr {data}"); }
      };

      command.Run("ffmpeg", $"-i \"{input}\" {FfmpegParams} \"{output}\"");
      // var process = new Process
      // {
      //   StartInfo = {
      //     FileName = "ffmpeg",
      //     Arguments = $"-i \"{input}\" {FfmpegParams} \"{output}\"",
      //     UseShellExecute = false,
      //     RedirectStandardOutput = true,
      //     RedirectStandardError = true
      //   }
      // };

      // process.Start();

      // process.ErrorDataReceived +=
      //   (sender, line) =>
      //   {
      //     if (line.Data != null)
      //       Console.WriteLine(line.Data);
      //   };


      // process.BeginErrorReadLine();
      // process.WaitForExit();
    }
  }
}