using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecursiveFileConvert
{
  public class FileName
  {
    public string FullPath { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }

    public string NameAndExtension { get => $"{Name}{Extension}"; }

    static string Match(Regex pattern, string value) =>
      pattern.Match(value).Value;

    string CopyString(string input) =>
      new String(input.ToArray());

    public FileName Move(string newName)
    {
      var newPath = Path.Join(FullPath, $"{newName}{Extension}");

      File.Move(ToString(), newPath);
      return new FileName
      {
        FullPath = CopyString(FullPath),
        Name = newName,
        Extension = CopyString(Extension)
      };
    }

    public override string ToString()
    {
      return Path.Join(FullPath, $"{Name}{Extension}");
    }

    public static explicit operator FileName(string fileString)
    {
      var pathPatten = new Regex(@"\/(\w|\/)+\/");
      var namePattern = new Regex(@"(?<=\/)[^\/]+(?=\.)");
      var extensionPattern = new Regex(@"\.\w+$");

      return new FileName
      {
        FullPath = Match(pathPatten, fileString),
        Name = Match(namePattern, fileString),
        Extension = Match(extensionPattern, fileString)
      };
    }
  }
}