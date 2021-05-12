using Assets.SlotCar.Scripts.Race;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI {
	public class MainMenu : UIBase {
		public Button startRaceButton;
		public Button settingsButton;
		public Button noAdsButton;
		public Button garageButton;

		void Awake() {
			startRaceButton.onClick.AddListener(StartRaceClick);
		}

		void StartRaceClick() {
			GameInstance.Instance.SwitchTo(GameState.RaceTap);
		}
	}
}
