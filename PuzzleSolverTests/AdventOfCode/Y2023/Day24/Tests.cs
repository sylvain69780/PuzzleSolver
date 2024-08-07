using Algorithms.AdventOfCode.Y2023.Day24;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day24
{
    public class Tests
    {
        [Fact]
        public void Test()
        {
            Assert.Equal("2", Solutions.PartOne(Parser.Parse(input)).Last().Message);
            Assert.Equal("16939", Solutions.PartOne(Parser.Parse(input2)).Last().Message);
        }

        [Fact]
        public void Test2()
        {
            var solver = new Solutions();
            Assert.Equal("47", Solutions.PartTwo(Parser.Parse(input)).Last().Message);
            Assert.Equal("931193307668256", Solutions.PartTwo(Parser.Parse(input2)).Last().Message);
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day24";
        string input => File.ReadAllText($"{path}\\NeverTellMeTheOdds.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\NeverTellMeTheOdds_full.txt").Replace("\r", "");

    }

}
