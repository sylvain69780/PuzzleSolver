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
                                    return (type: ' ', x.name, x.destinations);
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
            var configuraton = input.ModuleConfiguration!;
            var flipFlops = configuraton
                .Where(x => x.type == '%')
                .ToDictionary(x => x.name, x => "off");
            var conjonctions = configuraton
                .Where(x => x.type == '&')
                .Select(x => (x.name, connected: configuraton.Where(y => y.destinations.Contains(x.name)).Select(y => (y.name, lastPulse: "-low")).ToDictionary(x => x.name, x => x.lastPulse)))
                .ToDictionary(x => x.name, x => x.connected);
            var types = configuraton.ToDictionary(x => x.name, x => x.type);
            var destinations = configuraton.ToDictionary(x => x.name, x => x.destinations);
            var high = 0;
            var low = 0;
            for (var i = 0; i < 1000; i++)
            {
                var bfs = new Queue<(string name, string pulse, string pulseOrigin)>();
                bfs.Enqueue(("button", "-low", string.Empty));
                while (bfs.Count > 0)
                {
                    var newQueue = new Queue<(string name, string pulse, string pulseOrigin)>();
                    while (bfs.TryDequeue(out var module))
                    {
                        if (module.name != "button")
                            if (module.pulse == "-high")
                                high++;
                            else
                                low++;
                        if (!types.TryGetValue(module.name, out var type))
                            continue;
                        if (type == ' ')
                        {
                            foreach (var item in destinations[module.name])
                                newQueue.Enqueue((item, module.pulse, module.name));
                        }
                        if (type == '%')
                        {
                            if (module.pulse == "-high")
                                continue;
                            var v = flipFlops[module.name];
                            flipFlops[module.name] = v == "on" ? "off" : "on";
                            var p = v == "on" ? "-low" : "-high";
                            foreach (var item in destinations[module.name])
                                newQueue.Enqueue((item, p, module.name));
                        }
                        if (type == '&')
                        {
                            var conjModule = conjonctions[module.name];
                            conjModule[module.pulseOrigin] = module.pulse;
                            var pulse = conjModule.All(x => x.Value == "-high") ? "-low" : "-high";
                            foreach (var targetModule in destinations[module.name])
                                newQueue.Enqueue((targetModule, pulse, module.name));
                        }
                    }
                    bfs = newQueue;
                }
            }

            yield return (high * low).ToString();
        }
        static IEnumerable<string> PartTwo(PulsePropagationDataModel input)
        {
            yield return string.Empty;
        }


    }
}
