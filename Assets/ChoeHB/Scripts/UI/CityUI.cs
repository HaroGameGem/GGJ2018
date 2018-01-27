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
            button.interactable = city.isDestroyed;

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
        city.OnDestroy += Transmission;
        city.OnRecovery += InterruptTransmission;
    }
    

    public void Click()
    {
        ZoomIn();
        if (OnClick != null)
            OnClick(city);
    }

    // 캐릭터들을 보냄
    public void Transmission()
    {
        if (characters == null)
        {
            characters = new Dictionary<Transmission, Character>();
            foreach (var nearCity in city.transmissions.Keys)
            {
                Transmission transmission = city.transmissions[nearCity];
                Character character = Instantiate(characterPrefab);
                character.gameObject.SetActive(false);
                characters.Add(transmission, character);
            }
        }

        foreach (var transmission in city.GetActivedTransmission())
        {
            CityUI target = cityUIs[transmission.dst];
            Character character = characters[transmission];
                character.Transmission(transmission);
        }
    }

    private void InterruptTransmission()
    {
        city.GetActivedTransmission().Select(tr => characters[tr]).ForEach(ch => ch.Interrupted());
    }

    private void ZoomIn()
    {
        CityZoomer.ZoomIn(this);
    }

    public static void StartHack()
    {
        state = State.Started;
    }
}
