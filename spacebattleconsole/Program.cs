using System.Collections;
using System.Collections.Concurrent;
using System.Net.Http.Json;
using Hwdtech;
using Hwdtech.Ioc;
using spacebattle;
using spacebattleapi;

namespace spacebattleconsole
{
    internal class Program
    {
        private static readonly Hashtable _gameThreadMap = new Hashtable();
        private static Exception _exception = new Exception();
        public static void Main()
        {
            InitScope();

            var id = Guid.NewGuid();
            _gameThreadMap.Add(3474, id);

            IoC.Resolve<spacebattle.ICommand>("Server.CreateAndStart", id, () => { }).Execute();

            ApiBuilder.Build(IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current")));

            var clientHandler = new HttpClientHandler();
            var client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri("http://localhost:7881");

            var points = new Dictionary<string, object>
            {
                { "cmd", "testcommand" },
                { "gameId", 3474 },
                { "gg", 54 },
                { "hh", 666 },
                { "Speed", 666 },
                { "CanRotate", true }
            };

            var content = JsonContent.Create(points);
            var response = client.PostAsync("/message", content);
            Console.WriteLine(response.Result.ToString());

            var ss = IoC.Resolve<spacebattle.ICommand>("Server.Commands.SoftStop", id, () => { }, () => { });
            IoC.Resolve<spacebattle.ICommand>("Server.SendCommand", id, ss).Execute();
            Console.WriteLine(_exception.ToString());
        }

        private static void InitScope()
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
                    var q = new BlockingCollection<spacebattle.ICommand>(100);
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
                    IoC.Resolve<spacebattle.ICommand>($"HardStop.{(Guid)args[0]}", (Action)args[1]).Execute();
                });
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Commands.SoftStop", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    IoC.Resolve<spacebattle.ICommand>($"SoftStop.{(Guid)args[0]}", (Action)args[1], (Action)args[2]).Execute();
                });
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.SendCommand", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    var q = IoC.Resolve<BlockingCollection<spacebattle.ICommand>>($"Queue.{(Guid)args[0]}");
                    q.Add((spacebattle.ICommand)args[1]);
                });
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.BuildToGameCommand", (object[] args) =>
            {
                return new ActionCommand(() =>
                {

                });
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetThreadIdByGameId", (object[] args) =>
            {
                return _gameThreadMap[(int)args[0]];
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.CreateAndStart", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    IoC.Resolve<spacebattle.ICommand>("Server.Command.Create", (Guid)args[0], (Action)args[1]).Execute();
                    IoC.Resolve<spacebattle.ICommand>("Server.Command.Start", (Guid)args[0]).Execute();
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
    }
}
