using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityUI : MonoBehaviour {

    public static bool isStarted;

    public event Action<City> OnClick;

    [SerializeField] Image cityImage;

    private Button button;
    private City city;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetCity(City city)
    {
        this.city = city;
    }

    public void Click()
    {
        if (OnClick != null)
            OnClick(city);
    }
    
    public static void StartHack()
    {
        isStarted = true;
    }

    private void Update()
    {
        if (!isStarted)
            return;
        button.interactable = city.isHacked;
    }
}
