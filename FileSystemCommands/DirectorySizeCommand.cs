using CommandLib;
namespace FileSystemCommands;

[DisplayName("Команда, вычисляющая размер каталога")]
[Version(1,0)]
public class DirectorySizeCommand : ICommand
{
    private readonly string _path;
    
    public DirectorySizeCommand(string path)
    {
        _path = path;
    }
    
    [DisplayName("Метод вычисления размера каталога")]
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
