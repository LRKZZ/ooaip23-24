using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    internal interface IMoveCommandStartable
    {
        UObject Target { get; }
        string cmd { get; }
        IDict<string, object> Args { get; set; }
    }
}
