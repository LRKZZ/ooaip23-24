namespace spacebattle
{
    public class ServerStartStrategy : IStrategy
{
    public object Invoke(params object[] args)
    {
        return new StartServer((int)args[0]);
    }
}
}