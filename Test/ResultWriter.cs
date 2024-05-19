using System.Text;

namespace ParallelBucketSort.Test;

public class ResultWriter
{
    public string FilePath { get; set; }

    public ResultWriter(string filePath)
    {
        FilePath = filePath;
    }

    public void Write(List<TestResult> results)
    {
        var stringBuilder = new StringBuilder();
        
        var grouped = from result in results
            group result by result.SortingType
            into g
            select new
            {
                SortingType = g.Key,
                Results = g
            };

        foreach (var group in grouped)
        {
            stringBuilder.Append($"{group.SortingType}\n");
            var copy = group.Results;

            var arraySizeGroup = from g in copy
                group g by g.ArraySize
                into arrSizeG
                select new
                {
                    ArraySize = arrSizeG.Key,
                    Results = arrSizeG
                };
            
            foreach (var arrSizeG in arraySizeGroup)
            {
                var copy2 = arrSizeG.Results;
                var bucketSizeGroup = from g in copy2
                    group g by g.BucketsCount
                    into g2
                    select new
                    {
                        BucketSize = g2.Key,
                        Results = g2
                    };
                
                stringBuilder.Append($"\tArray size: {arrSizeG.ArraySize}\n");

                
                foreach (var bucketGroup in bucketSizeGroup)
                {
                    stringBuilder.Append($"\t\tBuckets: {bucketGroup.BucketSize}\n");

                    foreach (var testResult in bucketGroup.Results)
                    {
                        stringBuilder.Append($"\t\t\tElapsed milliseconds: {testResult.ElapsedMilliseconds}\n");

                        if (testResult.IsSortedProperly is not null)
                        {
                            stringBuilder.Append($"\t\t\tIs sorted properly: {testResult.IsSortedProperly}\n");
                        }
                        
                        if (testResult is ParallelTestResult parRes)
                        {
                            stringBuilder.Append($"\t\t\tThreads: {parRes.ThreadsCount}\n");
                            stringBuilder.Append($"\t\t\tSpeedup: {parRes.Speedup}\n");
                        }
                        
                        stringBuilder.Append("\n");
                    }
                }
                

                
            }
        }

        var line = stringBuilder.ToString();
        
        File.WriteAllText(FilePath, line);
    }
}