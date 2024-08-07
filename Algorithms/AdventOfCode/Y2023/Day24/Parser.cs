using System;
using System.Linq;

namespace Algorithms.AdventOfCode.Y2023.Day24
{
    public static class Parser
    {

        public static Input Parse(string input)
        {
            (long x, long y, long z) ParsePoint(string str)
            {
                var r = str.Split(new string[] { ", " }, StringSplitOptions.None).Select(x => long.Parse(x)).ToArray();
                return (x: r[0], y: r[1], z: r[2]);
            }
            var lines = input.Split('\n');
            var value = lines
                .Select(l => l.Split(new string[] { " @ " }, StringSplitOptions.None))
                .Select(l => (ParsePoint(l[0]), ParsePoint(l[1])))
                .ToArray();

            return new Input()
            {
                Hailstones = value
            };
        }
    }
}
