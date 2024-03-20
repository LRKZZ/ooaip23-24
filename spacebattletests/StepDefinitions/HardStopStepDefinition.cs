namespace spacebattle;

using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class ServerThreadTest
{
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
    }

    [Fact]
    public void HardStopCommandShouldStopServer()
    {
        var list = new ThreadsList();
        var mre = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);
        var p = IoC.Resolve<ServerThread>("Server.Command.Create", 1, q, list);

        var hs = IoC.Resolve<ICommand>("Server.Commands.HardStop", 1, list, () => { mre.Set(); });

        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { }), list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, hs, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { }), list).Execute();

        IoC.Resolve<ICommand>("Server.Command.Start", 1, list).Execute();
        mre.WaitOne();

        Assert.Single(q);
    }

    [Fact]
    public void HardStopCommandExeptionError()
    {
        var list = new ThreadsList();
        var exCommand = new Mock<ICommand>();
        var mre = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);
        var p = IoC.Resolve<ServerThread>("Server.Command.Create", 1, q, list);

        exCommand.Setup(x => x.Execute()).Throws<Exception>().Verifiable();
        var hs = IoC.Resolve<ICommand>("Server.Commands.HardStop", 1, list, () => { mre.Set(); });

        IoC.Resolve<ICommand>("Server.SendCommand", 1, exCommand.Object, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, hs, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { }), list).Execute();

        IoC.Resolve<ICommand>("Server.Command.Start", 1, list).Execute();
        mre.WaitOne();

        exCommand.Verify(m => m.Execute(), Times.Once);
    }
}
