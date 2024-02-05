using Algorithms.AdventOfCode.Y2023.Day23;
using System.Reflection;

namespace Algorithms.AdventOfCode
{
    public abstract class SolutionBase<T>
    {
        private Dictionary<string, Func<T, IEnumerable<string>>>? cachedParts;
        private Dictionary<string, Func<T, IEnumerable<string>>> Parts()
        {
            if (cachedParts is null)
            {
                var parts = new Dictionary<string, Func<T, IEnumerable<string>>>();
                // Get the type of the current class
                Type type = this.GetType();
                // Get all methods with the specified attribute
                MethodInfo[] methodsWithAttribute = type.GetMethods()
                    .Where(method => Attribute.IsDefined(method, typeof(SolutionMethodAttribute)))
                    .ToArray();
                foreach (var method in methodsWithAttribute)
                {
                    var customAttribute = (SolutionMethodAttribute)Attribute.GetCustomAttribute(method, typeof(SolutionMethodAttribute))!;
                    parts.Add(customAttribute.Description, (Func<T, IEnumerable<string>>)method.CreateDelegate(typeof(Func<T, IEnumerable<string>>)));
                }
                cachedParts = parts;
            }
            return cachedParts;
        }

        public IEnumerable<string> Strategies => Parts().Keys;

        public IEnumerable<string> Solve(string input, string part)
        {
            var algo = Parts()[part];
            var data = Parse(input);
            return algo(data);
        }
        protected abstract T Parse(string input);
    }
}