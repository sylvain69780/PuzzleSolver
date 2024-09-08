using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Algorithms.AdventOfCode.Y2023.Day24
{
    [SolutionFinder("Never Tell Me The Odds - Part 1")]
    public class SolutionFinder1 : SolutionFinderEnum<Input>, IVisualizationNone
    {

        protected override IEnumerable<int> Steps(Input input)
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
            Solution = counter.ToString();
            yield return 0;
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
