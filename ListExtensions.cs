namespace ParallelBucketSort;

public static class ListExtensions
{
    public static string AsString(this List<int> items)
    {
        return string.Join(" ", items);
    }

    public static List<int> Extract(this List<int>[] buckets)
    {
        return buckets.SelectMany(b => b).ToList();
    }
}