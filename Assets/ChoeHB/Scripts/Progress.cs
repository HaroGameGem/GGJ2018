using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Progress
{
    public int dst;
    public string format;

    private int current_;
    public int current
    {
        get { return current_; }
        set
        {
            current_ = Mathf.Min(dst, value);
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
        return string.Format(format, current, dst);
    }

    public static Progress operator +(Progress progress, int toAdd)
    {
        progress.current += toAdd;
        return progress;
    }

    public static Progress operator ++(Progress progress)
    {
        progress.current++;
        return progress;
    }

}
