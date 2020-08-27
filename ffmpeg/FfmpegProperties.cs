using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VideoConvert
{
  public partial class Ffmpeg : IFfmpeg
  {
    public Preset Preset { get; set; }
    public string Codec { get; set; }
    public int Crf { get; set; } = 28;
    public bool FastStart { get; set; }
  }
}