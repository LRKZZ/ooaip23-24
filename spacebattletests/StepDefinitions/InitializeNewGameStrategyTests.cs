using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;
namespace spacebattletests;

public class InitializeNewGameStrategyTests
{
    [Fact]
    public void SuccessfulGameInitializationStrategyTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var mockCommand = new Mock<spacebattle.ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockStrategyReturningCommand = new Mock<Strategy>();
        mockStrategyReturningCommand.Setup(x => x.Execute(It.IsAny<object[]>())).Returns(mockCommand.Object);

        var mockStrategyReturningDictionary = new Mock<Strategy>();
        mockStrategyReturningDictionary.Setup(x => x.Execute()).Returns(new Dictionary<int, spacebattle.ICommand> { { 1, mockCommand.Object } });

        var mockStrategyReturningScopeDictionary = new Mock<Strategy>();
        mockStrategyReturningScopeDictionary.Setup(x => x.Execute()).Returns(new Dictionary<int, object> { { 0, IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))) } });

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RetrieveGameScope", (object[] args) => mockStrategyReturningScopeDictionary.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameMappings", (object[] args) => mockStrategyReturningDictionary.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MacroCommand", (object[] args) => mockStrategyReturningCommand.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "InjectCommand", (object[] args) => mockStrategyReturningCommand.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RepeatCommand", (object[] args) => mockStrategyReturningCommand.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand", (object[] args) => mockStrategyReturningCommand.Object.Execute(args)).Execute();

        var gameStrategy = new CreateGameStrategy();
        var result = gameStrategy.Execute(0);

        var gameMap = IoC.Resolve<IDictionary<int, spacebattle.ICommand>>("GameMappings");
        Assert.Equal(gameMap[0], mockCommand.Object);
    }
}
