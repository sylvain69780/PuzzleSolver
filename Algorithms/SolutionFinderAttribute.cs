using System;

namespace Algorithms
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SolutionFinderAttribute : Attribute
    {
        public string Description { get; }

        public SolutionFinderAttribute(string description)
        {
            Description = description;
        }
    }
}
