namespace spacebattle
{
    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _t;

        private readonly Action _action;
        public SoftStopCommand(ServerThread t)
        {
            _t = t;
            var _stop = _t.GetStop();
            var _queue = _t.GetQue();
            _action = () =>
            {
                while (!(_queue.Count == 0))
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
