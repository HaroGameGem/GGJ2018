using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension_Casting
{
    public static int ToInt(this string str)
    {
        int num;
        bool canParse = int.TryParse(str, out num);
        if (!canParse)
            throw new Exception(string.Format("{0} can't parse int", str));
        return num;
    }

    public static float ToFloat(this string str)
    {
        float num;
        bool canParse = float.TryParse(str, out num);
        if (!canParse)
            throw new Exception(string.Format("{0} can't parse float", str));
        return num;
    }
}

