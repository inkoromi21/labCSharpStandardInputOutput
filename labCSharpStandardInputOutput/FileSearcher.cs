using System;
using System.Collections.Generic;
using System.IO;

namespace TextFileEditor
{
  public class FileSearcher
  {
    private List<string> _keywords;

    public FileSearcher()
    {
      _keywords = new List<string>();
    }

    public FileSearcher(List<string> keywords)
    {
      _keywords = keywords;
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

      foundFiles = new List<string>();
      directoryExists = Directory.Exists(directoryPath);

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

      allFiles = Directory.GetFiles(directoryPath, "*.txt", searchOption);

      foreach (string file in allFiles)
      {
        bool containsKeywords;

        containsKeywords = FileContainsKeywords(file);

        if (containsKeywords)
        {
          foundFiles.Add(file);
        }
      }

      return foundFiles;
    }

    private bool FileContainsKeywords(string filePath)
    {
      try
      {
        string fileContent;
        string contentLower;

        fileContent = File.ReadAllText(filePath);
        contentLower = fileContent.ToLower();

        foreach (string keyword in _keywords)
        {
          string keywordLower;
          bool containsKeyword;

          keywordLower = keyword.ToLower();
          containsKeyword = contentLower.Contains(keywordLower);

          if (containsKeyword)
          {
            return true;
          }
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