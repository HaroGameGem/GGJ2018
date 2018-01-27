using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class Character : SerializedMonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] float fadingTime;

    private Animation animation;
    private Transmission transmission;

    private const float NEAR = 0.1f;

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    public void Transmission(Transmission transmission)
    {
        Debug.Log("New " + transmission);
        this.transmission = transmission;
        CityUI src = CityUI.cityUIs[transmission.src];
        transform.position = src.transform.position;
    }

    private void Update()
    {
        Vector3 src = transform.position;

        var firewalls = transmission.firewalls;

        // 다음 목적지가 도시일 때
        Vector3 dst;
        

        if(firewalls.Count == 0)
        {
            dst = CityUI.cityUIs[transmission.dst].transform.position;

            // 도착?
            if ((dst - src).sqrMagnitude < NEAR * NEAR)
            {
                Victory();
                return;
            }

            // 아직 안 도착?
            else
                Run(dst - src);
        }

        // 다음 목적지가 방화벽일 때
        else
        {
            dst = FirewallUI.firewallUIs[transmission.firewalls.Peek()].transform.position;

            // 도착?
            if ((dst - src).sqrMagnitude < NEAR * NEAR)
            {
                Idle();
                return;
            }

            // 아직 안 도착?
            else
                Run(dst - src);
        }

        

        transform.position = Vector2.MoveTowards(src, dst, speed * Time.deltaTime);
    }

    public void Run(Vector2 direction)
    {
        if (direction.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);

        if (0 < direction.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);

        animation.Play("Run");
    }

    public void Idle()
    {
        animation.Play("Idle");
    }

    private bool isVictoried;
    public void Victory()
    {
        if (isVictoried)
            return;
        isVictoried = true;
        animation.Play("Victory");
        transmission.dst.DestroyCity();
        SpriteRenderer[] spriteRenders = GetComponentsInChildren<SpriteRenderer>();
        //foreach(var sr in spriteRenders)
        //    sr.DOFade(0, fadingTime);
        StartCoroutine(Disabling());
    }


    private IEnumerator Disabling()
    {
        yield return new WaitForSeconds(fadingTime);
        gameObject.SetActive(false);

    }
}
