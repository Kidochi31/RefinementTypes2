using RefinementTypes2.Expressions;
using RefinementTypes2.Resolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RefinementTypes2.Typing.Refinement;

namespace RefinementTypes2.StandardTyping
{
    internal class StandardRefinement(Expression left, Predicate predicate, Expression right, bool inverted = false)
    {
        public bool Inverted = inverted;
        public Expression Left = left;
        public Predicate Predicate = predicate;
        public Expression Right = right;

        public StandardRefinement Invert()
        {
            return new(Left, Predicate, Right, !Inverted);
        }

        public bool Contradicts(StandardRefinement other, Context context)
        {
            return Predicate.ContradictionBetween(this, other, context);
        }

        public override string ToString()
        {
            //return $"{(Inverted ? "not " : "")}{Predicate} ({Left}, {Right})";
            return $"{(Inverted ? "not " : "")}{Predicate} {Right}";
            //return $"{(Inverted ? "not " : "")}{Predicate}";
        }
    }
}
