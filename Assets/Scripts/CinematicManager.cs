using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour {

    public Image imgFade;
    public Image imgBG;
    	
    public Image[] arrImgPrologue;
    public string[] arrStrPrologue;
    bool isPlayingPrologue = false;

    public Image[] arrImgEnding;
    public string[] arrStrEnding;
    bool isPlayingEnding = false;

    public Button btnBeginPlay;
    public Button btnGoToTitle;
    public Button btnSkip;

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
		isPlayingPrologue = true;

		btnSkip.transform.localScale = Vector3.zero;
		btnSkip.gameObject.SetActive(true);
		btnSkip.transform.DOScale(Vector3.one, 0.5f)
					.SetEase(Ease.OutQuad);

        for (int i = 0; i < arrImgPrologue.Length; i++)
        {
            FadeHelper.FadeIn(arrImgPrologue[i], 1f);
			ChatText(textIntervalSecond, textVariabilitySecond, arrStrPrologue[i]);
			yield return new WaitWhile(() => isChatting);
			yield return new WaitUntil(() => Input.GetMouseButton(0));
            FadeHelper.FadeOut(arrImgPrologue[i], 1f);
		}

        btnSkip.transform.DOScale(Vector3.zero, 0.5f)
			.SetEase(Ease.InQuad)
			.OnComplete(() => btnSkip.gameObject.SetActive(false));

        btnBeginPlay.transform.localScale = Vector3.zero;
        btnBeginPlay.gameObject.SetActive(true);
        btnBeginPlay.transform.DOScale(Vector3.one, 0.5f)
                    .SetEase(Ease.OutQuad);

        routinePrologue = null;
        Debug.Log("hi Show");
    }

    public void HidePrologue()
    {
        if(routinePrologue != null)
        {
			StopCoroutine(routinePrologue);
            if(routineChatText != null)
                StopCoroutine(routineChatText);
		}
		routinePrologue = StartCoroutine(CoHidePrologue());
    }

    IEnumerator CoHidePrologue()
    {
		btnBeginPlay.transform.DOScale(Vector3.zero, 0.5f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => btnBeginPlay.gameObject.SetActive(false));
        textBox.gameObject.SetActive(false);
        btnSkip.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        foreach (var item in arrImgPrologue)
        {
            item.gameObject.SetActive(false);
        }
		isPlayingPrologue = false;
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
        isPlayingEnding = true;
        FadeHelper.FadeIn(imgBG, 1f);
		yield return new WaitForSeconds(0.8f);

		btnSkip.transform.localScale = Vector3.zero;
		btnSkip.gameObject.SetActive(true);
		btnSkip.transform.DOScale(Vector3.one, 0.5f)
					.SetEase(Ease.OutQuad);

		for (int i = 0; i < arrImgPrologue.Length; i++)
		{
            FadeHelper.FadeIn(arrImgEnding[i], 1f);
			ChatText(textIntervalSecond, textVariabilitySecond, arrStrEnding[i]);
			yield return new WaitWhile(() => isChatting);
			yield return new WaitUntil(() => Input.GetMouseButton(0));
            FadeHelper.FadeOut(arrImgEnding[i], 1f);
		}

		btnSkip.transform.DOScale(Vector3.zero, 0.5f)
			.SetEase(Ease.InQuad)
			.OnComplete(() => btnSkip.gameObject.SetActive(false));

        btnGoToTitle.transform.localScale = Vector3.zero;
		btnGoToTitle.gameObject.SetActive(true);
		btnGoToTitle.transform.DOScale(Vector3.one, 0.5f)
					.SetEase(Ease.OutQuad);
					
		routineEnding = null;
    }

	public void HideEnding()
	{
		if (routineEnding == null)
			return;
        StopAllCoroutines();
		routineEnding = StartCoroutine(CoHideEnding());
	}

	IEnumerator CoHideEnding()
	{
		btnGoToTitle.transform.DOScale(Vector3.zero, 0.5f)
					.SetEase(Ease.InQuad)
					.OnComplete(() => btnBeginPlay.gameObject.SetActive(false));
        textBox.text = "";
        FadeHelper.FadeIn(imgFade, 1f);
		yield return new WaitForSeconds(1f);
		foreach (var item in arrImgEnding)
		{
			item.gameObject.SetActive(false);
		}
        FadeHelper.FadeOut(imgBG, 1f);
        isPlayingEnding = false;
        SceneManager.LoadSceneAsync("TitleScene");
	}




	Coroutine routineChatText = null;
	public void ChatText(float intervalSecond, float variabilitySecond, string text)
    {
        isChatting = true;
        if(routineChatText != null)
        {
            StopCoroutine(routineChatText);
        }
        routineChatText = StartCoroutine(CoChatText(intervalSecond, variabilitySecond, text));
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
