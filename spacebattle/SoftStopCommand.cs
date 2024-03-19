using System.Collections.Concurrent;

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
                        Console.WriteLine(e);
                    }
                }
            };
        }

        public void Execute()
        {
            _t.SetBehaviour(_action);
        }
    }
}
