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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Command.Create", (object[] args) =>
        {
            var t = new ServerThread((BlockingCollection<ICommand>)args[1]);
            new ThreadIdStrategy((Guid)args[0], t).Run();
            new AfterOpenThreadStrategy(t, (Action)args[3]).Run();
            t.SetScope(args[2]);
            return t;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Command.Start", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var t = IoC.Resolve<ServerThread>($"GetThreadId.{(Guid)args[0]}");
                t.Start();
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Commands.HardStop", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var t = IoC.Resolve<ServerThread>($"GetThreadId.{(Guid)args[0]}");
                new HardStopCommand(t).Execute();
                new AfterCloseThreadStrategy(t, (Action)args[1]).Run();
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.SendCommand", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var t = IoC.Resolve<ServerThread>($"GetThreadId.{(Guid)args[0]}");
                var q = t.GetQue();
                q.Add((ICommand)args[1]);
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.CreateAndStart", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                IoC.Resolve<ServerThread>("Server.Command.Create", (Guid)args[0], (BlockingCollection<ICommand>)args[1], args[2], (Action)args[3]);
                IoC.Resolve<ICommand>("Server.Command.Start", (Guid)args[0]).Execute();
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
        var id = Guid.NewGuid();
        var mre = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);

        IoC.Resolve<ICommand>("Server.CreateAndStart", id, q, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () => { }).Execute();

        var hs = IoC.Resolve<ICommand>("Server.Commands.HardStop", id, () => { mre.Set(); });

        IoC.Resolve<ICommand>("Server.SendCommand", id, new ActionCommand(() => { })).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id, hs).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id, new ActionCommand(() => { })).Execute();

        mre.WaitOne();
        IoC.Resolve<ServerThread>($"GetThreadId.{id}").Wait();

        Assert.Single(q);
    }

    [Fact]
    public void HardStopCommandExeptionError()
    {
        var id = Guid.NewGuid();
        var cmd = new Mock<ICommand>();
        var mre = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);

        IoC.Resolve<ICommand>("Server.CreateAndStart", id, q, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () => { }).Execute();

        var exCommand = new Mock<ICommand>();
        exCommand.Setup(x => x.Execute()).Throws<Exception>().Verifiable();
        var hs = IoC.Resolve<ICommand>("Server.Commands.HardStop", id, () => { mre.Set(); });

        IoC.Resolve<ICommand>("Server.SendCommand", id, exCommand.Object).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id, cmd.Object).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id, hs).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id, new ActionCommand(() => { })).Execute();

        mre.WaitOne();
        IoC.Resolve<ServerThread>($"GetThreadId.{id}").Wait();

        Assert.Single(q);
        exCommand.Verify(m => m.Execute(), Times.Once);
        cmd.Verify(m => m.Execute(), Times.Once);
    }

    [Fact]
    public void HardStopCommandSendIntoAnotherThread()
    {
        var id_1 = Guid.NewGuid();
        var id_2 = Guid.NewGuid();
        var cmd = new Mock<ICommand>();
        var mre = new ManualResetEvent(false);
        var mre1 = new ManualResetEvent(false);
        var q_1 = new BlockingCollection<ICommand>(100);
        var q_2 = new BlockingCollection<ICommand>(100);

        IoC.Resolve<ICommand>("Server.CreateAndStart", id_1, q_1, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () => { }).Execute();
        IoC.Resolve<ICommand>("Server.CreateAndStart", id_2, q_2, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")), () => { }).Execute();

        var hs_1 = IoC.Resolve<ICommand>("Server.Commands.HardStop", id_1, () => { mre.Set(); });
        var hs_2 = IoC.Resolve<ICommand>("Server.Commands.HardStop", id_2, () => { mre1.Set(); });

        IoC.Resolve<ICommand>("Server.SendCommand", id_1, hs_1).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id_2, hs_1).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id_2, hs_2).Execute();

        mre.WaitOne();
        mre1.WaitOne();

        IoC.Resolve<ServerThread>($"GetThreadId.{id_1}").Wait();
        IoC.Resolve<ServerThread>($"GetThreadId.{id_2}").Wait();

        Assert.Equal("WRONG!", _exception.Message);
    }
}
