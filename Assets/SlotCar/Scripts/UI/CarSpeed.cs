using System.Globalization;
using DG.Tweening;
using PathCreation.Examples;
using TMPro;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class CarSpeed : MonoBehaviour {

		public PathFollower car;
		private TextMeshProUGUI text;
		public float timeDown = 0.1f;
		public float timeUp = 0.1f;

		void Awake() {
			text = GetComponent<TextMeshProUGUI>();
		}
		// Start is called before the first frame update
		void OnGUI() {
			if (car != null) {
				if (car.speed == car.maxSpeed) {
					text.text = "MAX";
				}
				else {
					text.text = car.speed.ToString();
				}
			}
		}

		public void bounceAnimationRed() {
			transform.DOScale(new Vector3(0.75f, 0.75f, 1.0f), timeDown)
				.OnComplete(() => {
					transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), timeUp);
				});
		}

		public void bounceAnimationGreen() {
			transform.DOScale(new Vector3(1.25f, 1.25f, 1.0f), timeDown)
				.OnComplete(() => {
					transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), timeUp);
				});
		}

	}
}
