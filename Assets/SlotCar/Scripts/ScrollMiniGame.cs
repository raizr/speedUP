using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using PathCreation.Examples;
using Unity.InteractiveTutorials;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts {

	enum StateScrollGame {
		Start,
		Stop
	}

	public enum ScrollZone {
		Red,
		Yellow,
		Green
	}

	public class ScrollMiniGame : MonoBehaviour {
		public Button button;
		public float moveTime = 0.1f;
		private StateScrollGame state = StateScrollGame.Start;
		public Canvas redZone;
		public Canvas yellowZone;
		public Canvas greenZone;
		public RectTransform handle;
		public PathFollower car;
		private bool reverse = false;
		public float RedZoneSpeed = 10.0f;
		public float YellowZoneSpeed = 15.0f;
		public float GreenZoneSpeed = 25.0f;
		public float handlePos;

		void Start() {
			button.onClick.AddListener(SpeedUpClick);
			GenerateZonePos();
		}
		private void SpeedUpClick() {
			state = StateScrollGame.Stop;
			SpeedUp(GetCurrentZone());
			GenerateZonePos();
			state = StateScrollGame.Start;
		}

		void GenerateZonePos() {
			RectTransform redTransform = redZone.GetComponent<RectTransform>();
			RectTransform yellowTransform = yellowZone.GetComponent<RectTransform>();
			RectTransform greenTransform = greenZone.GetComponent<RectTransform>();
			//yellowTransform.sizeDelta = new Vector2(yellowTransform.rect.width - car.speed, yellowTransform.rect.height);
			//greenTransform.sizeDelta = new Vector2(yellowTransform.rect.width - car.speed * 2.0f, yellowTransform.rect.height);
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

		void SpeedUp(ScrollZone zone) {
			float speed = 0.0f;
			switch (zone) {
				case ScrollZone.Yellow:
					speed = YellowZoneSpeed; 
					break;
				case ScrollZone.Green:
					speed = GreenZoneSpeed;
					break;
				default:
					speed = RedZoneSpeed;
					break;
			}
			car.SpeedUp(speed);
		}

		void Update() {
			handlePos = handle.GetComponent<RectTransform>().position.x;
			if (state == StateScrollGame.Stop) {
				return;
			}

			float coeff = (reverse ? -1.0f : 1.0f) * moveTime * Time.deltaTime;
			handle.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				handle.GetComponent<RectTransform>().anchoredPosition.x + coeff,
				handle.GetComponent<RectTransform>().anchoredPosition.y);
			var xPos = handle.GetComponent<RectTransform>().anchoredPosition.x;
			if (xPos >= redZone.GetComponent<RectTransform>().rect.width && !reverse) {
				reverse = true;
			}
			else if (xPos <= 0.0f && reverse) {
				reverse = false;
			}
		}
	}
}
