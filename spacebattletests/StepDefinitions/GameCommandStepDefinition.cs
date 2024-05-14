namespace spacebattle;
using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class GameCommandTest
{
    public GameCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
            IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();
    }

    [Fact]
    public void SuccessGameTimeoutExecute()
    {
        var newScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
        var queue = new Queue<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetTimeQuant", (object[] args) =>
        {
            var time = new TimeSpan(0, 0, 0);
            return (object)time;
        }).Execute();
        queue.Enqueue(new ActionCommand(() => { }));
        queue.Enqueue(new ActionCommand(() => { }));
        var gamecmd = new GameCommand(444, newScope, queue);
        gamecmd.Execute();
        Assert.True(queue.Count == 2);

    }
    [Fact]
    public void SuccessGameAllCommandsExecute()
    {
        var newScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
        var queue = new Queue<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetTimeQuant", (object[] args) =>
        {
            var time = new TimeSpan(0, 0, 10);
            return (object)time;
        }).Execute();
        queue.Enqueue(new ActionCommand(() => { }));
        var gamecmd = new GameCommand(444, newScope, queue);
        gamecmd.Execute();
        Assert.Empty(queue);
    }
    [Fact]
    public void ExceptionHandlerFindHandler()
    {
        var newScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
        var mockGoodHandler = new Mock<IHandler>();
        mockGoodHandler.Setup(x => x.Handle()).Verifiable();
        var queue = new Queue<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetTimeQuant", (object[] args) =>
        {
            var time = new TimeSpan(0, 0, 10);
            return (object)time;
        }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) => mockGoodHandler.Object).Execute();
        queue.Enqueue(new ActionCommand(() => { throw new Exception(); }));
        var gamecmd = new GameCommand(444, newScope, queue);
        gamecmd.Execute();
        mockGoodHandler.Verify(x => x.Handle(), Times.Once);
    }
    [Fact]
    public void ExceptionHandlerNotFindHandler()
    {
        var newScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
        var queue = new Queue<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetTimeQuant", (object[] args) =>
        {
            var time = new TimeSpan(0, 0, 10);
            return (object)time;
        }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) => new DefaultHandler((Exception)args[1])).Execute();
        queue.Enqueue(new ActionCommand(() => { throw new Exception(); }));
        var gamecmd = new GameCommand(444, newScope, queue);
        Assert.Throws<Exception>(() =>
        {
            gamecmd.Execute();
        });
    }
}
