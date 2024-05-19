namespace ParallelBucketSort;

public class ArrayHelper
{
    public static bool CheckArrays<T>(List<T> arr1, List<T> arr2, IEqualityComparer<T> comparer)
    {
        if (arr1.Count != arr2.Count)
        {
            return false;
        }

        for (var i = 0; i < arr1.Count; i++)
        {
            if (!comparer.Equals(arr1[i], arr2[i]))
            {
                return false;
            }
        }

        return true;
    }
}