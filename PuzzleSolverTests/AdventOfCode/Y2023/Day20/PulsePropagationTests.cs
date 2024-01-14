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
            Assert.Equal("11687500", solver.Solve(input1, "Part 1").Last());
            Assert.Equal("818649769", solver.Solve(input2, "Part 1").Last());
            Assert.Equal("246313604784977", solver.Solve(input2, "Part 2").Last());
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day20";
        string input => File.ReadAllText($"{path}\\PulsePropagation.txt").Replace("\r", "");
        string input1 => File.ReadAllText($"{path}\\PulsePropagation2.txt").Replace("\r", "");

        string input2 => File.ReadAllText($"{path}\\PulsePropagation_full.txt").Replace("\r","");
    }
}
