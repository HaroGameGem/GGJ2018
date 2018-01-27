using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransmissionUI : SerializedMonoBehaviour {

    private Transmission transmission;

    [SerializeField] Button hackButton;
    [SerializeField] Text firewallCountText;
    [SerializeField] FirewallUI firewallPrefab;
    [SerializeField] Transform firewallHolder;

    public void SetTransmission(Transmission transmission)
    {
        this.transmission = transmission;
        name = string.Format("{0} => {1}",transmission.src.name, transmission.dst.name);

        foreach (var firewall in transmission.firewalls)
            AddFirewall(firewall);

        transmission.OnSuccessDestroy       += Disable;
        transmission.OnSuccessHack          += HackFirewall;
        transmission.OnAddFirewall          += AddFirewall;
    }

    private void Update()
    {
        hackButton.interactable = transmission.isActived;
        firewallCountText.transform.rotation = Quaternion.identity;
    }

    public void TryHack()
    {
        Action<bool> resultCallback = transmission.TryHack;
        NumPad.instance.Float();
        NumPad.instance.Active(transmission.firewalls[transmission.firewalls.Count-1].difficulty, resultCallback);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void AddFirewall(Firewall firewall)
    {
        var firewallUI = Instantiate(firewallPrefab, firewallHolder);
            firewallUI.SetFirewall(firewall);
            firewallUI.transform.SetParent(firewallHolder, false);
            firewallUI.gameObject.SetActive(true);
            firewallUI.name = string.Format("Firewall({0})", transmission.firewalls.IndexOf(firewall));

        firewallCountText.text = "" + transmission.firewalls.Count;
    }

    private void HackFirewall(Firewall fire)
    {
        Destroy(FirewallUI.firewallUIs[fire].gameObject);
        firewallCountText.text = "" + transmission.firewalls.Count;
    }
}
