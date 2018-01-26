using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class CityUI : SerializedMonoBehaviour {

    public static bool isStarted;

    [SerializeField] Image cityImage;

    public event Action<City> OnClick;

    private Button button;
    private City city;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (!isStarted)
            return;
        button.interactable = city.isHacked;
    }

    public void SetCity(City city)
    {
        this.city = city;
    }

    public void Click()
    {
        ZoomIn();
        if (OnClick != null)
            OnClick(city);
    }

    private void ZoomIn()
    {
        CityZoomer.ZoomIn(this);
    }

    public static void StartHack()
    {
        isStarted = true;
    }
}
