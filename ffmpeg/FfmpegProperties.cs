using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VideoConvert
{
  public partial class Ffmpeg : IFfmpeg
  {
    string _preset;
    string _codec;
    string _crf;
    string _fastStart;

    public Preset Preset
    {
      get
      {
        if (_preset == null || _preset.Length == 0)
          return default(Preset);

        var preset = RemoveParameter(_preset, "-preset ");
        return Enum.Parse<Preset>(_preset);
      }
      set => _preset = $"-preset {value.ToString().ToLower()}";
    }

    public string Codec
    {
      get
      {
        if (_codec == null)
          return null;

        var codec = RemoveParameter(_codec, "-c:v ");
        return codec;
      }
      set => _codec = $"-c:v {value}";
    }
    public int Crf
    {
      get
      {
        if (_crf == null)
          return 0;

        var crf = RemoveParameter(_crf, "-crf ");
        return int.Parse(crf);
      }
      set => _crf = $"-crf {value}";
    }
    public bool FastStart
    {
      get => _fastStart != null;
      set
      {
        if (!value)
          _fastStart = null;

        _fastStart = "-movflags faststart";
      }
    }

    string RemoveParameter(string input, string parameterName) =>
      input.Remove(0, parameterName.Length);
  }
}