using Algorithms.AdventOfCode.Y2023.Day20;

namespace PuzzleSolverTests.AdventOfCode.Y2023.Day20
{
    public class Tests
    {
        [Fact]
        public void Test()
        {
            //var strategies = new List<(string,Func<Input,IEnumerable<Func<State>>>)>();
            //Type type = typeof(Solutions);
            //// Get all methods with the specified attribute
            //MethodInfo[] methodsWithAttribute = type.GetMethods()
            //    .Where(method => Attribute.IsDefined(method, typeof(SolutionMethodAttribute)))
            //    .ToArray();
            //foreach (var method in methodsWithAttribute)
            //{
            //    var customAttribute = (SolutionMethodAttribute)Attribute.GetCustomAttribute(method, typeof(SolutionMethodAttribute))!;
            //    strategies.Add((customAttribute.Description, (Func<Input, IEnumerable<Func<State>>>)method.CreateDelegate(typeof(Func<Input, IEnumerable<Func<State>>>))));
            //}
            var solution1 = new SolutionFinder1();
            solution1.Start(Parser.Parse(input));
            while (solution1.IsRunning)
                solution1.Update();
            Assert.Equal("32000000", solution1.Solution);
            solution1.Start(Parser.Parse(input1));
            while (solution1.IsRunning)
                solution1.Update();
            Assert.Equal("11687500", solution1.Solution);
            solution1.Start(Parser.Parse(input2));
            while (solution1.IsRunning)
                solution1.Update();
            Assert.Equal("818649769", solution1.Solution);
            var solution2 = new SolutionFinder2();
            solution2.Start(Parser.Parse(input2));
            while (solution2.IsRunning)
                solution2.Update();
            Assert.Equal("246313604784977", solution2.Solution);
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\AdventOfCode\\Y2023\\Day20";
        string input => File.ReadAllText($"{path}\\PulsePropagation.txt").Replace("\r", "");
        string input1 => File.ReadAllText($"{path}\\PulsePropagation2.txt").Replace("\r", "");

        string input2 => File.ReadAllText($"{path}\\PulsePropagation_full.txt").Replace("\r","");
    }
}
