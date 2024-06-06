using Hwdtech;

namespace spacebattle
{
    public class SoftStopIdStrategy
    {
        private readonly Guid _id;
        private readonly ServerThread _thread;
        public SoftStopIdStrategy(Guid id, ServerThread thread)
        {
            _id = id;
            _thread = thread;
        }

        public void Run()
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"SoftStop.{_id}", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    new ActionCommand((Action)args[1]).Execute();
                    new SoftStopCommand(_thread).Execute();
                    new AfterCloseThreadStrategy(_thread, (Action)args[0]).Run();
                });
            }).Execute();
        }
    }
}
