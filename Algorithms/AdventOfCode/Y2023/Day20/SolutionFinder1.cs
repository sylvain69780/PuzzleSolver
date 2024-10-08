﻿using System.Collections.Generic;
using System.Linq;

namespace Algorithms.AdventOfCode.Y2023.Day20
{
    [SolutionFinder("Pulse Propagation - Part 1")]
    public class SolutionFinder1 : SolutionFinderEnum<Input> , IVisualizationInfo
    {
        public string Info { get; private set; } = "Bonour";

        protected override IEnumerable<int> Steps(Input input)
        {
            var configuraton = input.ModuleConfiguration;
            var flipFlops = configuraton
                .Where(x => x.type == '%')
                .ToDictionary(x => x.name, x => "off");
            var conjonctions = configuraton
                .Where(x => x.type == '&')
                .Select(x => (x.name, connected: configuraton.Where(y => y.destinations.Contains(x.name)).Select(y => (y.name, lastPulse: "-low")).ToDictionary(e => e.name, e => e.lastPulse)))
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
                    var module = bfs.Dequeue();
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
            Solution = (high * low).ToString();
            yield return 0;
        }

    }
}
