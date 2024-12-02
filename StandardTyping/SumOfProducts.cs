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
    internal class SumOfProducts : IEnumerable<RefinementProduct>
    {
        public List<RefinementProduct> Products;

        public SumOfProducts()
        {
            Products = [new RefinementProduct([])];
        }

        public SumOfProducts(List<RefinementProduct> bases)
        {
            Products = bases;
        }

        public SumOfProducts(IEnumerable<List<Refinement.Standard>> bases)
        {
            Products = (from product in bases select new RefinementProduct(product)).ToList();
        }

        public List<List<Refinement.Standard>> GetSumOfProductsRefinements() => (from product in Products select product.Refinements).ToList();
        public static SumOfProducts Or(SumOfProducts a, SumOfProducts b) => new SumOfProducts([.. a, .. b]);

        public static SumOfProducts And(SumOfProducts a, SumOfProducts b)
            => new SumOfProducts(a.Select(productA => b.Select(productB => new RefinementProduct([.. productA, .. productB]))).SelectMany(i => i).ToList());

        public static SumOfProducts Invert(SumOfProducts a)
            => a.Aggregate(new SumOfProducts(), (sop, p) => And(sop, p.Invert()));


        public IEnumerator<RefinementProduct> GetEnumerator() => Products.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Products.GetEnumerator();

        public override string ToString()
        {
            return string.Join(" | ", Products);
        }
    }
}
