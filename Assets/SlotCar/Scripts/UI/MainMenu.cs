using System.Collections.Generic;
using Assets.SlotCar.Scripts.Race;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI {
	public class MainMenu : UIBase {
		public Button startRaceButton;
		public Button settingsButton;
		public Button noAdsButton;
		public Button garageButton;


		void Awake() {
			startRaceButton.onClick.AddListener(StartRaceClick);
			animated.Add(AddAnimated(startRaceButton.transform,
						new Vector2(0.0f, 150.0f),
						new Vector2(0.0f, -170.0f)));
			animated.Add(AddAnimated(settingsButton.transform,
				new Vector2(71.0f, -70.0f),
				new Vector2(71.0f, 150.0f)));
			animated.Add(AddAnimated(noAdsButton.transform,
				new Vector2(84.0f, -148.0f),
				new Vector2(-128.0f, -148.0f)));
			animated.Add(AddAnimated(garageButton.transform,
				new Vector2(-84.0f, -148.0f),
				new Vector2(128.0f, -148.0f)));
		}

		public override void Show() {
			if (showed) {
				return;
			}
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

		void StartRaceClick() {
			GameInstance.Instance.SwitchTo(GameState.RaceTap);
		}
	}
}
