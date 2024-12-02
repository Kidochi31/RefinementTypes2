using RefinementTypes2.Typing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefinementTypes2.StandardTyping
{
    internal class StandardType : IEnumerable<RefinementProduct>
    {
        public List<RefinementProduct> Products;

        public static StandardType GetDefaultOrType() => new StandardType(new List<RefinementProduct>());

        public static StandardType GetDefaultAndType() => new StandardType([[]]);

        public StandardType(List<RefinementProduct> bases)
        {
            Products = bases;
        }

        public StandardType(IEnumerable<List<Refinement.Standard>> bases)
        {
            Products = (from product in bases select new RefinementProduct(product)).ToList();
        }

        public List<List<Refinement.Standard>> GetSumOfProductsRefinements() => (from product in Products select product.Refinements).ToList();
        public static StandardType Or(StandardType a, StandardType b) => new StandardType([.. a, .. b]);

        public static StandardType Or(StandardType a, IEnumerable<Refinement.Standard> refinements)
            => new StandardType([..a, new RefinementProduct(refinements)]);

        public static StandardType And(StandardType a, StandardType b)
            => new StandardType(a.Select(productA => b.Select(productB => new RefinementProduct([.. productA, .. productB]))).SelectMany(i => i).ToList());

        public static StandardType And(StandardType a, IEnumerable<Refinement.Standard> refinements)
            => new StandardType(a.Select(productA => new RefinementProduct([.. productA, .. refinements])).ToList());

        public static StandardType Invert(StandardType a)
            => a.Aggregate(GetDefaultAndType(), (sop, p) => And(sop, p.Invert()));


        public IEnumerator<RefinementProduct> GetEnumerator() => Products.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Products.GetEnumerator();

        public override string ToString()
        {
            return string.Join(" | ", Products);
        }
    }
}
