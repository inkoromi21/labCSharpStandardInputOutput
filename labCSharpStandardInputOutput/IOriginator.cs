using System;

namespace TextFileEditor
{
  public interface IOriginator
  {
    object GetMemento();
    void SetMemento(object memento);
  }
}
