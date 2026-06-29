using System;
using System.Reflection;
using System.Collections.Generic;
namespace task05;
public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        var methods = _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        return methods.Select(m => m.Name);
    }

    public IEnumerable<string> GetMethodParams(string methodname)
    {
        var method = _type.GetMethod(methodname);

        if (method == null)
        return Enumerable.Empty<string>();

        var parameters = method.GetParameters().Select(p => p.Name);
        var returnValue = method.ReturnType.Name;
        var result = new List<string> { returnValue };
        result.AddRange(parameters!);
        return result;
    }

    public IEnumerable<string> GetAllFields()
    {
        var fields = _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        return fields.Select(f => f.Name);
    }

    public IEnumerable<string> GetProperties()
    {
        var properties = _type.GetProperties();
        return properties.Select(p => p.Name);
    }
    
    public bool HasAttribute<T>() where T : Attribute
    {
        bool attribute = _type.IsDefined(typeof(T), inherit: true);
        return attribute;
    }
}
