namespace spacebattle;
using Hwdtech;
public class QueuePopStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var id = (int)args[0];
        _ = (ICommand)args[1];

        var queue = IoC.Resolve<Queue<ICommand>>("RetrieveQueueByGameId", id);

        return queue.Dequeue();
    }
}
