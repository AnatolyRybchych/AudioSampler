using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSampler.Extensions
{
    internal static class Enumerable
    {
        public static IEnumerable<T> ConcatTogether<T>(this IEnumerable<IEnumerable<T>> complexEnumeration)
        {
            foreach (var enumeration in complexEnumeration)
                foreach (var item in enumeration)
                    yield return item;
        }
    }
}
