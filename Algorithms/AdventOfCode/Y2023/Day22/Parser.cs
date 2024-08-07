using System.Linq;
using System.Text.RegularExpressions;

namespace Algorithms.AdventOfCode.Y2023.Day22
{
    public static class Parser
    {
        public static Input Parse(string input)
        {
            return new Input()
            {
                Bricks = input.Split('\n').Select(x => Regex.Match(x, @"(\d+),(\d+),(\d+)~(\d+),(\d+),(\d+)"))
                .Select(x => (start: (x: long.Parse(x.Groups[1].Value), y: long.Parse(x.Groups[2].Value), z: long.Parse(x.Groups[3].Value)),
                                end: (x: long.Parse(x.Groups[4].Value), y: long.Parse(x.Groups[5].Value), z: long.Parse(x.Groups[6].Value)))).ToList()
            };
        }
    }
}
