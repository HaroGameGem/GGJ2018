using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using System.Linq;

public class CityUI : SerializedMonoBehaviour {

    public static Dictionary<City, CityUI> cityUIs { get; private set; }

    [SerializeField] Image virus;
    [SerializeField] Vaccine vaccine;
    [SerializeField] Character characterPrefab;
    [SerializeField] Transform characterHolder;
    public event Action<City> OnClick;

    private Button button;
    private City city;

    private Dictionary<Transmission, Character> characters;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (WorldMap.instance.state == WorldMap.State.SelectingCity)
            button.interactable = city.canStart;

        if (WorldMap.instance.state == WorldMap.State.Playing)
        {
            // 도시가 파괴되지 않았으면서
            // 주변의 도시들중 하나라도 파괴된 도시가 있다면 이동가능하다
            button.interactable = !city.isDestroyed
                && city.transmissions.Keys.Where(city => city.isDestroyed).Any();
        }

    }

    public void OccurVaccine()
    {
        vaccine.Occur();
    }

    public void SetCity(City city)
    {
        this.city = city;
        vaccine.SetCity(city);
        cityUIs = cityUIs ?? new Dictionary<City, CityUI>();
        cityUIs.Add(city, this);
        city.OnDestroy += Destroyed;
        city.OnRecovery += Recovery;
    }
    

    public void Click()
    {
        Debug.Log(WorldMap.instance.state);
        if (WorldMap.instance.state == WorldMap.State.Playing)
        {
            CityZoomer.instance.ZoomIn(transform.position);
            NumPad.instance.Float();
        }
        if (OnClick != null)
            OnClick(city);
    }

    
    // 캐릭터들을 보내고 바이러스이미지 활성화
    public void Destroyed()
    {
        virus.gameObject.SetActive(true);
        foreach (var transmission in city.GetActivedTransmission())
            Run(transmission);
    }

    public void Run(Transmission transmission)
    {
        if (characters == null)
        {
            characters = new Dictionary<Transmission, Character>();
            foreach (var nearCity in city.transmissions.Keys)
            {
                Transmission instancedTransmission = city.transmissions[nearCity];
                Character instancedCharacter = Instantiate(characterPrefab);
                instancedCharacter.gameObject.SetActive(false);
                characters.Add(instancedTransmission, instancedCharacter);
            }
        }

        CityUI target = cityUIs[transmission.dst];
        if(!characters.ContainsKey(transmission))
        {
            Debug.Log("@ " + transmission);
            characters.Keys.ForEach(Debug.Log);
        }

        Character character = characters[transmission];
        character.Transmission(transmission);
    }

    private void Recovery()
    {
        virus.gameObject.SetActive(false);

        // 나가고 있던 애들은 죽고
        city.GetActivedTransmission().Select(tr => characters[tr]).ForEach(ch => ch.Interrupted());

        // 인접한 곳 중에서 파괴되지 않은 곳은 나에게 보낸다.
        foreach(var transmission in city.FromTransmissions())
        {
            if (!transmission.src.isDestroyed)
                continue;
            cityUIs[transmission.src].Run(transmission);
        }
        //city.FromTransmissions().Where( .ForEach(tr => cityUIs[tr.dst].Run(tr));
    }
    
    
}
