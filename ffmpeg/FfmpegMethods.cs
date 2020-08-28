using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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
      var ffmpegParams = FfmpegParams;

      var process = new Process
      {
        StartInfo = {
          FileName = "ffmpeg",
          Arguments = $"-y -i {input} {ffmpegParams} {output}",
          UseShellExecute = false,
          RedirectStandardOutput = true,
          RedirectStandardError = true
        }
      };

      process.Start();
      process.BeginOutputReadLine();
      process.WaitForExit();
    }
  }
}