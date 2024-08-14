﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Bonus.Sudoku
{
    public class Parser : IParser<Input>
    {
        public Input Parse(string input)
        {
            return new Input
            {
                Grid = String.Concat(input.Where(c => c != '\n' && c != '\r'))
            };
        }
    }
}