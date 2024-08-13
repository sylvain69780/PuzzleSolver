using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    internal static class Helpers
    {
        public static void Swap<T>(ref T x, ref T y) { var tmp = x; x = y; y = tmp; }
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection) => new HashSet<T>(collection);

        public static bool TryPop<T>(this Stack<T> stack, out T item)
        {
            if (stack.Count > 0)
            {
                item = stack.Pop();
                return true;
            }
            else
            {
                item = default;
                return false;
            }
        }

    }
}
