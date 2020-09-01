using System;
using System.Diagnostics;

namespace Terminal
{
  public interface ICommand
  {
    // Action<object, DataReceivedEventArgs> OnStdout { get; set; }
    // Action<object, DataReceivedEventArgs> OnStdErr { get; set; }

    Action<string> OnStdout { get; set; }
    Action<string> OnStdErr { get; set; }
    void Run(string command, string parameters);
  }
  public class Command : ICommand
  {
    public Action<string> OnStdout { get; set; }
    public Action<string> OnStdErr { get; set; }

    Action<DataReceivedEventArgs, Action<string>> dataRecieved =
      (DataReceivedEventArgs line, Action<string> OnOutput) =>
        {
          if (OnOutput != null &&
              line.Data != null)
            OnOutput(line.Data);
        };

    public void Run(string command, string parameters)
    {
      var process = new Process
      {
        StartInfo = {
          FileName = command,
          Arguments = parameters,
          UseShellExecute = false,
          RedirectStandardOutput = true,
          RedirectStandardError = true
        }
      };

      process.ErrorDataReceived +=
        (sender, line) => dataRecieved(line, OnStdErr);

      process.OutputDataReceived +=
        (sender, line) => dataRecieved(line, OnStdout);


      process.Start();
      process.BeginErrorReadLine();
      process.BeginOutputReadLine();
      process.WaitForExit();
    }
  }
}