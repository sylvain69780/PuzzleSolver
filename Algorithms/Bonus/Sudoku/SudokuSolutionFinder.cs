using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms;

namespace Algorithms.Bonus.Sudoku
{
    [SolutionFinder("Sudoku")]
    public class SudokuSolutionFinder : SolutionFinderEnum<Input>, IVisualization
    {
        private const string Digits = "123456789";
        public static string Entropy(int position, string grid)
        {
            var res = new HashSet<char>();
            var (col, row) = (position % 9, position / 9);
            var square = (Column: col / 3, Row: row / 3);
            for (var i = 0; i < 9; i++)
            {
                res.Add(grid[i + 9 * row]);
                res.Add(grid[col + 9 * i]);
                res.Add(grid[i % 3 + square.Column * 3 + (i / 3 + square.Row * 3) * 9]);
            }
            res.Remove('.');
            return string.Concat(Digits.Where(d => !res.Contains(d)));
        }
        public string Grid { get; private set; } 
        public Stack<string> Queue { get; private set; }
        protected override IEnumerable<int> Steps(Input input)
        {
            Queue = new Stack<string>();
            Queue.Push(input.Grid);
            Grid = string.Empty;
            while (Queue.TryPop(out var grid))
            {
                Grid = grid;
                yield return 0;
                var emptySlots = Enumerable.Range(0, 9 * 9).Where(x => Grid[x] == '.').ToArray();
                if (emptySlots.Length == 0)
                {
                    // the solution is found
                    break;
                }
                else
                {
                    var slotWithMinimalEntropy = emptySlots.Select(x => (p: x, e: Entropy(x, Grid))).OrderBy(x => x.e.Length).ThenBy(x => x.p).First();
                    var (p, e) = slotWithMinimalEntropy;
                    if (e == string.Empty)
                    {
                        continue;
                    }
                    var sb = new StringBuilder(Grid);
                    for (var i = 0; i < e.Length; i++)
                    {
                        sb[p] = e[i];
                        Queue.Push(sb.ToString());
                    }
                }
                if (Queue.Count == 0)
                    yield return 0;
            }
        }
    }
}
