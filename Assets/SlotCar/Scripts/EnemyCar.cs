using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation.Examples;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class EnemyCar : PathFollower {
		public PlayerCar playerCar;
		public ScrollMiniGame scrollMiniGame;
		public int speedUpCoeff = 1;
		[Header("Seconds")]
		public float timeSpeedUp = 1.0f;
		public eEnemyAiPreset aiPreset = eEnemyAiPreset.Ease;
		private EnemyPreset internalAiPreset;
		public ScrollZone curZone;
		private List<int> tempRanges = new List<int>();
		private int scrollCounter = 0;
		private int scrollCurrentCounter = 0;
		// Start is called before the first frame update
		void Start() {
			base.Start();
			//StartCoroutine(SpeedTick());
			chooseAiPreset();
			scrollMiniGame.zoneCompleteEvent.AddListener(SpeedAi);
			float[] zones = { internalAiPreset.redChance, internalAiPreset.yellowChance, internalAiPreset.greenChance };
			tempRanges = (from value in zones select (int)(value)).ToList();
			scrollCounter = Random.Range(2, 4);
		}

		void chooseAiPreset() {
			internalAiPreset = GetComponent<EnemyPresetEase>();
			switch (aiPreset) {
				case eEnemyAiPreset.Normal:
					internalAiPreset = GetComponent<EnemyPresetNormal>();
					break;
				case eEnemyAiPreset.Hard:
					internalAiPreset = GetComponent<EnemyPresetHard>();
					break;
			}
		}

		IEnumerator SpeedTick() {
			yield return new WaitForSeconds(timeSpeedUp);
			while (true) // Or whatever condition you want
			{
				SpeedUp(speedUpCoeff);
				yield return new WaitForSeconds(timeSpeedUp);
			}
		}

		ScrollZone GetCurrentZone() {
			ScrollZone result = ScrollZone.Red;
			int value = 0;
			int randomValue = Random.Range(0, internalAiPreset.redChance + internalAiPreset.yellowChance + internalAiPreset.greenChance);
			for (int i = 0; i < tempRanges.Count; i++) {
				value += tempRanges[i];
				if (randomValue <= value) {
					return (ScrollZone)(i);
				}

			}
			return result;
		}

		void SpeedAi() {
			scrollCurrentCounter++;
			if (scrollCurrentCounter < scrollCounter) {
				return;
			}
			scrollCurrentCounter = 0;
			curZone = GetCurrentZone();
			SpeedUp(curZone);
		}
	}
}
