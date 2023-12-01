﻿using System;
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

        public static Vector Add(Vector vec1, Vector vec2)
        {
            return new Vector(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Vector other = (Vector)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() * 397) ^ Y.GetHashCode();
        }
    }
}
