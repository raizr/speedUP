using System;
using System.Collections;
using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.Utils;

namespace Assets.SlotCar.Scripts {
	public class RewardSystem : Singleton<RewardSystem> {

		public RewardData rewards;

		public void Initialize() {

		}

		public int GetLevelCompleteReward(int playerPosition) {
			switch (playerPosition) {
				case 2:
					return rewards.moneyForSecondPlace;
				case 3:
					return rewards.moneyForThirdPlace;
			}

			return rewards.moneyForFirstPlace;
		}

		public void ApplyLevelCompleteReward(int playerPosition) {
			PlayerProfile.Instance.profile.money += GetLevelCompleteReward(playerPosition);
			PlayerProfile.Instance.SaveProfile();
		}

		public void ApplyMultiplierReward(int baseReward, int selectedMultiplier) {
			PlayerProfile.Instance.profile.money += GetMultiplierReward(baseReward, selectedMultiplier);
			PlayerProfile.Instance.SaveProfile();
		}

		public void ApplyMoney(int reward) {
			PlayerProfile.Instance.profile.money += reward;
		}

		public int GetMultiplierReward(int baseReward, int selectedMultiplier) {
			return baseReward * selectedMultiplier;
		}
	}
}
