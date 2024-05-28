namespace spacebattle;

using Hwdtech;
using Hwdtech.Ioc;

public class AdapterGeneratorTest
{
    public AdapterGeneratorTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
            IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();
    }

    [Fact]
    public void SuccessGenerateAdapter()
    {
        var oldtype = typeof(IUObject);
        var newtype = typeof(IMoveStartable);

        var expectedCode = @"using Hwdtech;
namespace spacebattle;

public class IMoveStartableAdapter : IMoveStartable
{
	private readonly IUObject _obj;
	publicIMoveStartableAdapter(IUObjectobj) => _obj = obj;

	public IUObject Order 
	{
		get
		{
			{
				return IoC.Resolve<IUObject>(""IUObject.Get"", _obj, ""Order"");
			}
		}
	}
        public Dictionary<string, object> PropertiesOfOrder 
	{
		get
		{
			{
				return IoC.Resolve<Dictionary<string, object>>(""IUObject.Get"", _obj, ""PropertiesOfOrder"");
			}
		}
	}
}".Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
        var generatedCode = new AdapterBuilder(oldtype, newtype).Build().Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

        Assert.Equal(expectedCode, generatedCode);
    }
}
