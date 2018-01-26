using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IntRange {

    [SerializeField] int min;
    [SerializeField] int max;

    // Include min, Include max)
    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public int Random()
    {
        return UnityEngine.Random.Range(min, max + 1);
    }
}
