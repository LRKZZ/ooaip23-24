using Hwdtech;

namespace spacebattle
{
    public class Scheduler
    {
        public static void SendCommand(Guid thread, ICommand command, object scope)
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
            IoC.Resolve<ICommand>("Server.SendCommand", thread, command).Execute();
        }
    }
}
