using DG.Tweening;
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
		public GameObject bottom;

		public float placeHolderScaleShow = 1.0f;
		public float placeHolderScaleHide = 0.0f;

		public void Awake() {
			levelRewardButton.onClick.AddListener(GetLevelReward);
			animated.Add(AddAnimated(bottom.transform,
				new Vector2(0.0f, 640.0f), new Vector2(0.0f, 0.0f)));
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

			var placeHolderRect = placeHolder.GetComponent<RectTransform>();
			placeHolderRect.localScale = new Vector3(placeHolderScaleHide, placeHolderScaleHide, 1.0f);
			placeHolderRect.DOScale(new Vector3(placeHolderScaleShow, placeHolderScaleShow, 1.0f), showDuration)
				.SetEase(Ease.OutBack);
		}

		public override void Hide() {
			if (!showed) {
				return;
			}
			base.Hide();
			var placeHolderRect = placeHolder.GetComponent<RectTransform>();
			placeHolderRect.localScale = new Vector3(placeHolderScaleShow, placeHolderScaleShow, 1.0f);
			placeHolderRect.DOScale(new Vector3(placeHolderScaleHide, placeHolderScaleHide, 1.0f), hideDuration /2.0f)
				.SetEase(Ease.InBack);
		}

		private void SetTextPlayerPosition() {
			switch (playerPosition) {
				case 1:
					playerPositionText.text = "1<size=70%>st";
					positionHint.text = "victory";
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
