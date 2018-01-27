using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Transmission {

    // 방화벽 1개 뚫었을 때
    public event Action<Firewall> OnSuccessHack;

    // 새로운 방화벽이 추가되었을 때
    public event Action<Firewall> OnAddFirewall;

    // 모든 방화벽을 뚫었을 때
    public event Action OnSuccessDestroy;

    public readonly City src;
    public readonly City dst;
    public readonly List<Firewall> firewalls;

    public bool isActived { get; private set; }

    private const int MIN_FIREWALL_COUNT = 4;

    public Transmission(City src, City dst, List<Firewall> firewalls)
    {
        this.src            = src;
        this.dst            = dst;
        this.firewalls      = firewalls;

        isActived = false;
    }
    

    // 약화
    public void Debuffed()
    {
        firewalls.ForEach(fw => fw.Debuffed());
    }
    
    // 활성화
    public void Active()
    {
        isActived = true;
    }

    public void TryHack(bool result)
    {
        if (result)
            SuccessHack();

        else
            FailHack();
    }

    public Firewall Top()
    {
        return firewalls[firewalls.Count - 1];
    }

    // 점령당함
    public void SuccessDestroy()
    {
        int firewallsCount = firewalls.Count;
        for (int i = 0; i < firewallsCount; i++)
            SuccessHack();

        isActived = false;
        
        if (OnSuccessDestroy != null)
            OnSuccessDestroy();
    }

    // 방화벽 1개 부숨
    private void SuccessHack()
    {
        Firewall top = Top();
        firewalls.Remove(top);

        if (OnSuccessHack != null)
            OnSuccessHack(top);
    }

    private void FailHack()
    {
    }

    // 백신이 방화벽을 복구
    public void Recovery()
    {
        Active();
        Firewall firewall = new Firewall(dst.difficulty);
            firewalls.Add(firewall);

        if (OnAddFirewall != null)
            OnAddFirewall(firewall);
    }

    public override string ToString()
    {
        return string.Format("{0} => {1} : {2}", src.name, dst.name, firewalls.Count);
    }
}
