using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;
namespace spacebattletests;

public class RemoveGameCommandTests
{
    [Fact]
    public void Execute_RemovesGameScopeFromGameScopeMap()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        var gameId = 123;

        var mockCommand = new Mock<spacebattle.ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockInjectable = new Mock<IInjectable>();
        mockInjectable.Setup(x => x.Inject(mockCommand.Object));

        var gameMapStrategyMock = new Mock<Strategy>();
        gameMapStrategyMock.Setup(x => x.Execute()).Returns(new Dictionary<int, IInjectable> { { gameId, mockInjectable.Object } });

        var emptyCommandStrategyMock = new Mock<Strategy>();
        emptyCommandStrategyMock.Setup(x => x.Execute(It.IsAny<spacebattle.ICommand>())).Returns(false).Verifiable();

        var mockScopeDictionary = new Mock<Strategy>();
        mockScopeDictionary.Setup(x => x.Execute()).Returns(new Dictionary<int, object> { { gameId, IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))) } });

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameScopeMapping", (object[] args) => mockScopeDictionary.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameMappings", (object[] args) => gameMapStrategyMock.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "EmptyCommand", (object[] args) => emptyCommandStrategyMock.Object.Execute(args)).Execute();

        var gameMap = IoC.Resolve<IDictionary<int, IInjectable>>("GameMappings");
        var emptyCommand = IoC.Resolve<spacebattle.ICommand>("EmptyCommand");
        var gameScopeMap = IoC.Resolve<IDictionary<int, object>>("GameScopeMapping");
        var removeGameCommand = new RemoveGameCommand(gameId);
        removeGameCommand.Execute();

        Assert.Empty(gameScopeMap);
    }
}
