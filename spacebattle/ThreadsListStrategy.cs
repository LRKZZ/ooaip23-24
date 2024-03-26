using Hwdtech;
namespace spacebattle
{
    public class ThreadsListStrategy
    {
        private readonly ThreadsList _threads;
        public ThreadsListStrategy()
        {
            _threads = IoC.Resolve<ThreadsList>("GetThreadsList");
        }

        public ServerThread GetThread(int threadId)
        {
            return _threads.GetThread(threadId);
        }

        public void AddThread(int id, ServerThread st)
        {
            _threads.AddThread(id, st);
        }
    }
}
