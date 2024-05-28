namespace spacebattle
{
    public interface IMoveStartable
    {
        public IUObject Order { get; }
        public Dictionary<string, object> PropertiesOfOrder { get; }
    }
}
