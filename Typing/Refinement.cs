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

        internal class Expression() : Refinement
        {

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
    }
}
