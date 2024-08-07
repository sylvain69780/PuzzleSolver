using Algorithms.AdventOfCode.Y2023.Day22;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day22
{
    public class Tests
    {
        [Fact]
        public void Test()
        {
            Assert.Equal("5", Solutions.PartOne(Parser.Parse(input)).Last().Message);
            Assert.Equal("509", Solutions.PartOne(Parser.Parse(input2)).Last().Message);
        }
        [Fact]
        public void Test2()
        {
            Assert.Equal("7", Solutions.PartTwo(Parser.Parse(input)).Last().Message);
            Assert.Equal("102770",Solutions.PartTwo(Parser.Parse(input2)).Last().Message);
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day22";
        string input => File.ReadAllText($"{path}\\SandSlabs.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\SandSlabs_full.txt").Replace("\r", "");
    }
}
