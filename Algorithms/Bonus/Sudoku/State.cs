using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.Bonus.Sudoku
{
    public class State : StateBase
    {
        public string Grid { get; set; }
        public Stack<string> Queue { get; set; }
    }
}
