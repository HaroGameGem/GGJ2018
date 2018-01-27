using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorldMap : StaticComponent<WorldMap> {

    // RoadData에서 사용
    public static string[] cityNames    { get { return instance.cityDatas.Select(city => city.name).ToArray(); } }

    [TabGroup("City")] [TableList] [SerializeField] List<CityData> cityDatas;
    [TabGroup("Road")] [TableList] [SerializeField] List<RoadData> roadDatas;

    [ValueDropdown("GetCityNames")]
    [SerializeField] string dstCityName;

    [SerializeField] Canvas canvas;
    [SerializeField] RoadUI roadUIPrafab;
    [SerializeField] Transform roadHolder;

    // 도시마다 다른 도시에 연결되어 있고 연결마다 방화벽이 n개 있다.
    private City dstCity;
    private List<Road> roads;
    private Dictionary<string, City> citys;

    public Dictionary<City, CityUI> cityUIs { get; private set; }


    private void Start()
    {
        Initialize();
    }

    private void InitializeCitys()
    {
        // 도시들 초기화
        citys = new Dictionary<string, City>();
        cityUIs = new Dictionary<City, CityUI>();
        foreach (var cityData in cityDatas)
        {
            string name = cityData.name;
            City city = new City(cityData);
            citys.Add(name, city);

            CityUI cityUI = cityData.cityUI;
            cityUIs.Add(city, cityUI);
            cityUI.SetCity(city);
        }
    }

    private void InitializeRoads()
    {
        // 도로 초기화
        roads = new List<Road>();
        foreach (var roadData in roadDatas)
        {
            City city1 = citys[roadData.city1];
            City city2 = citys[roadData.city2];

            Road road = City.Link(city1, city2);
            roads.Add(road);
        }
    }

    private void DrawRoads()
    {
        // 도로를 그림
        foreach (var road in roads)
        {
            CityUI city1 = cityUIs[road.city1];
            CityUI city2 = cityUIs[road.city2];
            
            RoadUI roadUI = Instantiate(roadUIPrafab);
            roadUI.name = road.ToString();
            roadUI.transform.SetParent(roadHolder, false);
            roadUI.SetRoad(road);

            // 방향
            Vector2 dir = city2.transform.position - city1.transform.position;

            // 위치
            Vector2 roadCenter = (city1.transform.position + city2.transform.position) / 2;
            roadUI.transform.position = roadCenter;

            // 각도
            float angle = Mathf.Atan2(dir.y, dir.x) * 180 / 3.14f;
            float distance = dir.magnitude;
            roadUI.transform.rotation = Quaternion.Euler(0, 0, angle);

            // 길이
            RectTransform roadRectTransform = roadUI.GetComponent<RectTransform>();
            Vector2 sizeDelta = roadRectTransform.sizeDelta;
            sizeDelta.x = distance / canvas.transform.localScale.x;
            roadRectTransform.sizeDelta = sizeDelta;

            // 도로 완료
            roadUI.gameObject.SetActive(true);
        }
    }

    private void Initialize()
    {
        InitializeCitys();
        InitializeRoads();
        DrawRoads();

        // 시작지점 설정
        dstCity = citys[dstCityName];
        dstCity.OnDestroy += GameClear;

        foreach (var cityUI in cityUIs.Values)
            cityUI.OnClick += StartCity;
    }

    public void StartCity(City city)
    {
        if (city == dstCity)
            return;

        foreach (var cityUI in cityUIs.Values)
            cityUI.OnClick -= StartCity;

        city.StartingCity();
        CityUI.StartHack();
    }

    private string[] GetCityNames()
    {
        return cityNames;
    }

    public void GameClear()
    {
        Debug.Log("Game Clear");
    }

}
