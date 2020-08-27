using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VideoConvert
{
  public partial class Ffmpeg : IFfmpeg
  {
    readonly List<string> Params = new List<string>();

    public Ffmpeg() { }

    public void Convert(string input, string output)
    {
      var process = new Process
      {
        StartInfo = {
          FileName = "ffmpeg",
          Arguments = $"-y -i {input} {output}",
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