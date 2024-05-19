using System.Numerics;
using ParallelBucketSort.Algorithm.Core;
using ParallelBucketSort.Algorithm.Helpers;
using ParallelBucketSort.Commands.Core;

namespace ParallelBucketSort.Algorithm.Implementation;

public class SequentialBucketSorter(int bucketsCount) : ISorter
{
    private int _bucketsCount { get; set; } = bucketsCount;

    public List<T1> Sort<T1, TKey>(List<T1> items, ISortingCommand<T1, TKey> command)
    {
        var strategy = SortingHelper<TKey>.GetSequentialSortingStrategy(typeof(TKey));
        
        return strategy.Sort(items, command, _bucketsCount);
    }
}