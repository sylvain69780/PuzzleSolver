using System;
using System.Collections.Generic;
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

        static bool IsInpossible(char tile,(int x, int y) dir)
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

        public IEnumerable<string> PartTwo(ALongWalkDataModel model)
        {
            var map = model.Map!;
            var start = (x: 1, y: 0);
            var exit = (x: map[0].Length - 2, y: map.Length - 1);
            var hash = new HashSet<(int x, int y)>();
            hash.Add(start);
            var bfs = new Queue<(HashSet<(int x, int y)> hash,(int x, int y) pos)>();
            bfs.Enqueue((hash,start));
            var results = new List<int>();
            var best = new List<(int x, int y)>();
            while (bfs.Count > 0)
            {
                var newQueue = new Queue<(HashSet<(int x, int y)> hash, (int x, int y) pos)>();
                while (bfs.TryDequeue(out var rec))
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
                            results.Add(rec.hash.Count);
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
                            newQueue.Enqueue((newHash,p));
                        });
                    if (forks.Count > 0)
                    {
                        var p = forks[0];
                        rec.hash.Add(p);
                        newQueue.Enqueue((rec.hash,p));
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