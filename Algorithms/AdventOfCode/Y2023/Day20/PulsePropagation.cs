using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        static PulsePropagationDataModel Parse(string input)
        {
            var broadcaster = new string[] { "broadcaster" };
            return new PulsePropagationDataModel()
            {
         
                ModuleConfiguration = input.Split("\n")
                            .Select(x => x.Split(" -> "))
                            .Select(x => (name: x[0], destinations: x[1].Split(", ")))
                            .Select(x =>
                            {
                                if (x.name[0] == '&' || x.name[0] == '%')
                                    return (type: x.name[0], name: x.name[1..], x.destinations);
                                else
                                    return (type:' ',x.name,x.destinations);
                            })
                            .Append((' ', "button", broadcaster))
                            .Append((' ', "output", Array.Empty<string>()))
                            .ToArray(),
            };
        }

        static Dictionary<string, Func<PulsePropagationDataModel, IEnumerable<string>>> _parts = new()
        {
            { "Part 1" ,PartOne },
            { "Part 2", PartTwo }
        };

        public IEnumerable<string> Strategies => _parts.Keys;

        static IEnumerable<string> PartOne(PulsePropagationDataModel input)
        {
            var c = input.ModuleConfiguration!;
            var flipFlops = c
                .Where(x => x.type == '%')
                .ToDictionary(x =>x.name,x=> "off");
            var conjonctions = c
                .Where(x => x.type == '&')
                .Select(x => (x.name,connected:c.Where(y => y.destinations.Contains(x.name)).Select(y => (y.name,lastPulse:"-low")).ToArray()))
                .ToDictionary(x=>x.name,x=>x.connected);
            var types = c.ToDictionary(x => x.name, x => x.type);
            var destinations = c.ToDictionary(x => x.name, x => x.destinations);

            var bfs = new Queue<(string name, string pulse)>();
            bfs.Enqueue(("button", "-low"));
            while ( bfs.Count>0)
            {
                var newQueue = new Queue<(string name, string pulse)>();
                while(bfs.TryDequeue(out var pulse))
                {
                    var type = types[pulse.name];
                    if (type == ' ')
                    {
                        foreach (var item in destinations[pulse.name])
                            newQueue.Enqueue((item, pulse.pulse));
                    }
                    if (type == '%' && pulse.pulse == "-low")
                    {
                        var v = flipFlops[pulse.name];
                        flipFlops[pulse.name] = v == "on" ? "off" : "on";
                        var p = v == "on" ? "-low" : "-high";
                        foreach (var item in destinations[pulse.name])
                            newQueue.Enqueue((item, p));
                    }
                }
                bfs = newQueue;
            }

            yield return string.Empty;
        }
        static IEnumerable<string> PartTwo(PulsePropagationDataModel input)
        {
            yield return string.Empty;
        }


    }
}
