using System;
using System.Linq;

namespace Algorithms.AdventOfCode.Y2023.Day20
{
    public static class Parser
    {
        public static Input Parse(string input)
        {
            return new Input()
            {
                ModuleConfiguration = input.Split('\n')
                            .Select(x => x.Split(new[] { " -> " }, StringSplitOptions.None))
                            .Select(x => (name: x[0], destinations: x[1].Split(new[] { ", " }, StringSplitOptions.None)))
                            .Select(x => x.name[0] == '&' || x.name[0] == '%' ? (type: x.name[0], name: x.name.Substring(1), x.destinations) : (type: ' ', x.name, x.destinations))
                            .Append((' ', "button", new string[] { "broadcaster" }))
                            .Append((' ', "output", Array.Empty<string>()))
                            .ToArray(),
            };
        }
    }
}
