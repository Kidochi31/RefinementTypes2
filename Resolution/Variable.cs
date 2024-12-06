using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Resolution
{
    internal class Variable(string name, Typing.Type type)
    {
        public Typing.Type Type = type;
        public string Name = name;

        public override string ToString() => Name;
    }
}
