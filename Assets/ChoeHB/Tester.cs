using PathologicalGames;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : SerializedMonoBehaviour {

    [SerializeField] WorldMap worldMap;
    [SerializeField] Dictionary<string, Transform> prefabs;

    private void Awake()
    {
        worldMap.OnInitialize += Initialize;
    }

    private void Initialize()
    {
        // 도시의 해킹과 복구
        foreach (var city in worldMap.cityUIs.Keys)
        {
            City cachedCity = city;
            cachedCity.OnDestroy += () => DetectCityDestroy(cachedCity);
            cachedCity.OnRecovery += () => DetectRecoveryCity(cachedCity);
        }

        //// 백신의 생성
        //Vaccine.OnOccur += DetectOccurVaccine;

        //// 방화벽의 생성과 파괴
        //TransmissionUI.OnAddFirewall += DetectAddFirewall;
        //TransmissionUI.OnDestroyFirewall += DetectDestroyFirewall;
    }

    public void DetectCityDestroy(City city)
    {
        var cityUI = CityUI.cityUIs[city];

        string particleName;
        Transform child;

        particleName = "ParticleBreakCity";
        child = PoolManager.Pools[particleName].Spawn(prefabs[particleName]);
        child.transform.position = cityUI.transform.position;

        particleName = "ParticleBreakCityFace";
        child = PoolManager.Pools[particleName].Spawn(prefabs[particleName]);
        child.transform.position = cityUI.transform.position;
    }

    public void DetectRecoveryCity(City city)
    {
        var cityUI = CityUI.cityUIs[city];

        string particleName;
        Transform child;

        particleName = "ParticleRecoveryCity";
        child = PoolManager.Pools[particleName].Spawn(prefabs[particleName]);
        child.transform.position = cityUI.transform.position;

        particleName = "ParticleRecoveryCityFace";
        child = PoolManager.Pools[particleName].Spawn(prefabs[particleName]);
        child.transform.position = cityUI.transform.position;
    }

    public void DetectOccurVaccine(Vaccine vaccine)
    {
        // 백신은 모노라 바로 Transform으로 접근하면 됨
        Debug.Log("Occur Vaccine ");
    }

    public void DetectAddFirewall(Vector3 pos)
    {
        Debug.Log("Add Firewall " + pos);
    }

    public void DetectDestroyFirewall(Vector3 pos)
    {
        Debug.Log("Destroy Firewall " + pos);
    }
}
