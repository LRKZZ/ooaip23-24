using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;

namespace spacebattletests
{
    public class GameStrategyScopeTests
    {
        [Fact]
        public void RunStrategy_AddsScopeToGameScopeMapping()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            var mockCommand = new Mock<spacebattle.ICommand>();
            mockCommand.Setup(x => x.Execute());

            var mockStrategyReturnsCommand = new Mock<IStrategy>();
            mockStrategyReturnsCommand.Setup(x => x.Execute(It.IsAny<object[]>())).Returns(mockCommand.Object).Verifiable();

            var idGame = 321;
            var parentScope = IoC.Resolve<object>("Scopes.Root");
            var quantum = 4.0;

            var scope = IoC.Resolve<object>("Scopes.New", parentScope);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameScopeMapping", (object[] args) => new Dictionary<int, object>()).Execute();

            var strategy = new GameStrategyScope();

            var result = strategy.Execute(idGame, parentScope, quantum);
            Assert.NotEqual(parentScope, scope);
        }
    }
}
