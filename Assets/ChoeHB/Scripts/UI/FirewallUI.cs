using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallUI : MonoBehaviour {

    public static Dictionary<Firewall, FirewallUI> firewallUIs { get; private set; }

    private Firewall firewall;
    public void SetFirewall(Firewall firewall)
    {
        this.firewall = firewall;

        firewallUIs = firewallUIs ?? new Dictionary<Firewall, FirewallUI>();
        firewallUIs.Add(firewall, this);
    }

}
