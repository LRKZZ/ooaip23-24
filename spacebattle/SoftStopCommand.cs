using System.Collections.Concurrent;
using Hwdtech;

namespace spacebattle
{
    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _t;
        private readonly BlockingCollection<ICommand> _queue;
        private readonly Action _action;
        public SoftStopCommand(ServerThread t)
        {
            _t = t;
            _queue = _t.GetQue();
            _action = () =>
            {
                if (_queue.Count == 0)
                {
                    _t.Stop();
                }
                else
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
                }
            };
        }

        public void Execute()
        {
            if (_t.Equals(Thread.CurrentThread))
            {
                _t.SetBehaviour(_action);
            }
            else
            {
                throw new Exception("WRONG!");
            }
        }
    }
}
