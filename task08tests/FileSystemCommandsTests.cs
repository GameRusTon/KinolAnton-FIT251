using CommandLib;
using FileSystemCommands;
using Xunit;
namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        var origin = Console.Out;

        using (StringWriter line = new StringWriter())
        {
            Console.SetOut(line);
            command.Execute();
            string output = line.ToString();
            Assert.Contains("10",output);
        }
        Console.SetOut(origin);
        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        var origin = Console.Out;

        using (StringWriter line = new StringWriter())
        {
            Console.SetOut(line);
            command.Execute();
            string output = line.ToString();
            Assert.Contains("file1.txt", output);
            Assert.DoesNotContain("file2.log", output);
        }
        Console.SetOut(origin);
        Directory.Delete(testDir, true);
    }

    [Fact]
    public void DirectorySizeCommand_NonExistentDirectory()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "testDir" + Guid.NewGuid());
        var command = new DirectorySizeCommand(testDir);
        var origin = Console.Out;

        using (StringWriter line = new StringWriter())
        {
            Console.SetOut(line);
            command.Execute();
            string output = line.ToString();
            Assert.Contains("найден не был", output);
        }
        Console.SetOut(origin);
    }

    [Fact]
    public void FindFilesCommand_NonExistentDirectory()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "testDir" + Guid.NewGuid());
        var command = new FindFilesCommand(testDir, "*.txt");
        var origin = Console.Out;

        using (StringWriter line = new StringWriter())
        {
            Console.SetOut(line);
            command.Execute();
            string output = line.ToString();
            Assert.Contains("найден не был", output);
        }
        Console.SetOut(origin);
    }

    [Fact]
    public void FindFilesCommand_ShouldNotFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.log"), "Log");
        File.WriteAllText(Path.Combine(testDir, "file2.png"), "Image");

        var command = new FindFilesCommand(testDir, "*.txt");
        var origin = Console.Out;

        using (StringWriter line = new StringWriter())
        {
            Console.SetOut(line);
            command.Execute();
            string output = line.ToString();
            Assert.Contains("Файлов по заданной маске найдено не было.", output);
        }
        Console.SetOut(origin);
        Directory.Delete(testDir, true);
    }
}
