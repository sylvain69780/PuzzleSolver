using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.AdventOfCode.Y2023.Day23
{
    [SolutionFinder("A Long Walk - Part 1")]
    public class SolutionFinder1 : SolutionFinderEnum<Input>, IVisualizationNone
    {

        static (int x, int y)[] directions = new (int x, int y)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

        static readonly char[] TilesOk = new char[] { '.', '>', '<', '^', 'v' };

        static bool IsInpossible(char tile, (int x, int y) dir)
        {
            return
                (tile == '>' && dir == (-1, 0)) ||
                (tile == '<' && dir == (1, 0)) ||
                (tile == '^' && dir == (0, 1)) ||
                (tile == 'v' && dir == (0, -1))
                ;
        }

        static char Tile(string[] map, (int x, int y) pos)
        {
            if (pos.x < 0 || pos.x >= map[0].Length || pos.y < 0 || pos.y >= map.Length)
                return '#';
            return map[pos.y][pos.x];
        }
        protected override IEnumerable<int> Steps(Input model)
        {
            var map = model.Map;
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
                var stack = bfs.Dequeue();
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
                    for (var i = 1; i < forks.Count; i++)
                    {
                        var l = forks[i];
                        var newStack = stack.Clone();
                        for (var j = 0; j < l.Count; j++)
                            newStack.Push(l[j]);
                        bfs.Enqueue(newStack);
                    }
                if (forks.Count > 0)
                {
                    forks[0].ForEach(p => stack.Push(p));
                    bfs.Enqueue(stack);
                }
            }
            foreach (var p in best)
            {
                var line = new StringBuilder(map[p.y]);
                line[p.x] = 'O';
                map[p.y] = line.ToString();
                var test = string.Join("\n", map);
            }
            Solution = (results.Max() - 1).ToString();
            yield return 0;
        }

        // https://www.reddit.com/r/adventofcode/comments/18oy4pc/comment/kfyvp2g/

    }
}
