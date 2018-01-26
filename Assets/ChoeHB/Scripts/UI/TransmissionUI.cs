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

        //CityUI src = WorldMap.instance.cityUIs[transmission.src];
        //CityUI dst = WorldMap.instance.cityUIs[transmission.dst];

        //transform.SetParent(src.transform);
        //transform.localPosition = Vector2.zero;

        //Vector2 direction = dst.transform.position - src.transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * 180 / 3.14f;

        //arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        name = string.Format("{0} => {1}",transmission.src.name, transmission.dst.name);
        firewallCountText.text = "" + transmission.firewalls.Count;
        Debug.Log(firewallCountText.text);
    }

    private void Update()
    {
        firewallCountText.transform.rotation = Quaternion.identity;
    }

}
