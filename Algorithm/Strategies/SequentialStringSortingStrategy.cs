using System.Collections.Concurrent;
using ParallelBucketSort.Algorithm.Core;
using ParallelBucketSort.Algorithm.Implementation;
using ParallelBucketSort.Commands.Core;

namespace ParallelBucketSort.Algorithm.Strategies;

public class SequentialStringSortingStrategy : ISequentialSortingStrategy<string>
{
    private MutatingBubbleSort _mutatingBubbleSort { get; set; } = new();
    private int _bucketsCount { get; set; }
    
    public List<T> Sort<T>(List<T> items, ISortingCommand<T, string> command, int bucketsCount)
    {
        _bucketsCount = bucketsCount;
        var buckets = CreateBuckets(items, command);
        
        foreach (var bucket in buckets)
        {
            _mutatingBubbleSort.Sort(bucket, command);
        }
        
        return buckets.SelectMany(b => b).ToList();
    }

    private List<T>[] CreateBuckets<T>(List<T> items, ISortingCommand<T, string> command)
    {
        var buckets = new Dictionary<char, ConcurrentBag<T>>(_bucketsCount);
        var minASCII = items.Min(el => command.Selector(el)[0]);

        for (int i = 0; i < _bucketsCount; i++)
        {
            buckets.Add(minASCII, []);
            minASCII++;
        }
        
        foreach (var item in items)
        {
            var prop = command.Selector.Invoke(item);
            var letter = char.ToLower(prop[0]);
            
            if (!buckets.ContainsKey(letter))
            {
                buckets.Add(char.ToLower(letter), []);
            }
            
            buckets[char.ToLower(letter)].Add(item);
        }
        
        return buckets.Select(b => b.Value.ToList()).ToArray();
    }
}