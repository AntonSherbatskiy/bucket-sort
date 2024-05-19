using ParallelBucketSort.Commands.Core;
using ParallelBucketSort.Generators.Core;

namespace ParallelBucketSort.Schemes.Core;

public interface ISortingScheme<TValue, TKey>
{
    IDataGenerator<TValue> DataGenerator { get; set; }
    ISortingCommand<TValue, TKey> SortingCommand { get; set; }
    IEqualityComparer<TValue> EqualityComparer { get; set; }
}