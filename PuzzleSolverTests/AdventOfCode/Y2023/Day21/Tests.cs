﻿
using Algorithms.AdventOfCode.Y2023.Day21;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day21
{
    public class Tests
    {
        [Fact]
        public void Test()
        {
            var parser = new Parser();
            Assert.Equal("16", Solutions.PartOne(parser.Parse(input)).Last().Message);
            Assert.Equal("3615", Solutions.PartOne(parser.Parse(input2)).Last().Message);
        }
        [Fact]
        public void Test2()
        {
            var parser = new Parser();
            Assert.Equal("602259568764234", Solutions.PartTwo(parser.Parse(input2)).Last().Message);
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day21";
        string input => File.ReadAllText($"{path}\\StepCounter.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\StepCounter_full.txt").Replace("\r", "");
    }
}
