using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour {

    private float interval;
    private Action OnAlert;

    public void StartAlert(float interval, Action OnAlert)
    {
        this.interval   = interval;
        this.OnAlert    = OnAlert;
        StartCoroutine(Timer());
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    private IEnumerator Timer()
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            if (OnAlert != null)
                OnAlert();
        }
    }
}
