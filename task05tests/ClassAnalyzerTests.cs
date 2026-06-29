using task05;
using Xunit;

namespace task05tests;
public class TestClass
{
    public int PublicField;
    private string _privateField = default!;
    public int Property { get; set; }

    public void Method() { }
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetMethodParameters_ReturnExpectedParameters()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("Method");
        Assert.Contains("Void", parameters);
    }

    [Fact]
    public void GetProperties_ReturnExpectedProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();
        Assert.Contains("Property", properties);
    }

    [Fact]
    public void HasAttribute_ReturnInformationOfExistienceFalse()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var attribute = analyzer.HasAttribute<SerializableAttribute>();
        Assert.False(attribute);
    }

    [Fact]
    public void HasAttribute_ReturnInformationOfExistienceTrue()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        var attribute = analyzer.HasAttribute<SerializableAttribute>();
        Assert.True(attribute);
    }
}
