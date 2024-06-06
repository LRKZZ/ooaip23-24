namespace spacebattle;

public class RemoveGameStrategy : Strategy
{
    public object Execute(params object[] args)
    {
        return new RemoveGameCommand((int)args[0]);
    }
}
