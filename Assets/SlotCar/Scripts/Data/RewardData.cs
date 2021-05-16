using System;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Data {
	[Serializable, CreateAssetMenu(fileName = "RewardData", menuName = "Data/RewardData")]
	public class RewardData : ScriptableObject {
		public int moneyForFirstPlace = 500;
		public int moneyForSecondPlace = 250;
		public int moneyForThirdPlace = 100;

		[Header("value %")]
		public int[] rewardMultiplier = new[] {
			55,
			25,
			15,
			5
		};
		public int adsReward = 1500;
	}
}
