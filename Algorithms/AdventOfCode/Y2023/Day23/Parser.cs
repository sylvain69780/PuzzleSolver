namespace Algorithms.AdventOfCode.Y2023.Day23
{
    public static class Parser
    {
        public static Input Parse(string input)
        {
            return new Input
            {
                Map = input.Split('\n')
            };
        }
    }
}
