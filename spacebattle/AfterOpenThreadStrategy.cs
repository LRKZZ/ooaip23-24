namespace spacebattle
{
    public class AfterOpenThreadStrategy
    {
        private readonly ServerThread _thread;
        private readonly Action _action;
        public AfterOpenThreadStrategy(ServerThread thread, Action act)
        {
            _thread = thread;
            _action = act;
        }

        public void Run()
        {
            _thread.SetBeforeAction(_action);
        }
    }
}
