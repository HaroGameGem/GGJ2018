using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransmissionUI : SerializedMonoBehaviour {

    private Transmission transmission;

    [SerializeField] Image touchingArea;
    [SerializeField] Button hackButton;
    [SerializeField] FirewallUI firewallPrefab;
    [SerializeField] Transform firewallHolder;

    private static bool isTryingHack;

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
        touchingArea.raycastTarget = transmission.isActived;
        hackButton.interactable = transmission.isActived;
    }
    
    public void TryHack()
    {
        if (isTryingHack)
            return;

        AudioManager.PlaySound("Hacking");
        isTryingHack = true;

        Action<bool> resultCallback = (result) =>
        {
            string audioName = result ? "HackSuccess" : "HackFail";
            AudioManager.PlaySound(audioName);
            isTryingHack = false;
            transmission.TryHack(result);
        };

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
    }

    private void HackFirewall(Firewall fire)
    {
        Destroy(FirewallUI.firewallUIs[fire].gameObject);
    }
}
