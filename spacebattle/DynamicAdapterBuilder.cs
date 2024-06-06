namespace SpaceBattle;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Hwdtech;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

public class DynamicAdapterCompiler
{
    public static Assembly CompileCode(string sourceCode)
    {
        var assemblyName = Path.GetRandomFileName();
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IoC).Assembly.Location)
        };

        var compilation = CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        ms.Seek(0, SeekOrigin.Begin);
        return Assembly.Load(ms.ToArray());
    }
}
