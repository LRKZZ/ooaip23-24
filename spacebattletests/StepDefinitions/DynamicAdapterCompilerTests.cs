namespace spacebattletests;
using System;
using SpaceBattle;
using Xunit;

public class DynamicAdapterCompilerTests
{
    [Fact]
    public void EnsureDynamicCompilationSucceeds()
    {
        _ = new DynamicAdapterCompiler();
        var code = @"
            public interface ITestInterface { void TestMethod(); }
            public class TestClass : ITestInterface {
                public void TestMethod() {}
            }";

        var assemblyOutput = DynamicAdapterCompiler.CompileCode(code);
        var retrievedType = assemblyOutput.GetType("TestClass");
        Assert.NotNull(retrievedType);
    }

    [Fact]
    public void EnsureDynamicCompilationFails()
    {
        var code = @"
            public interface ITestInterface { void TestMethod(); }
            public class TestClass : ITestInterface {
            }";

        Exception capturedException = Assert.Throws<InvalidOperationException>(() => DynamicAdapterCompiler.CompileCode(code));
        Assert.Equal("Compilation failed", capturedException.Message);
    }
}