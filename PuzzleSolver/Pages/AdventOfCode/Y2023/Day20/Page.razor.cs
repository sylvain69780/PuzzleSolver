using Algorithms.AdventOfCode.Y2023.Day20;
using Algorithms;
using Microsoft.AspNetCore.Components;

namespace PuzzleSolver.Pages.AdventOfCode.Y2023.Day20
{
    public partial class Page : ComponentBase
    {
        private List<ISolutionFinder<Input>> solutionFinders = [new SolutionFinder1(), new SolutionFinder2()];
        private KeyValuePair<string, string>[] files = [
            new KeyValuePair<string,string>("Example","sample-data/AdventOfCode/Y2023/Day20/PulsePropagation.txt"),
            new KeyValuePair<string,string>("Example2","sample-data/AdventOfCode/Y2023/Day20/PulsePropagation2.txt"),
            new KeyValuePair<string,string>("Full","sample-data/AdventOfCode/Y2023/Day20/PulsePropagation_full.txt")
        ];
    }
}
