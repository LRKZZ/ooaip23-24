using Hwdtech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    internal class StartMoveCommand : ICommand
    {
        private IMoveCommandStartable order;

        StartMoveCommand(IMoveCommandStartable order)
        {
            this.order = order;
        }

        public void Execute()
        {
            //1.Устанвить скорость в таргет
            // order.target.set_property("Velocity", order.initialVelocity)
            //2.Создать операцию движения
            //3.записать операцию в таргет
            //4.Закинуть команду в очередь
        }
    }
}
