using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Enums;

namespace recursive_file_convert
{
  class Ffmpeg
  {
    public static async Task Convert(string input,
                                        string output,
                                        Action<TimeSpan> onProgress)
    {
      await FFMpegArguments
          .FromFileInput(input)
          .OutputToFile(output, true, options => options
              .WithVideoCodec("libx265")
              .WithConstantRateFactor(28)
              .WithFastStart()
          )
          .NotifyOnProgress(onProgress)
          .ProcessAsynchronously();
    }
  }
}
