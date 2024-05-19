using ParallelBucketSort.Commands;
using ParallelBucketSort.Commands.Core;
using ParallelBucketSort.Comparers;
using ParallelBucketSort.Generators;
using ParallelBucketSort.Generators.Core;
using ParallelBucketSort.Models;
using ParallelBucketSort.Schemes.Core;

namespace ParallelBucketSort.Schemes;

public class PersonByAgeSortingScheme : ISortingScheme<Person, int>
{
    public IDataGenerator<Person> DataGenerator { get; set; } = new PersonDataGenerator();
    public ISortingCommand<Person, int> SortingCommand { get; set; } = new PersonByAgeSortingCommand();
    public IEqualityComparer<Person> EqualityComparer { get; set; } = new PersonByAgeEqualityComparer();
}