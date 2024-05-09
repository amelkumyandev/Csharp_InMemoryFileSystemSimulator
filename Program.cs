using System;
using System.Collections.Generic;
using System.Linq;

public class File
{
    public string Name { get; set; }
    public string Content { get; set; }

    public File(string name, string content)
    {
        Name = name;
        Content = content;
    }
}

public class Directory
{
    public string Name { get; set; }
    public Dictionary<string, File> Files { get; private set; }
    public Dictionary<string, Directory> Directories { get; private set; }

    public Directory(string name)
    {
        Name = name;
        Files = new Dictionary<string, File>();
        Directories = new Dictionary<string, Directory>();
    }

    public void AddFile(string fileName, string content)
    {
        Files[fileName] = new File(fileName, content);
    }

    public void AddDirectory(string dirName)
    {
        Directories[dirName] = new Directory(dirName);
    }

    public File GetFile(string fileName)
    {
        return Files.GetValueOrDefault(fileName);
    }

    public Directory GetDirectory(string dirName)
    {
        return Directories.GetValueOrDefault(dirName);
    }

    public bool DeleteFile(string fileName)
    {
        return Files.Remove(fileName);
    }

    public bool DeleteDirectory(string dirName)
    {
        return Directories.Remove(dirName);
    }

    public void ListContents()
    {
        foreach (var dir in Directories.Keys)
            Console.WriteLine("Dir: " + dir);
        foreach (var file in Files.Keys)
            Console.WriteLine("File: " + file);
    }
}

public class FileSystem
{
    private Directory root;

    public FileSystem()
    {
        root = new Directory("root");
    }

    public void CreateFile(string path, string content)
    {
        var parts = path.Split('/');
        var current = root;
        for (int i = 0; i < parts.Length - 1; i++)
        {
            current = current.GetDirectory(parts[i]) ?? current;
        }
        current.AddFile(parts.Last(), content);
    }

    public void CreateDirectory(string path)
    {
        var parts = path.Split('/');
        var current = root;
        for (int i = 0; i < parts.Length; i++)
        {
            var next = current.GetDirectory(parts[i]);
            if (next == null)
            {
                current.AddDirectory(parts[i]);
                next = current.GetDirectory(parts[i]);
            }
            current = next;
        }
    }

    public void DeleteFile(string path)
    {
        var parts = path.Split('/');
        var current = root;
        for (int i = 0; i < parts.Length - 1; i++)
        {
            current = current.GetDirectory(parts[i]) ?? current;
        }
        current.DeleteFile(parts.Last());
    }

    public void ListDirectoryContents(string path)
    {
        var parts = path.Split('/');
        var current = root;
        foreach (var part in parts)
        {
            current = current.GetDirectory(part) ?? current;
        }
        current.ListContents();
    }

    public string ReadFile(string path)
    {
        var parts = path.Split('/');
        var current = root;
        for (int i = 0; i < parts.Length - 1; i++)
        {
            current = current.GetDirectory(parts[i]) ?? current;
        }
        var file = current.GetFile(parts.Last());
        return file != null ? file.Content : "File not found!";
    }
}

class Program
{
    static void Main(string[] args)
    {
        FileSystem fs = new FileSystem();
        fs.CreateDirectory("Documents");
        fs.CreateFile("Documents/Resume.txt", "Experienced .NET Developer");
        Console.WriteLine(fs.ReadFile("Documents/Resume.txt")); // Should output the content of the Resume.txt
        fs.ListDirectoryContents("Documents"); // Should list 'Resume.txt'
    }
}
