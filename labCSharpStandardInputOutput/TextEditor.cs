using System;
using System.Collections.Generic;
using System.IO;

namespace TextFileEditor
{
  public class TextEditor : IOriginator
  {
    private FileWithSerialization _currentFile;
    private Caretaker _caretaker;

    public TextEditor()
    {
      _currentFile = new FileWithSerialization();
      _caretaker = new Caretaker();
    }

    public void OpenFile(string filePath)
    {
      bool fileExists;

      fileExists = File.Exists(filePath);

      if (fileExists)
      {
        _currentFile = new FileWithSerialization(filePath);
        _caretaker.Clear();
        SaveState();
      }
    }

    public void CreateNewFile(string filePath)
    {
      _currentFile = new FileWithSerialization(filePath);
      _currentFile.Content = string.Empty;
      _caretaker.Clear();
      SaveState();
    }

    public string GetContent()
    {
      string content;

      content = _currentFile.Content;

      return content;
    }

    public void AddText(string newText)
    {
      SaveState();
      _currentFile.Content = _currentFile.Content + newText + Environment.NewLine;
      _caretaker.ClearRedo();
    }

    public void Undo()
    {
      _caretaker.Undo(this);
    }

    public void Redo()
    {
      _caretaker.Redo(this);
    }

    private void SaveState()
    {
      _caretaker.SaveState(this);
    }

    public void SaveFile()
    {
      File.WriteAllText(_currentFile.FilePath, _currentFile.Content);
    }

    public List<string> GetHistoryInfo()
    {
      List<string> historyInfo;
      List<TextEditorMemento> historyList;
      int stateIndex;
      string marker;
      TextEditorMemento currentState;
      DateTime stateTime;
      int contentLength;
      int offsetNumber;
      string info;
      string currentMarkerPrefix;
      string otherMarkerPrefix;
      string charactersText;

      historyInfo = new List<string>();
      historyList = _caretaker.GetHistory();
      currentMarkerPrefix = "-> ";
      otherMarkerPrefix = "   ";
      charactersText = " characters";
      offsetNumber = 1;

      for (stateIndex = 0; stateIndex < historyList.Count; ++stateIndex)
      {
        if (stateIndex == 0)
        {
          marker = currentMarkerPrefix;
        }
        else
        {
          marker = otherMarkerPrefix;
        }

        currentState = historyList[stateIndex];
        stateTime = currentState.Timestamp;
        contentLength = currentState.Content.Length;

        info = marker + (stateIndex + offsetNumber) + ". " + stateTime + ": " + contentLength + charactersText;
        historyInfo.Add(info);
      }

      return historyInfo;
    }

    public object GetMemento()
    {
      TextEditorMemento memento;

      memento = new TextEditorMemento(_currentFile.Content);

      return memento;
    }

    public void SetMemento(object memento)
    {
      TextEditorMemento textEditorMemento;

      textEditorMemento = memento as TextEditorMemento;

      if (textEditorMemento != null)
      {
        _currentFile.Content = textEditorMemento.Content;
      }
    }
  }
}