using Hwdtech;
namespace spacebattle;

public class QueuePushCommand : ICommand
{
    private readonly int _idGame;
    private readonly ICommand _command;

    public QueuePushCommand(int idGame, ICommand command)
    {
        _idGame = idGame;
        _command = command;
    }

    public void Execute()
    {
        var queue = IoC.Resolve<Queue<ICommand>>("RetrieveQueueByGameId", _idGame);
        queue.Enqueue(_command);
    }
}
