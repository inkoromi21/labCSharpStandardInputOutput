using System;

namespace TextFileEditor
{
  public class TextEditorMemento
  {
    public string Content;
    public DateTime Timestamp;

    public TextEditorMemento(string content)
    {
      Content = content;
      Timestamp = DateTime.Now;
    }
  }
}