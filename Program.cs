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
            SumOfProducts sop = new SumOfProducts([new RefinementProduct([new Refinement.Standard(new RelativeExpression(), new Predicate("predicate"),
                new Expression(), false)])]);
            Console.WriteLine(sop);
        }
    }
}
