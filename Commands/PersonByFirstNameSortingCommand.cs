using ParallelBucketSort.Commands.Core;
using ParallelBucketSort.Models;

namespace ParallelBucketSort.Commands;

public class PersonByFirstNameSortingCommand : ISortingCommand<Person, string>
{
    public int Compare(Person x, Person y)
    {
        return string.Compare(x.FirstName, y.FirstName, StringComparison.CurrentCultureIgnoreCase);
    }

    public Func<Person, string> Selector { get; set; } = p => p.FirstName;
}