using ParallelBucketSort.Commands.Core;
using ParallelBucketSort.Models;

namespace ParallelBucketSort.Commands;

public class PersonByAgeSortingCommand : ISortingCommand<Person, int>
{
    public int Compare(Person x, Person y)
    {
        return x.Age.CompareTo(y.Age);
    }

    public Func<Person, int> Selector { get; set; } = person => person.Age;
}