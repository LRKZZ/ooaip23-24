using Hwdtech;

namespace spacebattle
{
    public class HardStopIdStrategy
    {
        private readonly Guid _id;
        private readonly ServerThread _thread;
        public HardStopIdStrategy(Guid id, ServerThread thread)
        {
            _id = id;
            _thread = thread;
        }

        public void Run()
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"HardStop.{_id}", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    new HardStopCommand(_thread).Execute();
                    new AfterCloseThreadStrategy(_thread, (Action)args[0]).Run();
                });
            }).Execute();
        }
    }
}
