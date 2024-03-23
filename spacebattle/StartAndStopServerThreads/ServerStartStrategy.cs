namespace spacebattle;

public class StartAppStrategy : IStrategy
{
    public object Invoke(params object[] args)
    {
        return new StartServer((int)args[0]);
    }
}