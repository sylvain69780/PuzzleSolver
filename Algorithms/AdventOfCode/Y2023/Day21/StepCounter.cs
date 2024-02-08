using System.Collections.Generic;
using System.Linq;

namespace Algorithms.AdventOfCode.Y2023.Day21
{
    public class StepCounter : SolutionBase<StepCounterDataModel>
    {
        protected override StepCounterDataModel Parse(string input)
        {
            var map = input.Split('\n');
            var pos = map.Select((s, y) => (s, y)).Where(l => l.s.Contains('S')).Select(l => (x: l.s.IndexOf('S'), l.y)).Single();
            return new StepCounterDataModel
            {
                Map = map,
                Pos = pos
            };
        }

        static readonly (int x, int y)[] Directions = new (int x, int y)[] {(1, 0), (-1, 0), (0, 1), (0, -1)};

        static char Tile(string[] map, (int x, int y) pos)
        {
            if (pos.x < 0 || pos.x >= map[0].Length || pos.y < 0 || pos.y >= map.Length)
                return '.';
            var c = map[pos.y][pos.x];
            return c == 'S' ? '.' : c;
        }
        [SolutionMethod("Part 1")]

        public static IEnumerable<string> PartOne(StepCounterDataModel model)
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
            yield return bfs.Count.ToString();
        }

        static char Tile2(string[] map, (int x, int y) pos)
        {
            if (pos.x < 0 || pos.x >= map[0].Length || pos.y < 0 || pos.y >= map.Length)
                return '#';
            var c = map[pos.y][pos.x];
            return c == 'S' ? '.' : c;
        }

        static int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
        static char Tile3(string[] map, (int x, int y) pos)
        {
            pos = (Mod(pos.x,131),Mod(pos.y , 131));
            var c = map[pos.y][pos.x];
            return c == 'S' ? '.' : c;
        }
        [SolutionMethod("Part 2")]
        public static IEnumerable<string> PartTwo(StepCounterDataModel model)
        {
            var start = model.Pos;
            var map = model.Map;
            var bfs = new Queue<(int x, int y)>();
            bfs.Enqueue(start);
            var count = 0;
            var expansion = 1;
            var maxCount = 65+131*2*expansion;
            var counts = new List<long>
            {
                bfs.Count
            };
            while (count < maxCount)
            {
                count++;
                var newQueue = new Queue<(int x, int y)>();
                var grid = new HashSet<(int x, int y)>();
                while (bfs.Count > 0)
                {
                    var pos = bfs.Dequeue();
                    foreach (var (x, y) in Directions)
                    {
                        var tpos = (pos.x + x, pos.y + y);
                        var tile = Tile3(map, tpos);
                        if (tile == '#' || grid.Contains(tpos))
                            continue;
                        newQueue.Enqueue(tpos);
                        grid.Add(tpos);
                    }
                }
                bfs = newQueue;
                counts.Add(bfs.Count);
            }

            var countGrid = bfs.GroupBy(p => (x: (p.x+131*expansion*2) / 131, y: (p.y+131*expansion*2) / 131)).Select(g => (key:g.Key,count: g.Count()))
             .OrderBy(g => g.key).ToList();

            // 26501365 = 202300 * 131 + 65
            // diamond radius = 202300 + 1
            var gridv = new long[expansion*2*2+1,expansion *2* 2 + 1];
            var test = countGrid.Select(g => $"{g.key.x},{g.key.y},{g.count}").ToList();
            foreach (var e in countGrid)
            {
                gridv[e.key.x, e.key.y] = e.count;
            }
            /*	65+131*2 iterations => 92268						
     	00925	05558	00936	     
00925	06459	07354	06461	00936
05541	07354	07362	07354	05555
00937	06444	07354	06456	00941
     	00937	05538	00941	     	
            

            65+131*5 iterations => 445809
             */
            var expand = 101150; // 202300 /2 expansion in every direction of the initial 131*131 superplot
            // expand = 2; => 298530
            var res = 0L;
            res += gridv[0,2] + gridv[4,2] + gridv[2,0] + gridv[2, 4]; // vertexes
            res += (2* expand) * (gridv[1, 0] + gridv[3, 0] + gridv[1, 4] + gridv[3, 4]); // first layer diagonal
            res += ( 2 * expand-1) * (gridv[1, 1] + gridv[3, 1] + gridv[1, 3] + gridv[3, 3]); // second layer diagonal
            res += gridv[2, 2];
            for (var i = 1; i<expand; i++)
            {
                var c = i * 2 * 4; // 1 8 16 ..
                res += gridv[2, 2] * c;
            }
            for (var i = 0; i < expand; i++)
            {
                var c = 4 + i * 4 * 2 ; // 4 12
                res += gridv[2, 1] * c;
            }
            yield return res.ToString();
        }
    }
}
