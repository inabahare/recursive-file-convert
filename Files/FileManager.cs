using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecursiveFileConvert
{
  public interface IFileManager
  {
    string Path { get; set; }
    List<FileName> GetVideoFiles();
  }

  public class FileManager : IFileManager
  {
    readonly Regex ExtensionRegex = new Regex(@"\.\w*$");

    readonly List<string> VideoTypes = new List<string> {
      "avi", "mp4", "mkv", "webm"
    };

    public string Path { get; set; }

    string removeFirstSpace(string extension) =>
      extension.Substring(1);

    bool IsValidVideoFile(string filePath)
    {

      var extension =
        ExtensionRegex
          .Match(filePath);

      if (extension.Length == 0)
        return false;

      var extensionNoSpace = removeFirstSpace(extension.Value);

      return VideoTypes.Contains(extensionNoSpace);
    }

    public List<FileName> GetVideoFiles()
    {
      var files =
        Directory
          .GetFiles(Path, "*", SearchOption.AllDirectories)
          .Where(IsValidVideoFile)
          .Select(path => (FileName)path)
          .ToList();

      return files;
    }
  }
}