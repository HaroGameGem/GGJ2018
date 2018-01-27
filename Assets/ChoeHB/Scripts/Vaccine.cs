﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vaccine : MonoBehaviour {

    [SerializeField] float deadline;
    [SerializeField] Image filledArea;

    private Coroutine recovering;

    private City city;

    public void SetCity(City city)
    {
        this.city = city;
    }

    public void Occur()
    {
        filledArea.fillAmount = 1;

        gameObject.SetActive(true);
        recovering = StartCoroutine(Recovering(deadline));
    }

    public void Interrupt()
    {
        StopCoroutine(recovering);
        gameObject.SetActive(false);
    }

    private IEnumerator Recovering(float deadline)
    {
        float startingTime = Time.time;
        float remainingTime = startingTime;
        while(0 < remainingTime)
        {
            float elapedTime = Time.time - startingTime;
            remainingTime = deadline - elapedTime;
            filledArea.fillAmount = remainingTime / deadline;
            yield return new WaitForEndOfFrame();
        }

        city.Recovery();
        gameObject.SetActive(false);
    }
    
}