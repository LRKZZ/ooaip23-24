using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    internal class UObject
    {
        public IDict<string, object> args { get; set; }

        public UObject(IDict<string, object> args) 
        {
            this.args = args;
        }
    }
}
