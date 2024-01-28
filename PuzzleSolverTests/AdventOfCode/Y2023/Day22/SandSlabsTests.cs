using Algorithms.AdventOfCode.Y2023.Day22;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day22
{
    public class SandSlabsTests
    {
        [Fact]
        public void Test()
        {
            var solver = new SandSlabs();
            Assert.Equal("5", solver.Solve(input, "Part 1").Last());
            Assert.Equal("509", solver.Solve(input2, "Part 1").Last());
        }
        [Fact]
        public void Test2()
        {
            var solver = new SandSlabs();
            Assert.Equal("7", solver.Solve(input, "Part 2").Last());
            Assert.Equal("102770", solver.Solve(input2, "Part 2").Last());
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day22";
        string input => File.ReadAllText($"{path}\\SandSlabs.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\SandSlabs_full.txt").Replace("\r", "");
    }
}
