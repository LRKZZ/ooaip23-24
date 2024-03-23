using System.Collections.Concurrent;
using Hwdtech;

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
        private object? _scope;

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
                    IoC.Resolve<ICommand>("Exception.Handler", e).Execute();
                }
            };

            _thread = new Thread(() =>
            {
                if (_scope != null)
                {
                    IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();
                }

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

        public void SetScope(object scope)
        {
            _scope = scope;
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

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() == typeof(Thread))
            {
                return _thread == (Thread)obj;
            }

            if (obj.GetType() == typeof(ServerThread))
            {
                return GetHashCode() == obj.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
