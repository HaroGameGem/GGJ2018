using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumPad : StaticComponent<NumPad>
{
    [SerializeField] float deadline;

    System.Random random = new System.Random();

    public GameObject numPad;

    public Image image;
    public Image Pad;

    public float fadingTime;

    public Text[] text;
    public Button[] buttons;
    
    int usernum = 0, hack = 0, numcnt = 0, success = 0, clicked = 0;
    float timecount = 0.0f;

    private void Awake()
    {
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        //지금 해킹중이면 시간체크
        if (hack == 1)
        {
            image.fillAmount = 1 - (timecount / deadline);
            timecount += Time.deltaTime;
        }
        // 실패하거나 성공했을때
        if (resultCallback == null)
            return;

        //성공했을때
        if (success >= numcnt && (deadline - timecount) >= 0)
        {
            resultCallback(true);
            Close();
        }
        //실패했을때
        else if ((deadline - timecount) < 0)
        {
            resultCallback(false);
            Close();
        }

    }

    // 숫자칸에 들어갈 랜덤수 만들고 넣기
    void Random()
    {
        int filled = 0;
        while (filled < 9)
        {
            int check = 0;
            int n = random.Next(1, 10);
            for (int i = 0; i < filled; i++)
            {
                if (buttons[i].GetComponentInChildren<Text>().text == n.ToString())
                {
                    check = 1;
                }
            }
            if (check == 0)
            {
                buttons[filled].GetComponentInChildren<Text>().text = n.ToString();
                filled++;
            }
        }
    }

    //버튼 클릭이 되었을때 맞으면 0으로 바꾸고 틀리면 탈락
    public void OnClick(int index)
    {
        usernum = int.Parse(buttons[index].GetComponentInChildren<Text>().text);
        Text successText = text[success];
        if (successText.text == " ")
            return;
        if (usernum == int.Parse(successText.text) && hack == 1)
        {
            var seq = DOTween.Sequence();
                seq.Append(successText.DOFade(0, fadingTime));

            seq.OnComplete(() =>
            {
                successText.text = " ";
                successText.DOFade(1, 0);
            });

            // origin
            // text[success].GetComponent<Text>().text = " ";
            success++;
        }
        else
        {
            resultCallback(false);
            Close();
        }
    }

    Action<bool> resultCallback;
    
    //패드가 나옴
    public void Float()
    {
        numPad.gameObject.SetActive(true);
    }


    //해킹을 시작할때 
    public void Active(int numCount, Action<bool> resultCallback)
    {
        hack = 1;
        Random();
        int[] PinNum = new int[numCount];

        //해킹할 번호 보여주기
        text.ForEach(t => t.gameObject.SetActive(false));

        for (int i = 0; i < numCount; i++)
        {
            text[i].gameObject.SetActive(true);
            PinNum[i] = random.Next(1, 9);
            text[i].text = PinNum[i].ToString();
        }
        numcnt = numCount;
        this.resultCallback = resultCallback;
    }

    //끝났을때 다 없애기
    public void Close()
    {
        numPad.gameObject.SetActive(false);
        text.ForEach(t => t.text = " ");
        resultCallback = null;
        hack = 0;
        timecount = 0;
        image.fillAmount = 1;
        success = 0;
    }
}