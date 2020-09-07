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
    public Action<FfmpegOutput> OnProgress { get; set; }

    string FfmpegParams
    {
      get => $"{_codec} {_crf} {_preset} {_fastStart}";
    }

    public void Convert(FileName input, FileName output)
    {
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