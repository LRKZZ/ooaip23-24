using Hwdtech;
using System.Collections.Concurrent;

namespace spacebattle
{
    public class ServerThread
    {
        private BlockingCollection<ICommand> _q;
        private Thread _t;
        private bool _stop = false;
        private Action _behaviour;

        public ServerThread(BlockingCollection<ICommand> q)
        {
            _q = q;

            _behaviour = () =>
            {
                var cmd = _q.Take();
                try
                {
                    cmd.Execute();
                }
                catch (Exception e)
                {
                    IoC.Resolve<ICommand>("ExceptionHandler.Handle", cmd, e).Execute();
                }
            };

            _t = new Thread(() =>
            {
                while (!_stop)
                {
                    _behaviour();
                }
            });
        }

        public void Start()
        {
            _t.Start();
        }

        internal void Stop()
        {
            // HardStop должен вызывать команду Stop
            _stop = true;
        }

        internal void SetBehaviour(Action behaviour)
        {
            // SoftStop должен вызывать команду смены поведения и добавлять новое условие в цикл
            _behaviour = behaviour;
        }
    }
}
