using System.Collections.Concurrent;
using ParallelBucketSort.Algorithm.Core;
using ParallelBucketSort.Algorithm.Implementation;
using ParallelBucketSort.Commands.Core;

namespace ParallelBucketSort.Algorithm.Strategies;

public class ParallelStringSortingStrategy : IParallelSortingStrategy<string>
{
    private MutatingBubbleSort _mutatingBubbleSort { get; set; } = new();
    private int _bucketsCount { get; set; }
    private int _threadsCount { get; set; }
    private List<Task> _tasks { get; set; }
    public List<T> Sort<T>(List<T> items, ISortingCommand<T, string> command, int bucketsCount, int threadsCount)
    {
        _bucketsCount = bucketsCount;
        _threadsCount = threadsCount;
        var start = 0;
        var buckets = CreateBuckets(items, command);
        var threadStep = Math.Max(1, buckets.Length / threadsCount);
        var taskIndex = 0;
        
        _tasks.Clear();
        
        while (start < buckets.Length)
        {
            var end = Math.Min(start + threadStep, buckets.Length);
            var start1 = start;
            _tasks.Add(Task.Run(() =>
            {
                Parallel.For(start1, end, j =>
                {
                    if (buckets[j].Count > 0)
                    {
                        _mutatingBubbleSort.Sort(buckets[j], command);
                    }
                });
            }));
            
            start += threadStep;
            taskIndex++;
        }

        Task.WaitAll(_tasks.ToArray());
        
        return buckets.SelectMany(b => b).ToList();
    }

    private List<T>[] CreateBuckets<T>(List<T> items, ISortingCommand<T, string> command)
    {
        var buckets = new Dictionary<char, ConcurrentBag<T>>(_bucketsCount);
        var minASCII = items.Min(el => command.Selector(el)[0]);
        var threadStep = Math.Max(1, items.Count / _threadsCount);
        var start = 0;
        var taskIndex = 0;
        _tasks = new List<Task>();

        for (int i = 0; i < _bucketsCount; i++)
        {
            buckets.Add(minASCII, []);
            minASCII++;
        }
        
        while (start < items.Count)
        {
            var end = Math.Min(start + threadStep, items.Count);

            var start1 = start;
            var end1 = end;
            _tasks.Add(Task.Run(() =>
            {
                for (int i = start1; i < end1; i++)
                {
                    var prop = command.Selector.Invoke(items[i]);
                    var letter = char.ToLower(prop[0]);
            
                    if (!buckets.ContainsKey(letter))
                    {
                        buckets.Add(char.ToLower(letter), []);
                    }
            
                    buckets[char.ToLower(letter)].Add(items[i]);
                }
            }));
            start += threadStep;
            taskIndex++;
        }

        Task.WaitAll(_tasks.ToArray());

        return buckets.Select(b => b.Value.ToList()).ToArray();
    }
}