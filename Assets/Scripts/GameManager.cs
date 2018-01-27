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
    GameObject optionCanvas;
    GameObject popupExit;

    public Image imgBlackBoard;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
        StartCoroutine(CoInit());
    }

    IEnumerator CoInit()
    {
        if(optionCanvas == null)
        {
            Debug.Log("Test");
			yield return SceneManager.LoadSceneAsync("OptionScene", LoadSceneMode.Additive);
			Debug.Log(optionCanvas);
			optionCanvas = GameObject.Find("OptionCanvas");
            Debug.Log(optionCanvas);
			popupExit = GameObject.Find("PopupExit");
			imgBlackBoard = GameObject.Find("ImgBlackBoard").GetComponent<Image>();
			GameObject.Find("BtnExitAccept").GetComponent<Button>().onClick.AddListener(OnClickExitAccept);
			GameObject.Find("BtnExitCancel").GetComponent<Button>().onClick.AddListener(OnClickExitCancel);
			popupExit.gameObject.SetActive(false);
		}
		yield return new WaitForSeconds(0.5f);
        FadeHelper.FadeOut(imgBlackBoard, 1f);
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

    bool isPopuping = false;
    public void PopupExitPanel()
    {
        if (gameState == eGameState.loading)
            return;
        if (isPopuping)
            return;
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
			popupExit.SetActive(true);
			Animation anim = popupExit.GetComponent<Animation>();
			yield return new WaitWhile(() => anim.IsPlaying("Popup"));
            if(gameState == eGameState.playingGame)
    			Time.timeScale = 0f;
			isPopuping = false;
		}
	}

    public void OnClickExitAccept()
    {
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
        PopupExitPanel();
    }

    public void OnApplicationQuit()
    {
        Application.CancelQuit();
#if !UNITY_EDITOR
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
	}

    public void DebugOnClick()
    {
        Debug.Log("OnClick");
    }

    public void OnClickBeginPlay()
    {
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
		yield return new WaitForSeconds(1f);
        Camera.main.GetComponent<AudioListener>().enabled = false;
		yield return SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
		yield return SceneManager.UnloadSceneAsync("TitleScene");
        yield return SceneManager.UnloadSceneAsync("CinematicScene");
		FadeHelper.FadeOut(imgBlackBoard, 1f);
		gameState = eGameState.playingGame;
    }

    public void EndGame()
    {
        
    }

    IEnumerator CoEndGame()
    {
        yield return null;
		cinematicManager.btnBeginPlay.onClick.AddListener(OnClickPlayGame);
	}

    public void OnClickGoToTitle()
    {
        StartCoroutine(CoGoToTitle());
    }

    IEnumerator CoGoToTitle()
    {
        yield return null;
        Tweener tween = FadeHelper.FadeIn(imgBlackBoard, 1f);
        yield return new WaitWhile(() => tween.IsPlaying());
        OnClickExitCancel();
        yield return SceneManager.LoadSceneAsync("TitleScene");
		yield return SceneManager.UnloadSceneAsync("OptionScene");
		FadeHelper.FadeOut(imgBlackBoard, 1f);
	}
}
