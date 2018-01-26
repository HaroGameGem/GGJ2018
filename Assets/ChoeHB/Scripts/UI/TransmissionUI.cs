using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransmissionUI : SerializedMonoBehaviour {

    private Transmission transmission;

    [SerializeField] Text firewallCountText;

    public void SetTransmission(Transmission transmission)
    {
        this.transmission = transmission;
        name = string.Format("{0} => {1}",transmission.src.name, transmission.dst.name);

        UpdateFirewall();

        transmission.OnSuccessHack      += SuccessHack;
        transmission.OnSuccessFirewall  += UpdateFirewall;
    }

    private void Update()
    {
        firewallCountText.transform.rotation = Quaternion.identity;
    }

    private void SuccessHack()
    {
        gameObject.SetActive(false);
    }

    private void UpdateFirewall()
    {
        firewallCountText.text = "" + transmission.firewalls.Count;
    }
}
