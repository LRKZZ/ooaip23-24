namespace spacebattle;

using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class FireCommandTest
{
    public FireCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
            IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.FireCommand", (object[] args) =>
        {
            var adapter = IoC.Resolve<IShootable>("FireAdapter", args[0]);
            return new FireCommand(adapter);
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.IUObject.SetProperty",
                (object[] args) =>
                {
                    return new ActionCommand(() =>
                    {
                        var order = (IUObject)args[0];
                        var key = (string)args[1];
                        var value = args[2];

                        order.SetProperty(key, value);
                    });
                }
            ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Commands.Inject",
            (object[] args) =>
            {
                return new ReplaceCommand((ICommand)args[0]);
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => new EmptyCommand()).Execute();
    }

    [Fact]
    public void SuccessGameTimeoutExecute()
    {
        var spaceship = new Mock<IUObject>();
        var torpedo = new Mock<IUObject>();
        var firable = new Mock<IShootable>();
        var startable = new Mock<IMoveStartable>();
        var order = new Mock<IUObject>();
        var orderDict = new Dictionary<string, object>();
        var properties = new Dictionary<string, object>();
        var queue = new Mock<IQueue>();
        var realQueue = new Queue<ICommand>();

        startable = new Mock<IMoveStartable>();
        orderDict = new Dictionary<string, object>();

        var longMoveCommand = new Mock<ICommand>();
        longMoveCommand.Setup(x => x.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Commands.LongMove",
            (object[] args) =>
            {
                return longMoveCommand.Object;
            }
        ).Execute();

        torpedo.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);
        queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);

        IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Queue",
                (object[] args) =>
                {
                    return queue.Object;
                }
            ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "FireAdapter", (object[] args) =>
        {
            return firable.Object;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create.Torpedo", (object[] args) =>
        {
            var shoot = (IShootable)args[0];
            torpedo.Setup(s => s.GetProperty("Position")).Returns(shoot.Position);
            torpedo.Setup(s => s.GetProperty("Speed")).Returns(shoot.Speed);
            torpedo.Setup(s => s.GetProperty("ScalarSpeed")).Returns(shoot.ScalarSpeed);
            return torpedo.Object;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "IMoveStartableAdapter", (object[] args) =>
        {
            var obj = (IUObject)args[0];
            properties = new Dictionary<string, object>
            {
                { "Position",  obj.GetProperty("Position")},
                { "Speed",  obj.GetProperty("Speed")},
                { "ScalarSpeed",  obj.GetProperty("ScalarSpeed")},
            };
            startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
            startable.SetupGet(s => s.Order).Returns((IUObject)args[0]);
            return startable.Object;
        }).Execute();

        var firecmd = IoC.Resolve<ICommand>("Game.Commands.FireCommand", spaceship);

        firecmd.Execute();

        Assert.Contains("Position", orderDict.Keys);
        Assert.Contains("Speed", orderDict.Keys);
        Assert.Contains("ScalarSpeed", orderDict.Keys);
        Assert.NotEmpty(realQueue);

        realQueue.Dequeue().Execute();

        longMoveCommand.Verify(cmd => cmd.Execute(), Times.Once);
    }
}
