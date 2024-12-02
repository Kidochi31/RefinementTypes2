using RefinementTypes2.Typing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Expressions
{
    internal class Expression
    {
        public override string ToString()
        {
            return "Expression";
        }

        internal class TypeExpr(Typing.Type type) : Expression
        {
            public Typing.Type Type = type;

            public override string ToString()
            {
                return $"{Type}";
            }
        }
    }
}
