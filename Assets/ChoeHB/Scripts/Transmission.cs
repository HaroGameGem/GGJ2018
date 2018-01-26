using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Transmission {

    // 방화벽 1개 뚫었을 때
    public event Action OnSuccessFirewall;

    // 모든 방화벽을 뚫었을 때
    public event Action OnSuccessHack;

    public readonly City src;
    public readonly City dst;
    public readonly Stack<Firewall> firewalls;

    public Transmission(City src, City dst, Stack<Firewall> firewalls)
    {
        this.src            = src;
        this.dst            = dst;
        this.firewalls      = firewalls;
    }

    public void SuccessFirewall()
    {
        firewalls.Pop();

        if (OnSuccessFirewall != null)
            OnSuccessFirewall();

        if (firewalls.Count == 0)
            SuccessHack();
    }

    public void SuccessHack()
    {
        firewalls.Clear();
        if (OnSuccessHack != null)
            OnSuccessHack();
    }

    public override string ToString()
    {
        return string.Format("{0} => {1} : {2}", src.name, dst.name, firewalls.Count);
    }
}
