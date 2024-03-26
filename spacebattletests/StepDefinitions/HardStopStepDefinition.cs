namespace spacebattle;

using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class ServerThreadTest
{
    private Exception _exception = new Exception();
    public ServerThreadTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
            IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetThreadById", (object[] args) =>
        {
            return new ThreadsListStrategy((ThreadsList)args[1]).GetThread((int)args[0]);
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Command.Create", (object[] args) =>
        {
            var t = new ServerThread((BlockingCollection<ICommand>)args[1]);
            new ThreadsListStrategy((ThreadsList)args[2]).AddThread((int)args[0], t);
            new AfterOpenThreadStrategy(new ThreadsListStrategy((ThreadsList)args[2]).GetThread((int)args[0]), (Action)args[3]).Run();
            return t;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Command.Start", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var tl = new ThreadsListStrategy((ThreadsList)args[1]);
                tl.GetThread((int)args[0]).Start();
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Commands.HardStop", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                new HardStopCommand(new ThreadsListStrategy((ThreadsList)args[1]).GetThread((int)args[0])).Execute();
                new AfterCloseThreadStrategy(new ThreadsListStrategy((ThreadsList)args[1]).GetThread((int)args[0]), (Action)args[2]).Run();
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.SendCommand", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var tl = new ThreadsListStrategy((ThreadsList)args[2]);
                var q = tl.GetThread((int)args[0]).GetQue();
                q.Add((ICommand)args[1]);
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.CreateAndStart", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var t = new ServerThread((BlockingCollection<ICommand>)args[1]);
                new ThreadsListStrategy((ThreadsList)args[2]).AddThread((int)args[0], t);
                var tl = new ThreadsListStrategy((ThreadsList)args[2]);
                new AfterOpenThreadStrategy(new ThreadsListStrategy((ThreadsList)args[2]).GetThread((int)args[0]), (Action)args[4]).Run();
                tl.GetThread((int)args[0]).SetScope(args[3]);
                tl.GetThread((int)args[0]).Start();
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                _exception = (Exception)args[0];
            });
        }).Execute();
    }

    [Fact]
    public void HardStopCommandShouldStopServer()
    {
        var list = new ThreadsList();
        var mre = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);
        IoC.Resolve<ICommand>("Server.CreateAndStart", 1, q, list, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () =>
        {

        }).Execute();
        var hs = IoC.Resolve<ICommand>("Server.Commands.HardStop", 1, list, () => { mre.Set(); });

        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { }), list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, hs, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { }), list).Execute();

        mre.WaitOne();
        IoC.Resolve<ServerThread>("Server.GetThreadById", 1, list).Wait();

        Assert.Single(q);
    }

    [Fact]
    public void HardStopCommandExeptionError()
    {
        var list = new ThreadsList();
        var exCommand = new Mock<ICommand>();
        var cmd = new Mock<ICommand>();
        var mre = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);
        IoC.Resolve<ICommand>("Server.CreateAndStart", 1, q, list, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () =>
        {

        }).Execute();
        exCommand.Setup(x => x.Execute()).Throws<Exception>().Verifiable();
        var hs = IoC.Resolve<ICommand>("Server.Commands.HardStop", 1, list, () => { mre.Set(); });

        IoC.Resolve<ICommand>("Server.SendCommand", 1, exCommand.Object, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, cmd.Object, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, hs, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { }), list).Execute();

        mre.WaitOne();
        IoC.Resolve<ServerThread>("Server.GetThreadById", 1, list).Wait();
        Assert.Single(q);
        exCommand.Verify(m => m.Execute(), Times.Once);
        cmd.Verify(m => m.Execute(), Times.Once);
    }

    [Fact]
    public void HardStopCommandSendIntoAnotherThread()
    {
        var list = new ThreadsList();
        var cmd = new Mock<ICommand>();
        var mre = new ManualResetEvent(false);
        var q_1 = new BlockingCollection<ICommand>(100);
        var q_2 = new BlockingCollection<ICommand>(100);
        IoC.Resolve<ICommand>("Server.CreateAndStart", 1, q_1, list, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () =>
        {

        }).Execute();
        IoC.Resolve<ICommand>("Server.CreateAndStart", 2, q_2, list, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () =>
        {

        }).Execute();
        var hs_1 = IoC.Resolve<ICommand>("Server.Commands.HardStop", 1, list, () => { mre.Set(); });
        var hs_2 = IoC.Resolve<ICommand>("Server.Commands.HardStop", 2, list, () => { mre.Set(); });

        IoC.Resolve<ICommand>("Server.SendCommand", 1, cmd.Object, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, hs_1, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 2, hs_1, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 2, hs_2, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { }), list).Execute();

        mre.WaitOne();
        IoC.Resolve<ServerThread>("Server.GetThreadById", 1, list).Wait();
        IoC.Resolve<ServerThread>("Server.GetThreadById", 2, list).Wait();

        Assert.Equal("WRONG!", _exception.Message);
    }
}
