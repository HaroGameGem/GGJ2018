using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour {

    [SerializeField] Animation animation;

    [SerializeField] Text timeText;
    [SerializeField] Text creditText;
    [SerializeField] Text cityText;

    public void Float(GameResult result)
    {
        timeText.text = result.time.ToString();
        creditText.text = result.credit.ToString();
        cityText.text = result.citys.ToString();

        gameObject.SetActive(true);
        animation.Play(result.isCleard ? "Result_Clear" : "Result_Fail");
    }

    public void Restart()
    {
        //SceneManager.LoadScene("GameScene");
        GameManager.Instance.OnClickRestart();
    }

    public void Title()
    {
        //SceneManager.LoadScene("TitleScene");
        GameManager.Instance.OnClickGoToTitle();
    }

    public void Ending()
    {
        GameManager.Instance.OnClickEnding();

    }
}
