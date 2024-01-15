using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.AdventOfCode.Y2023.Day21
{
    public class StepCounterDataModel
    {
        public required string[] Map { get; set; }
        public required (int x,int y) Pos { get; set; }
    }
}
