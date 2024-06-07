namespace spacebattle
{
    public interface IShootable
    {
        public Vector Position { get; set; }
        public Vector Speed { get; set; }

        public float ScalarSpeed { get; set; }
    }
}
