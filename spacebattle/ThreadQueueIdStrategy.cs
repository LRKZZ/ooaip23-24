using System.Collections.Concurrent;
using Hwdtech;

namespace spacebattle
{
    public class ThreadQueueIdStrategy
    {
        private readonly Guid _id;
        private readonly BlockingCollection<ICommand> _queue;
        public ThreadQueueIdStrategy(Guid id, BlockingCollection<ICommand> queue)
        {
            _id = id;
            _queue = queue;
        }

        public void Run()
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Queue.{_id}", (object[] args) =>
            {
                return _queue;
            }).Execute();
        }
    }
}
