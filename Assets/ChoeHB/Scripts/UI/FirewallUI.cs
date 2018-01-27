using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirewallUI : MonoBehaviour {

    public static Dictionary<Firewall, FirewallUI> firewallUIs { get; private set; }

    private Firewall firewall;

    public const int RED = 10;

    [SerializeField] Image image;

    public void SetFirewall(Firewall firewall)
    {
        this.firewall = firewall;
        firewall.OnUpdateDifficulty += UpdateColor;
        firewallUIs = firewallUIs ?? new Dictionary<Firewall, FirewallUI>();
        firewallUIs.Add(firewall, this);

        UpdateColor();
    }

    private void UpdateColor()
    {
        float t = firewall.difficulty / (float)RED;
        image.color = Color.Lerp(Color.green, Color.red, t);
    }

}

