namespace spacebattle
{
    public class ThreadsList
    {
        private readonly Dictionary<int, ServerThread> _threads;
        public ThreadsList()
        {
            _threads = new Dictionary<int, ServerThread>();
        }

        public void AddThread(int id, ServerThread thread)
        {
            _threads.Add(id, thread);
        }

        public ServerThread GetThread(int id)
        {
            return _threads[id];
        }
    }
}
