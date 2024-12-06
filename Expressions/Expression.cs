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

        public override abstract string ToString();

        public abstract Typing.Type GetExpressionType(Context context);

        internal class TypeExpr(Typing.Type type) : Expression
        {
            public Typing.Type Type = type;

            public override bool CanSimplifyToValue(Context context) => true;

            public override Typing.Type GetExpressionType(Context context) => NamedType.Type;

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

            public override Typing.Type GetExpressionType(Context context) => context.ThisExpression.GetExpressionType(context);

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

            public override Typing.Type GetExpressionType(Context context) => Type;

            public override bool IsSameAs(Expression other, Context context) => other.CanSimplifyToValue(context)
                                                                                ? Value.Equals(((ValueExpr)other.Simplify(context)).Value)
                                                                                : false;
            public override Expression Simplify(Context context) => this;

            public override string ToString() => Value.ToString();
        }

        internal class VariableRef(Variable variable) : Expression
        {
            public readonly Variable Variable = variable;
            public override bool CanSimplifyToValue(Context context) => false;

            public override Typing.Type GetExpressionType(Context context) => Variable.Type;

            public override bool IsRelative() => false;

            public override bool IsSameAs(Expression other, Context context) => other is VariableRef vRef && vRef.Variable.Equals(Variable);

            public override Expression Simplify(Context context) => this;

            public override string ToString() => Variable.ToString();
        }
    }
}
