using System.Linq;

namespace Algorithms.AdventOfCode.Y2023.Day21
{
    public static class Parser
    {
        public  static Input Parse(string input)
        {
            var map = input.Split('\n');
            var pos = map.Select((s, y) => (s, y)).Where(l => l.s.Contains('S')).Select(l => (x: l.s.IndexOf('S'), l.y)).Single();
            return new Input
            {
                Map = map,
                Pos = pos
            };
        }
    }
}
