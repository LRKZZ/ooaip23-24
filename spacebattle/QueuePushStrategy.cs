namespace spacebattle;

public class QueuePushStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var idGame = (int)args[0];
        var command = (ICommand)args[1];

        return new QueuePushCommand(idGame, command);
    }
}
