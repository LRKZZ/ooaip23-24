using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacebattle
{
    internal interface IDict<T, P>
    {
        public P GetP(T key);
        public void Add(T key, P value);
        public IDictionary<T, P> dictionary { get; }
    }
}
