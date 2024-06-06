namespace spacebattle
{
    public interface IQueue
    {
        void Add(ICommand cmd);
        ICommand Take();
    }
}
