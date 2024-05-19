using ParallelBucketSort.Algorithm.Core;
using ParallelBucketSort.Algorithm.Strategies;

namespace ParallelBucketSort.Algorithm.Helpers;

public class SortingHelper<T>
{
    public static IParallelSortingStrategy<T> GetParallelStrategy(Type type)
    {
        if (type == typeof(string))
        {
            return (IParallelSortingStrategy<T>)new ParallelStringSortingStrategy();
        }

        return (IParallelSortingStrategy<T>)new ParallelNumberSortingStrategy();
    }

    public static ISequentialSortingStrategy<T> GetSequentialSortingStrategy(Type type)
    {
        if (type == typeof(string))
        {
            return (ISequentialSortingStrategy<T>)new SequentialStringSortingStrategy();
        }

        return (ISequentialSortingStrategy<T>)new SequentialNumberSortingStrategy();
    }
}