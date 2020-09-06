using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace VideoConvert
{
  public class OutputFormatter
  {
    readonly static string NumberPattern = @"\d+(\.\d+)?";
    readonly static string HhMmSs = @"(\d\d:\d\d:\d\d\.\d\d)";

    readonly static string FramePattern = @"(frame=\s*\d+)";
    readonly static string FpsPattern = @"(fps=\s*\d+.?\d*)";
    readonly static string QPattern = @"(q=\s*\d+.?\d*)";
    readonly static string SizePattern = @"(size=\s*\d+.?\d*kB)";
    readonly static string TimePattern = $"(time=\\s*{HhMmSs})";
    readonly static string BitratePattern = @"(bitrate=\s*\d+\.\d+kbits\/s)";
    readonly static string SpeedPattern = @"(speed=\d+\.\d+x)";

    readonly Regex FullFfmpegOutput = new Regex($"{FramePattern}\\s{FpsPattern}\\s{QPattern}\\s{SizePattern}\\s{TimePattern}\\s{BitratePattern}\\s{SpeedPattern}");
    readonly Regex DurationPattern = new Regex($"(?<=Duration:\\s){HhMmSs}");

    Regex GenerateKeyRegex(string key) =>
      new Regex($"(?<={key.ToLower()}\\=)\\s*(((\\d\\d\\:){{2}}\\d\\d\\.\\d\\d)|((\\-)?(\\d+(\\.\\d+)?)))");

    readonly FfmpegOutput Output = new FfmpegOutput();

    private T GetValue<T>(string data, string key)
    {
      var keyPattern = GenerateKeyRegex(key);

      var value =
        keyPattern
          .Match(data)
          .Value
          .Trim(); // There is a chance of the value having trailing spaces, so nooope

      var converter = TypeDescriptor.GetConverter(typeof(T));

      return (T)converter.ConvertFrom(value);
    }

    void SetDuration(string data)
    {
      var match =
        DurationPattern
          .Match(data)
          .Value
          .Trim();

      Output.Duration = TimeSpan.Parse(match);
    }



    void HandleProcessing(string data)
    {
      Output.BitRate = GetValue<double>(data, "BitRate");
      Output.Fps = GetValue<double>(data, "FPS");
      Output.Frame = GetValue<int>(data, "Frame");
      Output.Q = GetValue<double>(data, "Q");
      Output.Size = GetValue<int>(data, "Size");
      Output.Speed = GetValue<double>(data, "Speed");
      Output.Time = GetValue<TimeSpan>(data, "Time");

      // TODO: Calculate percentge here
    }

    public FfmpegOutput Format(string data)
    {
      var isDuration = DurationPattern.Match(data).Success;
      if (isDuration)
        SetDuration(data);

      var isProcessing = FullFfmpegOutput.Match(data).Success;
      if (isProcessing)
        HandleProcessing(data);

      return Output;
    }
  }
}