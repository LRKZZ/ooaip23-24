namespace spacebattle;
using Hwdtech;

public class InterpretationCommands : ICommand
{
    private readonly IMessage _message;

    public InterpretationCommands(IMessage msg)
    {
        _message = msg;
    }

    public void Execute()
    {
        var cmd = CreateCommand();
        var queue = GetQueue();
        InsertIntoQueue(queue, cmd);
    }

    private ICommand CreateCommand()
    {
        return IoC.Resolve<ICommand>("Command." + _message.cmdType);
    }

    private Queue<ICommand> GetQueue()
    {
        var gameID = int.Parse(_message.GameID.ToString());

        Queue<ICommand>? queue;
        var gameQueues = IoC.Resolve<IDictionary<int, Queue<ICommand>>>("GetGameQueue");

        if (!gameQueues.TryGetValue(gameID, out queue))
        {
            throw new Exception("Очередь не найдена для игры " + gameID);
        }

        return queue ?? throw new Exception("Очередь возвращена как null");
    }

    private static void InsertIntoQueue(Queue<ICommand> queue, ICommand cmd)
    {
        queue.Enqueue(cmd);
    }
}
