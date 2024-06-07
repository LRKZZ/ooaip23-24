namespace spacebattle
{
    public interface IWagonShip
    {
        public Angle angle { get; set; }
        public Dictionary<string, object> angleSpeed { get; }
    }
}
