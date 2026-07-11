using System.Reflection;
using System.Runtime.Loader;
using CommandLib;
namespace AssemblyInspector;
public class Program
{
    public static void Main(string [] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Путь до библиотеки указан не был.");
            return;
        }
        string path = args[0];
        if (!File.Exists(path))
        {
            Console.WriteLine("Файл DLL по указанному пути не найден.");
            return;
        }
        
        try
        {
            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);

            foreach (Type type in assembly.GetTypes())
            {

                if (!type.IsClass)
                continue;

                Console.WriteLine($"Класс: {type.FullName}");

                object [] attributes = type.GetCustomAttributes(false);

                if (attributes.Length > 0)
                {
                    Console.WriteLine("Атрибуты:");
                    foreach (var attribute in attributes)
                    {
                        Console.WriteLine(attribute.GetType().Name);
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("У класса не объявлены кастомные атрибуты.");
                }

                ConstructorInfo[] constructors = type.GetConstructors();

                if (constructors.Length > 0)
                {
                    foreach (var constructor in constructors)
                    {
                        Console.WriteLine($"Имя конструктора: {constructor.Name}");
                        ParameterInfo[] parameters = constructor.GetParameters();
                        Console.WriteLine("Параметры конструктора:");
                        foreach (var parameter in parameters)
                        {
                            Console.WriteLine($"Тип: {parameter.ParameterType.Name} - Имя: {parameter.Name}");
                        }
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("У класса нет конструктора.");
                }

                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                if (methods.Length > 0)
                {
                    foreach (var method in methods)
                    {
                        Console.WriteLine($"Имя метода: {method.Name}");
                        ParameterInfo[] parameters = method.GetParameters();
                        Console.WriteLine("Параметры метода:");
                        foreach (var parameter in parameters)
                        {
                            Console.WriteLine($"Тип: {parameter.ParameterType.Name} - Имя: {parameter.Name}");
                        }
                        var methodAttributes = method.GetCustomAttributes(false);
                        if (methodAttributes.Length > 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Атрибуты метода:");
                            foreach (var attribute in methodAttributes)
                            {
                                Console.WriteLine(attribute.GetType().Name);
                            }
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("У метода не объявлены кастомные атрибуты.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("В классе нет методов.");
                }
                Console.WriteLine();
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
