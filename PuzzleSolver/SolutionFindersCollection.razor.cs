using Algorithms;
using Algorithms.AdventOfCode.Y2023.Day20;
using Microsoft.AspNetCore.Components;

namespace PuzzleSolver
{
    public partial class SolutionFindersCollection<TInput, TVisualization> : ComponentBase
    {
        [Parameter]
        public List<ISolutionFinder<TInput>>? SolutionFinders { get; set; }
        [Parameter]
        public KeyValuePair<string, string>[]? InputFiles { get; set; }
        [Parameter]
        public Func<string,TInput>? Parse { get; set; }
        [Parameter]
        public RenderFragment<TVisualization>? ChildContent { get; set; }

        private int selectedSolution = 0;
        private int delay = 1;

        private TInput? input;
        private ISolutionFinder<TInput> SelectedSolutionFinder => SolutionFinders![selectedSolution];
        private TVisualization Visualisation => (TVisualization)SelectedSolutionFinder;
    }
}
