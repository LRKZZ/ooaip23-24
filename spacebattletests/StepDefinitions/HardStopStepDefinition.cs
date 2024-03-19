namespace spacebattle;

using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;

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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Commands.HardStop", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                new HardStopCommand((ServerThread)args[0]).Execute();
                new AfterCloseThreadStrategy((ServerThread)args[0], (Action)args[1]).Run();
            });
        }).Execute();
    }

    [Fact]
    public void HardStopCommandShouldStopServer()
    {
        var mre = new ManualResetEvent(false);
        var q = new BlockingCollection<ICommand>(100);
        var t = new ServerThread(q);

        var hs = IoC.Resolve<ICommand>("Server.Commands.HardStop", t, () => { mre.Set(); });

        q.Add(new ActionCommand(() => { }));
        q.Add(hs);
        q.Add(new ActionCommand(() => { }));

        t.Start();
        mre.WaitOne();

        Assert.Single(q);
    }
}
