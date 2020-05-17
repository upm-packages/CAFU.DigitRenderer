using System.Collections.Generic;
using System.Linq;

namespace CAFU.NumberRenderer.Internal
{
    internal static class InternalUtility
    {
        internal static IEnumerable<int> SplitDigits(int number)
        {
            if (number == 0)
            {
                return new[] { 0 };
            }

            var list = new List<int>();
            while (number > 0)
            {
                list.Add(number % 10);
                number /= 10;
            }

            return list;
        }

        internal static IEnumerable<int> FillEmpty(this IEnumerable<int> self, int count)
        {
            var array = self.ToArray();
            if (array.Length > count)
            {
                return array;
            }
            var fillCount = count - array.Length;
            return array.Concat(Enumerable.Repeat(-1, fillCount));
        }
    }
}