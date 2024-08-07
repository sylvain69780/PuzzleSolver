using Algorithms.AdventOfCode.Y2023.Day20;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day20
{
    public class Tests
    {
        [Fact]
        public void Test()
        {
            Assert.Equal("32000000", Solutions.PartOne(Parser.Parse(input)).Last().Message);
            Assert.Equal("11687500", Solutions.PartOne(Parser.Parse(input1)).Last().Message);
            Assert.Equal("818649769", Solutions.PartOne(Parser.Parse(input2)).Last().Message);
            Assert.Equal("246313604784977", Solutions.PartTwo(Parser.Parse(input2)).Last().Message);
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day20";
        string input => File.ReadAllText($"{path}\\PulsePropagation.txt").Replace("\r", "");
        string input1 => File.ReadAllText($"{path}\\PulsePropagation2.txt").Replace("\r", "");

        string input2 => File.ReadAllText($"{path}\\PulsePropagation_full.txt").Replace("\r","");
    }
}
