using Algorithms.AdventOfCode.Y2023.Day25;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day25
{
    public class Tests
    {
        [Fact]
        public void Test()
        {
            Assert.Equal("54", Solutions.PartOne(Parser.Parse(input)).Last().Message);
            Assert.Equal("568214", Solutions.PartOne(Parser.Parse(input2)).Last().Message);
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day25";
        string input => File.ReadAllText($"{path}\\Snowverload.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\Snowverload_full.txt").Replace("\r", "");
    }
}
