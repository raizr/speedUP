using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI.RaceResult {

	public class RaceResultUI : UIBase {
		public Canvas placeHolder;
		public TextMeshProUGUI playerPositionText;
		public TextMeshProUGUI positionHint;
		public Button levelRewardButton;
		public FortuneWheel fortuneWheel;
		public TextMeshProUGUI moneyCounter;
		public FlyMoneyManager flyMoneyHolder;
		public int playerPosition;

		public void Awake() {
			levelRewardButton.onClick.AddListener(GetLevelReward);
		}

		public override void Show() {
			if (showed) {
				return;
			}
			base.Show();
			var raceManager = GameInstance.Instance.raceManager;
			if (raceManager) {
				playerPosition = GameInstance.Instance.raceManager.playerPosition;
			}

			fortuneWheel.raceResult = this;
			fortuneWheel.playerPosition = playerPosition;
			fortuneWheel.StartWheel();
			SetTextPlayerPosition();
		}

		private void SetTextPlayerPosition() {
			switch (playerPosition) {
				case 1:
					playerPositionText.text = "1<size=70%>st";
					positionHint.text = "vectory";
					break;
				case 2:
					playerPositionText.text = "2<size=70%>nd";
					positionHint.text = "nice try";
					break;
				case 3:
					playerPositionText.text = "3<size=70%>rd";
					positionHint.text = "nice try";
					break;
			}
		}

		public void GetLevelReward() {
			RewardSystem.Instance.ApplyLevelCompleteReward(playerPosition);
			GameInstance.Instance.SwitchTo(GameState.Main);
		}

		public void OnGUI() {
			moneyCounter.text = RewardSystem.Instance.GetLevelCompleteReward(playerPosition).ToString();
		}
	}
}
