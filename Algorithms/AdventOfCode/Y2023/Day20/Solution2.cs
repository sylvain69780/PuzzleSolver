using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.AdventOfCode.Y2023.Day20
{
    [Solution("Pulse Propagation - Part 2")]
    public class Solution2 : ISolution<Input>
    {
        public string Solution { get; private set; }

        private IEnumerator<int> enumerator;
        public bool IsRunning { get; private set; }
        public void Start(Input input)
        {
            var emum = PartTwo(input);
            enumerator = emum.GetEnumerator();
            IsRunning = true;
            Solution = null;
        }

        public void Update()
        {
            if (IsRunning)
                IsRunning = enumerator.MoveNext();
        }

        private IEnumerable<int> PartTwo(Input input)
        {
            var configuration = input.ModuleConfiguration;
            var flipFlops = configuration
                .Where(x => x.type == '%')
                .ToDictionary(x => x.name, x => "off");
            var conjonctions = configuration
                .Where(x => x.type == '&')
                .Select(x => (x.name, connected: configuration.Where(y => y.destinations.Contains(x.name)).Select(y => (y.name, lastPulse: "-low")).ToDictionary(e => e.name, e => e.lastPulse)))
                .ToDictionary(x => x.name, x => x.connected);
            var types = configuration.ToDictionary(x => x.name, x => x.type);
            var destinations = configuration.ToDictionary(x => x.name, x => x.destinations);
 
            var connectedToRx = configuration.Single(x => x.destinations.Contains("rx")).name;
            var modulesTargingIt = configuration.Where(x => x.destinations.Contains(connectedToRx)).Select(x => x.name).ToArray();
            var counters = modulesTargingIt.Select(x => (name: x, occu: new List<(int count, int time, int period)>())).ToDictionary(x => x.name, x => x.occu);

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
                    var module = bfs.Dequeue();
                    if (module.name == connectedToRx)
                    {
                        if (module.pulse == "-high" && counters.TryGetValue(module.pulseOrigin, out var list))
                        {
                            list.Add((count, time, list.Count > 0 ? count - list[list.Count - 1].count : 0));
                            if (counters.Values.All(x => x.Count >= 2))
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
                            bfs.Enqueue((item, module.pulse, module.name));
                    }
                    if (type == '%')
                    {
                        if (module.pulse == "-high")
                            continue;
                        var v = flipFlops[module.name];
                        flipFlops[module.name] = v == "on" ? "off" : "on";
                        var p = v == "on" ? "-low" : "-high";
                        foreach (var item in destinations[module.name])
                            bfs.Enqueue((item, p, module.name));
                    }
                    if (type == '&')
                    {
                        var conjModule = conjonctions[module.name];
                        conjModule[module.pulseOrigin] = module.pulse;
                        var pulse = conjModule.All(x => x.Value == "-high") ? "-low" : "-high";
                        foreach (var targetModule in destinations[module.name])
                            bfs.Enqueue((targetModule, pulse, module.name));
                    }
                }
                yield return 0;
            }
            var solution = counters.Values.Select(x => x[x.Count - 1].period).Aggregate(1L, (a, b) => (a * b));
            Solution = solution.ToString();
            yield return 0;
        }
    }
}
