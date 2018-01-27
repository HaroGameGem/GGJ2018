using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer {

    public float remain
    {
        get {
            float elapsedTime = Time.time - startTime;
            return maxTime - elapsedTime;
        }
    }

    private float maxTime;
    private float startTime;

    public Timer(float maxTime)
    {
        this.maxTime = maxTime;
        startTime = Time.time;
    }

}
