using RefinementTypes2.Proof;
using RefinementTypes2.Resolution;
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
        public abstract bool IsRelative();

        public abstract bool CanSimplifyToValue(Context context);

        public abstract Expression Simplify(Context context);

        public abstract bool IsSameAs(Expression other, Context context);

        public override string ToString() => "Expression";

        internal class TypeExpr(Typing.Type type) : Expression
        {
            public Typing.Type Type = type;

            public override bool CanSimplifyToValue(Context context) => true;

            public override bool IsRelative() => false;

            public override bool IsSameAs(Expression other, Context context)
            {
                if (other is TypeExpr otherType)
                {
                    StandardType thisStandard = TypeSimplifier.SimplifyType(Type);
                    StandardType otherStandard = TypeSimplifier.SimplifyType(otherType.Type);
                    return Imposition.ImposeBOnA(thisStandard, otherStandard) && Imposition.ImposeBOnA(otherStandard, thisStandard);
                }
                return false;
            }

            public override Expression Simplify(Context context) => this;

            public override string ToString() => $"{Type}";
        }

        internal class This() : Expression
        {
            public override bool IsRelative() => true;

            public override bool CanSimplifyToValue(Context context) => (bool)context.ThisExpression?.CanSimplifyToValue(context);

            public override bool IsSameAs(Expression other, Context context) => other is This;

            public override string ToString() => $"this";

            public override Expression Simplify(Context context) => context.ThisExpression?.Simplify(context);
        }

        internal class ValueExpr(object value, Typing.Type type) : Expression
        {
            public object Value = value;
            public Typing.Type Type = type;

            public override bool CanSimplifyToValue(Context context) => true;

            public override bool IsRelative() => false;

            public override bool IsSameAs(Expression other, Context context) => other.CanSimplifyToValue(context)
                                                                                ? Value.Equals(((ValueExpr)other.Simplify(context)).Value)
                                                                                : false;
            public override Expression Simplify(Context context) => this;

            public override string ToString() => Value.ToString();
        }
    }
}
