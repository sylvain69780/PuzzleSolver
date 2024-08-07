using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    internal static class Helpers
    {
        public static void Swap<T>(ref T x, ref T y) { var tmp = x; x = y; y = tmp; }
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection) => new HashSet<T>(collection);


    }
}
