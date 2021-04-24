using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.SlotCar.Scripts.UI {
	public class Countdown : MonoBehaviour
	{
		TextMeshProUGUI text;
		public int seconds = 3;
		private int startSeconds;
		public UnityEvent countdownFinish;
		private AudioSource sound;
		// Start is called before the first frame update
		void Awake()
		{
			text = GetComponent<TextMeshProUGUI>();
			sound = GetComponent<AudioSource>();
		}

		public void StartCountdown() {
			gameObject.SetActive(true);
			startSeconds = seconds;
			text.text = startSeconds.ToString();
			GetComponent<RectTransform>().localScale = new Vector3(0.0f, 0.0f, 1.0f);
			StartCoroutine("SpeedTick");
			sound.Play();
		}

		IEnumerator SpeedTick() {
			RectTransform tr = GetComponent<RectTransform>();
			tr.DOScale(Vector3.one, 0.9f)
				.OnComplete(() => tr.localScale = new Vector3(0.0f, 0.0f, 1.0f));
			yield return new WaitForSeconds(1.0f);
			while (true) // Or whatever condition you want
			{
				startSeconds--;
				if (startSeconds == 0) {
					text.text = "GO!";
					tr.DOScale(Vector3.one, 0.9f)
						.OnComplete(() => {
							countdownFinish.Invoke();
							gameObject.SetActive(false);
						});
					StopCoroutine("SpeedTick");
					yield return null;
				}
				tr.DOScale(Vector3.one, 0.9f)
					.OnComplete(() => tr.localScale = new Vector3(0.0f, 0.0f, 1.0f));
				text.text = startSeconds.ToString();
				yield return new WaitForSeconds(1.0f);
			}
		}
	}
}
