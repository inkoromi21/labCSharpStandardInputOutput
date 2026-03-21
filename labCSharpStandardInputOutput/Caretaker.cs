using System;
using System.Collections.Generic;

namespace TextFileEditor
{
  public class Caretaker
  {
    private Stack<object> _undoStack;
    private Stack<object> _redoStack;

    public Caretaker()
    {
      _undoStack = new Stack<object>();
      _redoStack = new Stack<object>();
    }

    public void SaveState(IOriginator originator)
    {
      object memento;

      memento = originator.GetMemento();
      _undoStack.Push(memento);
    }

    public void Undo(IOriginator originator)
    {
      bool canUndo;
      object memento;

      canUndo = _undoStack.Count > 0;

      if (canUndo)
      {
        memento = originator.GetMemento();
        _redoStack.Push(memento);

        memento = _undoStack.Pop();
        originator.SetMemento(memento);
      }
    }

    public void Redo(IOriginator originator)
    {
      bool canRedo;
      object memento;

      canRedo = _redoStack.Count > 0;

      if (canRedo)
      {
        memento = originator.GetMemento();
        _undoStack.Push(memento);

        memento = _redoStack.Pop();
        originator.SetMemento(memento);
      }
    }

    public void ClearRedo()
    {
      _redoStack.Clear();
    }

    public void Clear()
    {
      _undoStack.Clear();
      _redoStack.Clear();
    }

    public List<TextEditorMemento> GetHistory()
    {
      List<TextEditorMemento> historyList;
      object[] historyArray;
      int stateIndex;
      TextEditorMemento memento;

      historyList = new List<TextEditorMemento>();
      historyArray = _undoStack.ToArray();

      for (stateIndex = 0; stateIndex < historyArray.Length; ++stateIndex)
      {
        memento = historyArray[stateIndex] as TextEditorMemento;

        if (memento != null)
        {
          historyList.Add(memento);
        }
      }

      return historyList;
    }
  }
}