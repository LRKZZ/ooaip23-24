namespace spacebattle;

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class GameCommandTest
{
    //private Exception _exception = new Exception();
    private readonly Hashtable _gameThreadMap = new Hashtable();
    public GameCommandTest()
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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Commands.SoftStop", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                IoC.Resolve<ICommand>($"SoftStop.{(Guid)args[0]}", (Action)args[1], (Action)args[2]).Execute();
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

        // Попытаться вот эту штуку реализовать как отдельную стратегию
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetThreadIdByGameId", (object[] args) =>
        {
            return _gameThreadMap[(int)args[0]];
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.CreateAndStart", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                IoC.Resolve<ICommand>("Server.Command.Create", (Guid)args[0], (Action)args[1]).Execute();
                IoC.Resolve<ICommand>("Server.Command.Start", (Guid)args[0]).Execute();
            });
        }).Execute();

        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        //{
        //    return new ActionCommand(() =>
        //    {
        //        _exception = (Exception)args[0];
        //    });
        //}).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SendCommandToScheduler", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                var threadid = (Guid)_gameThreadMap[(int)args[0]];
                var tmpcmd = new GameCommand((int)args[0], args[1], (Queue<ICommand>)args[2]);
                Scheduler.SendCommand(threadid, tmpcmd, args[1]);
            });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetTimeQuant", (object[] args) =>
        {
            var time = new TimeSpan(0, 0, 5);
            return (object)time;
        }).Execute();
    }

    [Fact]
    public void SuccessTest()
    {
        var gameQueue = new Queue<ICommand>();
        var mockCmd = new Mock<ICommand>();
        mockCmd.Setup(x => x.Execute()).Verifiable();
        gameQueue.Enqueue(mockCmd.Object);
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
        var gamecmd = new GameCommand(432, scope, gameQueue);
        var id = Guid.NewGuid();
        _gameThreadMap.Add(432, id);

        IoC.Resolve<ICommand>("Server.CreateAndStart", id, () => { }).Execute();

        IoC.Resolve<ICommand>("Server.SendCommand", id, gamecmd).Execute();

        var ss = IoC.Resolve<ICommand>("Server.Commands.HardStop", id, () => { }, () => { });
        IoC.Resolve<ICommand>("Server.SendCommand", id, ss).Execute();

        mockCmd.Verify(m => m.Execute(), Times.Once);
    }
}
