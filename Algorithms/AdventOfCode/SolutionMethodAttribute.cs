namespace Algorithms.AdventOfCode
{
    internal class SolutionMethodAttribute : Attribute
    {
        public string Description { get; }

        public SolutionMethodAttribute(string description)
        {
            Description = description;
        }
    }
}
