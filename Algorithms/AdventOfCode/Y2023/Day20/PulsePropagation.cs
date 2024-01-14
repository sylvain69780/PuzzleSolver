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
        static IEnumerable<string> PartTwoNaive(PulsePropagationDataModel input)
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

            var count = 0;
            var found = false;
            while (!found)
            {
                count++;
                var bfs = new Queue<(string name, string pulse, string pulseOrigin)>();
                bfs.Enqueue(("button", "-low", string.Empty));
                while (bfs.Count > 0 && !found)
                {
                    var newQueue = new Queue<(string name, string pulse, string pulseOrigin)>();
                    while (bfs.TryDequeue(out var module) && !found)
                    {
                        if (module.name == "rx")
                            if (module.pulse == "-low")
                            {
                                found = true;
                                break;
                            }
                            else
                                continue;
                        var type = types[module.name];
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
            yield return count.ToString();
        }

        static IEnumerable<string> PartTwo(PulsePropagationDataModel input)
        {
            var configuration = input.ModuleConfiguration!;
            var flipFlops = configuration
                .Where(x => x.type == '%')
                .ToDictionary(x => x.name, x => "off");
            var conjonctions = configuration
                .Where(x => x.type == '&')
                .Select(x => (x.name, connected: configuration.Where(y => y.destinations.Contains(x.name)).Select(y => (y.name, lastPulse: "-low")).ToDictionary(x => x.name, x => x.lastPulse)))
                .ToDictionary(x => x.name, x => x.connected);
            var types = configuration.ToDictionary(x => x.name, x => x.type);
            var destinations = configuration.ToDictionary(x => x.name, x => x.destinations);
            /*
                This time decided to go for a more organized and readable code, using abstract classes for Pulse and Module and creating implementation acondingly with the rules for each Module/Pulse type. But the parsing still looks a bit complicate to understand.
                For part 2 best way to solve was analyzing manually which configuration would make rx receive a low pulse, and that was:
                The conjunction module that target rx had to send a low pulse to it. In my case that was "xm"
                For "xm" to send a low, it required all modules that target it to send high pulses. In my case those were 4 modules.
                So I just had to check individually for those 4 how many button pushes they required to send a high pulse. With the right number for each, just had to get the product of them.            
            */
            var connectedToRx = configuration.Single(x => x.destinations.Contains("rx")).name;
            var modulesTargingIt = configuration.Where(x => x.destinations.Contains(connectedToRx)).Select(x => x.name).ToArray();
            var counters = modulesTargingIt.Select(x => (name: x, occu: new List<(int count,int time,int period)>())).ToDictionary(x => x.name, x => x.occu);

            var count = 0;
            var found = false;
            while (!found)
            {
                count++;
                var bfs = new Queue<(string name, string pulse, string pulseOrigin)>();
                var time = 0;
                bfs.Enqueue(("button", "-low", string.Empty));
                while (bfs.Count > 0 && !found)
                {
                    time++;
                    var newQueue = new Queue<(string name, string pulse, string pulseOrigin)>();
                    while (bfs.TryDequeue(out var module) && !found)
                    {
                        if (module.name==connectedToRx)
                        {
                            if (module.pulse == "-high" && counters.TryGetValue(module.pulseOrigin, out var list))
                            {
                                list.Add((count, time,list.Count>0 ? count - list[^1].count : 0));
                                if (counters.Values.All(x => x.Count >= 10))
                                    found = true;
                            }
                        }
                        if (module.name == "rx")
                            if (module.pulse == "-low")
                            {
                                found = true;
                                break;
                            }
                            else
                                continue;
                        var type = types[module.name];
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
            var solution = counters.Values.Select(x => x[^1].period).Aggregate(1L, (a, b) => (a * b));
            yield return solution.ToString();
        }


    }
}
