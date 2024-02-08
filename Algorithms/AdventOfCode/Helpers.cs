using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.AdventOfCode
{
    internal static class Helpers
    {
        public static bool TryDequeue<T>(this Queue<T> queue, out T value)
        {
            if (queue.Count > 0)
            {
                value = queue.Dequeue();
                return true;
            }
            value = default;
            return false;
        }

        public static bool TryPop<T>(this Stack<T> queue, out T value)
        {
            if (queue.Count > 0)
            {
                value = queue.Pop();
                return true;
            }
            value = default;
            return false;
        }
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection) => new HashSet<T>(collection);


    }
}
