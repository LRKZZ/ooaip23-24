namespace spacebattle;

using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class SoftStopTest
{
    private Exception _exception = new Exception();
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
            return new ThreadsListStrategy().GetThread((int)args[0]);
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Command.Create", (object[] args) =>
        {
            var t = new ServerThread((BlockingCollection<ICommand>)args[1]);
            var tl = new ThreadsListStrategy();
            tl.AddThread((int)args[0], t);
            new AfterOpenThreadStrategy(tl.GetThread((int)args[0]), (Action)args[3]).Run();
            tl.GetThread((int)args[0]).SetScope(args[2]);
            return t;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Command.Start", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var tl = new ThreadsListStrategy();
                tl.GetThread((int)args[0]).Start();
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Commands.SoftStop", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                new ActionCommand((Action)args[2]).Execute();
                new SoftStopCommand(new ThreadsListStrategy().GetThread((int)args[0])).Execute();
                new AfterCloseThreadStrategy(new ThreadsListStrategy().GetThread((int)args[0]), (Action)args[1]).Run();
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.CreateAndStart", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                IoC.Resolve<ServerThread>("Server.Command.Create", (int)args[0], (BlockingCollection<ICommand>)args[1], args[2], (Action)args[3]);
                IoC.Resolve<ICommand>("Server.Command.Start", (int)args[0]).Execute();
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.SendCommand", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var tl = new ThreadsListStrategy();
                var q = tl.GetThread((int)args[0]).GetQue();
                q.Add((ICommand)args[1]);
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                _exception = (Exception)args[0];
            });
        }).Execute();

        var list = new ThreadsList();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadsList", (object[] args) =>
        {
            return list;
        }).Execute();
    }

    [Fact]
    public void SoftStopCommandShouldStopServer()
    {
        var mre = new ManualResetEvent(false);
        var stop = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);
        var cmd = new Mock<ICommand>();
        var ss = IoC.Resolve<ICommand>("Server.Commands.SoftStop", 1, () => { mre.Set(); }, () => { stop.WaitOne(); });
        cmd.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<ICommand>("Server.CreateAndStart", 1, q, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () => { }).Execute();

        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { })).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, ss).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { })).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, cmd.Object).Execute();

        stop.Set();
        mre.WaitOne();
        IoC.Resolve<ServerThread>("Server.GetThreadById", 1).Wait();

        cmd.Verify(m => m.Execute(), Times.Once);
        Assert.Empty(q);
    }

    [Fact]
    public void SoftStopCommandExeptionError()
    {
        var cmd = new Mock<ICommand>();
        var mre = new ManualResetEvent(false);
        var stop = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);

        IoC.Resolve<ICommand>("Server.CreateAndStart", 1, q, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () => { }).Execute();

        var exCommand = new Mock<ICommand>();
        exCommand.Setup(x => x.Execute()).Throws<Exception>().Verifiable();
        var ss = IoC.Resolve<ICommand>("Server.Commands.SoftStop", 1, () => { mre.Set(); }, () => { stop.WaitOne(); });

        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { })).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, ss).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, exCommand.Object).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, cmd.Object).Execute();

        stop.Set();
        mre.WaitOne();
        IoC.Resolve<ServerThread>("Server.GetThreadById", 1).Wait();

        Assert.Empty(q);
        exCommand.Verify(m => m.Execute(), Times.Once);
        cmd.Verify(m => m.Execute(), Times.Once);
    }

    [Fact]
    public void StopSoftCommandSendIntoAnotherThread()
    {
        var cmd = new Mock<ICommand>();
        var mre = new ManualResetEvent(false);
        var stop = new ManualResetEvent(false);
        var q_1 = new BlockingCollection<ICommand>(100);
        var q_2 = new BlockingCollection<ICommand>(100);

        IoC.Resolve<ICommand>("Server.CreateAndStart", 1, q_1, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () => { }).Execute();
        IoC.Resolve<ICommand>("Server.CreateAndStart", 2, q_2, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () => { }).Execute();

        var ss_1 = IoC.Resolve<ICommand>("Server.Commands.SoftStop", 1, () => { mre.Set(); }, () => { stop.WaitOne(); });
        var ss_2 = IoC.Resolve<ICommand>("Server.Commands.SoftStop", 2, () => { mre.Set(); }, () => { stop.WaitOne(); });

        IoC.Resolve<ICommand>("Server.SendCommand", 1, cmd.Object).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, ss_1).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 2, ss_1).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 2, ss_2).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", 1, new ActionCommand(() => { })).Execute();

        stop.Set();
        mre.WaitOne();
        IoC.Resolve<ServerThread>("Server.GetThreadById", 1).Wait();
        IoC.Resolve<ServerThread>("Server.GetThreadById", 2).Wait();

        Assert.Equal("WRONG!", _exception.Message);
    }
}
