using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI {
	public class RaceTapUI : UIBase {
		public Slider raceProgressSlider;
		public PlayerPosition playerPosition;
		public Countdown countdown;
		public CarSpeed carSpeed;
		public ScrollMiniGame scrollMiniGame;

		public override void Show() {
			base.Show();
			countdown.gameObject.SetActive(true);
		}
	}
}
