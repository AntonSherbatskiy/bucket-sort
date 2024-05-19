using ParallelBucketSort.Commands.Core;

namespace ParallelBucketSort.Algorithm.Core;

public interface ISequentialSortingStrategy<TKey>
{
    List<T> Sort<T>(List<T> items, ISortingCommand<T, TKey> command, int bucketsCount);
}