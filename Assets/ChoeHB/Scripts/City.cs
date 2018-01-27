using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class City {

    public event Action OnRecovery;
    public event Action OnDestroy;

    public readonly string name;
    public readonly bool canStart;
    public readonly int income;

    public readonly IntRange difficulty;    // 각각의 벽당 생기는 수의 갯수
    public readonly Dictionary<City, Transmission> transmissions;

    private readonly IntRange firewallCount; // 벽 갯수

    public bool isDestroyed { get; private set; }

    public City(CityData cityData)
    {
        name            = cityData.name;
        canStart        = cityData.canStart;
        income          = cityData.income;
        firewallCount   = cityData.fireWallCount;
        difficulty      = cityData.difficulty;

        transmissions   = new Dictionary<City, Transmission>();
    }

    public void StartingCity()
    {
        if (!canStart)
            throw new Exception("Can't Start " + name);
        DestroyCity();
    }

    // 도시가 파괴되면
    public void DestroyCity()
    {
        isDestroyed = true;

        // 인접한 도시를
        foreach (var nearCity in transmissions.Keys)
        {
            // 향하는 Transmission들을 약화시킨다.
            nearCity.FromTransmissions().ForEach(tr => tr.Debuffed());

            // 해킹되지 않았으면 내가 향할 Transmission들을 활성화하고
            if (!nearCity.isDestroyed)
            {
                Transmission to = transmissions[nearCity];
                to.Active();
            }
        }

        // 나를 향하고 있는 Transmission들을 파괴한다.
        FromTransmissions().ForEach(tr => tr.SuccessDestroy());

        Debug.Log("Destroy " + name);
        if (OnDestroy != null)
            OnDestroy();
    }

    // 도시가 복구되면
    public void Recovery()
    {
        isDestroyed = false;
        FromTransmissions().ForEach(tr => tr.Recovery());

        if (OnRecovery != null)
            OnRecovery();
    }


    public IEnumerable<Transmission> GetActivedTransmission()
    {
        return transmissions.Values.Where(tr => tr.isActived);
    }

    public IEnumerable<Transmission> FromTransmissions()
    {
        foreach(var nearCity in transmissions.Keys)
            yield return nearCity.transmissions[this];
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
        List<Firewall> firewalls = new List<Firewall>();
        int firewallCount = dst.firewallCount;
        for (int i = 0; i < firewallCount; i++)
        {
            int difficulty = dst.difficulty;
            Firewall firewall = new Firewall(difficulty);
            firewalls.Add(firewall);
        }

        Transmission transmission = new Transmission(src, dst, firewalls);
        src.transmissions.Add(dst, transmission);
        return transmission;
    }
    #endregion
}
