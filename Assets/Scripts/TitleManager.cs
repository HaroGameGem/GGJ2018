using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Text;
using Sirenix.OdinInspector;

public class TitleManager : MonoBehaviour {

	public Image imgTitle;
	
    public Image imgPrologue0;
    public Image imgPrologue1;

    public Text textBox;

    public AudioSource keyClickSource;

    public float textIntervalSecond = 0.12f;
    public float textVariabilitySecond = 0.08f;
    public string text;

	// Use this for initialization
	void Start () {
		ChatText(0.12f, 0.08f, "김동글이 다아아 김도윽으응 김동 김동\n김도오옹글으을");
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
	}

    public int loadPrologueIndex = 0;
    
    [Button]
    public void FadeInPrologue()
    {
        Image img;
        if(loadPrologueIndex == 0)
        {
            img = imgPrologue0;
        }
        else
        {
            img = imgPrologue1;
        }

        FadeIn(img, 1f);
    }

    void FadeIn(Image img, float duration)
    {
        if(!img.IsActive())
        {
            img.color = Color.white;
            img.gameObject.SetActive(true);
        }
        Color color = img.color;
        color.a = 255f;
        img.DOColor(color, duration);
    }

    [Button]
    public void FadeOutPrologue()
    {
		Image img;
		if (loadPrologueIndex == 0)
		{
			img = imgPrologue0;
		}
		else
		{
			img = imgPrologue1;
		}

        FadeOut(img, 1f);
	}

    void FadeOut(Image img, float duration)
    {
		if (!img.IsActive())
		{
            return;
		}
        Color color = Color.black;
		img.DOColor(color, duration);
        img.gameObject.SetActive(true);
	}


    public void ChatText(float intervalSecond, float variabilitySecond, string text)
    {
        StartCoroutine(CoChatText(intervalSecond, variabilitySecond, text));
    }

    IEnumerator CoChatText(float intervalSecond, float variabilitySecond, string text)
    {
        StringBuilder builder = new StringBuilder(100);
        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds(intervalSecond + Random.Range(-variabilitySecond / 2f, variabilitySecond / 2f));
            builder.Append(text[i]);
            //사운드
            keyClickSource.pitch = Random.Range(0.8f, 1.3f);
            keyClickSource.PlayOneShot(keyClickSource.clip, Random.Range(0.8f, 1f));
            textBox.text = builder.ToString();
        }
    }
}
