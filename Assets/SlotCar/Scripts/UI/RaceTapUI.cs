using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI {
	public class RaceTapUI : UIBase {
		public Slider raceProgressSlider;
		public PlayerPosition playerPosition;
		public Countdown countdown;
		public CarSpeed carSpeed;
		public ScrollMiniGame scrollMiniGame;
		public GameObject top;
		public GameObject bottom;

		void Awake() {
			animated.Add(AddAnimated(top.transform,
				new Vector2(0.0f, -660.0f), new Vector2(0.0f, -250.0f)));
			animated.Add(AddAnimated(bottom.transform,
				new Vector2(0.0f, 640.0f), new Vector2(0.0f, 0.0f)));
		}

		public override void Show() {
			if (showed) {
				return;
			}

			countdown.gameObject.SetActive(true);
			gameObject.SetActive(true);
			foreach (var anim in animated) {
				var rect = anim.animObj.gameObject.GetComponent<RectTransform>();
				rect.DOAnchorPos(anim.showPos, showDuration)
					.SetEase(Ease.OutBack)
					.OnComplete(() => {
						showed = true;
						onShowed.Invoke();
					});
			}
		}

		public override void Hide() {
			if (!showed) {
				return;
			}
			foreach (var anim in animated) {
				var rect = anim.animObj.gameObject.GetComponent<RectTransform>();
				rect.DOAnchorPos(anim.hidePos, hideDuration)
					.SetEase(Ease.InBack)
					.OnComplete(() => {
						showed = false;
						onHided.Invoke();
						gameObject.SetActive(false);
					});
			}
		}
	}
}
