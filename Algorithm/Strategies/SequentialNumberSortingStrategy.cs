using System.Collections.Concurrent;
using ParallelBucketSort.Algorithm.Core;
using ParallelBucketSort.Algorithm.Implementation;
using ParallelBucketSort.Commands.Core;

namespace ParallelBucketSort.Algorithm.Strategies;

public class SequentialNumberSortingStrategy : ISequentialSortingStrategy<int>
{
    private int _bucketsCount { get; set; }
    private MutatingBubbleSort _mutatingSorter = new();
    
    public List<T> Sort<T>(List<T> items, ISortingCommand<T, int> command, int bucketsCount)
    {
        _bucketsCount = bucketsCount;
        
        var minValue = items.Min(command);
        var maxValue = items.Max(command);
        var buckets = CreateBuckets(items, command, minValue, maxValue);
        
        foreach (var bucket in buckets)
        {
            _mutatingSorter.Sort(bucket, command);
        }
        
        return buckets.SelectMany(b => b.ToList()).ToList();
    }
    
    private List<T>[] CreateBuckets<T>(List<T> values, ISortingCommand<T, int> sortingCommand, T minValue, T maxValue)
    {
        var buckets = new List<T>[_bucketsCount];
        var bucketStep = Math.Max((sortingCommand.Selector(maxValue) - sortingCommand.Selector(minValue)) / _bucketsCount, 1);
        
        for (var i = 0; i < buckets.Length; i++)
        {
            buckets[i] = [];
        }
        
        foreach (var value in values)
        {
            var bucketIndex = Math.Min(((sortingCommand.Selector(value) - sortingCommand.Selector(minValue)) / bucketStep), _bucketsCount - 1);
            buckets[bucketIndex].Add(value);
        }
        
        return buckets.AsParallel().Select(b => b.ToList()).ToArray();
    }
}