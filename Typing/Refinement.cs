using RefinementTypes2.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.Typing
{
    internal abstract class Refinement
    {
        internal enum LogicalBinaryOperator
        {
            AND,
            OR,
            XOR
        }

        internal class LogicalNot(Refinement refinement) : Refinement
        {
            public Refinement Refinement = refinement;
        }

        internal class LogicalBinary(Refinement left, LogicalBinaryOperator op, Refinement right) : Refinement
        {
            public Refinement Left = left;
            public LogicalBinaryOperator Operator = op;
            public Refinement Right = right;
        }

        internal class Absolute(Expression left, Predicate predicate, Expression right, bool inverted = false) : Refinement
        {
            public bool Inverted = inverted;
            public Expression Left = left;
            public Predicate Predicate = predicate;
            public Expression Right = right;

            public Absolute Invert()
            {
                return new(Left, Predicate, Right, !Inverted);
            }

            public override string ToString()
            {
                //return $"{(Inverted ? "not " : "")}{Predicate} ({Left}, {Right})";
                return $"{(Inverted ? "not " : "")}{Predicate} {Right}";
                //return $"{(Inverted ? "not " : "")}{Predicate}";
            }
        }

        internal class Relative(RelativeExpression left, Predicate predicate, Expression right, bool inverted = false) : Refinement
        {
            public bool Inverted = inverted;
            public RelativeExpression Left = left;
            public Predicate Predicate = predicate;
            public Expression Right = right;

            public Relative Invert()
            {
                return new(Left, Predicate, Right, !Inverted);
            }

            public override string ToString()
            {
                //return $"{(Inverted ? "not " : "")}{Predicate} ({Left}, {Right})";
                return $"{(Inverted ? "not " : "")}{Predicate} {Right}";
                //return $"{(Inverted ? "not " : "")}{Predicate}";
            }
        }
    }
}
