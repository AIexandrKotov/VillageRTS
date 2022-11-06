using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRTS
{
    public class RandChooser<T>
    {
        public Random Random { get; set; } = new Random();
        public RandChooser(params (T, double)[] items)
        {
            CachedItems = items;
            Optimize();
        }

        internal void Optimize()
        {
            var overallweight = CachedItems.Sum(x => x.Item2);
            CachedItems = Array.ConvertAll(CachedItems, x => (x.Item1, x.Item2 / overallweight));
        }

        private (T, double)[] CachedItems;

        public T GetItem()
        {
            var t = Random.NextDouble();
            var curl = 0d;
            var curr = 0d;
            for (var i = 0; i < CachedItems.Length; i++)
            {
                curr += CachedItems[i].Item2;
                if (t >= curl && t < curr) return CachedItems[i].Item1;
                curl += CachedItems[i].Item2;
            }
            return CachedItems[CachedItems.Length - 1].Item1;
        }
    }
}
