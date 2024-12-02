using RefinementTypes2.StandardTyping;
using RefinementTypes2.Typing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Proof
{
    internal static class Imposition
    {
        public static bool ImposeBOnA(StandardType a, StandardType b)
        {
            // A fits into B iff A U (not B) = 0
            StandardType notB = StandardType.Invert(b);

            Context context = new Context();

            if (Contradiction.TypeSelfContradicts(b, context))
                return false;

            // Here, if B = (a b) | (c d), then (not B) = (a' | b') & (c' | d') = a'c' | a'd' | b'c' | b'd'
            // It must be shown that A & (not B) is a contradiction on all of these, AND on all cases of A
            // So, algorithm is to go through all products of A, and all products of inverted B, and check for contradiction.
            return a.All(sopA => notB.All(sopB => Contradiction.RefinementProductAContradictsB(sopA, sopB, context)));
        }
    }
}
