using RefinementTypes2.Typing;
using RefinementTypes2.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static RefinementTypes2.Typing.Refinement;

namespace RefinementTypes2.StandardTyping
{
    internal static class TypeSimplifier
    {
        public static StandardType SimplifyType(Typing.Type type)
        {
            switch (type)
            {
                case NamedType namedType:
                    if (namedType.Equals(NamedType.Any))
                        return StandardType.GetDefaultAndType();

                    StandardRefinement typeRefinement = new StandardRefinement(new Expression.This(), Predicate.Base, new Expression.TypeExpr(namedType));
                    return StandardType.And(SimplifyType(namedType.BaseType), [typeRefinement]);

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
                case Refinement.Relative relative:
                    return new StandardType([[new StandardRefinement(relative.Left.Substitute(new Expression.This()), relative.Predicate, relative.Right, relative.Inverted)]]);
                case Refinement.Absolute absolute:
                    return new StandardType([[new StandardRefinement(absolute.Left, absolute.Predicate, absolute.Right, absolute.Inverted)]]);
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