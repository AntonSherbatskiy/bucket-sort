using System.Collections.Concurrent;
using ParallelBucketSort.Algorithm.Core;
using ParallelBucketSort.Algorithm.Implementation;
using ParallelBucketSort.Commands.Core;

namespace ParallelBucketSort.Algorithm.Strategies;

public class ParallelNumberSortingStrategy : IParallelSortingStrategy<int>
{
    private int _bucketsCount { get; set; }
    private int _threadsCount { get; set; }
    private MutatingBubbleSort _mutatingSorter = new();
    
    public List<T> Sort<T>(List<T> items, ISortingCommand<T, int> command, int bucketsCount, int threadsCount)
    {
        _bucketsCount = bucketsCount;
        _threadsCount = threadsCount;
        
        var _minValue = items.Min(command);
        var _maxValue = items.Max(command);
        var buckets = CreateBuckets(items, command, _minValue, _maxValue);
        var threadStep = Math.Max(1, buckets.Length / _threadsCount);
        var start = 0;
        var _tasks = new List<Task>();
        while (start < buckets.Length)
        {
            var end = Math.Min(start + threadStep, buckets.Length);
            var start1 = start;
            _tasks.Add(Task.Run(() =>
            {
                Parallel.For(start1, end, j =>
                {
                    _mutatingSorter.Sort(buckets[j], command);
                });
            }));
            start += threadStep;
        }
        Task.WaitAll(_tasks.ToArray());
        var extractedAsEnumerable = buckets.SelectMany(b => b.ToList()).ToList();
        return extractedAsEnumerable;
    }
    
    private List<T>[] CreateBuckets<T>(List<T> values, ISortingCommand<T, int> sortingCommand, T minValue, T maxValue)
    {
        var tasks = new List<Task>();
        var buckets = new ConcurrentBag<T>[_bucketsCount];
        
        var bucketStep = Math.Max((sortingCommand.Selector(maxValue) - sortingCommand.Selector(minValue)) / _bucketsCount, 1);
        var threadStep = Math.Max(1, values.Count / _threadsCount);
        var start = 0;
        for (var i = 0; i < buckets.Length; i++)
        {
            buckets[i] = [];
        }
        while (start < values.Count)
        {
            var end = Math.Min(start + threadStep, values.Count);
            var start1 = start;
            var end1 = end;
            tasks.Add(Task.Run(() =>
            {
                for (int i = start1; i < end1; i++)
                {
                    var bucketIndex = Math.Min(((sortingCommand.Selector(values[i]) - sortingCommand.Selector(minValue)) / bucketStep), _bucketsCount - 1);
                    buckets[bucketIndex].Add(values[i]);
                }
            }));
            start += threadStep;
        }
        Task.WaitAll(tasks.ToArray());
        return buckets.AsParallel().Select(b => b.ToList()).ToArray();
    }
}