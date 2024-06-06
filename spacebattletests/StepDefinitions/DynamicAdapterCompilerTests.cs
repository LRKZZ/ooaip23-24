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

        var compiledAssembly = DynamicAdapterCompiler.CompileCode(code);
        var testType = compiledAssembly.GetType("TestClass");
        Assert.NotNull(testType);
        var instance = Activator.CreateInstance(testType);
        Assert.IsAssignableFrom(testType, instance);
    }
}
