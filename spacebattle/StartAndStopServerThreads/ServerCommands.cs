using System.Collections.Concurrent;
using Hwdtech;
namespace spacebattle
{
    public class StopServer : ICommand
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ConcurrentDictionary<int, object> _threadMap;

        public StopServer(ConcurrentDictionary<int, object> threadMap, IExceptionHandler exceptionHandler)
        {
            _threadMap = threadMap;
            _exceptionHandler = exceptionHandler;
        }

        public void Execute()
        {
            try
            {
                _threadMap.ToList().ForEach(pair =>
            {
                var threadId = pair.Key;
                IoC.Resolve<ICommand>("ServerCommandSend", threadId,
                    IoC.Resolve<ICommand>("StopServerCommand", threadId)).Execute();
            });
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex, "StopServerCommand");
            }
        }
    }

    public class StartServer : ICommand
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly int _threadCount;

        public StartServer(int threadCount, IExceptionHandler exceptionHandler)
        {
            _threadCount = threadCount;
            _exceptionHandler = exceptionHandler;
        }

        public void Execute()
        {
            try
            {
                Enumerable.Range(0, _threadCount)
            .ToList()
                .ForEach(i => IoC.Resolve<ICommand>("StartCommand", i).Execute());
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex, "StartCommand");
            }
        }
    }
}
