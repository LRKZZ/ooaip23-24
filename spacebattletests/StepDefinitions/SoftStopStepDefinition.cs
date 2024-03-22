namespace spacebattle;

using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class SoftStopTest
{
    public SoftStopTest()
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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Commands.SoftStop", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                new ActionCommand((Action)args[3]).Execute();
                new SoftStopCommand(new ThreadsListStrategy((ThreadsList)args[1]).GetThread((int)args[0])).Execute();
                new AfterCloseThreadStrategy(new ThreadsListStrategy((ThreadsList)args[1]).GetThread((int)args[0]), (Action)args[2]).Run();
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.CreateAndStart", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var t = new ServerThread((BlockingCollection<ICommand>)args[1]);
                new ThreadsListStrategy((ThreadsList)args[2]).AddThread((int)args[0], t);
                var tl = new ThreadsListStrategy((ThreadsList)args[2]);
                new AfterOpenThreadStrategy(new ThreadsListStrategy((ThreadsList)args[2]).GetThread((int)args[0]), (Action)args[3]).Run();
                tl.GetThread((int)args[0]).Start();
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
    public void SoftStopCommandShouldStopServer()
    {
        var list = new ThreadsList();
        var mre = new ManualResetEvent(false);
        var stop = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);
        var cmd = new Mock<ICommand>();
        //var t = IoC.Resolve<ServerThread>("Server.Command.Create", 1, q, list, () => { });
        var ss = IoC.Resolve<ICommand>("Server.Commands.SoftStop", 1, list, () => { mre.Set(); }, () => { stop.WaitOne(); });
        cmd.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<ICommand>("Server.CreateAndStart", 1, q, list, () => { }).Execute();

        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { }), list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, ss, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { }), list).Execute();

        //IoC.Resolve<ICommand>("Server.Command.Start", 1, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, cmd.Object, list).Execute();
        stop.Set();
        mre.WaitOne();
        cmd.Verify(m => m.Execute(), Times.Once);
        Assert.Empty(q);
    }

    [Fact]
    public void SoftStopCommandExeptionError()
    {
        var list = new ThreadsList();
        var exCommand = new Mock<ICommand>();
        var mre = new ManualResetEvent(false);
        var stop = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);
        //var t = IoC.Resolve<ServerThread>("Server.Command.Create", 1, q, list, () => { });
        IoC.Resolve<ICommand>("Server.CreateAndStart", 1, q, list, () => { }).Execute();
        exCommand.Setup(x => x.Execute()).Throws<Exception>().Verifiable();
        var ss = IoC.Resolve<ICommand>("Server.Commands.SoftStop", 1, list, () => { mre.Set(); }, () => { stop.WaitOne(); });

        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { }), list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, ss, list).Execute();

        //IoC.Resolve<ICommand>("Server.Command.Start", 1, list).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, exCommand.Object, list).Execute();
        stop.Set();
        mre.WaitOne();

        exCommand.Verify(m => m.Execute(), Times.Once);
    }
}
