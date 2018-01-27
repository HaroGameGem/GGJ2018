using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct VaccineOccurData {

    public float occurInterval;
    [Range(0,1)] public float defaultOccurRate;
    [Range(0,1)] public float occurRatePerCity;
    
    public float GetOccurRate(int destroyedCitysCount)
    {
        return defaultOccurRate + occurRatePerCity * destroyedCitysCount;
    }
}
