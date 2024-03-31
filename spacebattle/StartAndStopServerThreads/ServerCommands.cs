using System.Collections.Concurrent;
using Hwdtech;
namespace spacebattle
{
    public class StopServer : ICommand
    {
        private readonly ConcurrentDictionary<int, object> _threadMap;

        public StopServer(ConcurrentDictionary<int, object> threadMap)
        {
            _threadMap = threadMap;
        }

        public void Execute()
        {
            _threadMap.ToList().ForEach(pair =>
        {
            var threadId = pair.Key;
            IoC.Resolve<ICommand>("ServerCommandSend", threadId,
                IoC.Resolve<ICommand>("StopServerCommand", threadId)).Execute();
        });
        }
    }

    public class StartServer : ICommand
    {
        private readonly int _threadCount;

        public StartServer(int threadCount)
        {
            _threadCount = threadCount;
        }

        public void Execute()
        {
            Enumerable.Range(0, _threadCount)
            .ToList()
            .ForEach(i => IoC.Resolve<ICommand>("StartCommand", i).Execute());
        }
    }
}
