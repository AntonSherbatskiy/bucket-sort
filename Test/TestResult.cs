using System.Text.Json.Serialization;

namespace ParallelBucketSort.Test;

[JsonDerivedType(typeof(ParallelTestResult))]
public class TestResult
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SortingType SortingType { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public int ArraySize { get; set; }
    public bool? IsSortedProperly { get; set; }
    public int BucketsCount { get; set; }

    public override string ToString()
    {
        return
            $"{nameof(SortingType)}: {SortingType}, {nameof(ElapsedMilliseconds)}: {ElapsedMilliseconds}, {nameof(ArraySize)}: {ArraySize}, {nameof(IsSortedProperly)}: {IsSortedProperly}, {nameof(BucketsCount)}: {BucketsCount}";
    }
}