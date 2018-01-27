using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension_Color
{
    public static Color Alpha(this Color color, float a)
    {
        color.a = a;
        return color;
    }

    public static string Fill(this string text, string color)
    {
        return string.Format("<color={0}>{1}</color>", color, text);
    }
}