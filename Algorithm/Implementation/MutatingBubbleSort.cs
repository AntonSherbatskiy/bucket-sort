using ParallelBucketSort.Algorithm.Core;

namespace ParallelBucketSort.Algorithm.Implementation;

public class MutatingBubbleSort
{
    public void Sort<T>(List<T> values, IComparer<T> comparer)
    {
        for (var i = 0; i < values.Count - 1; i++)
        {
            for (var j = 0; j < values.Count - i - 1; j++) 
            {
                if (comparer.Compare(values[j], values[j + 1]) == 1)
                {
                    (values[j], values[j + 1]) = (values[j + 1], values[j]);
                }
            }
        }
    }
}