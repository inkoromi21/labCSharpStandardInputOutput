using System;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TextFileEditor
{
  [Serializable]
  public class FileWithSerialization
  {
    public string FilePath;
    public string Content;

    public FileWithSerialization()
    {
      FilePath = string.Empty;
      Content = string.Empty;
    }

    public FileWithSerialization(string filePath)
    {
      bool fileExists;

      FilePath = filePath;
      fileExists = File.Exists(filePath);

      if (fileExists)
      {
        Content = File.ReadAllText(filePath);
      }
      else
      {
        Content = string.Empty;
      }
    }

    public void SaveToBinary(string savePath)
    {
      BinaryFormatter formatter;
      FileStream fileStream;
      FileMode fileMode;

      formatter = new BinaryFormatter();
      fileMode = FileMode.Create;
      fileStream = new FileStream(savePath, fileMode);

      using (fileStream)
      {
        formatter.Serialize(fileStream, this);
      }
    }

    public static FileWithSerialization LoadFromBinary(string filePath)
    {
      BinaryFormatter formatter;
      FileStream fileStream;
      FileWithSerialization loadedObject;
      FileMode fileMode;

      formatter = new BinaryFormatter();
      fileMode = FileMode.Open;
      fileStream = new FileStream(filePath, fileMode);

      using (fileStream)
      {
        loadedObject = (FileWithSerialization)formatter.Deserialize(fileStream);
      }

      return loadedObject;
    }

    public void SaveToXml(string savePath)
    {
      XmlSerializer serializer;
      FileStream fileStream;
      FileMode fileMode;
      Type serializationType;

      serializationType = typeof(FileWithSerialization);
      serializer = new XmlSerializer(serializationType);
      fileMode = FileMode.Create;
      fileStream = new FileStream(savePath, fileMode);

      using (fileStream)
      {
        serializer.Serialize(fileStream, this);
      }
    }

    public static FileWithSerialization LoadFromXml(string filePath)
    {
      XmlSerializer serializer;
      FileStream fileStream;
      FileWithSerialization loadedObject;
      FileMode fileMode;
      Type serializationType;

      serializationType = typeof(FileWithSerialization);
      serializer = new XmlSerializer(serializationType);
      fileMode = FileMode.Open;
      fileStream = new FileStream(filePath, fileMode);

      using (fileStream)
      {
        loadedObject = (FileWithSerialization)serializer.Deserialize(fileStream);
      }

      return loadedObject;
    }

    public void SaveToTextFile()
    {
      File.WriteAllText(FilePath, Content);
    }

    public void LoadFromTextFile()
    {
      bool fileExists;

      fileExists = File.Exists(FilePath);

      if (fileExists)
      {
        Content = File.ReadAllText(FilePath);
      }
    }
  }
}