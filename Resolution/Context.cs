using RefinementTypes2.Expressions;
using RefinementTypes2.StandardTyping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Resolution
{
    internal class Context(RefinementProduct? product, Expression? expression)
    {
        public readonly RefinementProduct? ThisProduct = product;

        public readonly Expression? ThisExpression = expression;

        public Context AddThisProduct(RefinementProduct refinementProduct)
        {
            return new Context(refinementProduct, ThisExpression);
        }

        public Context AddThisExpression(Expression expression)
        {
            return new Context(ThisProduct, expression);
        }
    }
}
