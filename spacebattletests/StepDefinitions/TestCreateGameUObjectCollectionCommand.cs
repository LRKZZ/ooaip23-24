namespace spacebattletests;
using spacebattle;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCreateGameUObjectCollectionCommand
{
    public TestCreateGameUObjectCollectionCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCreatingGameUObjects()
    {
        var gameUObjectMap = new Dictionary<int, IUObject>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Registry", (object[] args) => gameUObjectMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Create", (object[] args) => new Mock<IUObject>().Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Collection.Create",
            (object[] args) => new GenerateGameUObjectsCommand((int)args[0])
        ).Execute();

        Assert.Empty(gameUObjectMap);
        IoC.Resolve<spacebattle.ICommand>("Game.UObject.Collection.Create", 10).Execute();
        Assert.Equal(10, gameUObjectMap.Count);
    }
}