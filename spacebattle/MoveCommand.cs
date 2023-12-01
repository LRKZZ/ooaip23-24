using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    public class MoveCommand : ICommand
    {
        private IMovable obj;

        public MoveCommand(IMovable movable)
        {
            obj = movable;
        }

        public void Execute()
        {
            obj.Position += obj.Velocity;
        }
    }
}
