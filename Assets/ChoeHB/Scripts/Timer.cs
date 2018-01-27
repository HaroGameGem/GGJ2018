using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer {

    public float remain
    {
        get {
            float elapsedTime = (isStopped ? stoppedTime : Time.time) - startTime;
            float result = Mathf.Max(0, maxTime - elapsedTime);

            if (!isStopped && result == 0)
                Stop();
            return result;
        }
    }

    private float maxTime;
    private float startTime;

    private float stoppedTime;
    private bool isStopped;

    public Timer(float maxTime)
    {
        this.maxTime = maxTime;
        startTime = Time.time;
    }
    
    public void Stop()
    {
        isStopped = true;
        stoppedTime = Time.time;
    }

    public override string ToString()
    {
        int minuite = (int)remain / 60;
        int second = (int)remain % 60;

        return string.Format("{0} : {1}", minuite.ToString("00"), second.ToString("00"));
    }
}
