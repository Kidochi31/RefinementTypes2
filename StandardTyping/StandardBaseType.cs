using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.StandardTyping
{
    internal class StandardBaseType
    {
        public List<StandardRefinement> Refinements;

        public StandardBaseType(List<StandardRefinement> refinements)
        {
            Refinements = refinements;
        }
    }
}
