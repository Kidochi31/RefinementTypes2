using RefinementTypes2.Resolution;
using RefinementTypes2.StandardTyping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Proof
{
    internal static class Contradiction
    {
        public static bool TypeSelfContradicts(StandardType type, Context context)
            => type.All(product => RefinementProductAContradictsB(product, product, context));


        public static bool RefinementProductAContradictsB(RefinementProduct a, RefinementProduct b, Context context)
        {
            context = context.AddThisProduct(a.And(b));
            return a.Any(refinementA => b.Any(refinementB => RefinementAContradictsB(refinementA, refinementB, context)));
        }

        public static bool RefinementAContradictsB(StandardRefinement a, StandardRefinement b, Context context)
        {
            //Console.WriteLine($"Testing {a} and {b}");
            return a.Contradicts(b, context);
        }
    }
}
