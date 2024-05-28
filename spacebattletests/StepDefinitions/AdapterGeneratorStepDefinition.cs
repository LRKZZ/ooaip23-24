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
    public void SuccessGenerateAdapterForIMoveStartable()
    {
        var oldtype = typeof(IUObject);
        var newtype = typeof(IMoveStartable);

        var expectedCode = @"using Hwdtech;
namespace spacebattle;

public class IMoveStartableAdapter : IMoveStartable
{
	private readonly IUObject _obj;
	public IMoveStartableAdapter (IUObjectobj) => _obj = obj;

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

    [Fact]
    public void SuccessGenerateAdapterForIRotatable()
    {
        var oldtype = typeof(IUObject);
        var newtype = typeof(IRotatable);

        var expectedCode = @"using Hwdtech;
namespace spacebattle;

public class IRotatableAdapter : IRotatable
{
	private readonly IUObject _obj;
	public IRotatable Adapter(IUObject obj) => _obj = obj;

	public Angle angle 
	{
		get
		{
			{
				return IoC.Resolve<Angle>(""IUObject.Get"", _obj, ""angle"");
			}
		}
		set
		{
			{
				IoC.Resolve<ICommand>(""IUObject.Set"", _obj, ""angle"", value).Execute();
			}
		}
	}
        public Angle angleSpeed
	{
		get
		{
			{
				return IoC.Resolve<Angle>(""IUObject.Get"", _obj, ""angleSpeed"");
			}
		}
	}
}".Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
        var generatedCode = new AdapterBuilder(oldtype, newtype).Build().Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

        Assert.Equal(expectedCode, generatedCode);
    }
}
