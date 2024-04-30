namespace spacebattle;

using Hwdtech;
public class InterpretationCommand : ICommand
{
    private readonly IMessage _message;

    public InterpretationCommand(IMessage msg)
    {
        _message = msg ?? throw new ArgumentNullException(nameof(msg), "Message cannot be null");
    }

    public void Execute()
    {
        if (string.IsNullOrEmpty(_message.cmdType))
        {
            throw new Exception("CommandType cannot be empty");
        }

        var cmd = IoC.Resolve<ICommand>("CreateCommand", _message);
        IoC.Resolve<ICommand>("EnqueueCommand", _message.GameID, cmd).Execute();
    }
}