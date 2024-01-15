using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.AdventOfCode.Y2023.Day21
{
    public class StepCounter : PulsePropagationBase<StepCounterDataModel>, ISolution
    {
        protected override StepCounterDataModel Parse(string input)
        {
            var map = input.Split("\n");
            var pos = map.Select((s, y) => (s, y)).Where(l => l.s.Contains('S')).Select(l => (x: l.s.IndexOf('S'), l.y)).Single();
            return new StepCounterDataModel
            {
                Map = map,
                Pos = pos
            };
        }

        public StepCounter()
        {
            _parts.Add("Part 1", PartOne);
        }

        static readonly (int x, int y)[] Directions = [(1, 0), (-1, 0), (0, 1), (0, -1)];

        static char Tile(string[] map, (int x, int y) pos)
        {
            if (pos.x < 0 || pos.x >= map[0].Length || pos.y < 0 || pos.y >= map.Length)
                return '.';
            var c = map[pos.y][pos.x];
            return c == 'S' ? '.' : c;
        }

        static IEnumerable<string> PartOne(StepCounterDataModel model)
        {
            var start = model.Pos;
            var map = model.Map;
            var bfs = new Queue<(int x, int y)>();
            bfs.Enqueue(start);
            var count = 0;
            var maxCount = map.Length <= 11 ? 6 : 64;
            while (count < maxCount)
            {
                count++;
                var newQueue = new Queue<(int x, int y)>();
                while (bfs.TryDequeue(out var pos))
                {
                    foreach (var (x, y) in Directions)
                    {
                        var tpos = (pos.x + x, pos.y + y);
                        var tile = Tile(map, tpos);
                        if (tile == '#' || bfs.Contains(tpos) || newQueue.Contains(tpos))
                            continue;
                        newQueue.Enqueue(tpos);
                    }
                }
                bfs = newQueue;
            }

            yield return bfs.Count.ToString();
        }
    }
}
