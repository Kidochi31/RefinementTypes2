using RefinementTypes2.StandardTyping;
using RefinementTypes2.Typing;
using RefinementTypes2.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            /* // Test for AND, OR, NOT of refinements 
            Refinement.Standard A = new Refinement.Standard(new RelativeExpression(), new Predicate("A"), new Expression());
            Refinement.Standard B = new Refinement.Standard(new RelativeExpression(), new Predicate("B"), new Expression());
            Refinement.Standard C = new Refinement.Standard(new RelativeExpression(), new Predicate("C"), new Expression(), true);
            
            Refinement.Standard D = new Refinement.Standard(new RelativeExpression(), new Predicate("D"), new Expression());
            Refinement.Standard E = new Refinement.Standard(new RelativeExpression(), new Predicate("E"), new Expression());
            Refinement.Standard F = new Refinement.Standard(new RelativeExpression(), new Predicate("F"), new Expression(), true);

            StandardType sop1 = new StandardType([[A, B, C]]);
            StandardType sop2 = new StandardType([[D, E, F]]);
            Console.WriteLine(sop1);
            Console.WriteLine(StandardType.Invert(sop1));
            Console.WriteLine(sop2);
            Console.WriteLine(StandardType.Invert(sop2));
            Console.WriteLine(StandardType.And(sop1, sop2));
            Console.WriteLine(StandardType.Or(sop1, sop2));
            Console.WriteLine(StandardType.Or(sop1, StandardType.Invert(sop2)));
            Console.WriteLine(StandardType.And(sop1, StandardType.Invert(sop2)));
            Console.WriteLine(StandardType.Or(sop1, StandardType.Invert(sop1)));
            Console.WriteLine(StandardType.And(sop1, StandardType.Invert(sop1)));
            */

            // Test for type simplification of Named Types, And, Or Types
            Typing.Type A = new NamedType("A");
            Typing.Type B = new NamedType("B");
            Typing.Type C = new NamedType("C");
            var AandB = new AndType([A, B]);
            var BorC = new OrType([B, C]);

            Console.WriteLine(A);
            Console.WriteLine(B);
            Console.WriteLine(C);
            Console.WriteLine(TypeSimplifier.SimplifyType(A));
            Console.WriteLine(TypeSimplifier.SimplifyType(B));
            Console.WriteLine(TypeSimplifier.SimplifyType(C));
            Console.WriteLine(AandB);
            Console.WriteLine(TypeSimplifier.SimplifyType(AandB));
            Console.WriteLine(BorC);
            Console.WriteLine(TypeSimplifier.SimplifyType(BorC));
            Console.WriteLine(TypeSimplifier.SimplifyType(new AndType([AandB, BorC])));
            Console.WriteLine(TypeSimplifier.SimplifyType(new OrType([AandB, BorC])));

        }
    }
}
