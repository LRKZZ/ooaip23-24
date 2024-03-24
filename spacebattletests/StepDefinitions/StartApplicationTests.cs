using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;
namespace spacebattletests
{
    public class AppStartTest
    {
        [Fact]
        public void Execute_CreatesAndStartsApp()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", 
                IoC.Resolve<object>("Scopes.New", 
                    IoC.Resolve<object>("Scopes.Root"))).Execute();

            const int gameLength = 4;

            var serverStartStrategy = new ServerStartStrategy();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartAppStrategy", 
                (object[] args) => serverStartStrategy.Invoke(args)).Execute();

            var mockStrategyWithParams = new Mock<IStrategy>();
            mockStrategyWithParams.Setup(x => x.Invoke(It.IsAny<object[]>()))
                .Returns(new Mock<spacebattle.ICommand>().Object).Verifiable();

            IoC.Resolve<spacebattle.ICommand>("StartAppStrategy", gameLength).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StopServerStrategy", 
                (object[] args) => new StopServer()).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ConsoleOutputStrategy", 
                (object[] args) => mockStrategyWithParams.Object.Invoke(args)).Execute();
        }
    }
}