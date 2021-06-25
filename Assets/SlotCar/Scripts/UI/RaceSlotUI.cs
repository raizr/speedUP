using Assets.SlotCar.Scripts.Utils;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class RaceSlotUI : UIBase {
		public ProgressBar raceProgressSlider;
		public PlayerPosition playerPosition;
		public Countdown countdown;
		public CarSpeed carSpeed;
		public GameObject top;
		public GameObject bottom;

		void Awake() {
			animated.Add(AddAnimated(top.transform,
				new Vector2(0.0f, -660.0f), new Vector2(0.0f, -250.0f)));
			animated.Add(AddAnimated(bottom.transform,
				new Vector2(0.0f, 640.0f), new Vector2(0.0f, 0.0f)));
		}
	}
}