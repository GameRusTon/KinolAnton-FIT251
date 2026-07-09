using System.Reflection;
using System.Runtime.Loader;
using CommandLib;
namespace CommandRunner;
class Program
{
    static void Main()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileSystemCommands.dll");
        if (!File.Exists(path))
        {
            Console.WriteLine("Файл DLL не найден.");
            return;
        }
        
        try
        {
            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(ICommand).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    Console.WriteLine($"Была обнаружена команда: {type.Name}");
                    ICommand? command = null;
                    
                    if (type.Name == "DirectorySizeCommand")
                    {
                        string _testPath = Path.Combine(Path.GetTempPath(), "TaskFolder" + Guid.NewGuid());
                        command = Activator.CreateInstance(type, _testPath) as ICommand;
                    }
                    else if (type.Name == "FindFilesCommand")
                    {
                        string _testPath = Path.Combine(Path.GetTempPath(), "TaskFolder" + Guid.NewGuid());
                        command = Activator.CreateInstance(type, _testPath, "*.txt") as ICommand;
                    }

                    if (command != null)
                    {
                        command.Execute();
                    }
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл библиотеки не был найден.");
        }
        catch (BadImageFormatException)
        {
            Console.WriteLine("Файл поврежден.");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Непредвиденная ошибка: {exception.Message}");
        }
    }
}
