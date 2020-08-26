using System.Collections.Generic;

namespace RecursiveFileConvert
{
  public interface IFfmpeg
  {

  }
  public class Ffmpeg : IFfmpeg
  {
    readonly List<string> Params = new List<string>();

    public Ffmpeg()
    {

    }
  }
}