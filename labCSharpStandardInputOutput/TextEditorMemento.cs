using System;
using System.Collections.Generic;

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

  public class TextEditor
  {
    private FileWithSerialization _currentFile;
    private List<TextEditorMemento> _history;
    private int _currentStateIndex;
    private int _incrementValue;
    private int _initialStateIndex;

    public TextEditor()
    {
      _currentFile = new FileWithSerialization();
      _history = new List<TextEditorMemento>();
      _currentStateIndex = -1;
      _incrementValue = 1;
      _initialStateIndex = -1;
    }

    public void OpenFile(string filePath)
    {
      _currentFile = new FileWithSerialization(filePath);
      SaveState();
    }

    public void CreateNewFile(string filePath)
    {
      _currentFile = new FileWithSerialization(filePath);
      _currentFile.Content = string.Empty;
      SaveState();
    }

    public string GetContent()
    {
      string content;

      content = _currentFile.Content;

      return content;
    }

    public void SetContent(string newContent)
    {
      _currentFile.Content = newContent;
      SaveState();
    }

    public void SaveFile()
    {
      _currentFile.SaveToTextFile();
    }

    public void Undo()
    {
      bool canUndo;
      int previousStateIndex;
      string previousContent;

      canUndo = _currentStateIndex > 0;

      if (canUndo)
      {
        previousStateIndex = _currentStateIndex - _incrementValue;
        _currentStateIndex = previousStateIndex;
        previousContent = _history[_currentStateIndex].Content;
        _currentFile.Content = previousContent;
      }
    }

    public void Redo()
    {
      int lastIndex;
      bool canRedo;
      int nextStateIndex;
      string nextContent;

      lastIndex = _history.Count - _incrementValue;
      canRedo = _currentStateIndex < lastIndex;

      if (canRedo)
      {
        nextStateIndex = _currentStateIndex + _incrementValue;
        _currentStateIndex = nextStateIndex;
        nextContent = _history[_currentStateIndex].Content;
        _currentFile.Content = nextContent;
      }
    }

    private void SaveState()
    {
      int lastIndex;
      bool notAtEnd;
      int startIndex;
      int countToRemove;
      TextEditorMemento newState;
      int historyCount;

      lastIndex = _history.Count - _incrementValue;
      notAtEnd = _currentStateIndex < lastIndex;

      if (notAtEnd)
      {
        startIndex = _currentStateIndex + _incrementValue;
        countToRemove = _history.Count - startIndex;
        _history.RemoveRange(startIndex, countToRemove);
      }

      newState = new TextEditorMemento(_currentFile.Content);
      _history.Add(newState);

      historyCount = _history.Count;
      _currentStateIndex = historyCount - _incrementValue;
    }

    public List<string> GetHistoryInfo()
    {
      List<string> historyInfo;
      int historyCount;
      int index;
      string marker;
      TextEditorMemento state;
      DateTime stateTime;
      int contentLength;
      string info;
      string currentMarkerPrefix;
      string otherMarkerPrefix;
      string charactersText;

      historyInfo = new List<string>();
      historyCount = _history.Count;
      index = 0;
      currentMarkerPrefix = "-> ";
      otherMarkerPrefix = "   ";
      charactersText = " characters";

      while (index < historyCount)
      {
        if (index == _currentStateIndex)
        {
          marker = currentMarkerPrefix;
        }
        else
        {
          marker = otherMarkerPrefix;
        }

        state = _history[index];
        stateTime = state.Timestamp;
        contentLength = state.Content.Length;

        info = marker + (index + _incrementValue) + ". " + stateTime + ": " + contentLength + charactersText;
        historyInfo.Add(info);

        index = index + _incrementValue;
      }

      return historyInfo;
    }
  }
}