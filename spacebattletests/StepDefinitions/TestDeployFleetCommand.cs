namespace spacebattletests;
using spacebattle;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestDeployFleetCommand
{
    public TestDeployFleetCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulDeploymentOfFleet()
    {
        var mockGameUObjects = Enumerable.Repeat(new Mock<IUObject>().Object, 3).ToList();

        var mockPositionIterator = new Mock<IEnumerator<object>>();
        mockPositionIterator.Setup(x => x.Reset()).Verifiable();

        var mockCommand = new Mock<spacebattle.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Iterator.FleetPosition", (object[] args) => mockPositionIterator.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Deploy.Ship", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Deploy.Fleet", (object[] args) => new DeployFleetCommand((IEnumerable<IUObject>)args[0])).Execute();

        mockPositionIterator.Verify(x => x.Reset(), Times.Never());
        mockCommand.Verify(x => x.Execute(), Times.Never());

        IoC.Resolve<spacebattle.ICommand>("Game.Deploy.Fleet", mockGameUObjects).Execute();

        mockPositionIterator.Verify(x => x.Reset(), Times.Once());
        mockCommand.Verify(x => x.Execute(), Times.Exactly(3));
    }
}