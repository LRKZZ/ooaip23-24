using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;

public class EndMoveCommandTests
{
    private readonly Mock<IMoveCommandEndable> _mockEndable = new Mock<IMoveCommandEndable>();
    private readonly Mock<spacebattle.ICommand> _mockCommand = new Mock<spacebattle.ICommand>();
    private readonly Mock<IUObject> _target = new Mock<IUObject>();
    private readonly List<string> _keys;
    private readonly Dictionary<string, object> _characteristics;

    public EndMoveCommandTests()
    {
        InitializeIoC();
        _keys = new List<string>() { "Movement" };
        _characteristics = new Dictionary<string, object>();

        _target.Setup(t => t.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>((key, value) => _characteristics.Add(key, value));
        _target.Setup(t => t.DeleteProperty(It.IsAny<string>())).Callback<string>((string key) => _characteristics.Remove(key));
        _target.Setup(t => t.GetProperty(It.IsAny<string>())).Returns((string key) => _characteristics[key]);

        _mockEndable.SetupGet(e => e.Target).Returns(_target.Object);
        _mockEndable.SetupGet(e => e.args).Returns(_keys);
    }

    private static void InitializeIoC()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) =>
        {
            var target = (IInjectable)args[0];
            var injectedCommand = (spacebattle.ICommand)args[1];
            target.Inject(injectedCommand);
            return target;
        }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => new EmptyCommand()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EndMovement", (object[] args) => new EndMoveCommand((IMoveCommandEndable)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.DeleteProperty", (object[] args) =>
        {
            var _action = new Action(() =>
            {
                var target = (IUObject)args[0];
                var properties = (List<string>)args[1];
                properties.ForEach(prop => target.DeleteProperty(prop));
            });
            return _action;
        }).Execute();
    }

    [Fact]
    public void EndMovementCommand_RemovesPropertyAndThrowsKeyNotFoundException()
    {
        _target.Object.SetProperty("Movement", new ReplaceCommand(_mockCommand.Object));

        IoC.Resolve<spacebattle.EndMoveCommand>("Game.Command.EndMovement", _mockEndable.Object).Execute();

        var exception = Assert.Throws<KeyNotFoundException>(() => _target.Object.GetProperty("Movement"));
        Assert.NotNull(exception);
    }

    [Fact]
    public void InjectableCommand_InjectedIntoReplaceableCommand_ReplaceableCommandNotExecuted()
    {
        var injectCommand = new ReplaceCommand(_mockCommand.Object);

        injectCommand.Inject(IoC.Resolve<spacebattle.ICommand>("Game.Command.EmptyCommand"));
        injectCommand.Execute();

        _mockCommand.Verify(m => m.Execute(), Times.Never());
    }
}
