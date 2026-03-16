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

            isRunning = true;

            while (isRunning)
            {
                string userChoice;
                string choiceOne;
                string choiceTwo;
                string choiceThree;
                string choiceFour;

                Console.Clear();
                Console.WriteLine("=== TEXT EDITOR ===");
                Console.WriteLine("1. Text editor");
                Console.WriteLine("2. Search files by keywords");
                Console.WriteLine("3. Index files in directory");
                Console.WriteLine("4. Exit");
                Console.Write("Choose action: ");

                userChoice = Console.ReadLine();
                choiceOne = "1";
                choiceTwo = "2";
                choiceThree = "3";
                choiceFour = "4";

                if (userChoice == choiceOne)
                {
                    RunTextEditor();
                }
                else if (userChoice == choiceTwo)
                {
                    RunFileSearch();
                }
                else if (userChoice == choiceThree)
                {
                    RunFileIndexer();
                }
                else if (userChoice == choiceFour)
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
            string choiceOne;
            string choiceTwo;

            editor = new TextEditor();

            Console.Clear();
            Console.WriteLine("=== TEXT EDITOR ===");
            Console.WriteLine("1. Open file");
            Console.WriteLine("2. Create new file");
            Console.Write("Choose: ");

            userChoice = Console.ReadLine();
            choiceOne = "1";
            choiceTwo = "2";

            if (userChoice == choiceOne)
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
            else if (userChoice == choiceTwo)
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

            isEditing = true;

            while (isEditing)
            {
                string currentContent;
                string command;
                string cmdOne;
                string cmdTwo;
                string cmdThree;
                string cmdFour;
                string cmdFive;
                string cmdSix;
                string cmdSeven;
                string cmdEight;

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
                cmdOne = "1";
                cmdTwo = "2";
                cmdThree = "3";
                cmdFour = "4";
                cmdFive = "5";
                cmdSix = "6";
                cmdSeven = "7";
                cmdEight = "8";

                if (command == cmdOne)
                {
                    string newText;

                    Console.Write("Enter new text: ");
                    newText = Console.ReadLine();
                    editor.SetContent(newText);
                }
                else if (command == cmdTwo)
                {
                    editor.Undo();
                }
                else if (command == cmdThree)
                {
                    editor.Redo();
                }
                else if (command == cmdFour)
                {
                    editor.SaveFile();
                    Console.WriteLine("File saved!");
                    Console.ReadKey();
                }
                else if (command == cmdFive)
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
                else if (command == cmdSix)
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
                else if (command == cmdSeven)
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
                else if (command == cmdEight)
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

            foreach (string keyword in keywordsArray)
            {
                string trimmedKeyword;

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

            foreach (string keyword in keywordsArray)
            {
                string trimmedKeyword;

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