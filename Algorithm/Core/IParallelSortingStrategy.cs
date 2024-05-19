using ParallelBucketSort.Commands.Core;

namespace ParallelBucketSort.Algorithm.Core;

public interface IParallelSortingStrategy<TKey>
{
    List<T> Sort<T>(List<T> items, ISortingCommand<T, TKey> command, int bucketsCount, int threadsCount);
}