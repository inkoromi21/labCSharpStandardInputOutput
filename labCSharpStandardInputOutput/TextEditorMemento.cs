using System;
using System.Collections.Generic;
using System.IO;

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
    private Stack<TextEditorMemento> _historyStack;
    private Stack<TextEditorMemento> _redoStack;

    public TextEditor()
    {
      _currentFile = new FileWithSerialization();
      _historyStack = new Stack<TextEditorMemento>();
      _redoStack = new Stack<TextEditorMemento>();
    }

    public void OpenFile(string filePath)
    {
      bool fileExists;

      fileExists = File.Exists(filePath);

      if (fileExists)
      {
        _currentFile = new FileWithSerialization(filePath);
        _historyStack.Clear();
        _redoStack.Clear();
        SaveState();
      }
    }

    public void CreateNewFile(string filePath)
    {
      _currentFile = new FileWithSerialization(filePath);
      _currentFile.Content = string.Empty;
      _historyStack.Clear();
      _redoStack.Clear();
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
      _redoStack.Clear();
    }

    public void Undo()
    {
      bool canUndo;
      TextEditorMemento previousState;

      canUndo = _historyStack.Count > 0;

      if (canUndo)
      {
        _redoStack.Push(new TextEditorMemento(_currentFile.Content));
        previousState = _historyStack.Pop();
        _currentFile.Content = previousState.Content;
      }
    }

    public void Redo()
    {
      bool canRedo;
      TextEditorMemento nextState;

      canRedo = _redoStack.Count > 0;

      if (canRedo)
      {
        _historyStack.Push(new TextEditorMemento(_currentFile.Content));
        nextState = _redoStack.Pop();
        _currentFile.Content = nextState.Content;
      }
    }

    private void SaveState()
    {
      _historyStack.Push(new TextEditorMemento(_currentFile.Content));
    }

    public void SaveFile()
    {
      File.WriteAllText(_currentFile.FilePath, _currentFile.Content);
    }

    public List<string> GetHistoryInfo()
    {
      List<string> historyInfo;
      TextEditorMemento[] historyArray;
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
      historyArray = _historyStack.ToArray();
      currentMarkerPrefix = "-> ";
      otherMarkerPrefix = "   ";
      charactersText = " characters";
      offsetNumber = 1;

      for (stateIndex = 0; stateIndex < historyArray.Length; ++stateIndex)
      {
        if (stateIndex == 0)
        {
          marker = currentMarkerPrefix;
        }
        else
        {
          marker = otherMarkerPrefix;
        }

        currentState = historyArray[stateIndex];
        stateTime = currentState.Timestamp;
        contentLength = currentState.Content.Length;

        info = marker + (stateIndex + offsetNumber) + ". " + stateTime + ": " + contentLength + charactersText;
        historyInfo.Add(info);
      }

      return historyInfo;
    }
  }
}