namespace spacebattle
{
    public class HardStopCommand : ICommand
    {
        private ServerThread _t;
        public HardStopCommand(ServerThread t)
        {
            _t = t;
        }

        public void Execute()
        {
            _t.Stop();
        }
    }
}
