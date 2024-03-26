using System.Collections.Concurrent;

namespace spacebattle
{
    public class ThreadsList
    {
        private readonly ConcurrentDictionary<int, ServerThread> _threads;
        public ThreadsList()
        {
            _threads = new ConcurrentDictionary<int, ServerThread>();
        }

        public void AddThread(int id, ServerThread thread)
        {
            _threads.TryAdd(id, thread);
        }

        public ServerThread GetThread(int id)
        {
            return _threads[id];
        }
    }
}
