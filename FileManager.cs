using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecursiveFileConvert
{
  public class FileManager
  {
    readonly Regex ExtensionRegex = new Regex(@"\.\w*$");

    readonly List<string> VideoTypes = new List<string> {
      "avi", "mp4", "mkv", "webm"
    };

    public string Path { get; set; }

    string removeFirstSpace(string extension) =>
      extension.Substring(1);

    bool IsVideoFile(string filePath)
    {

      var extension =
        ExtensionRegex
          .Match(filePath);

      if (extension.Length == 0)
        return false;

      var extensionNoSpace = removeFirstSpace(extension.Value);

      return VideoTypes.Contains(extensionNoSpace);
    }

    public List<string> GetVideoFiles()
    {
      var files =
        Directory
          .GetFiles(Path, "*", SearchOption.AllDirectories)
          .Where(IsVideoFile)
          .ToList();

      return files;
    }
  }
}