using RefinementTypes2.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.StandardTyping
{
    internal class StandardRefinement(bool inverted, RelativeExpression left, Predicate predicate, Expression right)
    {
        public bool Inverted = inverted;
        public RelativeExpression Left = left;
        public Predicate Predicate = predicate;
        public Expression Right = right;

        public StandardRefinement Invert()
        {
            return new(!Inverted, Left, Predicate, Right);
        }
    }
}
