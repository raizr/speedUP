using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation.Examples;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class EnemyCar : PathFollower {
		public ScrollMiniGame scrollMiniGame;

		public eEnemyAiPreset aiPreset = eEnemyAiPreset.Ease;
		public ScrollZone curZone;

		private EnemyPreset internalAiPreset;
		private List<int> tempRanges = new List<int>();
		private int scrollCounter = 0;
		private int scrollCurrentCounter = 0;
		// Start is called before the first frame update
		void Start() {
			base.Start();
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
