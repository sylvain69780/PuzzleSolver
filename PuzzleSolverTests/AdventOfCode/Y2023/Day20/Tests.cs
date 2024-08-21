using Algorithms;
using Algorithms.AdventOfCode.Y2023.Day20;
using System.IO;
using System.Reflection;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day20
{
    public class Tests
    {
        [Fact]
        public void Test()
        {
            var strategies = new List<(string,Func<Input,IEnumerable<State>>)>();
            Type type = typeof(Solutions);
            // Get all methods with the specified attribute
            MethodInfo[] methodsWithAttribute = type.GetMethods()
                .Where(method => Attribute.IsDefined(method, typeof(SolutionMethodAttribute)))
                .ToArray();
            foreach (var method in methodsWithAttribute)
            {
                var customAttribute = (SolutionMethodAttribute)Attribute.GetCustomAttribute(method, typeof(SolutionMethodAttribute))!;
                strategies.Add((customAttribute.Description, (Func<Input, IEnumerable<State>>)method.CreateDelegate(typeof(Func<Input, IEnumerable<State>>))));
            }
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
