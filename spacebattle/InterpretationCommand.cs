namespace spacebattle;
public class InterpretationCommand : ICommand
{
    private readonly IMessage _message;

    public InterpretationCommand(IMessage msg)
    {
        _message = msg;
    }

    public void Execute()
    {
        var cmd = CommandFactory.CreateCommand(_message);
        var queue = QueueRetriever.GetQueue(_message.GameID);
        queue.Enqueue(cmd);
    }
}