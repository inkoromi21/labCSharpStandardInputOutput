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

        public TextEditor()
        {
            _currentFile = new FileWithSerialization();
            _history = new List<TextEditorMemento>();
            _currentStateIndex = -1;
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

            canUndo = _currentStateIndex > 0;

            if (canUndo)
            {
                string previousContent;

                _currentStateIndex = _currentStateIndex - 1;
                previousContent = _history[_currentStateIndex].Content;
                _currentFile.Content = previousContent;
            }
        }

        public void Redo()
        {
            int lastIndex;
            bool canRedo;

            lastIndex = _history.Count - 1;
            canRedo = _currentStateIndex < lastIndex;

            if (canRedo)
            {
                string nextContent;

                _currentStateIndex = _currentStateIndex + 1;
                nextContent = _history[_currentStateIndex].Content;
                _currentFile.Content = nextContent;
            }
        }

        private void SaveState()
        {
            int lastIndex;
            bool notAtEnd;

            lastIndex = _history.Count - 1;
            notAtEnd = _currentStateIndex < lastIndex;

            if (notAtEnd)
            {
                int startIndex;
                int countToRemove;

                startIndex = _currentStateIndex + 1;
                countToRemove = _history.Count - startIndex;
                _history.RemoveRange(startIndex, countToRemove);
            }

            TextEditorMemento newState;
            int historyCount;

            newState = new TextEditorMemento(_currentFile.Content);
            _history.Add(newState);

            historyCount = _history.Count;
            _currentStateIndex = historyCount - 1;
        }

        public List<string> GetHistoryInfo()
        {
            List<string> historyInfo;
            int historyCount;

            historyInfo = new List<string>();
            historyCount = _history.Count;

            for (int i = 0; i < historyCount; i++)
            {
                string marker;
                TextEditorMemento state;
                DateTime stateTime;
                int contentLength;
                string info;

                if (i == _currentStateIndex)
                {
                    marker = "-> ";
                }
                else
                {
                    marker = "   ";
                }

                state = _history[i];
                stateTime = state.Timestamp;
                contentLength = state.Content.Length;

                info = marker + (i + 1) + ". " + stateTime + ": " + contentLength + " characters";
                historyInfo.Add(info);
            }

            return historyInfo;
        }
    }
}
