using System.Diagnostics;
using System.Numerics;
using ParallelBucketSort.Generators.Core;
using ParallelBucketSort.Schemes;
using ParallelBucketSort.Algorithm.Implementation;
using ParallelBucketSort.Commands;
using ParallelBucketSort.Schemes.Core;

namespace ParallelBucketSort.Test;

public class AlgorithmTest<TArray, TKey>(
    int[] threadsQuantities,
    int[] arraySizes,
    int[] bucketSizes,
    ISortingScheme<TArray, TKey> sortingScheme,
    bool checkIfSorted,
    int repeatsOnThreads,
    int warmupsCount,
    int warmupArraySize)
{
    private int[] _threadsQuantities { get; set; } = threadsQuantities; // Масив кількостей потоків
    private int[] _arraySizes { get; set; } = arraySizes; // Масив розмірів масивів на яких буде проводитись тестування
    private int[] _bucketSizes { get; set; } = bucketSizes; // Масив розмірів бакетів
    private ISortingScheme<TArray, TKey> _sortingScheme { get; set; } = sortingScheme; // Схема сортування
    private bool _checkIfSorted { get; set; } = checkIfSorted; // Чи буде перевірятись коректність на кожній ітерації
    private int _repeatsOnThreads { get; set; } = repeatsOnThreads; // Кількість  прогонів
    private int _warmupsCount { get; set; } = warmupsCount; 
    private int _warmupArraySize { get; set; } = warmupArraySize;
    private Stopwatch _stopwatch { get; set; } = new();

    public List<TestResult> Test()
    {
        Console.WriteLine("------WARMUP START------");
        Warmup();
        Console.WriteLine("------WARMUP END-------");
        
        Console.WriteLine($"\nSorting command {sortingScheme.SortingCommand.GetType().Name}\n");
        
        var results = new List<TestResult>();
        
        foreach (var threadsQuantity in _threadsQuantities)
        {
            foreach (var arraySize in _arraySizes)
            {
                foreach (var bucketSize in _bucketSizes)
                {
                    var parallelSorter = new ParallelBucketSorter(bucketSize, threadsQuantity);
                    var sequentialSorter = new SequentialBucketSorter(bucketSize);
                    var tempResults = new List<TestResult>();
                
                    for (int i = 0; i < _repeatsOnThreads; i++)
                    {
                        var arr = _sortingScheme.DataGenerator.Generate(arraySize); // Масив на якому буде проводитись тестування
                        
                        _stopwatch.Reset();
                    
                        _stopwatch.Start();
                        var parallelSortedArray = parallelSorter.Sort(arr, _sortingScheme.SortingCommand);
                        _stopwatch.Stop();
                        var parallelElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
                        
                        _stopwatch.Reset();
                        
                        _stopwatch.Start();
                        var sequentialSortedArray = sequentialSorter.Sort(arr, _sortingScheme.SortingCommand);
                        _stopwatch.Stop();
                        var sequentialElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
                        double parallelSpeedup;
                        double parallelEfficiency = 0;

                        if (parallelElapsedMilliseconds == 0)
                        {
                            parallelSpeedup = 0;
                        }
                        else
                        {
                            parallelSpeedup = (double)sequentialElapsedMilliseconds / parallelElapsedMilliseconds;
                            parallelEfficiency = (double)sequentialElapsedMilliseconds / (threadsQuantity * parallelElapsedMilliseconds);
                        }
                        
                        var sequentialTestResult = new TestResult()
                        {
                            SortingType = SortingType.Sequential,
                            ElapsedMilliseconds = sequentialElapsedMilliseconds,
                            ArraySize = arraySize,
                            BucketsCount = bucketSize
                        };

                        var parallelTestResult = new ParallelTestResult()
                        {
                            SortingType = SortingType.Parallel,
                            ElapsedMilliseconds = parallelElapsedMilliseconds,
                            ArraySize = arraySize,
                            BucketsCount = bucketSize,
                            ThreadsCount = threadsQuantity,
                            Speedup = parallelSpeedup,
                            Efficiency = parallelEfficiency
                        };

                        if (_checkIfSorted)
                        {
                            arr.Sort(sortingScheme.SortingCommand);
                            var isSequentialSorted = ArrayHelper.CheckArrays(arr, sequentialSortedArray.ToList(), _sortingScheme.EqualityComparer);
                            var isParallelSorted = ArrayHelper.CheckArrays(arr, parallelSortedArray.ToList(), _sortingScheme.EqualityComparer);

                            sequentialTestResult.IsSortedProperly = isSequentialSorted;
                            parallelTestResult.IsSortedProperly = isParallelSorted;
                        }
                        
                        tempResults.AddRange([sequentialTestResult, parallelTestResult]);
                    }

                    var parallelTestResults = tempResults
                        .Where(t => t is ParallelTestResult)
                        .Cast<ParallelTestResult>()
                        .ToList();
                    
                    var sequentialTestResults = tempResults
                        .Where(r => r is not ParallelTestResult).ToList();

                    var averageSequentialResult = new TestResult
                    {
                        SortingType = SortingType.Sequential,
                        ElapsedMilliseconds = sequentialTestResults.Sum(r => r.ElapsedMilliseconds) / _repeatsOnThreads,
                        ArraySize = arraySize,
                        IsSortedProperly = sequentialTestResults.All(r => r.IsSortedProperly == true),
                        BucketsCount = bucketSize
                    };

                    var averageParallelResult = new ParallelTestResult
                    {
                        SortingType = SortingType.Parallel,
                        ElapsedMilliseconds = parallelTestResults.Sum(r => r.ElapsedMilliseconds) / _repeatsOnThreads,
                        ArraySize = arraySize,
                        IsSortedProperly = parallelTestResults.All(r => r.IsSortedProperly == true),
                        BucketsCount = bucketSize,
                        ThreadsCount = threadsQuantity,
                        Speedup = parallelTestResults.Sum(r => r.Speedup) / _repeatsOnThreads,
                        Efficiency = parallelTestResults.Sum(r => r.Efficiency) / _repeatsOnThreads
                    };

                    if (!_checkIfSorted)
                    {
                        averageSequentialResult.IsSortedProperly = null;
                        averageParallelResult.IsSortedProperly = null;
                    }

                    Console.WriteLine(averageSequentialResult);
                    Console.WriteLine(averageParallelResult);

                    results.AddRange([averageSequentialResult, averageParallelResult]);
                }
            }
        }

        return results;
    }

    private void Warmup()
    {
        for (int i = 0; i < _warmupsCount; i++)
        {
            var parallelSorter = new ParallelBucketSorter(10, 10);
            var warmupArr = Enumerable.Range(0, _warmupArraySize).Select(i => i).ToList();
            parallelSorter.Sort(warmupArr, new NumberSortingCommand());
        }
    }
}