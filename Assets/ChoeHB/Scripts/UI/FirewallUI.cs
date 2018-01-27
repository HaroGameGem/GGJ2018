using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirewallUI : MonoBehaviour {

    public static Dictionary<Firewall, FirewallUI> firewallUIs { get; private set; }

    private Firewall firewall;

    public const int RED = 8;

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
        float srcH, srcS, srcV;
        float dstH, dstS, dstV;

        Color.RGBToHSV(Color.green, out srcH, out srcS, out srcV);
        Color.RGBToHSV(Color.red, out dstH, out dstS, out dstV);
        
        float t = firewall.difficulty / (float)RED;
        float h, s, v;

        h = Mathf.Lerp(srcH, dstH, t);
        s = Mathf.Lerp(srcS, dstS, t);
        v = Mathf.Lerp(srcV, dstV, t);

        Color color = Color.HSVToRGB(h, s, v);
        image.color = color;
    }

}

