namespace VideoConvert
{
  public interface IFfmpeg
  {
    void Convert(string input, string output);

    Preset Preset { get; set; }
    string Codec { get; set; }
    int Crf { get; set; }
    bool FastStart { get; set; }
  }
}
