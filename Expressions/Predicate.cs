using RefinementTypes2.Proof;
using RefinementTypes2.Resolution;
using RefinementTypes2.StandardTyping;
using RefinementTypes2.Typing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RefinementTypes2.Expressions
{
    internal class Predicate
    {
        public enum PredicateMatch
        {
            Group,
            Self,
            Any
        }

        public class PredicateGroup { }
        public static PredicateGroup TotalOrder = new();


        public readonly string Name;
        public PredicateGroup? Group;
        public List<(PredicateMatch MatchType, Func<StandardRefinement, StandardRefinement, Context, bool> Contradiction)> Contradictions;
        public List<(PredicateMatch MatchType, Func<StandardRefinement, StandardRefinement, Context, bool> Inference)> Inferences;
        public Func<Expression.ValueExpr, Expression.ValueExpr, Context, bool> ApplyPredicate;
        public Func<Expression.ValueExpr, Expression.ValueExpr, Context, bool> ApplyInversePredicate;

        public Predicate(string name
            , PredicateGroup? group
            , List<(PredicateMatch MatchType, Func<StandardRefinement, StandardRefinement, Context, bool> Contradiction)> contradictions
            , List<(PredicateMatch MatchType, Func<StandardRefinement, StandardRefinement, Context, bool> Contradiction)> inferences
            , Func<Expression.ValueExpr, Expression.ValueExpr, Context, bool> applyPredicate
            , Func<Expression.ValueExpr, Expression.ValueExpr, Context, bool> applyInversePredicate)
        {
            Name = name;
            Group = group;
            Contradictions = contradictions;
            Inferences = inferences;
            ApplyPredicate = applyPredicate;
            ApplyInversePredicate = applyInversePredicate;
        }


        public override string ToString()
        {
            return Name;
        }

        public static bool ContradictionBetween(StandardRefinement thisRefinement, StandardRefinement other, Context context)
        {
            // check that left hand side of both are the same
            if (!thisRefinement.Left.IsSameAs(other.Left, context))
                return false;
            // find if thisRefinement supports contradiction with other refinement
            var contradictionFunc = GetContradictionChecker(thisRefinement.Predicate, other.Predicate);
            if(contradictionFunc == null)
                return false;
            // run the relevant contradiction checker
            return contradictionFunc(thisRefinement, other, context);
        }

        public static bool CanInfer(StandardRefinement target, StandardRefinement from, Context context)
        {
            // find if from supports inference with target
            var inferenceFunc = GetInferenceChecker(from.Predicate, target.Predicate);
            if (inferenceFunc == null)
                return false;
            // run the relevant inference checker
            return inferenceFunc(from, target, context);
        }

        static Func<StandardRefinement, StandardRefinement, Context, bool>? GetContradictionChecker(Predicate thisPredicate, Predicate other)
        {
            var contradictions = thisPredicate.Contradictions;
            // Self first
            bool self = thisPredicate.Equals(other);
            bool group = thisPredicate.Group?.Equals(other.Group) ?? false;
            if(self && contradictions.Any(contradiction => contradiction.MatchType == PredicateMatch.Self))
                return contradictions.First(contradiction => contradiction.MatchType == PredicateMatch.Self).Contradiction;
            if (group && contradictions.Any(contradiction => contradiction.MatchType == PredicateMatch.Group))
                return contradictions.First(contradiction => contradiction.MatchType == PredicateMatch.Group).Contradiction;
            // Any last
            if (contradictions.Any(contradiction => contradiction.MatchType == PredicateMatch.Any))
                return contradictions.First(contradiction => contradiction.MatchType == PredicateMatch.Any).Contradiction;
            return null;
        }

        static Func<StandardRefinement, StandardRefinement, Context, bool>? GetInferenceChecker(Predicate thisPredicate, Predicate other)
        {
            var inferences = thisPredicate.Inferences;
            // Self first
            bool self = thisPredicate.Equals(other);
            bool group = thisPredicate.Group?.Equals(other.Group) ?? false;
            if (self && inferences.Any(inference => inference.MatchType == PredicateMatch.Self))
                return inferences.First(inference => inference.MatchType == PredicateMatch.Self).Inference;
            if (group && inferences.Any(inference => inference.MatchType == PredicateMatch.Group))
                return inferences.First(inference => inference.MatchType == PredicateMatch.Group).Inference;
            // Any last
            if (inferences.Any(inference => inference.MatchType == PredicateMatch.Any))
                return inferences.First(inference => inference.MatchType == PredicateMatch.Any).Inference;
            return null;
        }

        public static Predicate Base = new Predicate("Base", null,
            // Contradictions
            [(PredicateMatch.Self, (StandardRefinement r1, StandardRefinement r2, Context context) => 
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
            })]
        , // Inferences
        []
        , // Predicate
        (v1, v2, context) => false
        , // Inverse Predicate
        (v1, v2, context) => false);

        public static Predicate Equal = new Predicate("=", null,
            // Contradictions
            [(PredicateMatch.Self, (StandardRefinement r1, StandardRefinement r2, Context context) =>
            {
                if(r1.Left.IsRelative()){
                    var A = r1.Right;
                    var predicate = r2.Predicate;
                    var B = r2.Right;
                    // x = A contradicts x.predicate(B) if A.predicate(B) is false
                    if(!r1.Inverted){
                        var inverted = !r2.Inverted;
                        var refinementToProve = new StandardRefinement(A, predicate, B, inverted);
                        return Inference.CanInfer(refinementToProve, context);
                    }
                }
                return false;
            })]
        , // Inferences
        []
        , // Predicate
        (v1, v2, context) => v1.Value.Equals(v2.Value)
        , // Inverse Predicate
        (v1, v2, context) => !v1.Value.Equals(v2.Value));

        public static Predicate LessThan = new Predicate("<", TotalOrder,
            // Contradictions
            [(PredicateMatch.Group, (StandardRefinement r1, StandardRefinement r2, Context context) =>
            {
                var A = r1.Right;
                var B = r2.Right;
                if(r1.Inverted)
                {
                    // x >= A
                    // Can contradict with x < B or x <= B
                    if (r2.Predicate.Equals(GreaterThan) && r2.Inverted){
                        // x <= B
                        // contradicts if A > B
                        var refinementToProve = new StandardRefinement(A, GreaterThan, B, false);
                        return Inference.CanInfer(refinementToProve, context);
                    }
                    else if (r2.Predicate.Equals(LessThan) && !r2.Inverted)
                    {
                        // x < B
                        // contradicts if A >= B (which is !(A < B))
                        var refinementToProve = new StandardRefinement(A, LessThan, B, true);
                        return Inference.CanInfer(refinementToProve, context);
                    }
                }
                else
                {
                    // x < A
                    // Can contradict with x > B or x >= B
                    if (r2.Predicate.Equals(LessThan) && r2.Inverted){
                        // x >= B
                        // contradicts if A <= B (which is !(A > B))
                        var refinementToProve = new StandardRefinement(A, GreaterThan, B, true);
                        return Inference.CanInfer(refinementToProve, context);
                    }
                    else if (r2.Predicate.Equals(GreaterThan) && !r2.Inverted)
                    {
                        // x > B
                        // contradicts if A <= B (which is !(A > B))
                        var refinementToProve = new StandardRefinement(A, GreaterThan, B, true);
                        return Inference.CanInfer(refinementToProve, context);
                    }
                }
                return false;
            })]
        , // Inferences
        []
        , // Predicate
        (v1, v2, context) => Convert.ToDouble(v1.Value) < Convert.ToDouble(v2.Value)
        , // Inverse Predicate
        (v1, v2, context) => !(Convert.ToDouble(v1.Value) < Convert.ToDouble(v2.Value)));

        public static Predicate GreaterThan = new Predicate(">", TotalOrder,
            // Contradictions
            [(PredicateMatch.Group, (StandardRefinement r1, StandardRefinement r2, Context context) =>
            {
                var A = r1.Right;
                var B = r2.Right;
                if(r1.Inverted)
                {
                    
                    // x <= A
                    // Can contradict with x > B or x >= B
                    if (r2.Predicate.Equals(LessThan) && r2.Inverted){
                        // x >= B
                        // contradicts if A < B
                        var refinementToProve = new StandardRefinement(A, LessThan, B, false);
                        return Inference.CanInfer(refinementToProve, context);
                    }
                    else if (r2.Predicate.Equals(GreaterThan) && !r2.Inverted)
                    {
                        // x > B
                        // contradicts if A <= B (which is !(A > B))
                        var refinementToProve = new StandardRefinement(A, GreaterThan, B, true);
                        return Inference.CanInfer(refinementToProve, context);
                    }
                }
                else
                {
                    // x > A
                    // Can contradict with x < B or x <= B
                    if (r2.Predicate.Equals(GreaterThan) && r2.Inverted){
                        // x <= B
                        // contradicts if A >= B (which is !(A < B))
                        var refinementToProve = new StandardRefinement(A, LessThan, B, true);
                        
                        return Inference.CanInfer(refinementToProve, context);
                    }
                    else if (r2.Predicate.Equals(LessThan) && !r2.Inverted)
                    {
                        // x < B
                        // contradicts if A >= B (which is !(A < B))
                        var refinementToProve = new StandardRefinement(A, LessThan, B, true);
                        return Inference.CanInfer(refinementToProve, context);
                    }
                }
                return false;
            })]
        , // Inferences
        []
        , // Predicate
        (v1, v2, context) => Convert.ToDouble(v1.Value) > Convert.ToDouble(v2.Value)
        , // Inverse Predicate
        (v1, v2, context) => !(Convert.ToDouble(v1.Value) > Convert.ToDouble(v2.Value)));
    }
}
