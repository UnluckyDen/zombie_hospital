using System;
using Random = UnityEngine.Random;

namespace Extension
{
    public static class Random
    {
        public static int[] GetUniqueIntArray(int minValue, int maxValue, int count)
        {
            if (maxValue - minValue < count)
                throw new Exception("The required unique number of items exceeds the number of possible items.");
            var result = new int[count];
            for (var i = 0; i < count; i++)
            {
                var unique = true;
                do
                {
                    result[i] = UnityEngine.Random.Range(minValue, maxValue);
                    unique = true;
                    for (var j = 0; j < i; j++)
                    {
                        if (result[j] != result[i]) continue;
                        unique = false;
                        break;
                    }
                } while (!unique);
            }

            return result;
        }
    }
}