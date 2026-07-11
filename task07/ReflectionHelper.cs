using System.Reflection;
namespace task07;

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        if (type == null)
        return;
        if (type.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute ClassName)
        {
            Console.WriteLine($"Отображаемое имя класса: {ClassName.DisplayName}");
        }
        if (type.GetCustomAttribute<VersionAttribute>() is VersionAttribute ClassVersion)
        {
            Console.WriteLine($"Версия класса: {ClassVersion.Major}.{ClassVersion.Minor}");
        }
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var method in methods)
        {
            if (method.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute MethodName)
            {
                Console.WriteLine($"Метод {method.Name} имеет отображаемое имя: {MethodName.DisplayName}");
            }
        }
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute PropertyName)
            {
                Console.WriteLine($"Свойство {property.Name} имеет отображаемое имя: {PropertyName.DisplayName}");
            }
        }
    }
}
