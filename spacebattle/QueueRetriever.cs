namespace spacebattle;
using Hwdtech;

public static class QueueRetriever
{
    public static Queue<ICommand> GetQueue(object gameID)
    {
        var id = int.Parse(gameID?.ToString() ?? throw new ArgumentNullException(nameof(gameID)));

        var gameQueues = IoC.Resolve<IDictionary<int, Queue<ICommand>>>("GetGameQueue");
        if (!gameQueues.TryGetValue(id, out var queue))
        {
            throw new Exception("Queue not found for game id " + id);
        }

        return queue ?? throw new Exception("Queue returned as null");
    }
}

