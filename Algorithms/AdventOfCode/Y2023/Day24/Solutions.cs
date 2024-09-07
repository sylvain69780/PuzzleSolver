using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Algorithms.AdventOfCode.Y2023.Day24
{
    [SolutionFinder("Never Tell Me The Odds")]
    public class Solutions 
    {

        [SolutionMethod("Part 1")]
        public static IEnumerable<State> PartOne(Input input)
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

            yield return new State
            {
                Message = counter.ToString()
            };
        }

        [SolutionMethod("Part 2")]
        public static IEnumerable<State> PartTwo(Input input)
        {
            var stones = input.Hailstones;

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
                var (px0, py0, pz0, vx0, vy0, vz0) = (new BigInteger(stones[i0].position.x), new BigInteger(stones[i0].position.y), new BigInteger(stones[i0].position.z), new BigInteger(stones[i0].velocity.x), new BigInteger(stones[i0].velocity.y), new BigInteger(stones[i0].velocity.z));
                var (px1, py1, pz1, vx1, vy1, vz1) = (new BigInteger(stones[i1].position.x), new BigInteger(stones[i1].position.y), new BigInteger(stones[i1].position.z), new BigInteger(stones[i1].velocity.x), new BigInteger(stones[i1].velocity.y), new BigInteger(stones[i1].velocity.z));
                var (px2, py2, pz2, vx2, vy2, vz2) = (new BigInteger(stones[i2].position.x), new BigInteger(stones[i2].position.y), new BigInteger(stones[i2].position.z), new BigInteger(stones[i2].velocity.x), new BigInteger(stones[i2].velocity.y), new BigInteger(stones[i2].velocity.z));
                var (px3, py3, pz3, vx3, vy3, vz3) = (new BigInteger(stones[i3].position.x), new BigInteger(stones[i3].position.y), new BigInteger(stones[i3].position.z), new BigInteger(stones[i3].velocity.x), new BigInteger(stones[i3].velocity.y), new BigInteger(stones[i3].velocity.z));

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
                foreach (var item in solution.Take(3))
                {
                    response += item;
                }
            }
            // 931193307668083  low
            // 387382059881002, 371825688904742, 171985558882512 @ -220, -167, 214 << not good result ?

            // I was adding velocities !!!!
            // 931193307668256


            // new try other method
            // https://pastebin.com/pnbxaCVu

            /*
             * 293346290659882, 246052847571494, 253091409835728 @ 20, 154, 7
             * 212487908127506, 258596344456430, 282025344332280 @ 109, 46, 7
             * 240358719931705, 238982264991164, 326472012534819 @ -23, 11, 7
             */

            /*

            decimal vx0 = 20;
            decimal vy0 = 154;
            decimal vz0 = 7;

            decimal vx1 = 109;
            decimal vy1 = 46;
            decimal vz1 = 7;

            decimal vx2 = -23;
            decimal vy2 = 11;
            decimal vz2 = 7;

            decimal x0 = 293346290659882;
            decimal y0 = 246052847571494;
            decimal z0 = 253091409835728;

            decimal x1 = 212487908127506;
            decimal y1 = 258596344456430;
            decimal z1 = 282025344332280;

            decimal x2 = 240358719931705;
            decimal y2 = 238982264991164;
            decimal z2 = 326472012534819;

            var vxr1 = vx1 - vx0;
            var vzr1 = vz1 - vz0;
            var vxr2 = vx2 - vx0;
            var vzr2 = vz2 - vz0;

            var xr1 = x1 - x0;
            var yr1 = y1 - y0;
            var zr1 = z1 - z0;

            var xr2 = x2 - x0;
            var yr2 = y2 - y0;
            var zr2 = z2 - z0;

            var num = (yr2 * xr1 * vzr1) - (vxr1 * yr2 * zr1) + (yr1 * zr2 * vxr1) - (yr1 * xr2 * vzr1);
            var den = yr1 * ((vzr1 * vxr2) - (vxr1 * vzr2));
            var t2 = num / den;

            num = (yr1 * xr2) + (yr1 * vxr2 * t2) - (yr2 * xr1);
            den = yr2 * vxr1;
            var t1 = num / den;

            var cx1 = x1 + (t1 * vx1);
var cy1 = y1 + (t1 * vy1);
var cz1 = z1 + (t1 * vz1);

            var cx2 = x2 + (t2 * vx2);
var cy2 = y2 + (t2 * vy2);
var cz2 = z2 + (t2 * vz2);

            var xm = (cx2 - cx1) / (t2 - t1);
var ym = (cy2 - cy1) / (t2 - t1);
var zm = (cz2 - cz1) / (t2 - t1);

            var xc = cx1 - (xm * t1);
var yc = cy1 - (ym * t1);
var zc = cz1 - (zm * t1);

            */

            yield return new State
            {
                Message = response.ToString()
            };
        }

        public static BigInteger[] SolveCramer(BigInteger[][] equations)
        {
            int size = equations.Length;
            if (equations.Any(eq => eq.Length != size + 1)) throw new ArgumentException($"Each equation must have {size + 1} terms.");
            var matrix = new BigInteger[size, size];
            var column = new BigInteger[size];
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

        private static BigInteger[] Solve(SubMatrix matrix)
        {
            var det = matrix.Det();
            if (det == 0) throw new ArgumentException("The determinant is zero.");

            var answer = new BigInteger[matrix.Size];
            for (int i = 0; i < matrix.Size; i++)
            {
                matrix.ColumnIndex = i;
                answer[i] = matrix.Det() / det;
            }
            return answer;
        }

        private class SubMatrix
        {
            private BigInteger[,] source;
            private SubMatrix prev;
            private BigInteger[] replaceColumn;

            public SubMatrix(BigInteger[,] source, BigInteger[] replaceColumn)
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

            public BigInteger this[int row, int column]
            {
                get
                {
                    if (source != null) return column == ColumnIndex ? replaceColumn[row] : source[row, column];
                    return prev[row + 1, column < ColumnIndex ? column : column + 1];
                }
            }

            public BigInteger Det()
            {
                if (Size == 1) return this[0, 0];
                if (Size == 2) return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
                SubMatrix m = new SubMatrix(this);
                BigInteger det = 0;
                BigInteger sign = 1;
                for (int c = 0; c < Size; c++)
                {
                    m.ColumnIndex = c;
                    BigInteger d = m.Det();
                    det += this[0, c] * d * sign;
                    sign = -sign;
                }
                return det;
            }
        }

    }
}
