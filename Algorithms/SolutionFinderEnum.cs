using System.Collections.Generic;
using System.Linq;

namespace Algorithms
{
    public abstract class SolutionFinderEnum<TInput> : ISolutionFinder<TInput>
    {

        private IEnumerator<int> enumerator;
        public bool IsRunning { get; protected set; }

        public int Iteration { get; private set; }

        public string Solution { get; protected set; }
        public void Start(TInput input)
        {
            var emum = Steps(input);
            enumerator = emum.GetEnumerator();
            IsRunning = true;
            Solution = null;
            Iteration = 0;
        }

        public void Update()
        {
            if (IsRunning)
                IsRunning = enumerator.MoveNext();
            Iteration++;
        }

        protected abstract IEnumerable<int> Steps(TInput input);

    }
}