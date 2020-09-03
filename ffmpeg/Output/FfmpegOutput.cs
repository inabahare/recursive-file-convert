using System;

namespace VideoConvert
{
  public class FfmpegOutput
  {
    public double Percentage { get; set; }
    public int Frame { get; set; }
    public int Fps { get; set; }
    public double Q { get; set; }
    public int Size { get; set; }
    public TimeSpan Time { get; set; }
    public double BitRate { get; set; }
    public double Speed { get; set; }
  }
}