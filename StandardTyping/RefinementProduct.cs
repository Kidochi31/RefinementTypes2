using RefinementTypes2.Typing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.StandardTyping
{
    internal class RefinementProduct (IEnumerable<StandardRefinement> refinements) : IEnumerable<StandardRefinement>
    {
        public List<StandardRefinement> Refinements = refinements.ToList();

        public StandardType Invert()
        {
            return new StandardType(from refinement in Refinements select new List<StandardRefinement>() { refinement.Invert() });
        }

        public RefinementProduct And(RefinementProduct other)
        {
            return new([.. Refinements, .. other]);
        }

        public IEnumerator<StandardRefinement> GetEnumerator() => Refinements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Refinements.GetEnumerator();

        public override string ToString()
        {
            return "Any" + Refinements.Aggregate("", (s, r) => s + $"[{r}]");
        }
    }
}
