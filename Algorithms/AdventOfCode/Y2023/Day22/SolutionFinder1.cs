using System.Collections.Generic;
using System.Linq;

namespace Algorithms.AdventOfCode.Y2023.Day22
{

    [SolutionFinder("Sand Slabs - Part 1")]
    public class SolutionFinder1 : SolutionFinderEnum<Input>, IVisualizationNone
    {
        static List<(long x, long y, long z)> Cubes(((long x, long y, long z) start, (long x, long y, long z) end) brick)
        {
            var cubes = new List<(long x, long y, long z)>();
            for (var x = brick.start.x; x <= brick.end.x; x++)
                for (var y = brick.start.y; y <= brick.end.y; y++)
                    for (var z = brick.start.z; z <= brick.end.z; z++)
                    {
                        cubes.Add((x, y, z));
                    }
            return cubes;
        }
        protected override IEnumerable<int> Steps(Input model)
        {
            var bricks = model.Bricks;
            List<(int brick, int supports)> supports = SimulateFall(bricks);

            var count = 0;
            for (var i = 0; i < bricks.Count; i++)
            {
                var supported = supports.Where(s => s.brick == i).Select(s => s.supports).ToList();
                if (supported.Count == 0 || supported.All(s => supports.Any(ss => ss.brick != i && ss.supports == s)))
                    count++;
            }
            Solution = count.ToString();
            yield return 0;
        }

        private static List<(int brick, int supports)> SimulateFall(List<((long x, long y, long z) start, (long x, long y, long z) end)> bricks)
        {
            var moveOccured = true;
            while (moveOccured)
            {
                moveOccured = false;
                var positions = bricks.SelectMany((b, i) => Cubes(b).Select(c => (c, i)))
                    .ToDictionary(a => a.c, a => a.i);

                for (var i = 0; i < bricks.Count; i++)
                {
                    var brick = bricks[i];
                    if (brick.start.z == 1)
                        continue;
                    // collision detection 
                    var cubes = Cubes(brick).Select(c => (c.x, c.y, z: c.z - 1));
                    var collision = false;
                    foreach (var c in cubes)
                        if (positions.TryGetValue(c, out var idx) && idx != i)
                        {
                            collision = true;
                            break;
                        }
                    if (!collision)
                    {
                        moveOccured = true;
                        // move below
                        bricks[i] = (start: (brick.start.x, brick.start.y, brick.start.z - 1), end: (brick.end.x, brick.end.y, brick.end.z - 1));
                    }
                }
            }
            var finalPositions = bricks.SelectMany((b, i) => Cubes(b).Select(c => (c, i)))
                .ToDictionary(a => a.c, a => a.i);
            var supports = finalPositions.Select(p => finalPositions.TryGetValue((p.Key.x, p.Key.y, p.Key.z + 1), out var over) ? (brick: p.Value, supports: over) : (brick: p.Value, supports: p.Value))
                .Where(b => b.brick != b.supports).Distinct().ToList();
            return supports;
        }
    }
}
