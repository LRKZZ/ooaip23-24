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
        var assemblyId = Path.GetRandomFileName();
        var codeStructure = CSharpSyntaxTree.ParseText(sourceCode);

        var assemblyReferences = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IoC).Assembly.Location)
        };

        var codeCompilation = CSharpCompilation.Create(
            assemblyId,
            new[] { codeStructure },
            assemblyReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var stream = new MemoryStream();
        var compilationResult = codeCompilation.Emit(stream);

        if (!compilationResult.Success)
        {
            throw new InvalidOperationException("Compilation failed");
        }

        stream.Seek(0, SeekOrigin.Begin);
        return Assembly.Load(stream.ToArray());
    }
}
