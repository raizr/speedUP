using System.Collections;
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
		public float moveTime = 1500.0f;
		public float maxMoveTime = 2.0f;
		public float minMoveTime = 0.7f;
		public float greenZoneBlockTime = 2.0f;
		private int allPoints = 0;
		public int pointsPerTap = 100;
		public int pointMulitpliers = 1;
		public int maxPointMulitpliers = 5;

		private StateScrollGame state = StateScrollGame.Stop;
		private bool reverse = false;
		[HideInInspector]
		public float handlePos;
		[HideInInspector]
		public UnityEvent zoneCompleteEvent;
		private TweenerCore<Vector2, Vector2, VectorOptions> moveHandle;

		void Awake() {
			button.onClick.AddListener(SpeedUpClick);
		}

		public void SetCurrentState(StateScrollGame currentState) {
			state = currentState;
			if (state == StateScrollGame.Start) {
				GenerateZonePos();
			}
		}

		void Start() {
			//GenerateZonePos();
		}
		private void SpeedUpClick() {
			/*if (state == StateScrollGame.Start) {
				return;
			}*/
			state = StateScrollGame.Start;
			moveHandle.Kill();
			ScrollZone zone = GetCurrentZone();
			SetFxByZone(zone);
			GenerateZonePos();
		}

		void GenerateZonePos() {
			Vector2 anchor = handle.GetComponent<RectTransform>().anchoredPosition;
			handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, anchor.y);
			Vector2 value = new Vector2(redZone.GetComponent<RectTransform>().rect.width, anchor.y);
			moveHandle = handle.GetComponent<RectTransform>()
				.DOAnchorPos(value, 2.0f).SetEase(Ease.OutQuad)
				.OnComplete(() => {
					zoneCompleteEvent.Invoke();
					SetFxByZone(ScrollZone.Red);
					GenerateZonePos();
					state = StateScrollGame.Stop;
				});
			/*
			RectTransform redTransform = redZone.GetComponent<RectTransform>();
			RectTransform yellowTransform = yellowZone.GetComponent<RectTransform>();
			RectTransform greenTransform = greenZone.GetComponent<RectTransform>();
			float yellowX = Random.Range(0, redTransform.sizeDelta.x- yellowTransform.rect.width);
			float greenX = Random.Range(yellowX + 5.0f, yellowX + yellowTransform.rect.width - greenTransform.rect.width);
			yellowZone.GetComponent<RectTransform>().anchoredPosition = new Vector3(yellowX, yellowZone.transform.localPosition.y);
			greenZone.GetComponent<RectTransform>().anchoredPosition = new Vector3(greenX, greenZone.transform.localPosition.y);
			*/
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
					state = StateScrollGame.Start;
					pointMulitpliers = 1;
					break;
				case ScrollZone.Green:
					carSpeedText.bounceAnimationGreen();
					car.SpeedUpByGreenZone();
					pointsPopup.ShowPopup(pointsPerTap, pointMulitpliers);
					if (pointMulitpliers < maxPointMulitpliers) {
						pointMulitpliers++;
					}
					allPoints += pointsPerTap * pointMulitpliers;
					break;
				default:
					carSpeedText.bounceAnimationRed();
					car.SpeedUp(zone);
					pointMulitpliers = 1;
					state = StateScrollGame.Start;
					break;
			}
		}

		IEnumerator SetBlockScrollByTime() {
			yield return new WaitForSeconds(greenZoneBlockTime);
			GenerateZonePos();
			state = StateScrollGame.Start;
		}

		void Update() {
			if (state == StateScrollGame.Stop) {
				return;
			}
			/*
			handlePos = handle.GetComponent<RectTransform>().position.x;
			float moveCoeff = minMoveTime + maxMoveTime * car.speed / car.maxSpeed;
			float coeff = (reverse ? -1.0f : 1.0f) * moveCoeff * moveTime * Time.deltaTime;
			handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				handle.GetComponent<RectTransform>().anchoredPosition.x + coeff,
				handle.GetComponent<RectTransform>().anchoredPosition.y);*/

			var xPos = handle.GetComponent<RectTransform>().anchoredPosition.x;
			if (xPos >= redZone.GetComponent<RectTransform>().rect.width && !reverse) {
				/*zoneCompleteEvent.Invoke();
				reverse = true;*/
				//state = StateScrollGame.Stop;
			}
			else if (xPos <= 0.0f && reverse) {
				zoneCompleteEvent.Invoke();
				reverse = false;
			}
		}
	}
}
