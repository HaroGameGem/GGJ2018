using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CityData {

    public string name;
    public CityUI cityUI;
    public int income;
    public bool canStart;
    public IntRange fireWallCount;
    public IntRange difficulty;
    
}
