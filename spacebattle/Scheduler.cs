using Hwdtech;

namespace spacebattle
{
    public class Scheduler
    {
        public static void SendCommand(Guid thread, ICommand command)
        {
            IoC.Resolve<ICommand>("SendCommand", thread, command).Execute();
        }
    }
}
