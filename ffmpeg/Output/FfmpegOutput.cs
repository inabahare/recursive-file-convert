using System;
using RecursiveFileConvert;

namespace VideoConvert
{
  public class FfmpegOutput
  {
    public FileName File { get; set; }
    public double Percentage { get; set; }
    public int Frame { get; set; }
    /// <summary>
    /// Double because initial value is "0.0"
    /// </summary>
    /// <value></value>
    public double Fps { get; set; }
    public double Q { get; set; }
    public int Size { get; set; }
    public TimeSpan Time { get; set; }
    public TimeSpan TotalTime { get; set; }
    public double BitRate { get; set; }
    public double Speed { get; set; }
    public TimeSpan Duration { get; set; }
  }
}