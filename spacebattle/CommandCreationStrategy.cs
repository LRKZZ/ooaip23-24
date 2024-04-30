namespace spacebattle;

using Hwdtech;

public class CommandFactory
{
    public static object Invoke(params object[] args)
    {
        var message = (IMessage)args[0];

        try
        {
            var cmd = IoC.Resolve<ICommand>("Command." + message.cmdType);
            return cmd;
        }
        catch (Exception ex)
        {
            throw new Exception("Unknown IoC dependency key: " + ex.Message);
        }
    }
}
