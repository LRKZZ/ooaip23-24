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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Commands.SoftStop", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                new SoftStopCommand((ServerThread)args[0]).Execute();
                new AfterCloseThreadStrategy((ServerThread)args[0], (Action)args[1]).Run();
            });
        }).Execute();
    }

    [Fact]
    public void SoftStopCommandShouldStopServer()
    {
        var mre = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);
        var t = new ServerThread(q);

        var ss = IoC.Resolve<ICommand>("Server.Commands.SoftStop", t, () => { mre.Set(); });

        q.Add(new ActionCommand(() => { }));
        q.Add(ss);
        q.Add(new ActionCommand(() => { }));
        q.Add(new ActionCommand(() => { }));

        t.Start();
        mre.WaitOne();

        Assert.Empty(q);
    }

    [Fact]
    public void SoftStopCommandExeptionError()
    {
        var exCommand = new Mock<ICommand>();
        var mre = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);
        var t = new ServerThread(q);
        exCommand.Setup(x => x.Execute()).Throws<Exception>().Verifiable();

        var ss = IoC.Resolve<ICommand>("Server.Commands.SoftStop", t, () => { mre.Set(); });

        q.Add(new ActionCommand(() => { }));
        q.Add(ss);
        q.Add(exCommand.Object);

        t.Start();
        mre.WaitOne();

        exCommand.Verify(m => m.Execute(), Times.Once);
    }
}
