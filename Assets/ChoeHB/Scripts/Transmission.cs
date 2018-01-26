using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Transmission {

    // 방화벽 1개 뚫었을 때
    public event Action OnSuccessHack;

    // 모든 방화벽을 뚫었을 때
    public event Action OnSuccessDestroy;

    public readonly City src;
    public readonly City dst;
    public readonly Stack<Firewall> firewalls;

    public Transmission(City src, City dst, Stack<Firewall> firewalls)
    {
        this.src            = src;
        this.dst            = dst;
        this.firewalls      = firewalls;
    }


    // 점령당함
    public void SuccessDestroy()
    {
        firewalls.Clear();
        if (OnSuccessDestroy != null)
            OnSuccessDestroy();
    }

    // 방화벽 1개 부숨
    private void SuccessHack()
    {
        firewalls.Pop();

        if (OnSuccessHack != null)
            OnSuccessHack();

        if (firewalls.Count == 0)
            dst.DestroyCity();

    }

    public void TryHack(bool result)
    {
        Debug.Log(result);
        if (result)
            SuccessHack();

        else
            FailHack();
    }

    private void FailHack()
    {
        Debug.Log("Fail..");
    }

    public override string ToString()
    {
        return string.Format("{0} => {1} : {2}", src.name, dst.name, firewalls.Count);
    }
}
