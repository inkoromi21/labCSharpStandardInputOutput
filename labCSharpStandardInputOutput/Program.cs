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
      string menuChoiceEditor;
      string menuChoiceSearch;
      string menuChoiceIndex;
      string menuChoiceExit;

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
        menuChoiceEditor = "1";
        menuChoiceSearch = "2";
        menuChoiceIndex = "3";
        menuChoiceExit = "4";

        if (userChoice == menuChoiceEditor)
        {
          RunTextEditor();
        }
        else if (userChoice == menuChoiceSearch)
        {
          RunFileSearch();
        }
        else if (userChoice == menuChoiceIndex)
        {
          RunFileIndexer();
        }
        else if (userChoice == menuChoiceExit)
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
      string editorChoiceOpen;
      string editorChoiceNew;

      editor = new TextEditor();

      Console.Clear();
      Console.WriteLine("=== TEXT EDITOR ===");
      Console.WriteLine("1. Open file");
      Console.WriteLine("2. Create new file");
      Console.Write("Choose: ");

      userChoice = Console.ReadLine();
      editorChoiceOpen = "1";
      editorChoiceNew = "2";

      if (userChoice == editorChoiceOpen)
      {
        string filePath;
        bool fileExists;

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
      else if (userChoice == editorChoiceNew)
      {
        string filePath;

        Console.Write("Enter path for new file: ");
        filePath = Console.ReadLine();
        editor.CreateNewFile(filePath);
      }
      else
      {
        return;
      }

      bool isEditing;
      string currentContent;
      string command;
      string editChoiceEdit;
      string editChoiceUndo;
      string editChoiceRedo;
      string editChoiceSave;
      string editChoiceHistory;
      string editChoiceBinary;
      string editChoiceXml;
      string editChoiceExit;
      string newText;

      isEditing = true;

      while (isEditing)
      {
        Console.Clear();
        Console.WriteLine("=== EDITING ===");
        Console.WriteLine("Current text:");
        Console.WriteLine(new string('-', 50));

        currentContent = editor.GetContent();
        Console.WriteLine(currentContent);

        Console.WriteLine(new string('-', 50));
        Console.WriteLine("1. Edit text");
        Console.WriteLine("2. Undo");
        Console.WriteLine("3. Redo");
        Console.WriteLine("4. Save file");
        Console.WriteLine("5. Show history");
        Console.WriteLine("6. Serialize to Binary");
        Console.WriteLine("7. Serialize to XML");
        Console.WriteLine("8. Exit to main menu");
        Console.Write("Choose action: ");

        command = Console.ReadLine();
        editChoiceEdit = "1";
        editChoiceUndo = "2";
        editChoiceRedo = "3";
        editChoiceSave = "4";
        editChoiceHistory = "5";
        editChoiceBinary = "6";
        editChoiceXml = "7";
        editChoiceExit = "8";

        if (command == editChoiceEdit)
        {
          Console.Write("Enter new text: ");
          newText = Console.ReadLine();
          editor.SetContent(newText);
        }
        else if (command == editChoiceUndo)
        {
          editor.Undo();
        }
        else if (command == editChoiceRedo)
        {
          editor.Redo();
        }
        else if (command == editChoiceSave)
        {
          editor.SaveFile();
          Console.WriteLine("File saved!");
          Console.ReadKey();
        }
        else if (command == editChoiceHistory)
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
        else if (command == editChoiceBinary)
        {
          string binaryPath;
          FileWithSerialization file;
          string content;

          Console.Write("Enter path for binary file: ");
          binaryPath = Console.ReadLine();

          file = new FileWithSerialization();
          content = editor.GetContent();
          file.Content = content;
          file.FilePath = "temp";

          file.SaveToBinary(binaryPath);

          Console.WriteLine("Serialization completed!");
          Console.ReadKey();
        }
        else if (command == editChoiceXml)
        {
          string xmlPath;
          FileWithSerialization fileXml;
          string content;

          Console.Write("Enter path for XML file: ");
          xmlPath = Console.ReadLine();

          fileXml = new FileWithSerialization();
          content = editor.GetContent();
          fileXml.Content = content;
          fileXml.FilePath = "temp";

          fileXml.SaveToXml(xmlPath);

          Console.WriteLine("Serialization completed!");
          Console.ReadKey();
        }
        else if (command == editChoiceExit)
        {
          isEditing = false;
        }
      }
    }

    static void RunFileSearch()
    {
      string keywordsInput;
      char[] separators;
      string[] keywordsArray;
      List<string> keywords;
      FileSearcher searcher;
      string directoryPath;
      List<string> foundFiles;
      int filesCount;

      Console.Clear();
      Console.WriteLine("=== FILE SEARCH ===");

      Console.Write("Enter keywords (comma separated): ");
      keywordsInput = Console.ReadLine();

      separators = new char[] { ',' };
      keywordsArray = keywordsInput.Split(separators, StringSplitOptions.RemoveEmptyEntries);

      keywords = new List<string>();

      string trimmedKeyword;

      foreach (string keyword in keywordsArray)
      {
        trimmedKeyword = keyword.Trim();
        keywords.Add(trimmedKeyword);
      }

      searcher = new FileSearcher(keywords);

      Console.Write("Enter directory path to search: ");
      directoryPath = Console.ReadLine();

      foundFiles = searcher.SearchInDirectory(directoryPath, true);

      filesCount = foundFiles.Count;

      Console.WriteLine("\nFiles found: " + filesCount);

      foreach (string file in foundFiles)
      {
        Console.WriteLine(file);
      }

      Console.ReadKey();
    }

    static void RunFileIndexer()
    {
      string directoryPath;
      string keywordsInput;
      char[] separators;
      string[] keywordsArray;
      List<string> keywords;
      FileIndexer indexer;
      string searchKeyword;
      List<string> files;
      int filesCount;

      Console.Clear();
      Console.WriteLine("=== FILE INDEXING ===");

      Console.Write("Enter directory path for indexing: ");
      directoryPath = Console.ReadLine();

      Console.Write("Enter keywords for indexing (comma separated): ");
      keywordsInput = Console.ReadLine();

      separators = new char[] { ',' };
      keywordsArray = keywordsInput.Split(separators, StringSplitOptions.RemoveEmptyEntries);

      keywords = new List<string>();

      string trimmedKeyword;

      foreach (string keyword in keywordsArray)
      {
        trimmedKeyword = keyword.Trim();
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

      foreach (string file in files)
      {
        Console.WriteLine("  - " + file);
      }

      Console.ReadKey();
    }
  }
}