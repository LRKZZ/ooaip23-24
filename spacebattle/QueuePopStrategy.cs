﻿namespace spacebattle;
using Hwdtech;
public class QueuePopStrategy : Strategy
{
    public object Execute(params object[] args)
    {
        var id = (int)args[0];

        var queue = IoC.Resolve<Queue<ICommand>>("RetrieveQueueByGameId", id);

        return queue.Dequeue();
    }
}
