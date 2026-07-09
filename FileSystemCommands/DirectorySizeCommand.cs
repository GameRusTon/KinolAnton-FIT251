using CommandLib;
namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private readonly string _path;
    
    public DirectorySizeCommand(string path)
    {
        _path = path;
    }

    public void Execute()
    {
        var DirInfo = new DirectoryInfo(_path);
        if (!DirInfo.Exists)
        {
            Console.WriteLine($"Файл по указанной директории: {_path} найден не был.");
            return;
        }

        long size = 0;

        foreach(FileInfo file in DirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
        {
            size += file.Length;
        }

        Console.WriteLine($"Размер каталога по указанному пути: {size} байт");   
    }
}
