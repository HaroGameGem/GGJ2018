using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public enum eGameState
{
    title,
    loading,
    playingGame
}

public class GameManager : MonoBehaviour {
    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public eGameState gameState = eGameState.title;
    CinematicManager cinematicManager = null;
    public GameObject optionCanvas;
    public GameObject popupExit;
    public Button btnExitAccept;
    public Button btnExitCancel;

    public Image imgBlackBoard;

    public AudioSource audioSource;
    public AudioClip clipUIBtnClick;

    private void Awake()
    {
        if (instance != null)
        {
			Destroy(gameObject);
            return;
        }

        instance = this;
        StartCoroutine(CoInit());
    }

    IEnumerator CoInit()
    {
        if(optionCanvas == null)
        {
			yield return SceneManager.LoadSceneAsync("OptionScene", LoadSceneMode.Additive);
			optionCanvas = GameObject.Find("OptionCanvas");
			popupExit = GameObject.Find("PopupExit");
			imgBlackBoard = GameObject.Find("ImgBlackBoard").GetComponent<Image>();
			btnExitAccept = GameObject.Find("BtnExitAccept").GetComponent<Button>();
            btnExitCancel = GameObject.Find("BtnExitCancel").GetComponent<Button>();
			btnExitAccept.onClick.AddListener(OnClickExitAccept);
			btnExitCancel.onClick.AddListener(OnClickExitCancel);
			popupExit.gameObject.SetActive(false);
		}
		yield return new WaitForSeconds(0.5f);
        FadeHelper.FadeOut(imgBlackBoard, 1f);

        Scene curScene = SceneManager.GetActiveScene();
        switch (curScene.name)
        {
            case "TitleScene":
				audioSource.Play();
				break;
            case "GameScene":
                break;
            default:
                break;
        }
	}

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(optionCanvas);
		SceneManager.UnloadSceneAsync("OptionScene");
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PopupExitPanel();
        }
	}

    public void PlaySFXUIBtnClick()
    {
		audioSource.PlayOneShot(clipUIBtnClick);
	}

    bool isPopuping = false;
    public void PopupExitPanel()
    {
        if (gameState == eGameState.loading)
            return;
        if (isPopuping)
            return;
        PlaySFXUIBtnClick();
        isPopuping = true;
        StartCoroutine(CoPopupExitPanel());
	}

    IEnumerator CoPopupExitPanel()
    {
		if (popupExit.activeSelf)
		{
			popupExit.SetActive(false);
			Time.timeScale = 1f;
			isPopuping = false;
		}
		else
		{
            btnExitAccept.interactable = false;
            btnExitCancel.interactable = false;
			popupExit.SetActive(true);
			Animation anim = popupExit.GetComponent<Animation>();
			yield return new WaitWhile(() => anim.IsPlaying("Popup"));
            if(gameState == eGameState.playingGame)
    			Time.timeScale = 0f;
			btnExitAccept.interactable = true;
			btnExitCancel.interactable = true;
			isPopuping = false;
		}
	}

    public void OnClickExitAccept()
    {
        audioSource.PlayOneShot(clipUIBtnClick);
        Time.timeScale = 1f;
        switch (gameState)
        {
            case eGameState.title:
				Application.Quit();
				break;
			case eGameState.playingGame:
                OnClickGoToTitle();
                break;
            default:
                break;
        }
    }

    public void OnClickExitCancel()
    {
        Time.timeScale = 1f;
        if(popupExit.activeSelf)
        {
			PlaySFXUIBtnClick();
			PopupExitPanel();
        }
    }

    public void OnApplicationQuit()
    {
        Application.CancelQuit();
#if !UNITY_EDITOR
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
	}

    public void OnClickBeginPlay()
    {
        PlaySFXUIBtnClick();
		StartCoroutine(CoBeginPlay());
    }

    IEnumerator CoBeginPlay()
    {
        gameState = eGameState.loading;
        Tweener tween = FadeHelper.FadeIn(imgBlackBoard, 1f);
        yield return new WaitWhile(() => tween.IsPlaying());
		yield return SceneManager.LoadSceneAsync("CinematicScene", LoadSceneMode.Additive);
        cinematicManager = FindObjectOfType<CinematicManager>().GetComponent<CinematicManager>();
        cinematicManager.btnBeginPlay.onClick.AddListener(OnClickPlayGame);
        cinematicManager.btnSkip.onClick.AddListener(OnClickPlayGame);
		cinematicManager.ShowPrologue();
        FadeHelper.FadeOut(imgBlackBoard, 1f);
        GameObject.Find("TitleCanvas").SetActive(false);

	}

    public void OnClickPlayGame()
    {
        PlaySFXUIBtnClick();
        StartCoroutine(CoPlayGame());
    }

    IEnumerator CoPlayGame()
    {
        cinematicManager.btnSkip.enabled = false;
        cinematicManager.btnBeginPlay.enabled = false;
        cinematicManager.btnGoToTitle.enabled = false;

		Tweener tween = FadeHelper.FadeIn(imgBlackBoard, 1f);
		yield return new WaitWhile(() => tween.IsPlaying());
		cinematicManager.HidePrologue();
        audioSource.DOFade(0f, 1f);
		yield return new WaitForSeconds(1f);
        audioSource.Stop();
        audioSource.volume = 1f;
        Camera.main.GetComponent<AudioListener>().enabled = false;
		yield return SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
		yield return SceneManager.UnloadSceneAsync("TitleScene");
        yield return SceneManager.UnloadSceneAsync("CinematicScene");
		FadeHelper.FadeOut(imgBlackBoard, 1f);
		gameState = eGameState.playingGame;
    }

    public void OnClickRestart()
    {
		PlaySFXUIBtnClick();
        StartCoroutine(CoRestart());
    }

    IEnumerator CoRestart()
    {
		gameState = eGameState.loading;
		Tweener tween = FadeHelper.FadeIn(imgBlackBoard, 1f);
		yield return new WaitWhile(() => tween.IsPlaying());
		yield return new WaitForSeconds(1f);
        yield return SceneManager.LoadSceneAsync("GameScene");
		FadeHelper.FadeOut(imgBlackBoard, 1f);
        gameState = eGameState.playingGame;
    }

	public void OnClickEnding()
	{
        PlaySFXUIBtnClick();
        StartCoroutine(CoClickEnding());
	}

    IEnumerator CoClickEnding()
    {
        yield return null;
    }

    public void OnClickGoToTitle()
    {
        PlaySFXUIBtnClick();
        StartCoroutine(CoGoToTitle());
    }

    IEnumerator CoGoToTitle()
    {
        yield return null;
        Tweener tween = FadeHelper.FadeIn(imgBlackBoard, 1f);
        yield return new WaitWhile(() => tween.IsPlaying());
        OnClickExitCancel();
        yield return SceneManager.LoadSceneAsync("TitleScene");
        GameObject.Find("BeginButton").GetComponent<Button>().onClick.AddListener(OnClickBeginPlay);
        GameObject.Find("ExitButton").GetComponent<Button>().onClick.AddListener(PopupExitPanel);
        audioSource.Play();

		FadeHelper.FadeOut(imgBlackBoard, 1f);
	}
}
