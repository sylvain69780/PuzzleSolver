﻿using System;

namespace Algorithms.AdventOfCode
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SolutionMethodAttribute : Attribute
    {
        public string Description { get; }

        public SolutionMethodAttribute(string description)
        {
            Description = description;
        }
    }
}