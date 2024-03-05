using Hwdtech;

namespace spacebattle
{
    public class SoftStopCommand : ICommand
    {
        private ServerThread _t;

        private Action _action;
        public SoftStopCommand(ServerThread t)
        {
            _t = t;
            var _stop = _t.GetStop();
            var _queue = _t.GetQue();
            _action = () =>
            {
                /*while (!_stop && _queue.IsCompleted)
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
                }*/
            };
        }

        public void Execute()
        {
            _t.SetBehaviour(_action);
            _t.Stop();
        }
    }
}
