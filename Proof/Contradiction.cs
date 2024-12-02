using RefinementTypes2.StandardTyping;
using RefinementTypes2.Typing;
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
            => a.Any(refinementA => b.Any(refinementB => RefinementAContradictsB(refinementA, refinementB, context)));

        public static bool RefinementAContradictsB(Refinement.Standard a, Refinement.Standard b, Context context)
        {
            return false;
        }
    }
}
