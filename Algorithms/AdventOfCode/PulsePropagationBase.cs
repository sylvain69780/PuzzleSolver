namespace Algorithms.AdventOfCode
{
    public abstract class PulsePropagationBase<T>
    {
        protected static Dictionary<string, Func<T, IEnumerable<string>>> _parts = [];

        public IEnumerable<string> Strategies => _parts.Keys;

        public IEnumerable<string> Solve(string input, string part)
        {
            var algo = _parts[part];
            var data = Parse(input);
            return algo(data);
        }
        protected abstract T Parse(string input);
    }
}