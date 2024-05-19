using ParallelBucketSort.Models;

namespace ParallelBucketSort.Comparers;

public class PersonByAgeEqualityComparer : IEqualityComparer<Person>
{
    public bool Equals(Person x, Person y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Age == y.Age;
    }

    public int GetHashCode(Person obj)
    {
        return obj.Age;
    }
}