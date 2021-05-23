using System;
using System.Collections;
using Assets.SlotCar.Scripts.Race;
using Assets.SlotCar.Scripts.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
		public PlayerCar car;
		public Canvas redZone;
		public Canvas yellowZone;
		public Canvas greenZone;
		public RectTransform handle;
		public PopupManager pointsPopup;
		public HandleBounce scrollHandle;

		public ParticleSystem greenZoneFX;
		public ParticleSystem yellowZoneFX;
		public ParticleSystem redZoneFX;

		private float moveTime = 2.0f;
		public float maxMoveTime = 2.0f;
		public float minMoveTime = 0.7f;

		private StateScrollGame state = StateScrollGame.Stop;
		[HideInInspector]
		public float handlePos;
		[HideInInspector]
		public UnityEvent zoneCompleteEvent;
		private TweenerCore<Vector2, Vector2, VectorOptions> moveHandle;
		private Vector3 defaultHolderPos;

		void Awake() {
			button.onClick.AddListener(SpeedUpClick);
			button.interactable = false;
			moveTime = maxMoveTime;
			defaultHolderPos = scrollHandle.transform.position;
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
			ScrollZone zone = GetCurrentZone();
			SetFxByZone(zone);
			scrollHandle.BounceAnimation();
			state = StateScrollGame.Stop;
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
					break;
				default:
					pointsPopup.ShowPopup("Miss", Color.red);
					carSpeedText.bounceAnimationRed();
					car.SpeedUp(zone);
					moveTime = Math.Min(moveTime + 0.2f, maxMoveTime);
					redZoneFX.Play();
					break;
			}
		}

		public void ResetProgress() {
			Vector2 anchor = handle.GetComponent<RectTransform>().anchoredPosition;
			handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, anchor.y);
			state = StateScrollGame.Stop;
			moveTime = maxMoveTime;
			button.interactable = false;
		}
	}
}
