using RefinementTypes2.StandardTyping;
using RefinementTypes2.Typing;
using RefinementTypes2.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefinementTypes2.Proof;

namespace RefinementTypes2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Test for AND, OR, NOT of refinements 
            //StandardRefinement A = new StandardRefinement(new RelativeExpression(), new Predicate("A"), new Expression());
            //StandardRefinement B = new StandardRefinement(new RelativeExpression(), new Predicate("B"), new Expression());
            //StandardRefinement C = new StandardRefinement(new RelativeExpression(), new Predicate("C"), new Expression(), true);
            
            //StandardRefinement D = new StandardRefinement(new RelativeExpression(), new Predicate("D"), new Expression());
            //StandardRefinement E = new StandardRefinement(new RelativeExpression(), new Predicate("E"), new Expression());
            //StandardRefinement F = new StandardRefinement(new RelativeExpression(), new Predicate("F"), new Expression(), true);

            //StandardType sop1 = new StandardType([[A, B, C]]);
            //StandardType sop2 = new StandardType([[D, E, F]]);
            //Console.WriteLine(sop1);
            //Console.WriteLine(StandardType.Invert(sop1));
            //Console.WriteLine(sop2);
            //Console.WriteLine(StandardType.Invert(sop2));
            //Console.WriteLine(StandardType.And(sop1, sop2));
            //Console.WriteLine(StandardType.Or(sop1, sop2));
            //Console.WriteLine(StandardType.Or(sop1, StandardType.Invert(sop2)));
            //Console.WriteLine(StandardType.And(sop1, StandardType.Invert(sop2)));
            //Console.WriteLine(StandardType.Or(sop1, StandardType.Invert(sop1)));
            //Console.WriteLine(StandardType.And(sop1, StandardType.Invert(sop1)));

            // Test for type simplification of Named Types, And, Or Types
            //Typing.Type A = new NamedType("A");
            //StandardType AStd = TypeSimplifier.SimplifyType(A);
            //Typing.Type B = new NamedType("B");
            //StandardType BStd = TypeSimplifier.SimplifyType(B);
            //Typing.Type C = new NamedType("C");
            //StandardType CStd = TypeSimplifier.SimplifyType(C);
            //var AandB = new AndType([A, B]);
            //StandardType AandBStd = TypeSimplifier.SimplifyType(AandB);
            //var BorC = new OrType([B, C]);
            //StandardType BorCStd = TypeSimplifier.SimplifyType(BorC);

            //Console.WriteLine(A);
            //Console.WriteLine(B);
            //Console.WriteLine(C);
            //Console.WriteLine(TypeSimplifier.SimplifyType(A));
            //Console.WriteLine(TypeSimplifier.SimplifyType(B));
            //Console.WriteLine(TypeSimplifier.SimplifyType(C));
            //Console.WriteLine(AandB);
            //Console.WriteLine(TypeSimplifier.SimplifyType(AandB));
            //Console.WriteLine(BorC);
            //Console.WriteLine(TypeSimplifier.SimplifyType(BorC));
            //Console.WriteLine(TypeSimplifier.SimplifyType(new AndType([AandB, BorC])));
            //Console.WriteLine(TypeSimplifier.SimplifyType(new OrType([AandB, BorC])));

            //Typing.Type A2 = new NamedType("A2", A);
            //StandardType A2Std = TypeSimplifier.SimplifyType(A2);
            //Typing.Type AandB2 = new NamedType("AandB2", AandB);
            //StandardType AandB2Std = TypeSimplifier.SimplifyType(AandB2);
            //Typing.Type BorC2 = new NamedType("BorC2", BorC);
            //StandardType BorC2Std = TypeSimplifier.SimplifyType(BorC2);

            //Console.WriteLine(A2);
            //Console.WriteLine(A2Std);
            //Console.WriteLine(AandBStd);
            //Console.WriteLine(AandB2);
            //Console.WriteLine(AandB2Std);
            //Console.WriteLine(BorC2);
            //Console.WriteLine(BorC2Std);

            //Console.WriteLine($"A2 -> A {Imposition.ImposeBOnA(A2Std, AStd)}");
            //Console.WriteLine($"A -> A2 {Imposition.ImposeBOnA(AStd, A2Std)}");
            //Console.WriteLine($"Failure: {Imposition.FindImpositionFailureBOnA(AStd, A2Std)}");
            //Console.WriteLine($"A -> A and B {Imposition.ImposeBOnA(AStd, AandBStd)}");
            //Console.WriteLine($"A -> B {Imposition.ImposeBOnA(AStd, BStd)}");
            //Console.WriteLine($"Failure: {Imposition.FindImpositionFailureBOnA(AStd, BStd)}");
            //Console.WriteLine($"B -> B or C {Imposition.ImposeBOnA(BStd, BorCStd)}");
            //Console.WriteLine($"AandB2 -> A and B {Imposition.ImposeBOnA(AandB2Std, AandBStd)}");
            //Console.WriteLine($"BorC2 -> B or C {Imposition.ImposeBOnA(BorC2Std, BorCStd)}");
            //Console.WriteLine($"A -> A {Imposition.ImposeBOnA(AStd, AStd)}");

            Expression.ValueExpr nineA = new Expression.ValueExpr(9, NamedType.Number);
            Expression.ValueExpr nineB = new Expression.ValueExpr(9, NamedType.Number);
            Expression.ValueExpr eight = new Expression.ValueExpr(8, NamedType.Number);
            Typing.Type NineA = new RefinedType(NamedType.Number, new Refinement.Absolute(new Expression.This(), Predicate.Equal, nineA));
            Typing.Type NineB = new RefinedType(NamedType.Number, new Refinement.Absolute(new Expression.This(), Predicate.Equal, nineB));
            Typing.Type Eight = new RefinedType(NamedType.Number, new Refinement.Absolute(new Expression.This(), Predicate.Equal, eight));
            StandardType NineAStd = TypeSimplifier.SimplifyType(NineA);
            StandardType NineBStd = TypeSimplifier.SimplifyType(NineB);
            StandardType EightStd = TypeSimplifier.SimplifyType(Eight);
            Console.WriteLine(NineAStd);
            Console.WriteLine($"NineA -> NineB : {Imposition.ImposeBOnA(NineAStd, NineBStd)}");
            Console.WriteLine(EightStd);
            Console.WriteLine($"NineA -> Eight : {Imposition.ImposeBOnA(NineAStd, EightStd)}");
            Console.WriteLine($"Failure : {Imposition.FindImpositionFailureBOnA(NineAStd, EightStd)}");
            //Console.WriteLine($"Failure : {Imposition.FindImpositionFailureBOnA(NineAStd, NineBStd)}");
        }
    }
}
