using System.Collections.Concurrent;

namespace spacebattle
{
    public class ServerThread
    {
        private Action _behaviour;
        private readonly BlockingCollection<ICommand> _queue;
        private readonly Thread _thread;
        private bool _stop = false;

        public ServerThread(BlockingCollection<ICommand> queue)
        {
            _queue = queue;

            _behaviour = () =>
            {
                while (!_stop)
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
                }
            };

            _thread = new Thread(() =>
            {
                _behaviour();
            });
        }

        internal void Stop()
        {
            _stop = !_stop;
        }

        internal Action GetBehaviour()
        {
            return _behaviour;
        }

        internal bool GetStop()
        {
            return _stop;
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
