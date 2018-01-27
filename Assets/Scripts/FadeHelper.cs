using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeHelper : MonoBehaviour {

	public static Tweener FadeIn(Image img, float duration)
	{
		Color color = img.color;
		if (!img.gameObject.activeSelf)
		{
			img.color = Color.clear;
			img.gameObject.SetActive(true);
		}

		return img.DOColor(color, duration);
	}

	public static Tweener FadeOut(Image img, float duration)
	{
		img.DOKill();
		if (!img.gameObject.activeSelf)
		{
			return null;
		}
		Color color = img.color;
		return img.DOColor(Color.clear, duration)
		   .OnComplete(() =>
		   {
			   img.gameObject.SetActive(false);
			   img.color = color;
		   });
	}

}
