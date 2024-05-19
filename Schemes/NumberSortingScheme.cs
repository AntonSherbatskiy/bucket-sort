using ParallelBucketSort.Commands;
using ParallelBucketSort.Commands.Core;
using ParallelBucketSort.Generators;
using ParallelBucketSort.Generators.Core;
using ParallelBucketSort.Schemes.Core;

namespace ParallelBucketSort.Schemes;

public class NumberSortingScheme : ISortingScheme<int, int>
{
    public IDataGenerator<int> DataGenerator { get; set; }
    public ISortingCommand<int, int> SortingCommand { get; set; }
    public IEqualityComparer<int> EqualityComparer { get; set; }

    public NumberSortingScheme(int min, int max)
    {
        DataGenerator = new MinMaxRandomArrayGenerator(min, max);
        EqualityComparer = EqualityComparer<int>.Default;
        SortingCommand = new NumberSortingCommand();
    }
}