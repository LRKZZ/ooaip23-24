namespace spacebattle
{
    public interface IMoveCommandEndable
    {
        public ICommand endCommand { get; }
        public IUObject Target { get; }
        public IEnumerable<string> args { get; }
    }
}
