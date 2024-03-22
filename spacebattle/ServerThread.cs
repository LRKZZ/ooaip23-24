using System.Collections.Concurrent;

namespace spacebattle
{
    public class ServerThread
    {
        private Action _behaviour;
        private readonly BlockingCollection<ICommand> _queue;
        private readonly Thread _thread;
        private bool _stop = false;
        private Action? _afterEvent;
        private Action? _beforeEvent;

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
                if (_beforeEvent != null)
                {
                    _beforeEvent();
                }

                while (!_stop)
                {
                    _behaviour();
                }

                if (_afterEvent != null)
                {
                    _afterEvent();
                }
            });
        }

        internal void Stop()
        {
            _stop = !_stop;
        }

        internal void SetAfterAction(Action action)
        {
            _afterEvent = action;
        }

        internal void SetBeforeAction(Action action)
        {
            _beforeEvent = action;
        }

        public BlockingCollection<ICommand> GetQue()
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
