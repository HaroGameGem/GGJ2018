using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransmissionUI : SerializedMonoBehaviour {

    private Transmission transmission;

    [SerializeField] Text firewallCountText;
    [SerializeField] Image firewallPrefab;
    [SerializeField] Transform firewallHolder;

    private Stack<Image> firewallImages = new Stack<Image>();
    
    public void SetTransmission(Transmission transmission)
    {
        this.transmission = transmission;
        name = string.Format("{0} => {1}",transmission.src.name, transmission.dst.name);

        UpdateFirewall();

        transmission.OnSuccessDestroy      += Disable;
        transmission.OnSuccessHack  += UpdateFirewall;
    }

    private void Update()
    {
        firewallCountText.transform.rotation = Quaternion.identity;
    }

    public void TryHack()
    {
        Action<bool> resultCallback = transmission.TryHack;
        NumPad.instance.Float();
        NumPad.instance.Active(transmission.firewalls.Peek().difficulty, resultCallback);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void UpdateFirewall()
    {
        int firewallCount = transmission.firewalls.Count;
        int gap = firewallCount - firewallImages.Count;

        // 이미 생성된 방화벽의 갯수 보다 목표 방화벽의 갯수가 더 많으면 새로운 방화벽을 추가한다.
        if (0 < gap)
        {
            for (int i = 0; i < gap; i++)
            {
                var instance = Instantiate(firewallPrefab);
                    instance.transform.SetParent(firewallHolder, false);
                    firewallImages.Push(instance);

                instance.gameObject.SetActive(true);
            }
        }

        // 기존에 있던 방화벽을 파괴한다.
        if(gap < 0)
        {
            for (int i = 0; i < gap; i++)
                Destroy(firewallImages.Pop());
        }

        firewallCountText.text = "" + firewallCount;
    }
}
