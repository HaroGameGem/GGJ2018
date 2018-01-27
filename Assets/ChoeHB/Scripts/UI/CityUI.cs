using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using System.Linq;

public class CityUI : SerializedMonoBehaviour {

    public enum State
    {
        Selecting, Started
    }

    private static State state = State.Selecting;

    public static Dictionary<City, CityUI> cityUIs { get; private set; }

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
        if (state == State.Selecting)
            button.interactable = city.canStart;

        if (state == State.Started)
            button.interactable = !city.isDestroyed;

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
        city.OnDestroy += RunAll;
        city.OnRecovery += Recovery;
    }
    

    public void Click()
    {
        ZoomIn();
        NumPad.instance.Float();
        if (OnClick != null)
            OnClick(city);
    }

    // 캐릭터들을 보냄
    public void RunAll()
    {
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

    private void ZoomIn()
    {
        CityZoomer.instance.ZoomIn(transform.position);
    }

    public static void StartHack()
    {
        state = State.Started;
    }
}
