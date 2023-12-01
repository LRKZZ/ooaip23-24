using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    public interface IRotatable
    {
        public Angle angle { get; set; }
        public Angle angleSpeed { get; }

    }
}
