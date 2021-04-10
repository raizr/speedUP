using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using Random = UnityEngine.Random;

namespace Assets.SlotCar.Scripts {
	public class EnemyCar : PathFollower {
		public ScrollMiniGame scrollMiniGame;
		public RaceManager raceManager;
		public eEnemyAiPreset aiPreset = eEnemyAiPreset.Easy;
		public int enemyCount = 0;
		public ScrollZone curZone;
		private EnemyPreset internalAiPreset;
		private List<int> tempRanges = new List<int>();
		private int scrollCounter = 0;
		private int scrollCurrentCounter = 0;
		// Start is called before the first frame update
		new void Start() {
			base.Start();

			chooseAiPreset();
			scrollMiniGame.zoneCompleteEvent.AddListener(SpeedAi);
			float[] zones = { internalAiPreset.redChance, internalAiPreset.yellowChance, internalAiPreset.greenChance };
			tempRanges = (from value in zones select (int)(value)).ToList();
			scrollCounter = Random.Range(2, 4);
		}

		void chooseAiPreset() {
			if (raceManager == null) {
				return;
			}
			internalAiPreset = raceManager.difficultData.easy[0];
			if (enemyCount > raceManager.difficultData.easy.Length) {
				enemyCount = 0;
			}
			internalAiPreset = raceManager.difficultData.easy[enemyCount];
			switch (aiPreset) {
				case eEnemyAiPreset.Normal:
					internalAiPreset = raceManager.difficultData.normal[enemyCount];
					break;
				case eEnemyAiPreset.Hard:
					internalAiPreset = raceManager.difficultData.hard[enemyCount];
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
			float distance = distanceTravelled - raceManager.playerCar.distanceTravelled;
			if (distance < -1) {
				curZone = ScrollZone.Green;
			} else if (distance > 3) {
				curZone = ScrollZone.Red;
			}
			else {
				curZone = GetCurrentZone();
			}
			SpeedUp(curZone);
		}
	}
}
