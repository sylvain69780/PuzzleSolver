using System.Collections.Generic;
using System.Linq;

namespace Algorithms.AdventOfCode.Y2023.Day21
{
    [SolutionFinder("Step Counter - Part 1")]
    public class SolutionFinder1 : SolutionFinderEnum<Input>, IVisualizationNone
    {
        static readonly (int x, int y)[] Directions = new (int x, int y)[] {(1, 0), (-1, 0), (0, 1), (0, -1)};
        static char Tile(string[] map, (int x, int y) pos)
        {
            if (pos.x < 0 || pos.x >= map[0].Length || pos.y < 0 || pos.y >= map.Length)
                return '.';
            var c = map[pos.y][pos.x];
            return c == 'S' ? '.' : c;
        }

        protected override IEnumerable<int> Steps(Input model)
        {
            var start = model.Pos;
            var map = model.Map;
            var bfs = new Queue<(int x, int y)>();
            var newQueue = new Queue<(int x, int y)>();
            bfs.Enqueue(start);
            var maxCount = map.Length <= 11 ? 6 : 64;
            var grid = new HashSet<(int x, int y)>();
            while ( --maxCount >= 0)
            {
                while (bfs.Count > 0 )
                {
                    var pos = bfs.Dequeue();
                    foreach (var (x, y) in Directions)
                    {
                        var tpos = (pos.x + x, pos.y + y);
                        var tile = Tile(map, tpos);
                        if (tile == '#'  || grid.Contains(tpos))
                            continue;
                        newQueue.Enqueue(tpos);
                        grid.Add(tpos);
                    }
                }
                (bfs,newQueue) = (newQueue,bfs);
                newQueue.Clear();
                grid.Clear();
            }
            Solution = bfs.Count.ToString();
            yield return 0;
        }
    }
}
