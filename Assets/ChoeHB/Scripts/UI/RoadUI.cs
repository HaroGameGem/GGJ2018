using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadUI : SerializedMonoBehaviour {

    [SerializeField] TransmissionUI city1to2;
    [SerializeField] TransmissionUI city2to1;

    Road road;
    public void SetRoad(Road road)
    {
        this.road = road;
        city1to2.SetTransmission(road.city1to2);
        city2to1.SetTransmission(road.city2to1);
    }
}
