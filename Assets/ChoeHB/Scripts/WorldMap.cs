using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorldMap : StaticComponent<WorldMap> {

    public static Image wallPrefab      { get { return instance.m_wallPrefab;   } }
    public static string[] cityNames    { get { return instance.cityDatas.Select(city => city.name).ToArray(); } }

    [TableList] [SerializeField] List<CityData> cityDatas;
    [TableList] [SerializeField] List<RoadData> roadDatas;

    [SerializeField] Image m_wallPrefab;

    [SerializeField] Image roadPrefab;
    [SerializeField] Transform roadHolder;

    //[TableList] [SerializeField] List<Road> roads;

    // 도시마다 다른 도시에 연결되어 있고 연결마다 방화벽이 n개 있다.
    private List<Road> roads;
    private Dictionary<string, City> citys;

    public void Initialize()
    {
        // 도시들 초기화
        citys = new Dictionary<string, City>();
        foreach(var cityData in cityDatas)
        {
            string name = cityData.name;
            City city = new City(cityData);
            citys.Add(name, city);
            cityData.cityUI.SetCity(city);
        }

        // 도로 초기화
        roads = new List<Road>();
        foreach(var roadData in roadDatas)
        {
            City city1 = citys[roadData.city1];
            City city2 = citys[roadData.city2];

            Road road = City.Link(city1, city2);
            roads.Add(road);
        }

        // 도로를 그림
        foreach(var road in roads)
        {
            CityUI city1 = (from cityData in cityDatas
                            where cityData.name == road.city1.name
                            select cityData.cityUI).Single();

            CityUI city2 = (from cityData in cityDatas
                            where cityData.name == road.city2.name
                            select cityData.cityUI).Single();

            Debug.Log(city1.name + " => " + city2.name);
            Vector2 dir = city2.transform.position - city1.transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * 180 / 3.14f;
            float distance = dir.magnitude;

            Image roadImage = Instantiate(roadPrefab);
            roadImage.name = road.ToString();
            roadImage.transform.SetParent(roadHolder);

            //위치
            roadImage.transform.position = city1.transform.position;

            //각도
            roadImage.transform.rotation = Quaternion.Euler(0, 0, angle);

            // 길이
            Vector2 sizeDelta = roadImage.rectTransform.sizeDelta;
            sizeDelta.x = distance;
            roadImage.rectTransform.sizeDelta = sizeDelta;

            roadImage.gameObject.SetActive(true);
        }
    }


    // 도시들을 연결
    [Button]
    private void UpdateLine()
    {
        //foreach (Transform child in roadHolder)
        //    DestroyImmediate(child.gameObject);

        //// 각각의 길을 만든다.
        //foreach (var road in roads)
        //{
        //    CityUI city1 = citys[road.city1];
        //    CityUI city2 = citys[road.city2];

        //    Vector2 dir = city2.transform.position - city1.transform.position;

        //    float angle = Mathf.Atan2(dir.y, dir.x) * 180 / 3.14f;
        //    float distance = dir.magnitude;

        //    Image roadImage = Instantiate(roadPrefab);
        //        roadImage.name = road.ToString();
        //        roadImage.transform.SetParent(roadHolder);

        //    //위치
        //    roadImage.transform.position = city1.transform.position;

        //    //각도
        //    roadImage.transform.rotation = Quaternion.Euler(0, 0, angle);

        //    // 길이
        //    Vector2 sizeDelta = roadImage.rectTransform.sizeDelta;
        //    sizeDelta.x = distance;
        //    roadImage.rectTransform.sizeDelta = sizeDelta;

        //    roadImage.gameObject.SetActive(true);
        //}
    }

    //// 도시들을 연결하고 각각의 연결에 방화벽들을 만든다.
    //public void Initialize()
    //{
    //    UpdateLine();

    //    // 도시 => Dic<도시, 방화벽들>
    //    firewalls = new Dictionary<City, Dictionary<City, List<Firewall>>>();

    //    foreach(var road in roads)
    //    {
    //        City city1 = road.city1;
    //        City city2 = road.city2;

    //        if (!firewalls.ContainsKey(city1))
    //            firewalls.Add(city1, new Dictionary<City, List<Firewall>>());

    //        if (!firewalls.ContainsKey(city2))
    //            firewalls.Add(city2, new Dictionary<City, List<Firewall>>());

    //        firewalls[city1].Add(city2, new List<Firewall>());
    //        firewalls[city2].Add(city1, new List<Firewall>());
    //    }

    //    //foreach(var city in citys)
    //    //    city.Initialize(firewalls[city]);
    //}

    private void Awake()
    {
        Initialize();
    }
}
