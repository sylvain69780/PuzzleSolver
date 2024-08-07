using System.Collections.Generic;
using System.Linq;

namespace Algorithms.AdventOfCode.Y2023.Day23
{
    public static class StackExtension
    {
        public static Stack<T> Clone<T>(this Stack<T> source)
        {
            return new Stack<T>(source.Reverse());
        }
    }
}
