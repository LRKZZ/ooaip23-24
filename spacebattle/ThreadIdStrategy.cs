using Hwdtech;

namespace spacebattle
{
    public class ThreadIdStrategy
    {
        private readonly Guid _id;
        private readonly ServerThread _thread;
        public ThreadIdStrategy(Guid id, ServerThread thread) 
        {
            _id = id;
            _thread = thread;
        }

        public void Run()
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register",$"GetThreadId.{_id}", (object[] args) =>
            {
                return _thread;
            }).Execute();
        }
    }
}
