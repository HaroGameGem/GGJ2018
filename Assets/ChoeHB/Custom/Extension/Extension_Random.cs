using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class Extension_Random
{
    public static T Random<T>(this IEnumerable<T> array)
    {
        int rand = UnityEngine.Random.Range(0, array.Count());
        return array.ElementAt(rand);
    }
}
