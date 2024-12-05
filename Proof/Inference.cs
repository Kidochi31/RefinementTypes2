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

            // if the refinement to infer is relative
            if (refinement.Left.IsRelative())
                return context.ThisProduct.Any(from => Predicate.CanInfer(refinement, from, context));
            return false;
        }
    }
}
