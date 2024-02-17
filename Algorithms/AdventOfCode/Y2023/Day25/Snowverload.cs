using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.AdventOfCode.Y2023.Day25
{
    public class SnowverloadInput
    {
        public List<string> Nodes { get; set; }
        public List<(string a,string b)> Joins { get; set; }
    }
    public class Snowverload : SolutionBase<SnowverloadInput>
    {
        protected override SnowverloadInput Parse(string input)
        {
            var lines = input.Split('\n');
            var nodes = new List<string>();
            var joins = new List<(string a, string b)>();
            foreach (var line in lines)
            {
                var rec = line.Split(new string[] { ": " },StringSplitOptions.None);
                var node = rec[0];
                var targets = rec[1].Split(' ');
                nodes.Add(node);
                foreach (var node2 in targets)
                {
                    joins.Add((node, node2));
                    if (!joins.Contains((node2,node)))
                        joins.Add((node2, node));
                }

            }
            return new SnowverloadInput { Nodes = nodes, Joins = joins, };
        }

        [SolutionMethod("Part 1")]
        public static IEnumerable<string> PartOne(SnowverloadInput input)
        {
            yield return string.Empty;
        }
    }
}
