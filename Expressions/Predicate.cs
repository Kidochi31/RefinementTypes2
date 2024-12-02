using RefinementTypes2.StandardTyping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Expressions
{
    internal class Predicate(string name)
    {
        public string Name = name;

        public override string ToString()
        {
            return Name;
        }

        public static Predicate Base = new Predicate("Base");
    }
}
