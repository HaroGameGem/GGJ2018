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

    [SerializeField] Character characterPrefab;
    [SerializeField] Transform characterHolder;

    public event Action<City> OnClick;

    private Button button;
    private City city;

    

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (state == State.Selecting)
            button.interactable = city.canStart;

        if (state == State.Started)
            button.interactable = city.isDestroied;
        
    }

    public void SetCity(City city)
    {
        this.city = city;
        cityUIs = cityUIs ?? new Dictionary<City, CityUI>();
        cityUIs.Add(city, this);
        city.OnDestroy += Transmission;
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
        foreach(var transmission in city.GetActivedTransmission())
        {
            CityUI target = cityUIs[transmission.dst];
            Character character = Instantiate(characterPrefab);
            //character.transform.SetParent(characterHolder, false);
            character.Transmission(transmission);
            character.gameObject.SetActive(true);
        }
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
