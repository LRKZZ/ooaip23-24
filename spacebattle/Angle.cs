using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    public class Angle
    {
        int X { get; }

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

            Angle other = (Angle)obj;
            return GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return X.GetHashCode();
        }

    }
}
