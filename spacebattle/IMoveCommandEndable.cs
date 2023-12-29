namespace spacebattle
{
    public interface IMoveCommandEndable
    {
        public IUObject Target { get; }
        public IEnumerable<string> args { get; }
    }
}
