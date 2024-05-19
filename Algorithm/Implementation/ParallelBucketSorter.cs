using ParallelBucketSort.Algorithm.Core;
using ParallelBucketSort.Algorithm.Helpers;
using ParallelBucketSort.Commands.Core;

namespace ParallelBucketSort.Algorithm.Implementation;

public class ParallelBucketSorter(int bucketsCount, int threadsCount) : ISorter
{
    public int BucketsCount { get; set; } = bucketsCount;
    public int ThreadsCount { get; set; } = threadsCount;

    public List<T> Sort<T, TKey>(List<T> items, ISortingCommand<T, TKey> command)
    {
        var strategy = SortingHelper<TKey>.GetParallelStrategy(typeof(TKey));
        
        return strategy.Sort(items, command, BucketsCount, ThreadsCount);
    }
}