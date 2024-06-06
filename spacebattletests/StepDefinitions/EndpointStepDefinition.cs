namespace spacebattle;

using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
using Hwdtech;
using Hwdtech.Ioc;
using Microsoft.AspNetCore.Http;
using Moq;
using spacebattleapi;

public class EndpointTest2
{
    private Exception _exception = new Exception();
    private readonly Hashtable _gameThreadMap = new Hashtable();
    public EndpointTest2()
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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new ActionCommand(() =>
            {
                _exception = (Exception)args[0];
            });
        }).Execute();
    }

    [Fact]
    public void SuccessPostRequest()
    {
        var _mockCommand = new Mock<ICommand>();
        _mockCommand.Setup(x => x.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.BuildToGameCommand", (object[] args) =>
        {
            return _mockCommand.Object;
        }).Execute();
        var id = Guid.NewGuid();
        _gameThreadMap.Add(3474, id);
        var mre = new ManualResetEvent(false);
        var tmp = _exception.Data;

        IoC.Resolve<ICommand>("Server.CreateAndStart", id, () => { }).Execute();

        MessageHandler.SetScope(IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")));
        var status = MessageHandler.Insert(new Message("test", 3474));

        var ss = IoC.Resolve<ICommand>("Server.Commands.SoftStop", id, () => { mre.Set(); }, () => { });
        IoC.Resolve<ICommand>("Server.SendCommand", id, ss).Execute();

        mre.WaitOne();
        Assert.Equal(Results.Accepted().GetType(), status.GetType());
        _mockCommand.Verify(m => m.Execute(), Times.Once());
    }

    [Fact]
    public void UnsuccessPostRequest()
    {
        var _mockCommand = new Mock<ICommand>();
        _mockCommand.Setup(x => x.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.BuildToGameCommand", (object[] args) =>
        {
            return _mockCommand.Object;
        }).Execute();
        var id = Guid.NewGuid();
        _gameThreadMap.Add(3474, id);
        var mre = new ManualResetEvent(false);
        var tmp = _exception.Data;

        ApiBuilder.Build(IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")));

        IoC.Resolve<ICommand>("Server.CreateAndStart", id, () => { }).Execute();

        var status = MessageHandler.Insert(new Message("test", 3574));

        var ss = IoC.Resolve<ICommand>("Server.Commands.SoftStop", id, () => { mre.Set(); }, () => { });
        IoC.Resolve<ICommand>("Server.SendCommand", id, ss).Execute();

        mre.WaitOne();
        Assert.Equal(Results.BadRequest().GetType(), status.GetType());
        _mockCommand.Verify(m => m.Execute(), Times.Never());
    }

    [Fact]
    public void MappingTest()
    {
        var _mockCommand = new Mock<ICommand>();
        _mockCommand.Setup(x => x.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.BuildToGameCommand", (object[] args) =>
        {
            return _mockCommand.Object;
        }).Execute();

        var id = Guid.NewGuid();
        _gameThreadMap.Add(3477, id);

        IoC.Resolve<spacebattle.ICommand>("Server.CreateAndStart", id, () => { }).Execute();

        ApiBuilder.Build(IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")));

        var clientHandler = new HttpClientHandler();
        var client = new HttpClient(clientHandler);
        client.BaseAddress = new Uri("http://localhost:7881");

        var points = new Dictionary<string, object>
            {
                { "cmd", "testcommand" },
                { "gameId", 3477 },
                { "gg", 54 },
                { "hh", 666 },
                { "Speed", 666 },
                { "CanRotate", true }
            };

        var content = JsonContent.Create(points);
        var status = client.PostAsync("/message", content);

        var ss = IoC.Resolve<spacebattle.ICommand>("Server.Commands.SoftStop", id, () => { }, () => { });
        IoC.Resolve<spacebattle.ICommand>("Server.SendCommand", id, ss).Execute();

        Assert.Equal(HttpStatusCode.Accepted, status.Result.StatusCode);
    }
}
