namespace Algorithms
{
    public interface ISolution<TInput>
    {
        string Solution { get; }

        bool IsRunning { get; }

        void Start(TInput input);
        void Update();
    }
}