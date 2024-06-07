namespace spacebattletests;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;

public class TestDeployShipCommand
{
    public TestDeployShipCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulDeploymentOfShip()
    {
        var mockUObject = new Mock<IUObject>();

        var mockPositionIterator = new Mock<IEnumerator<object>>();
        mockPositionIterator.SetupGet(x => x.Current).Verifiable();
        mockPositionIterator.Setup(x => x.MoveNext()).Verifiable();

        var mockCommand = new Mock<spacebattle.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.SetPosition", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Deploy.Ship",
            (object[] args) => new DeployShipCommand((IUObject)args[0], (IEnumerator<object>)args[1])
        ).Execute();

        mockCommand.Verify(x => x.Execute(), Times.Never());

        IoC.Resolve<spacebattle.ICommand>("Game.Deploy.Ship", mockUObject.Object, mockPositionIterator.Object).Execute();

        mockCommand.Verify(x => x.Execute(), Times.Once());
        mockPositionIterator.VerifyAll();
    }
}
