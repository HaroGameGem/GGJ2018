using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension_String
{
    public static string Format(this string str, params object[] parms)
    {
        return string.Format(str, parms);
    }
}