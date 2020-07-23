using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funky.Durables.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<List<T>> SplitList<T>(this IEnumerable<T> locations, int nSize = 100)
        {
            var items = locations?.ToList() ?? new List<T>();
            if (!items.Any())
            {
                yield return null;
            }
            for (var i = 0; i < items.Count; i += nSize)
            {
                yield return items.GetRange(i, Math.Min(nSize, items.Count - i));
            }
        }
    }
}
