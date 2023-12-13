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
            var command = new LongObjectCommand(order.Target, order.cmd);
            order.Args.dictionary.ToList().ForEach(e => 
                order.Target.args.Add(e.Key, e.Value)
            );

            order.Target.args.Add(
                order.cmd,
                Hwdtech.IoC.Resolve<ICommand>(
                        order.cmd,
                        order.Target
                    )
                );
        }
    }
}
