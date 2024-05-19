using ParallelBucketSort.Commands.Core;

namespace ParallelBucketSort.Commands;

public class NumberSortingCommand : ISortingCommand<int, int>
{
    public int Compare(int x, int y)
    {
        return x.CompareTo(y);
    }

    public Func<int, int> Selector { get; set; } = i => i;
}