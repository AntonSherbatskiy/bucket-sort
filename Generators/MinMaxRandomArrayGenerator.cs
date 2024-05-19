using ParallelBucketSort.Generators.Core;

namespace ParallelBucketSort.Generators;

public class MinMaxRandomArrayGenerator : IDataGenerator<int>
{
    public int MaxElement { get; set; }
    public int MinElement { get; set; }

    public MinMaxRandomArrayGenerator(int minElement, int maxElement)
    {
        MinElement = minElement;
        MaxElement = maxElement;
    }

    public List<int> Generate(int elementsCount)
    {
        var randomizer = new Random();

        return Enumerable.Range(0, elementsCount).Select(_ => randomizer.Next(MinElement, MaxElement)).ToList();
    }
}