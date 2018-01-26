using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Road {

    public City city1;
    public City city2;

    public Transmission city1to2;
    public Transmission city2to1;

    public Road(City city1, City city2, Transmission city1to2, Transmission city2to1)
    {
        this.city1      = city1;
        this.city2      = city2;
        this.city1to2   = city1to2;
        this.city2to1   = city2to1;
    }

    public override string ToString()
    {
        return string.Format("Road {0}, {1}", city1.name, city2.name);
    }
}
