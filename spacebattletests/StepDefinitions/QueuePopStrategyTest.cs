using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;

namespace spacebattletests
{
    public class QueuePopStrategyTests
    {
        [Fact]
        public void RunStrategy_ShouldReturnDequeuedCommand_Success()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            var queue = new Queue<spacebattle.ICommand>();
            var mockStrategy = new Mock<Strategy>();
            mockStrategy.Setup(c => c.Execute(It.IsAny<object[]>())).Returns(queue).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RetrieveQueueByGameId", (object[] args) => mockStrategy.Object.Execute(args)).Execute();

            var commandMock = new Mock<spacebattle.ICommand>();

            var queuePushCommand = new QueuePushCommand(1, commandMock.Object);
            queuePushCommand.Execute();

            var gameId = 1;
            var strategy = new QueuePopStrategy();
            var cmd = strategy.Execute(gameId);

            Assert.Empty(IoC.Resolve<Queue<spacebattle.ICommand>>("RetrieveQueueByGameId", 1));
        }
    }
}
