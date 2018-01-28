using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] string format = "{0} / {1}";
    [SerializeField] float updatingInterval;

    private Progress progress;


    private int dst;

    private int srcCurrent;
    private int dstCurrent;

    public void SetProgress(Progress progress)
    {
        this.progress = progress;
        StartCoroutine(Updating());
    }


    private IEnumerator Updating()
    {
        while(true)
        {
            yield return new WaitForSeconds(updatingInterval);

        }
    }
}
