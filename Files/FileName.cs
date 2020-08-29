using System.Text.RegularExpressions;

namespace RecursiveFileConvert
{
  public class FileName
  {
    public string Path { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }

    static string Match(Regex pattern, string value) =>
      pattern.Match(value).Value;

    public static explicit operator FileName(string fileString)
    {
      var pathPatten = new Regex(@"\/(\w|\/)+\/");
      var namePattern = new Regex(@"(?<=\/)[^\/]+(?=\.)");
      var extensionPattern = new Regex(@"\.\w+$");

      return new FileName
      {
        Path = Match(pathPatten, fileString),
        Name = Match(namePattern, fileString),
        Extension = Match(extensionPattern, fileString)
      };
    }
  }
}