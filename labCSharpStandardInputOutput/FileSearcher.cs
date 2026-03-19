using System;
using System.Collections.Generic;
using System.IO;

namespace TextFileEditor
{
  public class FileSearcher
  {
    public List<string> SearchInDirectory(string directoryPath, List<string> keywords, bool searchSubdirectories)
    {
      List<string> foundFiles;
      bool directoryExists;
      SearchOption searchOption;
      string[] allFiles;
      int fileIndex;
      string currentFile;
      bool fileHasKeywords;

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

      for (fileIndex = 0; fileIndex < allFiles.Length; ++fileIndex)
      {
        currentFile = allFiles[fileIndex];
        fileHasKeywords = FileContainsKeywords(currentFile, keywords);

        if (fileHasKeywords)
        {
          foundFiles.Add(currentFile);
        }
      }

      return foundFiles;
    }

    private bool FileContainsKeywords(string filePath, List<string> keywords)
    {
      bool result;
      string fileContent;
      string contentLower;
      int keywordIndex;
      string currentKeyword;
      string keywordLower;

      result = false;

      try
      {
        fileContent = File.ReadAllText(filePath);
        contentLower = fileContent.ToLower();

        for (keywordIndex = 0; keywordIndex < keywords.Count; ++keywordIndex)
        {
          currentKeyword = keywords[keywordIndex];
          keywordLower = currentKeyword.ToLower();

          if (contentLower.Contains(keywordLower))
          {
            result = true;
            break;
          }
        }
      }

      catch
      {
        result = false;
      }

      return result;
    }
  }
}