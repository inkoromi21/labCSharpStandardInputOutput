using System;
using System.Collections.Generic;
using System.IO;

namespace TextFileEditor
{
  class Program
  {
    static void Main(string[] args)
    {
      bool isRunning;
      string userChoice;

      isRunning = true;

      while (isRunning)
      {
        Console.Clear();
        Console.WriteLine("=== TEXT EDITOR ===");
        Console.WriteLine("1. Text editor");
        Console.WriteLine("2. Search files by keywords");
        Console.WriteLine("3. Index files in directory");
        Console.WriteLine("4. Exit");
        Console.Write("Choose action: ");

        userChoice = Console.ReadLine();

        if (userChoice == "1")
        {
          RunTextEditor();
        }
        else if (userChoice == "2")
        {
          RunFileSearch();
        }
        else if (userChoice == "3")
        {
          RunFileIndexer();
        }
        else if (userChoice == "4")
        {
          isRunning = false;
        }
        else
        {
          Console.WriteLine("Invalid choice!");
          Console.ReadKey();
        }
      }
    }

    static void RunTextEditor()
    {
      TextEditor editor;
      string userChoice;
      string filePath;
      bool fileExists;
      bool isEditing;
      string command;
      string newText;

      editor = new TextEditor();

      Console.Clear();
      Console.WriteLine("=== TEXT EDITOR ===");
      Console.WriteLine("1. Open file");
      Console.WriteLine("2. Create new file");
      Console.Write("Choose: ");

      userChoice = Console.ReadLine();

      if (userChoice == "1")
      {
        Console.Write("Enter file path: ");
        filePath = Console.ReadLine();

        fileExists = File.Exists(filePath);

        if (fileExists)
        {
          editor.OpenFile(filePath);
        }
        else
        {
          Console.WriteLine("File not found!");
          Console.ReadKey();
          return;
        }
      }
      else if (userChoice == "2")
      {
        Console.Write("Enter path for new file: ");
        filePath = Console.ReadLine();
        editor.CreateNewFile(filePath);
      }
      else
      {
        return;
      }

      isEditing = true;

      while (isEditing)
      {
        Console.Clear();
        Console.WriteLine("=== EDITING ===");
        Console.WriteLine("Current text:");
        Console.WriteLine(new string('-', 50));
        Console.WriteLine(editor.GetContent());
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("1. Add text");
        Console.WriteLine("2. Undo");
        Console.WriteLine("3. Redo");
        Console.WriteLine("4. Save file");
        Console.WriteLine("5. Show history");
        Console.WriteLine("6. Exit");
        Console.Write("Choose action: ");

        command = Console.ReadLine();

        if (command == "1")
        {
          Console.Write("Enter text: ");
          newText = Console.ReadLine();
          editor.AddText(newText);
        }
        else if (command == "2")
        {
          editor.Undo();
        }
        else if (command == "3")
        {
          editor.Redo();
        }
        else if (command == "4")
        {
          editor.SaveFile();
          Console.WriteLine("File saved!");
          Console.ReadKey();
        }
        else if (command == "5")
        {
          List<string> historyInfo;

          Console.WriteLine("\nHistory of changes:");
          historyInfo = editor.GetHistoryInfo();

          foreach (string info in historyInfo)
          {
            Console.WriteLine(info);
          }

          Console.ReadKey();
        }
        else if (command == "6")
        {
          isEditing = false;
        }
      }
    }

    static void RunFileSearch()
    {
      string keywordsInput;
      string[] keywordsArray;
      List<string> keywords;
      FileSearcher searcher;
      string directoryPath;
      List<string> foundFiles;
      int filesCount;
      int keywordIndex;
      string trimmedKeyword;
      int fileIndex;

      Console.Clear();
      Console.WriteLine("=== FILE SEARCH ===");

      Console.Write("Enter keywords (comma separated): ");
      keywordsInput = Console.ReadLine();

      keywordsArray = keywordsInput.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      keywords = new List<string>();

      for (keywordIndex = 0; keywordIndex < keywordsArray.Length; ++keywordIndex)
      {
        trimmedKeyword = keywordsArray[keywordIndex].Trim();
        keywords.Add(trimmedKeyword);
      }

      searcher = new FileSearcher();

      Console.Write("Enter directory path to search: ");
      directoryPath = Console.ReadLine();

      foundFiles = searcher.SearchInDirectory(directoryPath, keywords, true);
      filesCount = foundFiles.Count;

      Console.WriteLine("\nFiles found: " + filesCount);

      for (fileIndex = 0; fileIndex < foundFiles.Count; ++fileIndex)
      {
        Console.WriteLine(foundFiles[fileIndex]);
      }

      Console.ReadKey();
    }

    static void RunFileIndexer()
    {
      string directoryPath;
      string keywordsInput;
      string[] keywordsArray;
      List<string> keywords;
      FileIndexer indexer;
      string searchKeyword;
      List<string> files;
      int filesCount;
      int keywordIndex;
      string trimmedKeyword;
      int fileIndex;

      Console.Clear();
      Console.WriteLine("=== FILE INDEXING ===");

      Console.Write("Enter directory path for indexing: ");
      directoryPath = Console.ReadLine();

      Console.Write("Enter keywords for indexing (comma separated): ");
      keywordsInput = Console.ReadLine();

      keywordsArray = keywordsInput.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      keywords = new List<string>();

      for (keywordIndex = 0; keywordIndex < keywordsArray.Length; ++keywordIndex)
      {
        trimmedKeyword = keywordsArray[keywordIndex].Trim();
        keywords.Add(trimmedKeyword);
      }

      indexer = new FileIndexer();
      indexer.IndexDirectory(directoryPath, keywords, true);
      indexer.PrintIndex();

      Console.WriteLine("\nSearch by index:");
      Console.Write("Enter keyword to search: ");
      searchKeyword = Console.ReadLine();

      files = indexer.FindFilesByKeyword(searchKeyword);
      filesCount = files.Count;

      Console.WriteLine("Files found: " + filesCount);

      for (fileIndex = 0; fileIndex < files.Count; ++fileIndex)
      {
        Console.WriteLine("  - " + files[fileIndex]);
      }

      Console.ReadKey();
    }
  }
}