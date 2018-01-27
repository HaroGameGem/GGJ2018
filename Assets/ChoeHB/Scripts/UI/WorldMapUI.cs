using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour {

    [SerializeField] WorldMap worldMap;
    [SerializeField] Text timeText;
    [SerializeField] Text creditText;
    [SerializeField] Text destroyedCityText;

    [SerializeField] Image timerBlackCurtain;
    [SerializeField] float blinkInterval;

    private void Awake()
    {
        StartCoroutine(Blinking());
    }

    private void Update()
    {
        if (worldMap.state == WorldMap.State.SelectingCity)
            timeText.text = FormatTime(worldMap.deadline);

        else
            timeText.text = FormatTime(worldMap.timer.remain);

        creditText.text         = worldMap.credit.ToString();
        destroyedCityText.text  = worldMap.destroyedCitys.ToString();
    }

    private string FormatTime(float time)
    {
        int minuite = (int)time / 60;
        int second  = (int)time % 60;

        return string.Format("{0} : {1}", minuite.ToString("00"), second.ToString("00"));
    }
    

    private IEnumerator Blinking()
    {
        while(true)
        {
            bool currentActive = timerBlackCurtain.gameObject.activeSelf;
            timerBlackCurtain.gameObject.SetActive(!currentActive);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}

