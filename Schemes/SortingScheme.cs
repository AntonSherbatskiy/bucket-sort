namespace ParallelBucketSort.Schemes;

public static class SortingScheme
{
    public static PersonByAgeSortingScheme PersonByAgeSortingScheme => new();
    public static NumberSortingScheme RandomNumberSortingScheme => new(1, 100000);
}