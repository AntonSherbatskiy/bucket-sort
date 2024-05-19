using System.Numerics;
using ParallelBucketSort.Generators.Core;
using ParallelBucketSort.Schemes.Core;

namespace ParallelBucketSort.Test;

public class TestBuilder<TValue, TKey>
{
    public int[] ThreadsQuantities { get; set; }
    public int[] ArraySizes { get; set; }
    public int[] BucketSizes { get; set; }
    public bool CheckIfSorted { get; set; }
    public int RepeatsOnThreads { get; set; }
    public int WarmupsCount { get; set; }
    public int WarmupArraySize { get; set; }
    public ISortingScheme<TValue, TKey> SortingScheme { get; set; }

    public static TestBuilder<TValue, TKey> CreateBuilder()
    {
        return new TestBuilder<TValue, TKey>();
    }
    
    public TestBuilder<TValue, TKey> WithThreads(int[] threadQuantities)
    {
        ThreadsQuantities = threadQuantities;

        return this;
    }

    public TestBuilder<TValue, TKey> WithArraySizes(int[] arraySizes)
    {
        ArraySizes = arraySizes;

        return this;
    }

    public TestBuilder<TValue, TKey> IsCheckSorted(bool checkSorted)
    {
        CheckIfSorted = checkSorted;
        
        return this;
    }
    
    public TestBuilder<TValue, TKey> WithRepeatsOnThreads(int repeats)
    {
        RepeatsOnThreads = repeats;

        return this;
    }

    public TestBuilder<TValue, TKey> WithBucketSizes(int[] bucketSizes)
    {
        BucketSizes = bucketSizes;

        return this;
    }
    
    public TestBuilder<TValue, TKey> WithWarmupsCount(int warmupsCount)
    {
        WarmupsCount = warmupsCount;

        return this;
    }

    public TestBuilder<TValue, TKey> UseWarmUpArraySize(int warmupArraySize)
    {
        WarmupArraySize = warmupArraySize;

        return this;
    }
    
    public TestBuilder<TValue, TKey> UseSortingScheme(ISortingScheme<TValue, TKey> sortingScheme)
    {
        SortingScheme = sortingScheme;

        return this;
    }

    public AlgorithmTest<TValue, TKey> Build()
    {
        return new AlgorithmTest<TValue, TKey>(
            threadsQuantities: ThreadsQuantities,
            arraySizes: ArraySizes,
            bucketSizes: BucketSizes,
            sortingScheme: SortingScheme,
            checkIfSorted: CheckIfSorted,
            repeatsOnThreads: RepeatsOnThreads,
            warmupsCount: WarmupsCount,
            warmupArraySize: WarmupArraySize);
    }
    
}