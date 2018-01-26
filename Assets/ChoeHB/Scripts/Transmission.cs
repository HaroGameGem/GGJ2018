using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Transmission {

    public readonly City src;
    public readonly City dst;
    public readonly List<Firewall> firewalls;

    public Transmission(City src, City dst, List<Firewall> firewalls)
    {
        this.src            = src;
        this.dst            = dst;
        this.firewalls      = firewalls;
    }

    public override string ToString()
    {
        return string.Format("{0} => {1} : {2}", src.name, dst.name, firewalls.Count);
    }
}
