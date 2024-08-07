using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    public interface IParser<TInput>
    {
        TInput Parse(string input);
    }
}
