namespace spacebattle
{
    public interface Order
    {
        public UObject obj { get; }
        public string cmd { get; }
        public IDict<string, object> args { get; }
    }
}
