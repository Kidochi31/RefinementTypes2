using RefinementTypes2.Typing;
using RefinementTypes2.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.StandardTyping
{
    internal static class TypeSimplifier
    {
        public static StandardType SimplifyType(Typing.Type type)
        {
            switch (type)
            {
                case NamedType namedType:
                    return new StandardType([[new Refinement.Standard(new RelativeExpression(), Predicate.Base, new Expression.TypeExpr(namedType))]]);

                case RefinedType refinedType:
                    return StandardType.And(SimplifyType(refinedType.BaseType), SimplifyRefinement(refinedType.Refinement));

                case OrType orType:
                    return orType.BaseTypes.Aggregate(StandardType.GetDefaultOrType(), (sop, orBase) => StandardType.Or(sop, SimplifyType(orBase)));

                case AndType andType:
                    return andType.BaseTypes.Aggregate(StandardType.GetDefaultAndType(), (sop, andBase) => StandardType.And(sop, SimplifyType(andBase)));
            }
            Console.WriteLine($"Invalid type: {type}!");
            return null;
        }

        static StandardType SimplifyRefinement(Refinement refinement)
        {
            switch (refinement)
            {
                case Refinement.Standard standard:
                    return new StandardType([[standard]]);
                case Refinement.LogicalBinary lbinary:
                    switch (lbinary.Operator)
                    {
                        case Refinement.LogicalBinaryOperator.AND:
                            return StandardType.And(SimplifyRefinement(lbinary.Left), SimplifyRefinement(lbinary.Right));
                        case Refinement.LogicalBinaryOperator.OR:
                            return StandardType.Or(SimplifyRefinement(lbinary.Left), SimplifyRefinement(lbinary.Right));

                        case Refinement.LogicalBinaryOperator.XOR:
                            // A xor B = A'B or AB'
                            StandardType A = SimplifyRefinement(lbinary.Left);
                            StandardType ANot = StandardType.Invert(A);
                            StandardType B = SimplifyRefinement(lbinary.Right);
                            StandardType BNot = StandardType.Invert(B);
                            return StandardType.Or(
                                StandardType.And(ANot, B),
                                StandardType.And(BNot, A));
                        default:
                            break;
                    }
                    break;
                case Refinement.LogicalNot lnot:
                    return StandardType.Invert(SimplifyRefinement(lnot.Refinement));
            }
            throw new Exception("Invalid refinement, cannot be converted to sum of products");
        }
    }
}