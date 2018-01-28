using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Progress
{
    public int dst;
    public string format;

    private float current_;
    public float current
    {
        get { return current_; }
        set
        {
            current_ = Mathf.Min(dst, value);
            current_ = Mathf.Max(0, current_);
        }
    }

    public bool isSuccessed
    {
        get { return current == dst; }
    }

    public Progress(int dst, string format = "{0}/{1}")
    {
        this.dst        = dst;
        this.format     = format;
    }

    public override string ToString()
    {
        return string.Format(format, (int)current, dst);
    }

    public static Progress operator +(Progress progress, float toAdd)
    {
        progress.current += toAdd;
        return progress;
    }

    public static Progress operator ++(Progress progress)
    {
        progress.current++;
        return progress;
    }


    public static Progress operator --(Progress progress)
    {
        progress.current--;
        return progress;
    }

}
