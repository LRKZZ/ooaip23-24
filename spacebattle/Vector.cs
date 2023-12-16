namespace spacebattle
{
    public class Vector
    {
        private float X { get; }
        private float Y { get; }

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            var newX = vector1.X + vector2.X;
            var newY = vector1.Y + vector2.Y;
            return new Vector(newX, newY);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                throw new Exception();
            }

            var other = (Vector)obj;
            return GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() * 397) ^ Y.GetHashCode();
        }
    }
}
