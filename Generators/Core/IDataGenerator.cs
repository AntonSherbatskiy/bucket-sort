namespace ParallelBucketSort.Generators.Core;

public interface IDataGenerator<T>
{
    List<T> Generate(int elementsCount);
}