using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class City {

    public string name;
    public Dictionary<City, Transmission> transmissions { get; private set; }

    private IntRange firewallCount; // 벽 갯수
    private IntRange difficulty;    // 각각의 벽당 생기는 수의 갯수

    public City(CityData cityData)
    {
        name            = cityData.name;
        firewallCount   = cityData.fireWallCount;
        difficulty      = cityData.difficulty;

        transmissions       = new Dictionary<City, Transmission>();
    }

    public static Road Link(City city1, City city2)
    {
        Transmission city1to2 = AddTransmission(city1, city2);
        Transmission city2to1 = AddTransmission(city2, city1);
        return new Road(city1, city2, city1to2, city2to1);
    }

    private static Transmission AddTransmission(City src, City dst)
    {
        List<Firewall> firewalls = new List<Firewall>();
        int firewallCount = dst.firewallCount.Random();
        for (int i = 0; i < firewallCount; i++)
        {
            int difficulty = dst.difficulty.Random();
            Firewall firewall = new Firewall(difficulty);
            firewalls.Add(firewall);
        }

        Transmission transmission = new Transmission(dst, firewalls);
        src.transmissions.Add(dst, transmission);

        return transmission;
    }

    //public void Initialize(Dictionary<City, List<Firewall>> firewalls)
    //{
    //    this.firewalls = firewalls;

    //    foreach (List<Firewall> firewallList in firewalls.Values)
    //    {
    //        int firewallCount = this.firewallCount.Random();

    //        // 각각의 방화벽을 만든다.
    //        for (int i = 0; i < firewallCount; i++)
    //        {
    //            int difficulty = this.difficulty.Random();
    //            Firewall firewall = new Firewall(difficulty);
    //            firewallList.Add(firewall);
    //        }
    //    }
    //}
    
}
