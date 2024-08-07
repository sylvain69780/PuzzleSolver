using System;
using System.Collections.Generic;

namespace Algorithms.AdventOfCode.Y2023.Day25
{
    public static class Parser
{
        public static Input Parse(string input)
        {
            var lines = input.Split('\n');
            var nodes = new List<string>();
            var edges = new List<(string a, string b)>();
            foreach (var line in lines)
            {
                var rec = line.Split(new string[] { ": " }, StringSplitOptions.None);
                var node = rec[0];
                var targets = rec[1].Split(' ');
                nodes.Add(node);
                foreach (var node2 in targets)
                {
                    edges.Add((node, node2));
                }
            }
            return new Input { Nodes = nodes, Edges = edges, };
        }

    }
}
