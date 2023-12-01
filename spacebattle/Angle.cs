using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    public class Angle
    {
        float X { get; }

        public Angle(float x)
        {
            X = x;
        }

        // необходимо проработать случаи, когда угловая скорость принимает отрицательные значения

        public static Angle operator +(Angle angle, Angle angularVelocity)
        {
            return new Angle((angle.X + angularVelocity.X) % 360);
        }

        // Переопределение метода Equals с использованием хэш-кода
        public override bool Equals(object obj)
        {
            //if (obj == null || GetType() != obj.GetType())
            //{
                //return false;
            //}

            Angle other = (Angle)obj;
            return GetHashCode() == other.GetHashCode();
        }

        // Переопределение метода GetHashCode для вычисления хэш-кода объекта
        public override int GetHashCode()
        {
            return X.GetHashCode();
        }

    }
}
