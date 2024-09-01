namespace Algorithms
{
    public interface ISolution<TInput>
    {
        string Solution { get; }

        void Start(TInput input);
        void Update();
    }
}