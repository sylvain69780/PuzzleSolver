namespace Algorithms
{
    public interface ISolutionFinder<TInput>
    {
        string Solution { get; }

        bool IsRunning { get; }

        int Iteration {  get; }

        void Start(TInput input);
        void Update();
    }
}