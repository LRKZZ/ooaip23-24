namespace spacebattle;
using Hwdtech;

public class GetQueue : IStrategy
{
    public object Execute(params object[] args)
    {
        if (IoC.Resolve<IDictionary<int, Queue<ICommand>>>("GetGameQueue").TryGetValue((int)args[0], out var queue))
        {
            return queue;
        }

        throw new Exception("Queue not found for the specified game ID.");
    }
}
