﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.AdventOfCode.Y2023.Day25
{
    public class SnowverloadInput
    {
        public List<string> Nodes { get; set; }
        public List<(string a,string b)> Edges { get; set; }
    }
    public class Snowverload : SolutionBase<SnowverloadInput>
    {
        protected override SnowverloadInput Parse(string input)
        {
            var lines = input.Split('\n');
            var nodes = new List<string>();
            var edges = new List<(string a, string b)>();
            foreach (var line in lines)
            {
                var rec = line.Split(new string[] { ": " },StringSplitOptions.None);
                var node = rec[0];
                var targets = rec[1].Split(' ');
                nodes.Add(node);
                foreach (var node2 in targets)
                {
                    edges.Add((node, node2));
                }
            }
            return new SnowverloadInput { Nodes = nodes, Edges = edges, };
        }

        [SolutionMethod("Part 1")]
        public static IEnumerable<string> PartOne(SnowverloadInput input)
        {
            var neighbours = input.Edges.GroupBy(x => x.a).ToDictionary(x => x.Key, x => x.Select(y => y.b).ToArray());
            var node = input.Nodes[0];
            var cut = new HashSet<(string a,string b)>();
            var group1 = new List<string>();
            var group2 = new List<string>();

            // Karger's Algorithm
            var found = false;
            var rand = new Random();
            var result = 0;
            while (!found)
            {
                var nodes = new List<string>(input.Nodes);
                var edges = new List<(string a, string b)>(input.Edges);
                while (nodes.Count >2)
                {
                    var index = rand.Next(edges.Count);
                    var (u, v) = edges[index];
                    var newNode = u + '-' + v;
                    nodes.Add(newNode);
                    for ( var i = 0; i<edges.Count; i++) 
                    { 
                        var (a, b) = edges[i];
                        if (a == u || a == v)
                            edges[i] = (newNode, edges[i].b);
                        if (b == u || b == v)
                            edges[i] = (edges[i].a,newNode);
                    }
                    nodes.Remove(u);
                    nodes.Remove(v);
                    edges = edges.Where(x => x.a != x.b).ToList();
                }
                found = edges.Count ==3;
                if (found)
                    result = (nodes[0].Count(c => c == '-')+1) * (nodes[1].Count(c => c == '-') + 1);
            }
            yield return result.ToString();
        }
    }
}
