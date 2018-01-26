using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class City {

    public readonly string name;
    public readonly Dictionary<City, Transmission> transmissions;

    private readonly IntRange firewallCount; // 벽 갯수
    private readonly IntRange difficulty;    // 각각의 벽당 생기는 수의 갯수

    public bool isHacked { get; private set; }

    public City(CityData cityData)
    {
        name            = cityData.name;
        firewallCount   = cityData.fireWallCount;
        difficulty      = cityData.difficulty;

        transmissions   = new Dictionary<City, Transmission>();
    }

    public void StartingCity()
    {
        Hack();
    }

    public void Hack()
    {
        isHacked = true;
        foreach(var nearCity in transmissions.Keys)
        {
            // 나를 향하고 있는 Transmission들
            Transmission trans = nearCity.transmissions[this];
                trans.SuccessHack();
        }
    }

    public override string ToString()
    {
        return "City(" + name + ")";
    }

    #region Static - Link 
    public static Road Link(City city1, City city2)
    {
        Transmission city1to2 = AddTransmission(city1, city2);
        Transmission city2to1 = AddTransmission(city2, city1);
        return new Road(city1, city2, city1to2, city2to1);
    }

    private static Transmission AddTransmission(City src, City dst)
    {
        Stack<Firewall> firewalls = new Stack<Firewall>();
        int firewallCount = dst.firewallCount.Random();
        for (int i = 0; i < firewallCount; i++)
        {
            int difficulty = dst.difficulty.Random();
            Firewall firewall = new Firewall(difficulty);
            firewalls.Push(firewall);
        }

        Transmission transmission = new Transmission(src, dst, firewalls);
        src.transmissions.Add(dst, transmission);
        return transmission;
    }
    #endregion
}
