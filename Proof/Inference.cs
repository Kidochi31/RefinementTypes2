using RefinementTypes2.Expressions;
using RefinementTypes2.Resolution;
using RefinementTypes2.StandardTyping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Proof
{
    internal class Inference
    {
        public static bool CanInfer(StandardRefinement refinement, Context context)
        {
            if (context.ThisProduct is null)
                throw new Exception("Cannot infer without a valid \"ThisProduct\" in context.");
            Expression left = refinement.Left.Simplify(context);
            Expression right = refinement.Right.Simplify(context);
            if (left is Expression.ValueExpr leftV && right is Expression.ValueExpr rightV)
                return Predicate.Apply(refinement.Predicate, leftV, rightV, context, refinement.Inverted);

            if(Predicate.ApplyToExpression(refinement.Predicate, left, right, context, refinement.Inverted))
            {
                return true;
            }

            //Otherwise, need to try to infer the refinement
            //A -> B if and only if not A or B is true
            //This means A and not B must be false (a contradiction)
            //A -> B if and only if A contradicts not B (Tres importante)

            // In this case, the left expression, right expression, or both are not simple values
            // to infer A.Predicate(B) through A, must prove contradiction in typeof(A) and (not this.Predicate(B))
            if (left is not Expression.ValueExpr)
            {
                StandardType leftType = TypeSimplifier.SimplifyType(left.GetExpressionType(context));
                context = context.AddThisExpression(left);
                bool inverted = !refinement.Inverted;
                StandardRefinement adjustedRefinement = new StandardRefinement(new Expression.This(), refinement.Predicate, refinement.Right, inverted);
                
                return leftType.All(product => Contradiction.RefinementProductAContradictsB(product, new RefinementProduct([adjustedRefinement]), context));
            }
            return false;
        }
    }
}
