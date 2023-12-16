namespace spacebattle
{
    public interface IDict<T, P>
    {
        public P GetP(T key);
        public void Add(T key, P value);
        public IDictionary<T, P> dictionary { get; }
    }
}
