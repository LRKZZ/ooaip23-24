namespace spacebattle;
using Hwdtech;

public class GetGameQueue : IStrategy
{
    public object Execute(params object[] args)
    {
        var queue = IoC.Resolve<Queue<ICommand>>("GetQueue", (int)args[0]);
        return new ActionCommand(() => { queue.Enqueue((ICommand)args[1]); });
    }
}