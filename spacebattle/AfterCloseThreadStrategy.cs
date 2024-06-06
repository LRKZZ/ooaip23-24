namespace spacebattle
{
    public class AfterCloseThreadStrategy
    {
        private readonly ServerThread _thread;
        private readonly Action _action;
        public AfterCloseThreadStrategy(ServerThread thread, Action act)
        {
            _thread = thread;
            _action = act;
        }

        public void Run()
        {
            _thread.SetAfterAction(_action);
        }
    }
}
