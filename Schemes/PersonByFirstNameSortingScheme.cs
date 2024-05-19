using ParallelBucketSort.Commands;
using ParallelBucketSort.Commands.Core;
using ParallelBucketSort.Comparers;
using ParallelBucketSort.Generators;
using ParallelBucketSort.Generators.Core;
using ParallelBucketSort.Models;
using ParallelBucketSort.Schemes.Core;

namespace ParallelBucketSort.Schemes;

public class PersonByFirstNameSortingScheme : ISortingScheme<Person, string>
{
    public IDataGenerator<Person> DataGenerator { get; set; } = new PersonDataGenerator();
    public ISortingCommand<Person, string> SortingCommand { get; set; } = new PersonByFirstNameSortingCommand();
    public IEqualityComparer<Person> EqualityComparer { get; set; } = new PersonByFirstNameEqualityComparer();
}