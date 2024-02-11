using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms.AdventOfCode.Y2023.Day24
{
    public class NeverTellMeTheOddsInput
    {
        public ((long x, long y, long z) position, (long x, long y, long z) velocity)[] Hailstones { get; set; }
    }
    public class NeverTellMeTheOdds : SolutionBase<NeverTellMeTheOddsInput>
    {
        protected override NeverTellMeTheOddsInput Parse(string input)
        {
            (long x, long y, long z) ParsePoint(string str)
            {
                var r = str.Split(new string[] { ", " }, StringSplitOptions.None).Select(x => long.Parse(x)).ToArray();
                return (x: r[0], y: r[1], z: r[2]);
            }
            var lines = input.Split('\n');
            var value = lines
                .Select(l => l.Split(new string[] { " @ " }, StringSplitOptions.None))
                .Select(l => (ParsePoint(l[0]), ParsePoint(l[1])))
                .ToArray();

            return new NeverTellMeTheOddsInput()
            {
                Hailstones = value
            };
        }

        [SolutionMethod("Part 1")]
        public static IEnumerable<string> PartOne(NeverTellMeTheOddsInput input)
        {
            var stones = input.Hailstones;
            (long lower, long upper) = stones.Length <= 5 ? (7, 27) : (200000000000000, 400000000000000);
            var counter = 0;
            bool IsIntersecting(((long x, long y, long z) position, (long x, long y, long z) velocity) ray1, ((long x, long y, long z) position, (long x, long y, long z) velocity) ray2)
            {
                var ro1 = ray1.position;
                var ro2 = ray2.position;
                var rd1 = ray1.velocity;
                var rd2 = ray2.velocity;
                /*
                    ro1+t1*rd1 == ro1+t2*rd1

                    ro1.x + t1 * rd1.x = ro2.x + t2 * rd2.x // t1 * rd1.x - t2 * rd2.x = (ro2.x - ro1.x) 
                    ro1.y + t1 * rd1.y = ro2.y + t2 * rd2.y

                    t1 = (ro2.x + t2 * rd2.x) / rd1.x - ro1.x
                    t2 = (ro1.y + t1 * rd1.x) / rd2.x - ro2.x
                   
                */
                var s = (x: ro2.x - ro1.x, y: ro2.y - ro1.y);
                var a = (decimal)rd1.x;
                var b = (decimal)-rd2.x;
                var c = (decimal)rd1.y;
                var d = (decimal)-rd2.y;
                var e = (decimal)s.x;
                var f = (decimal)s.y;

                // parallel DET is zero
                var det = a * d - b * c;
                if (det == 0)
                    if (e * c - f * a == 0)
                        return true;
                    else
                        return false;
                else
                {
                    var t1 = (e * d - b * f) / det;
                    var t2 = (a * f - e * c) / det;
                    if (t1 >= 0 && t2 >= 0)
                    {
                        var p = (x: t1 * a + (decimal)ro1.x, y: t1 * c + (decimal)ro1.y);
                        if (p.x >= lower && p.y >= lower && p.x <= upper && p.y <= upper)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            for (var i = 0; i < stones.Length - 1; i++)
                for (var j = i + 1; j < stones.Length; j++)
                {
                    if (IsIntersecting(stones[i], stones[j]))
                        counter++;
                }

            yield return counter.ToString();
        }

        [SolutionMethod("Part 2")]
        public static IEnumerable<string> PartTwo(NeverTellMeTheOddsInput input)
        {
            var stones = input.Hailstones;
            (long lower, long upper) = stones.Length <= 5 ? (7, 27) : (200000000000000, 400000000000000);

            // https://www.reddit.com/r/adventofcode/comments/18pnycy/2023_day_24_solutions/
            //            vy0 - vy1    vx1 - vx0    0            py1 - py0    px0 - px1    0 = px0 * vy0 - py0 * vx0 - px1 * vy1 + py1 * vx1
            //            vz0 - vz1    0            vx1 - vx0    pz1 - pz0    0            px0 - px1 = px0 * vz0 - pz0 * vx0 - px1 * vz1 + pz1 * vx1
            //            vy0 - vy2    vx2 - vx0    0            py2 - py0    px0 - px2    0 = px0 * vy0 - py0 * vx0 - px2 * vy2 + py2 * vx2
            //            vz0 - vz2    0            vx2 - vx0    pz2 - pz0    0            px0 - px2 = px0 * vz0 - pz0 * vx0 - px2 * vz2 + pz2 * vx2
            //            vy0 - vy3    vx3 - vx0    0            py3 - py0    px0 - px3    0 = px0 * vy0 - py0 * vx0 - px3 * vy3 + py3 * vx3
            //            vz0 - vz3    0            vx3 - vx0    pz3 - pz0    0            px0 - px3 = px0 * vz0 - pz0 * vx0 - px3 * vz3 + pz3 * vx3
            // https://rosettacode.org/wiki/Cramer%27s_rule
            BigInteger response = 0;
            for (var i = 0; i < stones.Length - 4; i++)
            {
                var (i0, i1, i2, i3) = (i, i+1, i+2, i+3);
                var (px0, py0, pz0, vx0, vy0, vz0) = (stones[i0].position.x, stones[i0].position.y, stones[i0].position.z, stones[i0].velocity.x, stones[i0].velocity.y, stones[i0].velocity.z);
                var (px1, py1, pz1, vx1, vy1, vz1) = (stones[i1].position.x, stones[i1].position.y, stones[i1].position.z, stones[i1].velocity.x, stones[i1].velocity.y, stones[i1].velocity.z);
                var (px2, py2, pz2, vx2, vy2, vz2) = (stones[i2].position.x, stones[i2].position.y, stones[i2].position.z, stones[i2].velocity.x, stones[i2].velocity.y, stones[i2].velocity.z);
                var (px3, py3, pz3, vx3, vy3, vz3) = (stones[i3].position.x, stones[i3].position.y, stones[i3].position.z, stones[i3].velocity.x, stones[i3].velocity.y, stones[i3].velocity.z);

                var equations = new BigInteger[][] {
                new BigInteger [] { vy0 - vy1 ,   vx1 - vx0 ,   0         ,   py1 - py0  , px0 - px1 ,   0 , px0 * vy0 - py0 * vx0 - px1 * vy1 + py1 * vx1 },
                new BigInteger [] { vz0 - vz1 ,   0 ,           vx1 - vx0 ,   pz1 - pz0   , 0      ,      px0 - px1 , px0 * vz0 - pz0 * vx0 - px1 * vz1 + pz1 * vx1 },
                new BigInteger [] { vy0 - vy2  ,  vx2 - vx0  ,  0      ,      py2 - py0 ,  px0 - px2 ,   0 , px0 * vy0 - py0 * vx0 - px2 * vy2 + py2 * vx2 },
                new BigInteger [] {vz0 - vz2  ,  0       ,     vx2 - vx0 ,   pz2 - pz0  ,  0     ,       px0 - px2 , px0*vz0 - pz0*vx0 - px2*vz2 + pz2*vx2},
                new BigInteger [] {vy0 - vy3  ,  vx3 - vx0  ,  0        ,    py3 - py0  ,  px0 - px3 ,   0         , px0*vy0 - py0*vx0 - px3*vy3 + py3*vx3},
                new BigInteger [] {vz0 - vz3 ,   0     ,       vx3 - vx0  ,  pz3 - pz0 ,   0       ,     px0 - px3 , px0*vz0 - pz0*vx0 - px3*vz3 + pz3*vx3},
                    };
                var solution = SolveCramer(equations);
                response = 0;
                foreach (var item in solution)
                {
                    response += item;
                }
            }
            // 931193307668083 low
            yield return response.ToString();
        }

        public static BigInteger [] SolveCramer(BigInteger [][] equations)
        {
            int size = equations.Length;
            if (equations.Any(eq => eq.Length != size + 1)) throw new ArgumentException($"Each equation must have {size + 1} terms.");
            var matrix = new BigInteger [size, size];
            var column = new BigInteger [size];
            for (int r = 0; r < size; r++)
            {
                column[r] = equations[r][size];
                for (int c = 0; c < size; c++)
                {
                    matrix[r, c] = equations[r][c];
                }
            }
            return Solve(new SubMatrix(matrix, column));
        }

        private static BigInteger [] Solve(SubMatrix matrix)
        {
            var det = matrix.Det();
            if (det == 0) throw new ArgumentException("The determinant is zero.");

            var answer = new BigInteger [matrix.Size];
            for (int i = 0; i < matrix.Size; i++)
            {
                matrix.ColumnIndex = i;
                answer[i] = matrix.Det() / det;
            }
            return answer;
        }

        private class SubMatrix
        {
            private BigInteger [,] source;
            private SubMatrix prev;
            private BigInteger [] replaceColumn;

            public SubMatrix(BigInteger [,] source, BigInteger [] replaceColumn)
            {
                this.source = source;
                this.replaceColumn = replaceColumn;
                this.prev = null;
                this.ColumnIndex = -1;
                Size = replaceColumn.Length;
            }

            private SubMatrix(SubMatrix prev, int deletedColumnIndex = -1)
            {
                this.source = null;
                this.prev = prev;
                this.ColumnIndex = deletedColumnIndex;
                Size = prev.Size - 1;
            }

            public int ColumnIndex { get; set; }
            public int Size { get; }

            public BigInteger  this[int row, int column]
            {
                get
                {
                    if (source != null) return column == ColumnIndex ? replaceColumn[row] : source[row, column];
                    return prev[row + 1, column < ColumnIndex ? column : column + 1];
                }
            }

            public BigInteger  Det()
            {
                if (Size == 1) return this[0, 0];
                if (Size == 2) return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
                SubMatrix m = new SubMatrix(this);
                BigInteger  det = 0;
                BigInteger  sign = 1;
                for (int c = 0; c < Size; c++)
                {
                    m.ColumnIndex = c;
                    BigInteger  d = m.Det();
                    det += this[0, c] * d * sign;
                    sign = -sign;
                }
                return det;
            }
        }

    }
}
