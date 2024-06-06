namespace spacebattletests;
using Hwdtech;
using Hwdtech.Ioc;
using spacebattle;
using Xunit;

public class AdapterGeneratorTests
{
    public AdapterGeneratorTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Reflection.GenerateAdapterCode", (object[] args) => new AdapterCodeGeneratorStrategy().Execute(args)).Execute();
    }

    [Fact]
    public void AdapterCodeGeneratorTest_1()
    {

        var adapterCodeForStartable =
       @"class MoveStartableAdapter : IMoveStartable {
        Object target;
        public MoveStartableAdapter(Object target) => this.target = target; 
        public IUObject Order {
               get { return IoC.Resolve<IUObject>(""Game.Order.Get"", target); }
        }
        public Dictionary<String, Object> PropertiesOfOrder {
               get { return IoC.Resolve<Dictionary<String, Object>>(""Game.PropertiesOfOrder.Get"", target); }
        }
    }";

        var adapterCodeForMovable =
        @"class MovableAdapter : IMovable {
        Vector target;
        public MovableAdapter(Vector target) => this.target = target; 
        public Vector Position {
               get { return IoC.Resolve<Vector>(""Game.Position.Get"", target); }
               set { IoC.Resolve<_ICommand.ICommand>(""Game.Position.Set"", target, value).Execute(); }
        }
        public Vector Velocity {
               get { return IoC.Resolve<Vector>(""Game.Velocity.Get"", target); }
        }
    }";

        Assert.Equal(adapterCodeForStartable, IoC.Resolve<string>("Game.Reflection.GenerateAdapterCode", typeof(IMoveStartable), typeof(object)));
        Assert.Equal(adapterCodeForMovable, IoC.Resolve<string>("Game.Reflection.GenerateAdapterCode", typeof(IMovable), typeof(Vector)));
    }
}