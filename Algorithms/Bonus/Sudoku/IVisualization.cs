using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.Bonus.Sudoku
{
    public interface IVisualization 
    {
         string Grid { get; }
         Stack<string> Queue { get;  }
    }
}
