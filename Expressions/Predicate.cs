using RefinementTypes2.Resolution;
using RefinementTypes2.StandardTyping;
using RefinementTypes2.Typing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Expressions
{
    internal class Predicate
    {
        public enum PredicateMatch
        {
            Self
        }

        public readonly string Name;
        public List<(PredicateMatch MatchType, Func<StandardRefinement, StandardRefinement, Context, bool> Contradiction)> Contradictions;

        public Predicate(string name, List<(PredicateMatch MatchType, Func<StandardRefinement, StandardRefinement, Context, bool> Contradiction)> contradictions)
        {
            Name = name;
            Contradictions = contradictions;
        }


        public override string ToString()
        {
            return Name;
        }

        public static bool ContradictionBetween(StandardRefinement thisRefinement, StandardRefinement other, Context context)
        {
            // check that left hand side of both are the same
            if (!thisRefinement.Left.IsSameAs(other.Left))
                return false;
            // find if thisRefinement supports contradiction with other refinement
            var contradictionFunc = GetContradictionChecker(thisRefinement.Predicate, other.Predicate);
            if(contradictionFunc == null)
                return false;
            // run the relevant contradiction checker
            return contradictionFunc(thisRefinement, other, context);
        }

        static Func<StandardRefinement, StandardRefinement, Context, bool>? GetContradictionChecker(Predicate thisPredicate, Predicate other)
        {
            var contradictions = thisPredicate.Contradictions;
            bool self = thisPredicate.Equals(other);
            if(self && contradictions.Any(contradiction => contradiction.MatchType == PredicateMatch.Self))
            {
                return contradictions.First(contradiction => contradiction.MatchType == PredicateMatch.Self).Contradiction;
            }
            return null;
        }

        public static Predicate Base = new Predicate("Base",
            [(PredicateMatch.Self, (r1, r2, context) => 
            // Base =><= Base
            {
                var leftType = r1.Right; // A
                var rightType = r2.Right; // B
                if(leftType is not Expression.TypeExpr leftTypeExpr || rightType is not Expression.TypeExpr rightTypeExpr)
                    throw new Exception("invalid base types");
                if(leftTypeExpr.Type is not NamedType leftNamedType || rightTypeExpr.Type is not NamedType rightNamedType)
                    throw new Exception("invalid base types");
                // x : A and x : B -> contradiction if A :/ B and B:/ A
                if(!r1.Inverted && !r2.Inverted)
                    return leftNamedType.WontBeSubtypeOf(rightNamedType) && rightNamedType.WontBeSubtypeOf(leftNamedType);
                // x : A and x :/ B -> contradiction if A : B
                if (!r1.Inverted && r2.Inverted)
                    return leftNamedType.WillBeSubtypeOf(rightNamedType);
                // x :/ A and x : B -> contradiction if B : A
                if (r1.Inverted && !r2.Inverted)
                    return rightNamedType.WillBeSubtypeOf(leftNamedType);
                // x :/ A and x :/ B -> no contradiction
                else
                    return false;
            })]);
    }
}
