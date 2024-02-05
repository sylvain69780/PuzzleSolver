using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.AdventOfCode.Y2023.Day23
{
    public static class StackExtension
    {
        public static Stack<T> Clone<T>(this Stack<T> source)
        {
            return new Stack<T>(source.Reverse());
        }
    }
    public class ALongWalk : SolutionBase<ALongWalkDataModel>, ISolution
    {
        protected override ALongWalkDataModel Parse(string input)
        {
            return new ALongWalkDataModel
            {
                Map = input.Split("\n")
            };
        }
        public ALongWalk()
        {
            _parts.Add("Part 1", PartOne);
            _parts.Add("Part 2", PartTwo);
        }

        static (int x, int y)[] directions = [(1, 0), (-1, 0), (0, 1), (0, -1)];

        static readonly char[] TilesOk = ['.', '>', '<', '^', 'v'];

        static bool IsInpossible(char tile, (int x, int y) dir)
        {
            return
                (tile == '>' && dir == (-1, 0)) ||
                (tile == '<' && dir == (1, 0)) ||
                (tile == '^' && dir == (0, 1)) ||
                (tile == 'v' && dir == (0, -1))
                ;
        }

        char Tile(string[] map, (int x, int y) pos)
        {
            if (pos.x < 0 || pos.x >= map[0].Length || pos.y < 0 || pos.y >= map.Length)
                return '#';
            return map[pos.y][pos.x];
        }

        public IEnumerable<string> PartOne(ALongWalkDataModel model)
        {
            var map = model.Map!;
            var start = (x: 1, y: 0);
            var exit = (x: map[0].Length - 2, y: map.Length - 1);
            var path = new Stack<(int x, int y)>();
            path.Push(start);
            var bfs = new Queue<Stack<(int x, int y)>>();
            bfs.Enqueue(path);
            var results = new List<int>();
            var best = new List<(int x, int y)>();
            while (bfs.Count > 0)
            {
                var newQueue = new Queue<Stack<(int x, int y)>>();
                while (bfs.TryDequeue(out var stack))
                {
                    var head = stack.Peek();
                    var forks = new List<List<(int x, int y)>>();
                    foreach (var dir in directions)
                    {
                        var pos = (x: dir.x + head.x, y: dir.y + head.y);
                        var tile = Tile(map, pos);
                        if (tile == '#' || IsInpossible(tile, dir))
                            continue;
                        if (pos == exit)
                        {
                            stack.Push(pos);
                            results.Add(stack.Count);
                            best = stack.Reverse().ToList();
                            continue;
                        }
                        if (TilesOk.Contains(tile) && !stack.Contains(pos))
                        {
                            var fork = new List<(int x, int y)>();
                            if (tile != '.')
                                fork.Add(pos);
                            if (tile == '>')
                                pos = (pos.x + 1, pos.y);
                            if (tile == '<')
                                pos = (pos.x - 1, pos.y);
                            if (tile == '^')
                                pos = (pos.x, pos.y - 1);
                            if (tile == 'v')
                                pos = (pos.x, pos.y + 1);
                            fork.Add(pos);
                            forks.Add(fork);
                        }
                    }
                    if (forks.Count > 1)
                        forks[1..].ForEach(l =>
                        {
                            var newStack = stack.Clone();
                            l.ForEach(p => newStack.Push(p));
                            newQueue.Enqueue(newStack);
                        });
                    if (forks.Count > 0)
                    {
                        forks[0].ForEach(p => stack.Push(p));
                        newQueue.Enqueue(stack);
                    }
                }
                bfs = newQueue;
            }
            foreach (var p in best)
            {
                var line = new StringBuilder(map[p.y]);
                line[p.x] = 'O';
                map[p.y] = line.ToString();
                var test = string.Join('\n', map);
            }
            yield return (results.Max() - 1).ToString();
        }

        // https://www.reddit.com/r/adventofcode/comments/18oy4pc/comment/kfyvp2g/

        public IEnumerable<string> PartTwo(ALongWalkDataModel model)
        {
            var bfsMap = model.Map!.Select(l => l.ToArray()).ToArray();
            var start = (x: 1, y: 0);
            var exit = (x: bfsMap[0].Length - 2, y: bfsMap.Length - 1);
            var nodes = new HashSet<(int x, int y)>();
            nodes.Add(start);
            nodes.Add(exit);
            for (var y = 1; y < exit.y; y++)
                for (var x = 1; x <= exit.x; x++)
                {
                    var tile2 = bfsMap[y][x];
                    if (tile2 == '#')
                        continue;
                    var forks = 0;
                    foreach (var dir in directions)
                    {
                        var pos = (x: dir.x + x, y: dir.y + y);
                        var tile = bfsMap[pos.y][pos.x];
                        if (tile == '#')
                            continue;
                        forks++;
                    }
                    if (forks > 2)
                    {
                        bfsMap[y][x] = 'F';
                        nodes.Add((x, y));
                    }
                }
            // TO DO missing 19,13   5,13 - exploration en meme temps du meme segment.
            var bfsMap2 = bfsMap.Select(x => x.ToArray()).ToArray();

            var distances = new Dictionary<((int x, int y) p1, (int x, int y) p2), int>();

            foreach (var node in nodes)
            {
                foreach (var dir in directions)
                {
                    var pos = (x: dir.x + node.x, y: dir.y + node.y);
                    if (pos.y <= 0 || pos.y >= exit.y) continue;
                    var tile = bfsMap[pos.y][pos.x];
                    if (tile != '#')
                    {
                        var distance = 1;
                        var prev = node;
                        while (!nodes.Contains(pos))
                        {
                            distance++;
                            foreach (var dir2 in directions)
                            {
                                var npos = (x: dir2.x + pos.x, y: dir2.y + pos.y);
                                if (npos == prev)
                                    continue;
                                var tile2 = bfsMap[npos.y][npos.x];
                                if (tile2 != '#')
                                {
                                    prev = pos;
                                    pos = npos;
                                    break;
                                }
                            }
                        }
                        distances.Add((node, pos), distance);
                    }
                }

            }

            //var bfs = new Queue<((int x, int y) start, (int x, int y) pos, int distance)>();
            //bfsMap[0][1] = '#';
            //bfs.Enqueue((start,start, 0));
            //while (bfs.TryDequeue(out var item))
            //{
            //    var (orig, p, distance) = item;
            //    if (p == exit)
            //        continue;
            //    bfsMap[p.y][p.x] = '#';
            //    foreach (var dir in directions)
            //    {
            //        var pos = (x: dir.x + p.x, y: dir.y + p.y);
            //        var d = distance+1;
            //        if (pos.y <= 0) continue;
            //        if (orig != pos && nodes.Contains(pos))
            //        {
            //            distances.TryAdd((orig, pos), d);
            //            orig = pos;
            //            d = 0;
            //        }
            //        var tile = bfsMap[pos.y][pos.x];
            //        if (tile != '#')
            //            bfs.Enqueue((orig, pos, d));
            //    }
            //}
            //Visu2(bfsMap);
            //foreach(var n in nodes)
            //{
            //    bfsMap2[n.y][n.x] = 'X';
            //}

            var longestDistance = 0;
            var bestPath = new List<(int x, int y)>();
            var dfs = new Stack<((int x, int y) p, int d)>();
            var explored = new HashSet<(int x, int y)>();
            dfs.Push((start, 0));

            var distDico = distances.GroupBy(d => d.Key.p1)
                .Select(g => (g.Key, neighbourgs: g.Select(p => (neighbourg: p.Key.p2, d: p.Value)).ToList()))
                .ToDictionary(g => g.Key, g => g.neighbourgs);
            while (dfs.TryPop(out var item))
            {
                var (pos, distance) = item;
                if (explored.Contains(pos))
                {
                    explored.Remove(pos);
                    continue;
                }
                else
                {
                    dfs.Push(item);
                    explored.Add(pos);
                }
                var neighbors = distDico[pos]
                    .Where(n => !explored.Contains(n.neighbourg));
                foreach (var n in neighbors)
                {
                    var (npos, d) = n;
                    d += distance;
                    if (npos == exit)
                    {
                        if (d > longestDistance)
                        {
                            longestDistance = d;
                            bestPath = explored.ToList();
                        }

                        continue;
                    }
                    dfs.Push((npos, d));
                }
            }

            yield return longestDistance.ToString();
        }


        public IEnumerable<string> PartTwoNaive2(ALongWalkDataModel model)
        {
            var map = model.Map!;
            var start = (x: 1, y: 0);
            var exit = (x: map[0].Length - 2, y: map.Length - 1);
            var hash = new HashSet<(int x, int y)>();
            hash.Add(start);
            var dfs = new Stack<(HashSet<(int x, int y)> hash, (int x, int y) pos)>();
            dfs.Push((hash, start));
            var results = new List<int>();
            var best = new List<(int x, int y)>();
            while (dfs.TryPop(out var rec))
            {
                var head = rec.pos;
                var forks = new List<(int x, int y)>();
                foreach (var dir in directions)
                {
                    if (head.x == 1 && dir == (0, -1))
                        continue;
                    if (head.x == map[0].Length - 2 && dir == (0, -1))
                        continue;
                    if (head.y == 1 && dir == (-1, 0))
                        continue;
                    if (head.y == map.Length - 2 && dir == (-1, 0))
                        continue;
                    var pos = (x: dir.x + head.x, y: dir.y + head.y);
                    var tile = Tile(map, pos);
                    if (tile == '#')
                        continue;
                    if (pos == exit)
                    {
                        rec.hash.Add(pos);
                        if (rec.hash.Count > best.Count)
                        {
                            results.Add(rec.hash.Count);
                            best = [.. rec.hash];
                        }
                        //Visu(best, map);
                        continue;
                    }
                    if (TilesOk.Contains(tile) && !rec.hash.Contains(pos))
                    {
                        forks.Add(pos);
                    }
                }
                if (forks.Count > 1)
                    forks[1..].ForEach(p =>
                    {
                        var newHash = rec.hash.ToHashSet();
                        newHash.Add(p);
                        dfs.Push((newHash, p));
                    });
                if (forks.Count > 0)
                {
                    var p = forks[0];
                    rec.hash.Add(p);
                    dfs.Push((rec.hash, p));
                }
            }
            yield return (results.Max() - 1).ToString();
        }
        public IEnumerable<string> PartTwoNaive(ALongWalkDataModel model)
        {
            var map = model.Map!;
            var start = (x: 1, y: 0);
            var exit = (x: map[0].Length - 2, y: map.Length - 1);
            var hash = new HashSet<(int x, int y)>();
            hash.Add(start);
            var dfs = new Stack<(HashSet<(int x, int y)> hash, (int x, int y) pos)>();
            dfs.Push((hash, start));
            var results = new List<int>();
            var best = new List<(int x, int y)>();
            while (dfs.TryPop(out var rec))
            {
                var head = rec.pos;
                var forks = new List<(int x, int y)>();
                foreach (var dir in directions)
                {
                    var pos = (x: dir.x + head.x, y: dir.y + head.y);
                    var tile = Tile(map, pos);
                    if (tile == '#')
                        continue;
                    if (pos == exit)
                    {
                        rec.hash.Add(pos);
                        if (rec.hash.Count > best.Count)
                        {
                            results.Add(rec.hash.Count);
                            best = [.. rec.hash];
                        }
                        //Visu(best, map);
                        continue;
                    }
                    if (TilesOk.Contains(tile) && !rec.hash.Contains(pos))
                    {
                        forks.Add(pos);
                    }
                }
                if (forks.Count > 1)
                    forks[1..].ForEach(p =>
                    {
                        var newHash = rec.hash.ToHashSet();
                        newHash.Add(p);
                        dfs.Push((newHash, p));
                    });
                if (forks.Count > 0)
                {
                    var p = forks[0];
                    rec.hash.Add(p);
                    dfs.Push((rec.hash, p));
                }
            }
            yield return (results.Max() - 1).ToString();
        }

        static void Visu(List<(int x, int y)> best, string[] map)
        {
            var mapCopy = map.ToArray();
            foreach (var p in best)
            {
                var line = new StringBuilder(mapCopy[p.y]);
                line[p.x] = 'O';
                mapCopy[p.y] = line.ToString();
                //var test = string.Join('\n', mapCopy);
            }
            var test = string.Join('\n', mapCopy);
        }

        static void Visu2(char[][] map)
        {
            var test = string.Join('\n', map.Select(l => new string(l)));
        }
    }
}

/*
                           if (tile == '>')
                                if (dir == (-1, 0))
                                    continue;
                                else
                                    pos = (pos.x + 1, pos.y);
                            if (tile == '<')
                                if (dir == (1, 0))
                                    continue;
                                else
                                    pos = (pos.x - 1, pos.y);
                            if (tile == '^')
                                if (dir == (0, 1))
                                    continue;
                                else
                                    pos = (pos.x, pos.y - 1);
                            if (tile == 'v')
                                if (dir == (0, -1))
                                    continue;
                                else
                                    pos = (pos.x, pos.y + 1);
                            if (fork)
                                newpath = item.path.Clone();
 
 */ 