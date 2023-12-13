using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    internal class LongObjectCommand : ICommand
    {
        private readonly UObject _object;
        private readonly string _cmd;
        public LongObjectCommand(UObject obj, string cmd) 
        {
            _object = obj;
            _cmd = cmd;
        }
        public void Execute() 
        {
            ((ICommand)_object.args.GetP(_cmd)).Execute();
            Hwdtech.IoC.Resolve<Queue<ICommand>>("Game.Commands").Enqueue(this);
        }
    }
}
