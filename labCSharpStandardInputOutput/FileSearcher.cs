using System;
using System.Collections.Generic;
using System.IO;

namespace TextFileEditor
{
  public class FileSearcher
  {
    private List<string> _keywords;
    private int _incrementValue;
    private string _textFilePattern;

    public FileSearcher()
    {
      _keywords = new List<string>();
      _incrementValue = 1;
      _textFilePattern = "*.txt";
    }

    public FileSearcher(List<string> keywords)
    {
      _keywords = keywords;
      _incrementValue = 1;
      _textFilePattern = "*.txt";
    }

    public void SetKeywords(List<string> keywords)
    {
      _keywords = keywords;
    }

    public List<string> SearchInDirectory(string directoryPath, bool searchSubdirectories)
    {
      List<string> foundFiles;
      bool directoryExists;
      SearchOption searchOption;
      string[] allFiles;
      int index;
      string currentFile;
      bool fileHasKeywords;

      foundFiles = new List<string>();
      directoryExists = Directory.Exists(directoryPath);
      index = 0;

      if (!directoryExists)
      {
        return foundFiles;
      }

      if (searchSubdirectories)
      {
        searchOption = SearchOption.AllDirectories;
      }
      else
      {
        searchOption = SearchOption.TopDirectoryOnly;
      }

      allFiles = Directory.GetFiles(directoryPath, _textFilePattern, searchOption);

      while (index < allFiles.Length)
      {
        currentFile = allFiles[index];
        fileHasKeywords = FileContainsKeywords(currentFile);

        if (fileHasKeywords)
        {
          foundFiles.Add(currentFile);
        }

        index = index + _incrementValue;
      }

      return foundFiles;
    }

    private bool FileContainsKeywords(string filePath)
    {
      try
      {
        string fileContent;
        string contentLower;
        int keywordIndex;
        string currentKeyword;
        string keywordLower;
        bool keywordFound;

        fileContent = File.ReadAllText(filePath);
        contentLower = fileContent.ToLower();
        keywordIndex = 0;

        while (keywordIndex < _keywords.Count)
        {
          currentKeyword = _keywords[keywordIndex];
          keywordLower = currentKeyword.ToLower();
          keywordFound = contentLower.Contains(keywordLower);

          if (keywordFound)
          {
            return true;
          }

          keywordIndex = keywordIndex + _incrementValue;
        }
      }
      catch
      {
        return false;
      }

      return false;
    }
  }
}
