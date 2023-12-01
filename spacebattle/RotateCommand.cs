using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    public class RotateCommand : ICommand
    {
        private IRotatable obj;

        public RotateCommand(IRotatable rotatable)
        {
            obj = rotatable;
        }
        // необходимо добавить обработку исключений, если нет значения угла или угловой скорости
        public void Execute()
        {
            obj.angle += obj.angleSpeed;
        }
    }
}
