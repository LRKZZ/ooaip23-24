using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    internal class UObject
    {
        int id { get; set; }
        string action { get; set; }
        Dictionary<string, int> param { get; set; }

        public UObject(int id, string action, Dictionary<string, int> param) 
        {
            this.id = id;
            this.action = action;
            this.param = param;
        }
    }
}
