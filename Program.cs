using System;
using System.IO;
using System.Linq;
using VideoConvert;

namespace RecursiveFileConvert
{
  class Program
  {
    static void Main(string[] args)
    {
      new VideoConverter
      {
        FilePath = "~/Videos/Test"
      }
      .Configure()
      .Start();
    }
  }
}
