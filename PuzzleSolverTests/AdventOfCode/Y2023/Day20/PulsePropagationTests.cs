using Algorithms.AdventOfCode.Y2023.Day20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day20
{
    public class PulsePropagationTests
    {
        [Fact]
        public void Test()
        {
            var solver = new PulsePropagation();
            Assert.Equal("32000000", solver.Solve(input, "Part 1").Last());
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day20";
        string input => File.ReadAllText($"{path}\\PulsePropagation.txt");

        string input2 => File.ReadAllText($"{path}\\PulsePropagation_full.txt");
    }
}
