using CommandLib;
namespace FileSystemCommands;

[DisplayName("Команда, находящая файлы по маске")]
[Version(1,0)]
public class FindFilesCommand : ICommand
{
    private readonly string _path;
    private readonly string _mask;
    public FindFilesCommand(string path, string mask)
    {
        _path = path;
        _mask = mask;
    }

    [DisplayName("Метод нахождения файлов по маске")]
    public void Execute()
    {
        var DirInfo = new DirectoryInfo(_path);

        if (!DirInfo.Exists)
        {
            Console.WriteLine($"Файл по указанной директории: {_path} найден не был.");
            return;
        }

        string[] files = Directory.GetFiles(_path, _mask, SearchOption.AllDirectories);
        
        if (files.Length != 0)
        {
            Console.WriteLine("Найденные файлы:");
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }
        else
        Console.WriteLine("Файлов по заданной маске найдено не было.");
    }
}
