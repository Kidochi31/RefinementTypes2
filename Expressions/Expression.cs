using RefinementTypes2.Proof;
using RefinementTypes2.StandardTyping;
using RefinementTypes2.Typing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Expressions
{
    internal abstract class Expression
    {
        public abstract bool IsSameAs(Expression other);

        public override string ToString()
        {
            return "Expression";
        }

        internal class TypeExpr(Typing.Type type) : Expression
        {
            public Typing.Type Type = type;

            public override bool IsSameAs(Expression other)
            {
                if (other is TypeExpr otherType)
                {
                    StandardType thisStandard = TypeSimplifier.SimplifyType(Type);
                    StandardType otherStandard = TypeSimplifier.SimplifyType(otherType.Type);
                    return Imposition.ImposeBOnA(thisStandard, otherStandard) && Imposition.ImposeBOnA(otherStandard, thisStandard);
                }
                return false;
            }

            public override string ToString()
            {
                return $"{Type}";
            }
        }

        internal class This() : Expression
        {
            public override bool IsSameAs(Expression other)
            {
                return other is This;
            }

            public override string ToString()
            {
                return $"this";
            }
        }
    }
}
