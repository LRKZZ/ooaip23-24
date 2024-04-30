namespace spacebattle;
using Hwdtech;

public static class CommandFactory
{
    public static ICommand CreateCommand(IMessage message)
    {
        return IoC.Resolve<ICommand>("Command." + message.cmdType);
    }
}