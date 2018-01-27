﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour {

    [SerializeField] WorldMap worldMap;
    [SerializeField] Text timeText;
    [SerializeField] Text creditText;
    [SerializeField] Text destroyedCityText;

    private void Update()
    {
        if (worldMap.isStarted)
            timeText.text = FormatTime(worldMap.timer.remain);

        creditText.text         = worldMap.credit.ToString();
        destroyedCityText.text  = worldMap.destroyedCitys.ToString();
    }

    private string FormatTime(float time)
    {
        int minuite = (int)time / 60;
        int second  = (int)time % 60;

        return string.Format("{0} : {1}", minuite.ToString("00"), second.ToString("00"));
        //return (minuite < 10 ? "0" + minuite.ToString() : minuite.ToString()) + " : " + (second < 10 ? "0" + second.ToString() : second.ToString())
    }

}