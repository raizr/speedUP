using Assets.SlotCar.Scripts.Race;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI {

	public class FinishRaceUI : MonoBehaviour {
		public RaceManager raceManager;
		public Button retryButton;
		public TextMeshProUGUI message;

		// Start is called before the first frame update
		void Awake() {
			if (raceManager != null) {
				retryButton.onClick.AddListener(raceManager.RetryRace);
			}
		}

		public void ShowUI(FinishResult result) {
			gameObject.SetActive(true);
			if (result == FinishResult.WIN) {
				message.text = "You Win";
			}
			else {
				message.text = "You lose";
			}
		}
	}
}
