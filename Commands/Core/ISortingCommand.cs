namespace ParallelBucketSort.Commands.Core;

public interface ISortingCommand<T, TKey> : IComparer<T>
{
    public Func<T, TKey> Selector { get; set; }
}