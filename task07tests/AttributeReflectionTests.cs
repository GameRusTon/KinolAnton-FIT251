using Xunit;
using System.Reflection;
using System.IO;
using task07;
namespace task07tests;
public class AttributeReflectionTests
{
    [Fact]
    public void Class_HasDisplayNameAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Пример класса", attribute.DisplayName);
    }

    [Fact]
    public void Method_HasDisplayNameAttribute()
    {
        var method = typeof(SampleClass).GetMethod("TestMethod");
        Assert.NotNull(method);
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Тестовый метод", attribute.DisplayName);
    }

    [Fact]
    public void Property_HasDisplayNameAttribute()
    {
        var prop = typeof(SampleClass).GetProperty("Number");
        Assert.NotNull(prop);
        var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Числовое свойство", attribute.DisplayName);
    }

    [Fact]
    public void Class_HasVersionAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void ReflectionHelper_PrintCorrectInfo()
    {
        var type = typeof(SampleClass);
        var origin = Console.Out;
        using (StringWriter line = new StringWriter())
        {
            Console.SetOut(line);
            ReflectionHelper.PrintTypeInfo(type);
            string output = line.ToString();

            Assert.Contains("Отображаемое имя класса: Пример класса", output);
            Assert.Contains("Версия класса: 1.0", output);
            Assert.Contains("Метод TestMethod имеет отображаемое имя: Тестовый метод", output);
            Assert.Contains("Свойство Number имеет отображаемое имя: Числовое свойство", output);
        }
        Console.SetOut(origin);
    }
}