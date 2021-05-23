using Assets.SlotCar.Scripts.Race;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI {

	public class FinishRaceUI : UIBase {
		public RaceManager raceManager;
		public Button retryButton;
		public TextMeshProUGUI message;

		public override void Show() {
			if (showed) {
				return;
			}
			base.Show();
			raceManager = GameInstance.Instance.raceManager;
			if (raceManager != null) {
				retryButton.onClick.AddListener(() => {
					GameInstance.Instance.SwitchTo(GameState.Main);
				});
				if (raceManager.finishResult == FinishResult.WIN) {
					message.text = "You Win";
				} else {
					message.text = "You lose";
				}
			}
		}
	}
}
