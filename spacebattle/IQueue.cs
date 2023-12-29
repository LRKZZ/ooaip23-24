namespace spacebattle
{
    internal interface IQueue<T>
    {
        public void Add(T item);
        public T Take();
    }
}
