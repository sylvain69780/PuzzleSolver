﻿using Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Algorithms.Bonus.Sudoku;


namespace PuzzleSolverTests.Bonus.Sudoku
{
    public class Tests
    {
        [Fact]
        public void Test()
        {
            var solutionFinder = new SudokuSolutionFinder();
            solutionFinder.Start(Parser.Parse(input1));
            while (solutionFinder.IsRunning)
                solutionFinder.Update();
            Assert.Equal("461352987579168432832794156394816275218537694657429318945273861186945723723681549", solutionFinder.Grid);
            solutionFinder.Start(Parser.Parse(input2));
            while (solutionFinder.IsRunning)
                solutionFinder.Update();
            Assert.Equal(@"734596281165287439298143567912368745576429813483751926341875692627934158859612374", solutionFinder.Grid);
        }

        const string path = "..\\..\\..\\..\\PuzzleSolver\\wwwroot\\sample-data\\Bonus\\Sudoku";
        string input1 => File.ReadAllText($"{path}\\Sudoku.txt").Replace("\r", "");
        string input2 => File.ReadAllText($"{path}\\Sudoku_full.txt").Replace("\r", "");
    }
}
