using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.AdventOfCode.Y2023.Day20
{
    public class PulsePropagation
    {
        public IEnumerable<string> Solve(string input, string part)
        {
            var algo = _parts[part];
            var data = Parse(input);
            return algo(data);
        }

        StringDataModel Parse(string input)
        {
            return new StringDataModel()
            {
                DataToProcess = input,
            };
        }

        static Dictionary<string, Func<StringDataModel, IEnumerable<string>>> _parts = new()
        {
            { "Part 1" ,PartOne },
            { "Part 2", PartTwo }
        };

        public IEnumerable<string> Strategies => _parts.Keys;

        static IEnumerable<string> PartOne(StringDataModel input)
        {
            yield return input.DataToProcess;
        }
        static IEnumerable<string> PartTwo(StringDataModel input)
        {
            yield return input.DataToProcess;
        }


    }
}
