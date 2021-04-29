using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI {

	public class FinishRaceUI : MonoBehaviour {


		public GameObject raceManager;
		private IRaceManager interfaceRaceManager;
		public Button retryButton;
		public TextMeshProUGUI message;

		// Start is called before the first frame update
		void Awake() {
			interfaceRaceManager = raceManager.GetComponent<IRaceManager>();
			retryButton.onClick.AddListener(interfaceRaceManager.RetryRace);
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
