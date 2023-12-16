namespace spacebattle
{
    public interface IRotatable
    {
        public Angle angle { get; set; }
        public Angle angleSpeed { get; }
    }
}
