using Algorithms;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PuzzleSolver
{
    public static class PuzzleHelper
    {
        public static string ToStringCSS(this double value) =>
            value.ToString(CultureInfo.GetCultureInfo("en-GB"));

        public static string GetPuzzleTitle(Type type) => ((SolutionAttribute)Attribute.GetCustomAttribute(type, typeof(SolutionAttribute))!).Description;

        public static List<(string Name, Func<TInput, IEnumerable<Func<TState>>> Method)> GetPuzzleMethods<TInput, TState>(Type type)
        {
            MethodInfo[] methodsWithAttribute = type.GetMethods()
                .Where(method => Attribute.IsDefined(method, typeof(SolutionMethodAttribute)))
                .ToArray();
            var methods = new List<(string Name, Func<TInput, IEnumerable<Func<TState>>> Method)>();
            foreach (var method in methodsWithAttribute)
            {
                var customAttribute = (SolutionMethodAttribute)Attribute.GetCustomAttribute(method, typeof(SolutionMethodAttribute))!;
                methods.Add((customAttribute.Description, (Func<TInput, IEnumerable<Func<TState>>>)method.CreateDelegate(typeof(Func<TInput, IEnumerable<Func<TState>>>))));
            }
            return methods;
        }

    }
}
