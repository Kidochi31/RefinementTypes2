using RefinementTypes2.Typing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.StandardTyping
{
    internal class RefinementProduct (IEnumerable<Refinement.Standard> refinements) : IEnumerable<Refinement.Standard>
    {
        public List<Refinement.Standard> Refinements = refinements.ToList();

        public StandardType Invert()
        {
            return new StandardType(from refinement in Refinements select new List<Refinement.Standard>() { refinement.Invert() });
        }

        public IEnumerator<Refinement.Standard> GetEnumerator() => Refinements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Refinements.GetEnumerator();

        public override string ToString()
        {
            return "Any" + Refinements.Aggregate("", (s, r) => s + $"[{r}]");
        }
    }
}
