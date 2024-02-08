using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.AdventOfCode.Y2023.Day22
{
    public class SandSlabsDataModel
    {
        public List<((long x, long y, long z) start, (long x, long y, long z) end)> Bricks { get; set; }
    }
}
