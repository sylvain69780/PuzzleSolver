using Algorithms.AdventOfCode.Y2023.Day25;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day25
{
    public class SnowverloadTests
    {
        [Fact]
        public void Test()
        {
            var solver = new Snowverload();
            Assert.Equal("2", solver.Solve(input, "Part 1").Last());
            Assert.Equal("16939", solver.Solve(input2, "Part 1").Last());
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day25";
        string input => File.ReadAllText($"{path}\\Snowverload.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\Snowverload_full.txt").Replace("\r", "");
    }
}
