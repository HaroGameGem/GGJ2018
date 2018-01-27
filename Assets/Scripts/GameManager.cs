using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    CinematicManager cinematicManager = null;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);

	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
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
		yield return SceneManager.LoadSceneAsync("CinematicScene", LoadSceneMode.Additive);
        cinematicManager = FindObjectOfType<CinematicManager>().GetComponent<CinematicManager>();
        cinematicManager.ShowPrologue();
	}
}
