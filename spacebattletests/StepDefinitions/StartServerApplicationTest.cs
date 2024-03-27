namespace spacebattletests;

using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;

public class StartServerApplication
{
    private readonly Mock<spacebattle.ICommand> _mockCommand;
    private readonly Mock<IExceptionHandler> _mockExceptionHandler;

    public StartServerApplication() 
    {

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", 
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        _mockExceptionHandler = new Mock<IExceptionHandler>();
        _mockCommand = new Mock<spacebattle.ICommand>(); 
        _mockCommand.Setup(c => c.Execute()).Verifiable(); 

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartCommand", 
            (object[] args) => _mockCommand.Object
        ).Execute();
    }

    [Fact]
    public void IsServerStarted()
    {

        var numberOfThreads = 4;

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerStart",
            (object[] args) => new StartServer((int)args[0], _mockExceptionHandler.Object))       
            .Execute();

        IoC.Resolve<spacebattle.ICommand>("ServerStart", numberOfThreads).Execute();

        _mockCommand.Verify(c => c.Execute(), Times.Exactly(numberOfThreads));
    }
}