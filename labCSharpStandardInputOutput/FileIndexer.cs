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
                    string fileContent;
                    string contentLower;

                    fileContent = File.ReadAllText(file);
                    contentLower = fileContent.ToLower();

                    foreach (string keyword in keywords)
                    {
                        string keywordLower;
                        bool containsKeyword;

                        keywordLower = keyword.ToLower();
                        containsKeyword = contentLower.Contains(keywordLower);

                        if (containsKeyword)
                        {
                            List<string> filesForKeyword;
                            bool fileAlreadyAdded;

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

            keywordExists = _index.ContainsKey(keyword);

            if (keywordExists)
            {
                List<string> foundFiles;

                foundFiles = _index[keyword];

                return foundFiles;
            }

            List<string> emptyList;

            emptyList = new List<string>();

            return emptyList;
        }

        public Dictionary<string, List<string>> GetAllIndexData()
        {
            return _index;
        }

        public void PrintIndex()
        {
            Console.WriteLine("\n=== FILE INDEX ===");

            foreach (var pair in _index)
            {
                string keyword;
                List<string> files;
                int filesCount;

                keyword = pair.Key;
                files = pair.Value;
                filesCount = files.Count;

                Console.WriteLine("Keyword: '" + keyword + "'");
                Console.WriteLine("Files found: " + filesCount);

                foreach (string file in files)
                {
                    Console.WriteLine("  - " + file);
                }

                Console.WriteLine();
            }
        }
    }
}
