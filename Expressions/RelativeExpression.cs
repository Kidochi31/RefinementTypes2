using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Expressions
{
    internal class RelativeExpression
    {

        public Expression Substitute(Expression relativeTo)
        {
            return relativeTo;
        }

        public override string ToString()
        {
            return "value";
        }
    }
}
