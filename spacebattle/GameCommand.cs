using System.Diagnostics;
using Hwdtech;

namespace spacebattle
{
    public class GameCommand : ICommand
    {
        private readonly int _gameId;
        private readonly object _scope;
        private readonly Queue<ICommand> _queue;
        public GameCommand(int gameId, object scope, Queue<ICommand> queue)
        {
            _gameId = gameId;
            _scope = scope;
            _queue = queue;
        }
        public void Execute()
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();
            var timeout = IoC.Resolve<TimeSpan>("GetTimeQuant");
            var time = new Stopwatch();
            time.Start();
            while (_queue.Count != 0 && time.Elapsed < timeout)
            {
                var cmd = _queue.Dequeue();
                try
                {
                    cmd.Execute();
                }
                catch (Exception ex)
                {
                    IoC.Resolve<IHandler>("Exception.Handler", cmd, ex).Handle();
                }
            }

            time.Stop();
        }
    }
}
