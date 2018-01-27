using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransmissionUI : SerializedMonoBehaviour {

    public static event Action<Vector3> OnAddFirewall;
    public static event Action<Vector3> OnDestroyFirewall;

    private Transmission transmission;

    // Interact
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadingAlpha;

    [SerializeField] Image touchingArea;
    [SerializeField] FirewallUI firewallPrefab;
    [SerializeField] Transform firewallHolder;

    public static bool isTryingHack { get; private set; }

    private bool cachedIsActive;
    public void SetTransmission(Transmission transmission)
    {
        this.transmission = transmission;
        name = string.Format("{0} => {1}",transmission.src.name, transmission.dst.name);

        foreach (var firewall in transmission.firewalls)
            AddFirewall(firewall);

        transmission.OnSuccessHack          += HackFirewall;
        transmission.OnAddFirewall          += AddFirewall;

        cachedIsActive = !transmission.isActived;
    }


    private void Update()
    {
        if (transmission == null)
            return;

        if (cachedIsActive == transmission.isActived)
            return;

        canvasGroup.interactable = transmission.isActived;
        canvasGroup.blocksRaycasts = transmission.isActived;
        canvasGroup.alpha = transmission.isActived ? 1 : fadingAlpha;
        touchingArea.raycastTarget = transmission.isActived;

        cachedIsActive = transmission.isActived;
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
            CityZoomer.instance.ZoomOut();
            transmission.TryHack(result);
        };

        CityUI dst = CityUI.cityUIs[transmission.dst];
        CityZoomer.instance.ZoomIn(dst.transform.position);

        NumPad.instance.Float();

        List<Firewall> firewalls = transmission.firewalls;
        if (firewalls.Count == 0)
            return;

        NumPad.instance.Active(firewalls[firewalls.Count - 1].difficulty, resultCallback);
    }

    private void AddFirewall(Firewall firewall)
    {
        var firewallUI = Instantiate(firewallPrefab, firewallHolder);
            firewallUI.SetFirewall(firewall);
            firewallUI.transform.SetParent(firewallHolder, false);
            firewallUI.gameObject.SetActive(true);

        firewallUI.name = string.Format("Firewall({0})", transmission.firewalls.IndexOf(firewall));

        if (OnAddFirewall != null)
            OnAddFirewall(firewallUI.transform.position);
    }

    private void HackFirewall(Firewall fire)
    {
        Vector3 destroyedPosition = FirewallUI.firewallUIs[fire].gameObject.transform.position;
        Destroy(FirewallUI.firewallUIs[fire].gameObject);

        if (OnDestroyFirewall != null)
            OnDestroyFirewall(destroyedPosition);
    }
}
