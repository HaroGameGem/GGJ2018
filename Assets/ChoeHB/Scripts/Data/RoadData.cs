using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class RoadData {

    [ValueDropdown("GetCityNames1")] public string city1;
    [ValueDropdown("GetCityNames2")] public string city2;
    
    private string[] GetCityNames1()
    {
        return WorldMap.cityNames.Where(name => name != city2).ToArray();
    }

    private string[] GetCityNames2()
    {
        return WorldMap.cityNames.Where(name => name != city1).ToArray();
    }
}
