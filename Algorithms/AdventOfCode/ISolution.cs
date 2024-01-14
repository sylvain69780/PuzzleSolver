namespace Algorithms.AdventOfCode
{
    public interface ISolution
    {
        IEnumerable<string> Strategies { get; }

        IEnumerable<string> Solve(string input, string part);
    }
}