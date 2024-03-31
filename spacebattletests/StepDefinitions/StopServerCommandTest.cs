namespace spacebattletests;

using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;

public class StopServerCommandTest
{
    private readonly Mock<spacebattle.ICommand> _mockSendCommand;
    private readonly Mock<spacebattle.ICommand> _mockSoftStopThreadCommand;
    private readonly ConcurrentDictionary<int, object> _threadMap;

    public StopServerCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        _mockSendCommand = new Mock<spacebattle.ICommand>();
        _mockSoftStopThreadCommand = new Mock<spacebattle.ICommand>();
        _threadMap = new ConcurrentDictionary<int, object>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerThreadMap", (object[] args) => _threadMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerCommandSend", (object[] args) => _mockSendCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StopServerCommand", (object[] args) => _mockSoftStopThreadCommand.Object).Execute();
    }

    [Fact]
    public void StopServerCommand_ShouldSendStopCommandsToAllThreads()
    {
        _threadMap[1] = new object();
        _threadMap[2] = new object();

        _mockSendCommand
            .Setup(c => c.Execute())
            .Callback(() => _mockSoftStopThreadCommand.Object.Execute());

        var stopServerCommand = new StopServer(_threadMap);
        stopServerCommand.Execute();

        _mockSendCommand.Verify(c => c.Execute(), Times.Exactly(2));

        _mockSoftStopThreadCommand.Verify(c => c.Execute(), Times.Exactly(2));
    }
}
