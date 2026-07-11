using CommandLib;
using FileSystemCommands;
using AssemblyInspector;
using Xunit;
namespace task09tests;

public class AssemblyInspectorTests
{
    [Fact]
    public void AssemblyInspector_PrintsCorrectMetadata()
    {
        string _path = typeof(DirectorySizeCommand).Assembly.Location;

        string[] args = new string [] {_path};

        var origin = Console.Out;

        using (StringWriter line = new StringWriter())
        {
            Console.SetOut(line);

            Program.Main(args);

            string output = line.ToString();
            Assert.Contains("Класс: FileSystemCommands.DirectorySizeCommand", output);
            Assert.Contains("DisplayNameAttribute", output);
            Assert.Contains("VersionAttribute", output);
            Assert.Contains("Параметры конструктора:", output);
            Assert.Contains("Тип: String - Имя: path", output);
            Assert.Contains("Имя метода: Execute", output);
            Assert.Contains("Атрибуты метода:", output);

            Assert.Contains("Класс: FileSystemCommands.FindFilesCommand", output);
            Assert.Contains("Тип: String - Имя: mask", output);
        }
        Console.SetOut(origin);
    }

    [Fact]
    public void AssemblyInspector_NoPathToDLL()
    {
        string[] args = Array.Empty<string>();

        var origin = Console.Out;

        using (StringWriter line = new StringWriter())
        {
            Console.SetOut(line);

            Program.Main(args);

            string output = line.ToString();
            Assert.Contains("Путь до библиотеки указан не был.", output);
        }
        Console.SetOut(origin);
    }

    [Fact]
    public void AssemblyInspector_NonExistentLibrary()
    {
        string[] args = new string [] {"MissingFile.dll"};

        var origin = Console.Out;

        using (StringWriter line = new StringWriter())
        {
            Console.SetOut(line);

            Program.Main(args);

            string output = line.ToString();
            Assert.Contains("Файл DLL по указанному пути не найден.", output);
        }
        Console.SetOut(origin);
    }
}
