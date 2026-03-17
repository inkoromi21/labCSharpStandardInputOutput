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
      int fileIndex;
      string currentFile;
      string fileContent;
      string contentLower;
      int keywordIndex;
      string currentKeyword;
      string keywordLower;
      bool containsKeyword;

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

      for (keywordIndex = 0; keywordIndex < keywords.Count; ++keywordIndex)
      {
        currentKeyword = keywords[keywordIndex];
        _index[currentKeyword] = new List<string>();
      }

      for (fileIndex = 0; fileIndex < allFiles.Length; ++fileIndex)
      {
        currentFile = allFiles[fileIndex];

        try
        {
          fileContent = File.ReadAllText(currentFile);
          contentLower = fileContent.ToLower();

          for (keywordIndex = 0; keywordIndex < keywords.Count; ++keywordIndex)
          {
            currentKeyword = keywords[keywordIndex];
            keywordLower = currentKeyword.ToLower();
            containsKeyword = contentLower.Contains(keywordLower);

            if (containsKeyword)
            {
              _index[currentKeyword].Add(currentFile);
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

    public void PrintIndex()
    {
      string currentKeyword;
      List<string> files;
      int fileIndex;
      int filesCount;
      string currentFile;

      Console.WriteLine("\n=== FILE INDEX ===");

      foreach (KeyValuePair<string, List<string>> pair in _index)
      {
        currentKeyword = pair.Key;
        files = pair.Value;
        filesCount = files.Count;

        Console.WriteLine("Keyword: '" + currentKeyword + "'");
        Console.WriteLine("Files found: " + filesCount);

        for (fileIndex = 0; fileIndex < files.Count; ++fileIndex)
        {
          currentFile = files[fileIndex];
          Console.WriteLine("  - " + currentFile);
        }

        Console.WriteLine();
      }
    }
  }
}