using ParallelBucketSort.Models;
using ParallelBucketSort.Schemes;
using ParallelBucketSort.Test;

namespace ParallelBucketSort;

class Program
{
    static void Main()
    {
        int[] threadQuantities = [10, 16, 32];
        int[] arraySizes = [100, 1000, 10_000, 100_000, 500_000, 1_000_000, 2_000_000];
        int[] bucketSizes = [100, 500];
        const int warmupsCount = 1;
        const int repeatsCount = 1;
        const int warmupArraySize = 100_000;
        const string path = "/Users/anton/RiderProjects/ParallelBucketSort/ParallelBucketSort/Assets/results.txt";
        var resultWriter = new ResultWriter(path);
        var tester = TestBuilder<Person, int>
            .CreateBuilder()
            .WithThreads(threadQuantities)
            .WithArraySizes(arraySizes)
            .WithBucketSizes(bucketSizes)
            .WithRepeatsOnThreads(repeatsCount)
            .WithWarmupsCount(warmupsCount)
            .UseWarmUpArraySize(warmupArraySize)
            .UseSortingScheme(new PersonByAgeSortingScheme())
            .IsCheckSorted(false)
            .Build();
        
        var results = tester.Test();
        resultWriter.Write(results);
    }
}