using Algorithms.AdventOfCode.Y2023.Day23;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day23
{
    public class ALongWalkTests
    {
        [Fact]
        public void Test()
        {
            var solver = new ALongWalk();
            Assert.Equal("94", solver.Solve(input, "Part 1").Last());
            Assert.Equal("2154", solver.Solve(input2, "Part 1").Last());
        }
        [Fact]
        public void Test2()
        {
            var solver = new ALongWalk();
            Assert.Equal("154", solver.Solve(input, "Part 2").Last());
            Assert.Equal("6654", solver.Solve(input2, "Part 2").Last());
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day23";
        string input => File.ReadAllText($"{path}\\ALongWalk.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\ALongWalk_full.txt").Replace("\r", "");
    }
}
