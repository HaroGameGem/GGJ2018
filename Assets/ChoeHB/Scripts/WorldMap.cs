using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public struct GameResult
{
    public bool isCleard;
    public string time;
    public Progress credit;
    public Progress citys;
}

public class WorldMap : StaticComponent<WorldMap> {

    // RoadData에서 사용
    public static string[] cityNames    { get { return instance.cityDatas.Select(city => city.name).ToArray(); } }

    public event Action OnInitialize;

    [Header("Data")]
    [SerializeField] VaccineOccurData vaccineOccurData;     // 백신 발생에 대한 정보
    [SerializeField] int m_deadline;                          // 게임 타이머
    [SerializeField] float incomeInterval;                  // 인컴이 오르는 주기
    [SerializeField] Progress m_credit;                     // 현재 화폐 / 목표화폐

    [Header("Component")]
    [SerializeField] float incomeUpdatingInterval;
    [SerializeField] GameObject readyUI;
    [SerializeField] Animation warningUI;
    [SerializeField] ResultUI resultUI;

    [SerializeField] Alarm alarm;
    [SerializeField] Canvas canvas;
    [SerializeField] RoadUI roadUIPrafab;
    [SerializeField] Transform roadHolder;

    [TabGroup("City")] [TableList] [SerializeField] List<CityData> cityDatas;
    [TabGroup("Road")] [TableList] [SerializeField] List<RoadData> roadDatas;

    public enum State
    {
        SelectingCity, Playing, End
    }

    public State state { get; private set; }

    public int deadline                         { get { return m_deadline; } }
    public Dictionary<City, CityUI> cityUIs     { get; private set; }
    public Timer timer                          { get; private set; }
    public Progress credit
    {
        get { return m_credit; }
        private set { m_credit = value; }
    }

    public Progress destroyedCitys { get; private set; }

    private List<Road> roads;
    private Dictionary<string, City> citys;

    private void Start()
    {
        string bgmName = "BGM" + UnityEngine.Random.Range(1, 3); ;
        AudioManager.PlayMusic(bgmName);
        readyUI.gameObject.SetActive(true);
        Initialize();
    }

    private void InitializeCitys()
    {
        // 도시들 초기화
        citys = new Dictionary<string, City>();
        cityUIs = new Dictionary<City, CityUI>();
        destroyedCitys = new Progress(cityDatas.Count);
        foreach (var cityData in cityDatas)
        {
            string name = cityData.name;
            City city = new City(cityData);
            city.OnDestroy += OnDestroyCity;
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
        state = State.SelectingCity;
        foreach (var cityUI in cityUIs.Values)
            cityUI.OnClick += StartCity;

        if (OnInitialize != null)
            OnInitialize();
    }

    public void StartCity(City city)
    {
        if (!city.canStart)
            return;

        if (state != State.SelectingCity)
            throw new Exception("Already Started");

        state = State.Playing;

        timer = new Timer(deadline);
        foreach (var cityUI in cityUIs.Values)
            cityUI.OnClick -= StartCity;

        city.StartingCity();
        alarm.StartAlert(incomeUpdatingInterval, Income);

        StartCoroutine(VaccineOccuring());
    }

    private void GameEnd()
    {
        if (state == State.End)
            return;

        state = State.End;
        alarm.Stop();
        timer.Stop();
        bool isWin = credit.isSuccessed;
        GameResult result = new GameResult();
            result.credit = credit;
            result.citys = destroyedCitys;
            result.isCleard = isWin;
            result.time = timer.ToString();

        AudioManager.FadeOutBGM(1f);

        AudioManager.PlaySound(isWin ? "Win" : "Lose");

        StopAllCoroutines();

        resultUI.Float(result);
    }

    private void Income()
    {
        float sum = (from city in citys.Values where city.isDestroyed select city.income).Sum();
        sum *= (incomeUpdatingInterval / incomeInterval);
        credit += sum;

        if (credit.isSuccessed)
            GameEnd();
    }

    private IEnumerator VaccineOccuring()
    {
        HashSet<City> occureds = new HashSet<City>();
        while (true)
        {
            yield return new WaitForSeconds(vaccineOccurData.occurInterval);

            while (TransmissionUI.isTryingHack)
                yield return new WaitForEndOfFrame();

            IEnumerable<City> destroyedCitys = citys.Values.Where(city => city.isDestroyed && !city.isStaringCity);
            float occurRate = vaccineOccurData.GetOccurRate(destroyedCitys.Count());

            occureds.Clear();
            foreach (var destroyedCity in destroyedCitys)
                if (Random.Range(0, 1f) < occurRate)
                    occureds.Add(destroyedCity);

            // 발생할 백신이 있다면
            if(occureds.Count != 0)
            {
                // 사운드D
                AudioManager.PlaySound("Vaccine");

                // Warning UI
                warningUI.gameObject.SetActive(true);
                warningUI.Play("Warning");
                float playingTime = warningUI.clip.length;
                yield return new WaitForSeconds(playingTime);
                warningUI.gameObject.SetActive(false);

                // 줌아웃
                CityZoomer.instance.ZoomOut();

                foreach (var occured in occureds)
                    cityUIs[occured].OccurVaccine();

            }
                
        }
    }

    private void OnDestroyCity()
    {
        destroyedCitys++;
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            if (timer.remain == 0)
                GameEnd();
        }
    }

}
