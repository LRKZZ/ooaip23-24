using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    public class Vector
    {
        float X { get; }
        float Y { get; }

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            float newX = vector1.X + vector2.X;
            float newY = vector1.Y + vector2.Y;
            return new Vector(newX, newY);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Vector other = (Vector)obj;
            return GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() * 397) ^ Y.GetHashCode();
        }
    }
}
