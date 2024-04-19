namespace spacebattle;

using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
using Hwdtech;
using Hwdtech.Ioc;

public class EndpointTest
{
    private Exception _exception = new Exception();
    private readonly Hashtable _gameThreadMap = new Hashtable();
    public EndpointTest()
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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.BuildToGameCommand", (object[] args) =>
        {
            return new ActionCommand(() =>
            {

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
        var id = Guid.NewGuid();
        Task<HttpResponseMessage>? response;
        _gameThreadMap.Add(3474, id);
        var mre = new ManualResetEvent(false);
        var port = "7860";
        var tmp = _exception.Data;
        var clientHandler = new HttpClientHandler();
        var points = new Dictionary<string, object>
            {
                { "cmd", "testcommand" },
                { "gameId", 3474 },

                {"gg", 54},
                {"hh", 666},
                {"Speed", 666},
                {"CanRotate", true}
            };
        IoC.Resolve<ICommand>("Server.CreateAndStart", id, () => { }).Execute();
        var client = new HttpClient(clientHandler);
        client.BaseAddress = new Uri($"http://localhost:{port}");

        using (var endpoint = new Endpoint(port, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"))))
        {
            Endpoint.Run();
            var content = JsonContent.Create(points);
            response = client.PostAsync("/message", content);
            var ss = IoC.Resolve<ICommand>("Server.Commands.SoftStop", id, () => { mre.Set(); }, () => { });
            IoC.Resolve<ICommand>("Server.SendCommand", id, ss).Execute();
            Assert.Equal(HttpStatusCode.OK, response.Result.StatusCode);
        }
        // Необходимо создать поле HttpStatusCode = response.Result.StatusCode, после чего перенести Assert за пределы using
        mre.WaitOne();
    }

    [Fact]
    public void NotSuccessRequest()
    {
        var id = Guid.NewGuid();
        _gameThreadMap.Add(3464, id);
        Task<HttpResponseMessage>? response;
        var mre = new ManualResetEvent(false);
        var port = "7860";
        var clientHandler = new HttpClientHandler();
        var points = new Dictionary<string, object>
            {
                { "cmd", "testcommand" },
                { "gameId", 3464 },

                {"gg", 54},
                {"hh", 666},
                {"Speed", 666},
                {"CanRotate", true}
            };
        IoC.Resolve<ICommand>("Server.CreateAndStart", id, () => { }).Execute();
        var client = new HttpClient(clientHandler);
        client.BaseAddress = new Uri($"http://localhost:{port}");

        using (var endpoint = new Endpoint(port, IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"))))
        {
            Endpoint.Run();
            var content = JsonContent.Create(555);
            response = client.PostAsync("/message", content);
            var ss = IoC.Resolve<ICommand>("Server.Commands.SoftStop", id, () => { mre.Set(); }, () => { });
            IoC.Resolve<ICommand>("Server.SendCommand", id, ss).Execute();
            Assert.Equal(HttpStatusCode.BadRequest, response.Result.StatusCode);

        }

        mre.WaitOne();
    }
}
