using System.Collections.Concurrent;

namespace spacebattle
{
    public class ServerThread
    {
        private Action _behaviour;
        private readonly BlockingCollection<ICommand> _queue;
        private readonly Thread _thread;
        private bool _stop = false;
        private Action? _event;

        public ServerThread(BlockingCollection<ICommand> queue)
        {
            _queue = queue;

            _behaviour = () =>
            {
                var cmd = _queue.Take();
                try
                {
                    cmd.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            };

            _thread = new Thread(() =>
            {
                while (!_stop)
                {
                    _behaviour();
                }

                if (_event != null)
                {
                    _event();
                }
            });
        }

        internal void Stop()
        {
            _stop = !_stop;
        }

        internal void SetAction(Action action)
        {
            _event = action;
        }

        internal BlockingCollection<ICommand> GetQue()
        {
            return _queue;
        }

        internal void SetBehaviour(Action newBehaviour)
        {
            _behaviour = newBehaviour;
        }

        public void Start()
        {
            _thread.Start();
        }
    }
}
