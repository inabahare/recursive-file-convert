using System;
using RecursiveFileConvert;

namespace VideoConvert
{
  public interface IFfmpeg
  {
    void Convert(FileName input, FileName output);
    Action<FfmpegOutput> OnProgress { get; set; }

    Preset Preset { get; set; }
    string Codec { get; set; }
    int Crf { get; set; }
    bool FastStart { get; set; }
  }
}
