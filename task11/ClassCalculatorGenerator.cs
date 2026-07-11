using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace task11;

public static class ClassCalculatorGenerator
{
    public static CalculatorInterface CreateCalculator()
    {
    string code = @"
    using task11;
    
    public class Calculator: CalculatorInterface
    {
        public int Add(int a, int b) => a + b;
        public int Minus(int a, int b) => a - b;
        public int Mul(int a, int b) => a * b;
        public int Div(int a, int b) => a / b;
    }";

    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

    string DLL = Path.GetRandomFileName();

    var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
    var referencesList = new List<MetadataReference>();

    foreach (var assembly in loadedAssemblies)
    {
        if (!string.IsNullOrEmpty(assembly.Location))
        {
            referencesList.Add(MetadataReference.CreateFromFile(assembly.Location));
        }
    }

    referencesList.Add(MetadataReference.CreateFromFile(typeof(CalculatorInterface).Assembly.Location));

    MetadataReference[] references = referencesList.ToArray();

    CSharpCompilation compilation = CSharpCompilation.Create(
        DLL,
        syntaxTrees: new [] {syntaxTree},
        references: references,
        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    );
    using (MemoryStream memoryStream = new MemoryStream())
        {
            EmitResult result = compilation.Emit(memoryStream);

            if (!result.Success)
            {
                string exceptions = string.Join(Environment.NewLine, result.Diagnostics);
                throw new InvalidOperationException($"Ошибка компиляции динамического класса: {exceptions}");
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            Assembly assembly = Assembly.Load(memoryStream.ToArray()) ?? throw new InvalidOperationException("Не удалось загрузить сборку.");

            Type type = assembly.GetType("Calculator") ?? throw new TypeLoadException("Класс Calculator не найден в скомпилированной сборке.");

            object activated = Activator.CreateInstance(type) ?? throw new InvalidOperationException("Не удалось создать экземпляр класса: Calculator.");

            return (CalculatorInterface)activated;
        }
    }
}
