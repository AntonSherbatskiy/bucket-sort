namespace ParallelBucketSort.Test;

public class ParallelTestResult : TestResult
{
    public int ThreadsCount { get; set; }
    public double Speedup { get; set; }
    public double Efficiency { get; set; }

    public override string ToString()
    {
        return
            $"{base.ToString()}, {nameof(ThreadsCount)}: {ThreadsCount}, {nameof(Speedup)}: {Speedup}, {nameof(Efficiency)}: {Efficiency}";
    }
}