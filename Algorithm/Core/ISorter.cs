using ParallelBucketSort.Commands.Core;

namespace ParallelBucketSort.Algorithm.Core;

public interface ISorter
{
    List<T> Sort<T, TKey>(List<T> items, ISortingCommand<T, TKey> command);
}