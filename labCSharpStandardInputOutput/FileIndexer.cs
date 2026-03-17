using System;
using System.Collections.Generic;
using System.IO;

namespace TextFileEditor
{
  public class FileIndexer
  {
    private Dictionary<string, List<string>> _index;
    private string _directoryPath;

    public FileIndexer()
    {
      _index = new Dictionary<string, List<string>>();
      _directoryPath = string.Empty;
    }

    public void IndexDirectory(string directoryPath, List<string> keywords, bool searchSubdirectories)
    {
      bool directoryExists;
      SearchOption searchOption;
      string[] allFiles;
      string fileContent;
      string contentLower;
      string keywordLower;
      bool containsKeyword;
      List<string> filesForKeyword;
      bool fileAlreadyAdded;

      _directoryPath = directoryPath;
      _index.Clear();

      directoryExists = Directory.Exists(directoryPath);

      if (!directoryExists)
      {
        return;
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

      foreach (string keyword in keywords)
      {
        _index[keyword] = new List<string>();
      }

      foreach (string file in allFiles)
      {
        try
        {
          fileContent = File.ReadAllText(file);
          contentLower = fileContent.ToLower();

          foreach (string keyword in keywords)
          {
            keywordLower = keyword.ToLower();
            containsKeyword = contentLower.Contains(keywordLower);

            if (containsKeyword)
            {
              filesForKeyword = _index[keyword];
              fileAlreadyAdded = filesForKeyword.Contains(file);

              if (!fileAlreadyAdded)
              {
                filesForKeyword.Add(file);
              }
            }
          }
        }
        catch
        {
          continue;
        }
      }
    }

    public List<string> FindFilesByKeyword(string keyword)
    {
      bool keywordExists;
      List<string> foundFiles;
      List<string> emptyList;

      keywordExists = _index.ContainsKey(keyword);

      if (keywordExists)
      {
        foundFiles = _index[keyword];
        return foundFiles;
      }

      emptyList = new List<string>();
      return emptyList;
    }

    public Dictionary<string, List<string>> GetAllIndexData()
    {
      return _index;
    }

    public void PrintIndex()
    {
      string keyword;
      List<string> files;
      int filesCount;
      string file;

      Console.WriteLine("\n=== FILE INDEX ===");

      foreach (KeyValuePair<string, List<string>> pair in _index)
      {
        keyword = pair.Key;
        files = pair.Value;
        filesCount = files.Count;

        Console.WriteLine("Keyword: '" + keyword + "'");
        Console.WriteLine("Files found: " + filesCount);

        foreach (string currentFile in files)
        {
          file = currentFile;
          Console.WriteLine("  - " + file);
        }

        Console.WriteLine();
      }
    }
  }
}