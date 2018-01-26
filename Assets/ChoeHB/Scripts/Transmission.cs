using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Transmission {

    public City destination;
    public List<Firewall> firewalls;

    public Transmission(City destination, List<Firewall> firewalls)
    {
        this.destination    = destination;
        this.firewalls      = firewalls;
    }

    //public override string ToString()
    //{
    //    string txt = "to " + destination.name + "\n";
    //    foreach(var firewall in firw)
    //}
}
