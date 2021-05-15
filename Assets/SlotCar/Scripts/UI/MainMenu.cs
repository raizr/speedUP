using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI {
	public class MainMenu : MonoBehaviour {
		public GameObject raceManager;
		private IRaceManager interfaceRaceManager;
		public GameObject raceUI;
		public Button startRaceButton;
		void Awake() {
			startRaceButton.onClick.AddListener(StartRaceClick);
		}

		void StartRaceClick() {
			raceUI.SetActive(true);
			gameObject.SetActive(false);
			interfaceRaceManager = raceManager.GetComponent<IRaceManager>();
			interfaceRaceManager.StartRace();
		}
	}
}
