using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecursiveFileConvert
{
  public interface IFileManager
  {
    string Path { get; set; }
    List<string> Converted { get; set; }
    void SaveFile(string path);
    List<FileName> GetVideoFilesToConvert();

  }

  public class FileManager : IFileManager
  {
    readonly Regex ExtensionRegex = new Regex(@"\.\w*$");

    readonly List<string> VideoTypes = new List<string> {
      "avi", "mp4", "mkv", "webm"
    };

    public string Path { get; set; }
    public List<string> Converted { get; set; }

    void GetAlreadySavedVideos()
    {
      using (var file = new StreamReader(@"./converted"))
      {
        var contents = file.ReadToEnd();
        var list = contents.Split("\n").ToList();
        Converted = list;
      }
    }

    public FileManager()
    {
      GetAlreadySavedVideos();
    }

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

    bool IsNotAlreadyConverted(string filePath) =>
      !Converted.Contains(filePath);

    public List<FileName> GetVideoFilesToConvert()
    {
      var files =
        Directory
          .GetFiles(Path, "*", SearchOption.AllDirectories)
          .Where(IsValidVideoFile)
          .Where(IsNotAlreadyConverted)
          .Select(path => (FileName)path)
          .ToList();

      return files;
    }

    public void SaveFile(string filePath)
    {
      Converted.Add(filePath);
      using (var file = new StreamWriter(@"./converted", true))
        file.WriteLine(filePath);
    }
  }
}