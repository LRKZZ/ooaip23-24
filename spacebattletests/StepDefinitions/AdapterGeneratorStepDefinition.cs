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
    public void SuccessGenerateAdapterForIRotatable()
    {
        var oldtype = typeof(IUObject);
        var newtype = typeof(IRotatable);

        var expectedCode = @"using Hwdtech;
namespace spacebattle;

public class IRotatableAdapter : IRotatable
{
	private readonly IUObject _obj;
	public IRotatableAdapter(IUObject obj) => _obj = obj;

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

	 [Fact]
    public void SuccessGenerateAdapterForDictionary()
    {
        var oldtype = typeof(IUObject);
        var newtype = typeof(IWagonShip);

        var expectedCode = @"using Hwdtech;
namespace spacebattle;

public class IWagonShipAdapter : IWagonShip
{
	private readonly IUObject _obj;
	public IWagonShipAdapter(IUObject obj) => _obj = obj;

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
        public Dictionary<string, object> angleSpeed
	{
		get
		{
			{
				return IoC.Resolve<Dictionary<string, object>>(""IUObject.Get"", _obj, ""angleSpeed"");
			}
		}
	}
}".Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
        var generatedCode = new AdapterBuilder(oldtype, newtype).Build().Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

        Assert.Equal(expectedCode, generatedCode);
    }
}
