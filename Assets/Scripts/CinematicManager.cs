using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour {

    public Image imgBG;
    	
    public Image[] arrImgPrologue;
    public string[] arrStrPrologue;

    public Image[] arrImgEnding;
    public string[] arrStrEnding;

    public Button btnBeginPlay;

    public Text textBox;
    bool isChatting = false;

    public AudioSource keyClickSource;

    public float textIntervalSecond = 0.12f;
    public float textVariabilitySecond = 0.08f;
    public string text;

	Coroutine routinePrologue;
    [Button]
    public void ShowPrologue()
    {
        if (routinePrologue != null)
            return;
        routinePrologue = StartCoroutine(CoShowPrologue());
    }

    IEnumerator CoShowPrologue()
    {
        FadeIn(imgBG, 1f);
		yield return new WaitForSeconds(0.8f);

        for (int i = 0; i < arrImgPrologue.Length; i++)
        {
			FadeIn(arrImgPrologue[i], 1f);
			ChatText(textIntervalSecond, textVariabilitySecond, arrStrPrologue[i]);
			yield return new WaitWhile(() => isChatting);
			yield return new WaitUntil(() => Input.GetMouseButton(0));
            textBox.text = "";
			FadeOut(arrImgPrologue[i], 1f);
		}

        btnBeginPlay.transform.localScale = Vector3.zero;
        btnBeginPlay.gameObject.SetActive(true);
        btnBeginPlay.transform.DOScale(Vector3.one, 0.5f)
                    .SetEase(Ease.OutQuad);

        routinePrologue = null;
    }

    public void HidePrologue()
    {
        if (routinePrologue != null)
            return;
        routinePrologue = StartCoroutine(CoHidePrologue());
    }

    IEnumerator CoHidePrologue()
    {
        btnBeginPlay.transform.DOScale(Vector3.zero, 0.5f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => btnBeginPlay.gameObject.SetActive(false));
        yield return new WaitForSeconds(1f);
        foreach (var item in arrImgPrologue)
        {
            item.gameObject.SetActive(false);
        }
        yield return StartCoroutine(CoLoadGameScene());
        FadeOut(imgBG, 1f);
    }

    IEnumerator CoLoadGameScene()
    {
        //yield return SceneManager.LoadSceneAsync("GameScene");
        yield return null;
    }

    Coroutine routineEnding;
    [Button]
    public void ShowEnding()
    {
        if (routineEnding != null)
            return;
        routineEnding = StartCoroutine(CoShowEnding());
    }

    IEnumerator CoShowEnding()
    {
		FadeIn(imgBG, 1f);
		yield return new WaitForSeconds(0.8f);

		for (int i = 0; i < arrImgPrologue.Length; i++)
		{
			FadeIn(arrImgEnding[i], 1f);
			ChatText(textIntervalSecond, textVariabilitySecond, arrStrEnding[i]);
			yield return new WaitWhile(() => isChatting);
			yield return new WaitUntil(() => Input.GetMouseButton(0));
			FadeOut(arrImgEnding[i], 1f);
		}

        /*
		btnBeginPlay.transform.localScale = Vector3.zero;
		btnBeginPlay.gameObject.SetActive(true);
		btnBeginPlay.transform.DOScale(Vector3.one, 0.5f)
					.SetEase(Ease.OutQuad);
					*/
		routineEnding = null;
    }

    void FadeIn(Image img, float duration)
    {
        Color color = img.color;
        if(!img.gameObject.activeSelf)
        {
            img.color = Color.clear;
			img.gameObject.SetActive(true);
        }

        img.DOColor(color, duration);
    }

    void FadeOut(Image img, float duration)
    {
        img.DOKill();
        if (!img.gameObject.activeSelf)
		{
            return;
		}
        Color color = img.color;
        img.DOColor(Color.clear, duration)
           .OnComplete(() =>
        {
            img.gameObject.SetActive(false);
            img.color = color;
        });
	}


    public void ChatText(float intervalSecond, float variabilitySecond, string text)
    {
        isChatting = true;
        StartCoroutine(CoChatText(intervalSecond, variabilitySecond, text));
    }

    IEnumerator CoChatText(float intervalSecond, float variabilitySecond, string text)
    {
		StringBuilder builder = new StringBuilder(100);
		for (int i = 0; i < text.Length; i++)
        {
            if(text[i] == 'n')
            {
                builder.Append('\n');
                continue;
            }
                
            builder.Append(text[i]);
            //사운드
            keyClickSource.pitch = Random.Range(0.8f, 1.3f);
            keyClickSource.PlayOneShot(keyClickSource.clip, Random.Range(0.8f, 1f));
            textBox.text = builder.ToString();
			yield return new WaitForSeconds(intervalSecond + Random.Range(-variabilitySecond / 2f, variabilitySecond / 2f));
		}
        isChatting = false;
    }

    public void DebugOnClick()
    {
        Debug.Log("OnClick");
    }
}
