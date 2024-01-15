using Algorithms.AdventOfCode.Y2023.Day20;
using Algorithms.AdventOfCode.Y2023.Day21;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day21
{
    public class StepCounterTests
    {
        [Fact]
        public void Test()
        {
            var solver = new StepCounter();
            Assert.Equal("16", solver.Solve(input, "Part 1").Last());
            Assert.Equal("3615", solver.Solve(input2, "Part 1").Last());
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day21";
        string input => File.ReadAllText($"{path}\\StepCounter.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\StepCounter_full.txt").Replace("\r", "");
    }
}
