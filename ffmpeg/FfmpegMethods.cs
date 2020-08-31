using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VideoConvert
{
  public partial class Ffmpeg : IFfmpeg
  {
    string FfmpegParams
    {
      get => $"{_codec} {_crf} {_preset} {_fastStart}";
    }

    public void Convert(string input, string output)
    {
      var process = new Process
      {
        StartInfo = {
          FileName = "ffmpeg",
          Arguments = $"-i \"{input}\" {FfmpegParams} \"{output}\"",
          UseShellExecute = false,
          RedirectStandardOutput = true,
          RedirectStandardError = true
        }
      };

      process.Start();

      process.ErrorDataReceived +=
        (sender, line) =>
        {
          if (line.Data != null)
            Console.WriteLine(line.Data);
        };


      process.BeginErrorReadLine();
      process.WaitForExit();
    }
  }
}