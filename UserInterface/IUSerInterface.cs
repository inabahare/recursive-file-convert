namespace VideoConvert.Ui
{
  public interface IUserInterface
  {
    /// <summary>
    /// 
    /// </summary>
    void PrintPermanent(string value);

    /// <summary>
    /// For things like displaying percentage. Gets removed next time printing is called
    /// </summary>
    void PrintTmp(string value);
  }
}