using System;
using System.IO;
using System.Linq;

namespace RecursiveFileConvert
{
  class Program
  {
    static void Main(string[] args)
    {
      var path = "/home/inaba/Videos/Test";

      var fileManager = new FileManager
      {
        Path = path
      };

      var files = fileManager.GetVideoFiles();
      Console.WriteLine(files);
    }
  }
}
