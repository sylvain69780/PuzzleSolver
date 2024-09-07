using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms;

namespace Algorithms.Bonus.Sudoku
{
    [SolutionFinder("Sudoku")]
    public class Solutions
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
        [SolutionMethod("Sudoku")]
        public static IEnumerable<Func<State>> Sudoku(Input input)
        {
            var dfs = new Stack<string>();
            dfs.Push(input.Grid);
            var puzzleState = string.Empty;
            State stateFunc() => new State
            {
                Message = "Seaching ...",
                Grid = puzzleState,
                Queue = dfs
            };
            while (dfs.TryPop(out puzzleState))
            {
                yield return stateFunc;
                var emptySlots = Enumerable.Range(0, 9 * 9).Where(x => puzzleState[x] == '.').ToArray();
                if (emptySlots.Length == 0)
                {
                    // the solution is found
                    break;
                }
                else
                {
                    var slotWithMinimalEntropy = emptySlots.Select(x => (p: x, e: Entropy(x, puzzleState))).OrderBy(x => x.e.Length).ThenBy(x => x.p).First();
                    var (p, e) = slotWithMinimalEntropy;
                    if (e == string.Empty)
                    {
                        continue;
                    }
                    var sb = new StringBuilder(puzzleState);
                    for (var i = 0; i < e.Length; i++)
                    {
                        sb[p] = e[i];
                        dfs.Push(sb.ToString());
                    }
                }
                if (dfs.Count == 0)
                    yield return () => new State
                    {
                        Message = "No solution found"
                    };
            }
        }
    }
}
