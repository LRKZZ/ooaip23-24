using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    public interface IMovable
    {
        public Vector Position { get; set; }
        public Vector Velocity { get; }
    }
}
