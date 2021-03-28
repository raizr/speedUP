using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
	TextMeshProUGUI text;
	public float upFadeDuration = 0.5f;
	public float downFadeDuration = 2.0f;
	public float moveDuration = 1.0f;
	public float upYPos = 20.0f;
	void Awake() {
		text = GetComponent<TextMeshProUGUI>();
		text.alpha = 0.0f;
	}

	public void Show(int points, int multplier) {
		text.text = "+" + points + " x" + multplier;
		text.DOFade(255.0f, upFadeDuration);
		text.DOFade(0f, downFadeDuration);
		RectTransform rect = GetComponent<RectTransform>();
		rect.DOLocalMove(rect.localPosition + Vector3.up * upYPos, moveDuration)
			.OnComplete(() => {
				Destroy(gameObject);
			});
	}
}
