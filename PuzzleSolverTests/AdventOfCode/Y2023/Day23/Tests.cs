using Algorithms.AdventOfCode.Y2023.Day23;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day23
{
    public class Tests
    {
        [Fact]
        public void Test()
        {
            var solver = new Solutions();
            Assert.Equal("94", Solutions.PartOne(Parser.Parse(input)).Last().Message);
            Assert.Equal("2154", Solutions.PartOne(Parser.Parse(input2)).Last().Message);
        }
        [Fact]
        public void Test2()
        {
            var solver = new Solutions();
            Assert.Equal("154", Solutions.PartTwo(Parser.Parse(input)).Last().Message);
            Assert.Equal("6654", Solutions.PartTwo(Parser.Parse(input2)).Last().Message);
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day23";
        string input => File.ReadAllText($"{path}\\ALongWalk.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\ALongWalk_full.txt").Replace("\r", "");
    }
}
