namespace spacebattle
{
    public class Angle
    {
        private int X { get; }

        public Angle(int x)
        {
            X = x;
        }

        public static Angle operator +(Angle angle, Angle angularVelocity)
        {
            return new Angle((angle.X + angularVelocity.X) % 360);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                throw new Exception();
            }

            var other = (Angle)obj;
            return GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return X.GetHashCode();
        }
    }
}
