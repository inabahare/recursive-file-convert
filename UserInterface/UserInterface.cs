using System;

namespace VideoConvert.Ui
{
  public class UserInterface : IUserInterface
  {
    string Storage { get; set; } = "";

    void Print(string value)
    {
      Console.Clear();
      Console.WriteLine(Storage);
      Console.WriteLine(value);
    }

    public void PrintPermanent(string value)
    {
      Print(value);
      Storage += $"\n{value}";
    }


    public void PrintTmp(string value)
    {
      Print(value);
    }
  }
}