using Algorithms.AdventOfCode.Y2023.Day24;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day24
{
    public class NeverTellMeTheOddsTests
    {
        [Fact]
        public void Test()
        {
            var solver = new NeverTellMeTheOdds();
            Assert.Equal("2", solver.Solve(input, "Part 1").Last());
            Assert.Equal("16939", solver.Solve(input2, "Part 1").Last());
        }

        [Fact]
        public void Test2()
        {
            var solver = new NeverTellMeTheOdds();
            // Assert.Equal("47", solver.Solve(input, "Part 2").Last());
            Assert.Equal("16939", solver.Solve(input2, "Part 2").Last());
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day24";
        string input => File.ReadAllText($"{path}\\NeverTellMeTheOdds.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\NeverTellMeTheOdds_full.txt").Replace("\r", "");

    }

}
