using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumPad : MonoBehaviour
{
    public Button[] buttons;
    System.Random random = new System.Random();
    public Text[] text;
    public Image image;
    public Image Pad;
    public Text endtext;

    int usernum = 0, hack = 0, numcnt = 0, success = 0, clicked = 0;
    float timecount = 0.0f;

    // Update is called once per frame
    void Update()
    {
        //지금 해킹중이면 시간체크
        if (hack == 1)
        {
            image.fillAmount -= timecount / 360;
            timecount += Time.deltaTime;
        }
        // 실패하거나 성공했을때
        if (resultCallback == null)
            return;
        //성공했을때
        if (success >= numcnt && (5 - timecount) >= 0)
        {
            End();
            endtext.GetComponent<Text>().text = "win";
            endtext.rectTransform.localPosition = new Vector2(0, 0);
            resultCallback(true);
        }
        //실패했을때
        else if ((5 - timecount) < 0)
        {
            End();
            endtext.GetComponent<Text>().text = "Lose";
            endtext.rectTransform.localPosition = new Vector2(0, 0);
            resultCallback(false);
        }

    }

    // 숫자칸에 들어갈 랜덤수 만들고 넣기
    void Random()
    {
        int filled = 0;
        while (filled < 9)
        {
            int check = 0, n = random.Next(1, 10);
            for (int i = 0; i < filled; i++)
            {
                if (buttons[i].GetComponentInChildren<Text>().text == n.ToString())
                {
                    check = 1;
                }
            }
            if (check == 0)
            {
                Debug.Log("Set" + n);
                buttons[filled].GetComponentInChildren<Text>().text = n.ToString();
                filled++;
            }
        }
    }

    //버튼 클릭이 되었을때 맞으면 0으로 바꾸고 틀리면 탈락
    public void OnClick(int index)
    {
        usernum = int.Parse(buttons[index].GetComponentInChildren<Text>().text);
        Debug.Log(usernum);
        if (usernum == int.Parse(text[success].GetComponent<Text>().text) && hack == 1)
        {
            text[success].GetComponent<Text>().text = "0";
            success++;
        }
        else
        {
            End();
            endtext.GetComponent<Text>().text = "Lose";
            endtext.rectTransform.localPosition = new Vector2(0, 0);
            resultCallback(false);
        }
    }

    Action<bool> resultCallback;
    
    //패드가 나옴
    public void Float()
    {
        Pad.rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    //해킹을 시작할때 
    public void Active(int numCount, Action<bool> resultCallback)
    {
        Debug.Log("adad");
        hack = 1;
        Random();
        int[] PinNum = new int[numCount];

        //해킹할 번호 보여주기
        for (int i = 0; i < numCount; i++)
        {
            PinNum[i] = random.Next(1, 9);
            text[i].rectTransform.localPosition = new Vector2(-numCount * 135 / 2 + i * 135 + (float)+67.5, 0);
            text[i].GetComponent<Text>().text = PinNum[i].ToString();
        }
        numcnt = numCount;
        this.resultCallback = resultCallback;
    }

    //끝났을때 다 없애기
    void End()
    {
        Destroy(image.gameObject);
        Destroy(Pad.gameObject);
        for (int i = 0; i < 8; i++)
        {
            Destroy(text[i].gameObject);
        }
        resultCallback = null;
        hack = 0;
    }
}