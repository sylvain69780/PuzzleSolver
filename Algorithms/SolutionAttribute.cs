using System;

namespace Algorithms
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SolutionAttribute : Attribute
    {
        public string Description { get; }

        public SolutionAttribute(string description)
        {
            Description = description;
        }
    }
}
