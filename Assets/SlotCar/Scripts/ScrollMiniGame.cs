using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using PathCreation.Examples;
using Spine;
using Spine.Unity;
using Unity.InteractiveTutorials;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts {
	enum StateScrollGame {
		Start,
		Stop
	}

	public class ScrollMiniGame : MonoBehaviour {
		public Button button;
		public float moveTime = 1500.0f;
		public float maxMoveTime = 2.0f;
		public float minMoveTime = 0.7f;
		private StateScrollGame state = StateScrollGame.Start;
		public Canvas redZone;
		public Canvas yellowZone;
		public Canvas greenZone;
		public RectTransform handle;
		public PathFollower car;
		private bool reverse = false;
		public float handlePos;
		public UnityEvent zoneCompleteEvent;
		public SkeletonAnimation greenFX;

		public void OnSpineAnimationEnd(TrackEntry trackEntry) {
			GenerateZonePos();
		}

		void Awake() {
			greenFX.gameObject.SetActive(false);
			greenFX.AnimationState.Complete += OnSpineAnimationEnd;
			button.onClick.AddListener(SpeedUpClick);
		}

		void Start() {
			GenerateZonePos();
		}
		private void SpeedUpClick() {
			if (state == StateScrollGame.Stop) {
				return;
			}
			state = StateScrollGame.Stop;
			ScrollZone zone = GetCurrentZone();
			setFxByZone(zone);
			car.SpeedUp(zone);
		}

		void GenerateZonePos() {
			state = StateScrollGame.Start;
			RectTransform redTransform = redZone.GetComponent<RectTransform>();
			RectTransform yellowTransform = yellowZone.GetComponent<RectTransform>();
			RectTransform greenTransform = greenZone.GetComponent<RectTransform>();
			float yellowX = Random.Range(0, redTransform.sizeDelta.x- yellowTransform.rect.width);
			float greenX = Random.Range(yellowX + 5.0f, yellowX + yellowTransform.rect.width - greenTransform.rect.width);
			yellowZone.GetComponent<RectTransform>().anchoredPosition = new Vector3(yellowX, yellowZone.transform.localPosition.y);
			greenZone.GetComponent<RectTransform>().anchoredPosition = new Vector3(greenX, greenZone.transform.localPosition.y);
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

		void setFxByZone(ScrollZone zone) {
			switch (zone) {
				case ScrollZone.Yellow:
					GenerateZonePos();
					break;
				case ScrollZone.Green:
					greenFX.gameObject.SetActive(true);
					greenFX.AnimationState.SetAnimation(0, "green", false);
					break;
				default:
					GenerateZonePos();
					break;
			}
		}

		void Update() {
			handlePos = handle.GetComponent<RectTransform>().position.x;
			if (state == StateScrollGame.Stop) {
				return;
			}
			float moveCoeff = minMoveTime + maxMoveTime * car.speed / car.maxSpeed;
			float coeff = (reverse ? -1.0f : 1.0f) * moveCoeff * moveTime * Time.deltaTime;
			handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				handle.GetComponent<RectTransform>().anchoredPosition.x + coeff,
				handle.GetComponent<RectTransform>().anchoredPosition.y);
			var xPos = handle.GetComponent<RectTransform>().anchoredPosition.x;
			if (xPos >= redZone.GetComponent<RectTransform>().rect.width && !reverse) {
				zoneCompleteEvent.Invoke();
				reverse = true;
			}
			else if (xPos <= 0.0f && reverse) {
				zoneCompleteEvent.Invoke();
				reverse = false;
			}
		}
	}
}
