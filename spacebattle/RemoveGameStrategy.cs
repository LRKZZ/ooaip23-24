namespace spacebattle;

public class RemoveGameStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        return new RemoveGameCommand((int)args[0]);
    }
}
