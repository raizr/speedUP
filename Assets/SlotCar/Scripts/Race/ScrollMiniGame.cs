using System;
using System.Collections;
using Assets.SlotCar.Scripts.Race;
using Assets.SlotCar.Scripts.UI;
using Assets.SlotCar.Scripts.Utils;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts {
	public enum StateScrollGame {
		Start,
		Stop
	}

	public class ScrollMiniGame : MonoBehaviour {
		public Button button;
		public CarSpeed carSpeedText;
		public TextMeshProUGUI turboCounterLabel;
		public PlayerCar car;
		public Canvas redZone;
		public Canvas yellowZone;
		public Canvas greenZone;
		public RectTransform handle;
		public PopupManager pointsPopup;
		public HandleBounce scrollHandle;
		public ProgressBar turboProgress;

		public ParticleSystem greenZoneFX;
		public ParticleSystem yellowZoneFX;
		public ParticleSystem redZoneFX;

		public ParticleSystem greenScreenZoneFX;
		public ParticleSystem redScreenZoneFX;
		public ParticleSystem turboSpeedFX;


		private float moveTime = 2.0f;
		public float maxMoveTime = 2.0f;
		public float minMoveTime = 0.7f;

		private StateScrollGame state = StateScrollGame.Stop;
		[HideInInspector]
		public float handlePos;
		[HideInInspector]
		public UnityEvent zoneCompleteEvent;
		public int turboClickNumbers = 3;
		private TweenerCore<Vector2, Vector2, VectorOptions> moveHandle;
		private Vector3 defaultHolderPos;
		private int turboCounter;

		void Awake() {
			button.onClick.AddListener(SpeedUpClick);
			button.interactable = false;
			moveTime = maxMoveTime;
			defaultHolderPos = scrollHandle.transform.position;
			turboCounter = turboClickNumbers;
		}

		void ResetScroll() {
			scrollHandle.transform.position = defaultHolderPos;
		}

		public void SetCurrentState(StateScrollGame currentState) {
			if (currentState == StateScrollGame.Start) {
				button.interactable = true;
				GenerateZonePos();
			}
			state = currentState;
		}

		private void SpeedUpClick() {
			moveHandle.Kill();
			scrollHandle.BounceAnimation();
			state = StateScrollGame.Stop;
			ScrollZone zone = GetCurrentZone();
			SetFxByZone(zone);
			if (turboCounter <= 0) {
				car.turboEnd.AddListener(TurboEnd);
				car.StartTurbo();
				button.interactable = false;
				scrollHandle.gameObject.SetActive(false);
				redZone.gameObject.SetActive(false);
				turboProgress.gameObject.SetActive(true);
				turboProgress.Value = 100;
				TurboStartInternal();
				var main = turboSpeedFX.main;
				main.duration = car.turboTime;
				turboSpeedFX.Play();
				return;
			}
			GenerateZonePos();
		}

		private void TurboStartInternal() {
			DOTween.To(() => turboProgress.Value,
				x => turboProgress.Value = x,
				0, car.turboTime);
		}

		private void TurboEnd() {
			turboCounter = turboClickNumbers;
			scrollHandle.gameObject.SetActive(true);
			redZone.gameObject.SetActive(true);
			turboProgress.gameObject.SetActive(false);
			button.interactable = true;
			GenerateZonePos();
		}

		void GenerateZonePos() {
			if (state == StateScrollGame.Start) {
				return;
			}
			state = StateScrollGame.Start;
			zoneCompleteEvent.Invoke();
			Vector2 anchor = handle.GetComponent<RectTransform>().anchoredPosition;
			handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, anchor.y);
			Vector2 value = new Vector2(redZone.GetComponent<RectTransform>().rect.width, anchor.y);
			moveHandle = handle.GetComponent<RectTransform>()
				.DOAnchorPos(value, moveTime).SetEase(Ease.OutQuad)
				.OnComplete(() => {
					if (state == StateScrollGame.Stop) {
						return;
					}
					state = StateScrollGame.Stop;
					SetFxByZone(ScrollZone.Red);
					GenerateZonePos();
				});
		}

		ScrollZone GetCurrentZone() {
			ScrollZone result = ScrollZone.Red;
			var pos = handle.GetComponent<RectTransform>().anchoredPosition;
			if (pos.x >= yellowZone.GetComponent<RectTransform>().anchoredPosition.x
				&& pos.x <= yellowZone.GetComponent<RectTransform>().anchoredPosition.x + yellowZone.GetComponent<RectTransform>().rect.width) {
				result = ScrollZone.Yellow;
				if (pos.x >= greenZone.GetComponent<RectTransform>().anchoredPosition.x
					&& pos.x <= greenZone.GetComponent<RectTransform>().anchoredPosition.x + greenZone.GetComponent<RectTransform>().rect.width) {
					result = ScrollZone.Green;
				}
			}
			return result;
		}

		void SetFxByZone(ScrollZone zone) {
			switch (zone) {
				case ScrollZone.Yellow:
					car.SpeedUp(zone);
					pointsPopup.ShowPopup("Well", Color.yellow);
					yellowZoneFX.Play();
					break;
				case ScrollZone.Green:
					carSpeedText.bounceAnimationGreen();
					car.SpeedUpByGreenZone();
					pointsPopup.ShowPopup("Perfect", Color.green);
					moveTime = Math.Max(moveTime - 0.2f, minMoveTime);
					greenZoneFX.Play();
					greenScreenZoneFX.Play();
					turboCounter--;
					break;
				default:
					pointsPopup.ShowPopup("Miss", Color.red);
					carSpeedText.bounceAnimationRed();
					car.SpeedUp(zone);
					moveTime = Math.Min(moveTime + 0.2f, maxMoveTime);
					redZoneFX.Play();
					redScreenZoneFX.Play();
					break;
			}
		}

		public void ResetProgress() {
			Vector2 anchor = handle.GetComponent<RectTransform>().anchoredPosition;
			handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, anchor.y);
			state = StateScrollGame.Stop;
			moveTime = maxMoveTime;
			button.interactable = false;
			turboCounter = turboClickNumbers;
		}

		void OnGUI() {
			if (turboCounter != 0) {
				turboCounterLabel.text = turboCounter.ToString();
			}
			else {
				turboCounterLabel.text = "";
			}
		}
	}
}
