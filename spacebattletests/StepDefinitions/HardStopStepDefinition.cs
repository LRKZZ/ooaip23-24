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
            return new ActionCommand(() =>
            {
                var q = new BlockingCollection<ICommand>(100);
                var t = new ServerThread(q);
                new ThreadIdStrategy((Guid)args[0], t).Run();
                new HardStopIdStrategy((Guid)args[0], t).Run();
                new SoftStopIdStrategy((Guid)(args[0]), t).Run();
                new ThreadQueueIdStrategy((Guid)args[0], q).Run();
                new AfterOpenThreadStrategy(t, (Action)args[1]).Run();
                t.SetScope(IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")));
            });
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
                IoC.Resolve<ICommand>($"HardStop.{(Guid)args[0]}", (Action)args[1]).Execute();
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.SendCommand", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var q = IoC.Resolve<BlockingCollection<ICommand>>($"Queue.{(Guid)args[0]}");
                q.Add((ICommand)args[1]);
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.CreateAndStart", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                IoC.Resolve<ICommand>("Server.Command.Create", (Guid)args[0], (Action)args[1]).Execute();
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
        var cmd = new Mock<ICommand>();

        IoC.Resolve<ICommand>("Server.CreateAndStart", id, () => { }).Execute();

        var hs = IoC.Resolve<ICommand>("Server.Commands.HardStop", id, () => { mre.Set(); });

        IoC.Resolve<ICommand>("Server.SendCommand", id, new ActionCommand(() => { })).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id, hs).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id, cmd.Object).Execute();

        mre.WaitOne();
        IoC.Resolve<ServerThread>($"GetThreadId.{id}").Wait();

        Assert.Single(IoC.Resolve<BlockingCollection<ICommand>>($"Queue.{id}"));
        cmd.Verify(m => m.Execute(), Times.Never);
    }

    [Fact]
    public void HardStopCommandExeptionError()
    {
        var id = Guid.NewGuid();
        var cmd = new Mock<ICommand>();
        var mre = new ManualResetEvent(false);

        IoC.Resolve<ICommand>("Server.CreateAndStart", id, () => { }).Execute();

        var exCommand = new Mock<ICommand>();
        exCommand.Setup(x => x.Execute()).Throws<Exception>().Verifiable();
        var hs = IoC.Resolve<ICommand>("Server.Commands.HardStop", id, () => { mre.Set(); });

        IoC.Resolve<ICommand>("Server.SendCommand", id, exCommand.Object).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id, cmd.Object).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id, hs).Execute();
        IoC.Resolve<ICommand>("Server.SendCommand", id, new ActionCommand(() => { })).Execute();

        mre.WaitOne();
        IoC.Resolve<ServerThread>($"GetThreadId.{id}").Wait();

        Assert.Single(IoC.Resolve<BlockingCollection<ICommand>>($"Queue.{id}"));
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

        IoC.Resolve<ICommand>("Server.CreateAndStart", id_1, () => { }).Execute();
        IoC.Resolve<ICommand>("Server.CreateAndStart", id_2, () => { }).Execute();

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
