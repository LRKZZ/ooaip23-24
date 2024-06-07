namespace spacebattletests;
using spacebattle;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestAssignFuelToUObjectsCommand
{
    public TestAssignFuelToUObjectsCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulFuelAssignment()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.AssignFuel", (object[] args) => new ActionCommand(
            () => ((IUObject)args[0]).SetProperty((string)args[1], args[2])
        )).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Collection.AssignFuel",
            (object[] args) => new AssignFuelToUObjectsCommand((IEnumerable<IUObject>)args[0], (double)args[1])
        ).Execute();

        var mockUObjects = Enumerable.Range(0, 3).Select(x =>
        {
            var mock = new Mock<IUObject>();
            mock.Setup(x => x.SetProperty("Fuel", It.IsAny<object>())).Verifiable();
            return mock;
        }).ToList();

        mockUObjects.ForEach(mock => mock.Verify(x => x.SetProperty("Fuel", It.IsAny<object>()), Times.Never()));

        IoC.Resolve<spacebattle.ICommand>("Game.UObject.Collection.AssignFuel", mockUObjects.Select(x => x.Object), 100.0).Execute();

        mockUObjects.ForEach(mock => mock.Verify(x => x.SetProperty("Fuel", 100.0), Times.Once()));
    }
}