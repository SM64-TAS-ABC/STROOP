using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int IndexOfMin<T>(this IEnumerable<T> source) where T : IComparable
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var enumer = source.GetEnumerator();

            if (!enumer.MoveNext())
                throw new InvalidOperationException("Sequence was empty");

            T minValue = enumer.Current;
            int minIndex = 0;

            while (enumer.MoveNext())
            {
                if (enumer.Current.CompareTo(minValue) < 0)
                {
                    minValue = enumer.Current;
                }
            }

            return minIndex;
        }
    }
}
