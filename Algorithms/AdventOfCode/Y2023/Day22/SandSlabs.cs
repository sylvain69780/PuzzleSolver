using Algorithms.AdventOfCode.Y2023.Day21;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Algorithms.AdventOfCode.Y2023.Day22
{
    public class SandSlabs : SolutionBase<SandSlabsDataModel>, ISolution
    {
        protected override SandSlabsDataModel Parse(string input)
        {
            return new SandSlabsDataModel()
            {
                Bricks = input.Split("\n").Select(x => Regex.Match(x, @"(\d+),(\d+),(\d+)~(\d+),(\d+),(\d+)"))
                .Select(x => (start: (x: long.Parse(x.Groups[1].Value), y: long.Parse(x.Groups[2].Value), z: long.Parse(x.Groups[3].Value)),
                                end: (x: long.Parse(x.Groups[4].Value), y: long.Parse(x.Groups[5].Value), z: long.Parse(x.Groups[6].Value)))).ToList()
            };
        }
        public SandSlabs()
        {
            _parts.Add("Part 1", PartOne);
            _parts.Add("Part 2", PartTwo);
        }

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

        static IEnumerable<string> PartOne(SandSlabsDataModel model)
        {
            var bricks = model.Bricks!;
            List<(int brick, int supports)> supports = SimulateFall(bricks);

            var count = 0;
            for (var i = 0; i < bricks.Count; i++)
            {
                var supported = supports.Where(s => s.brick == i).Select(s => s.supports).ToList();
                if (supported.Count == 0 || supported.All(s => supports.Any(ss => ss.brick != i && ss.supports == s)))
                    count++;
            }
            yield return count.ToString();
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

        static IEnumerable<string> PartTwo(SandSlabsDataModel model)
        {
            var bricks = model.Bricks!;
            List<(int brick, int supports)> supports = SimulateFall(bricks);

            var count = 0;
            for (var i = 0; i < bricks.Count; i++)
            {
                var listOfBricks = Enumerable.Range(0, bricks.Count).Where(x => x != i && bricks[x].start.z > 1).ToList();
                var remainingSupports = supports.Where(s => s.brick != i).ToList();
                var isKeepFalling= true;
                while (isKeepFalling)
                {
                    isKeepFalling = false;
                    var supporting = remainingSupports.Select(x => x.supports).ToHashSet();
                    var falling = listOfBricks.Where(x => !supporting.Contains(x)).ToList();
                    if ( falling.Count > 0 )
                    {
                        isKeepFalling = true;
                        count += falling.Count;
                        listOfBricks.RemoveAll(x => falling.Contains(x));
                        remainingSupports.RemoveAll(s => falling.Contains(s.brick));
                    }
                }
            }
            yield return count.ToString();
        }
    }
}
