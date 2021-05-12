using System.Collections.Generic;
using System.Linq;
using Assets.SlotCar.Scripts.Data;
using PathCreation;
using Random = UnityEngine.Random;

namespace Assets.SlotCar.Scripts.Race {
	public class EnemyCar : PathFollower {
		public EnemyAiPreset aiPreset = EnemyAiPreset.Easy;
		public int enemyCount = 0;
		public ScrollZone curZone;
		private EnemyPreset internalAiPreset;
		private List<int> tempRanges = new List<int>();
		
		public void Init(RoadMeshCreator road, int count, int carId, int position) {
			base.Init(road, carId);
			enemyCount = count;
			linePosition = position;
			СhooseAiPreset();
			float[] zones = { internalAiPreset.redChance, internalAiPreset.yellowChance, internalAiPreset.greenChance };
			tempRanges = (from value in zones select (int) (value)).ToList();
		}

		void СhooseAiPreset() {
			var difficultData = GameDataInstance.Instance.GetEnemyDifficult();
			internalAiPreset = difficultData.easy[0];
			if (enemyCount > difficultData.easy.Length) {
				enemyCount = 0;
			}
			internalAiPreset = difficultData.easy[enemyCount];
			switch (aiPreset) {
				case EnemyAiPreset.Normal:
					internalAiPreset = difficultData.normal[enemyCount];
					break;
				case EnemyAiPreset.Hard:
					internalAiPreset = difficultData.hard[enemyCount];
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

		public void SpeedAi() {
			float playerDistance = GameInstance.Instance.raceManager.playerCar.distanceTravelled;
			float distance = distanceTravelled - playerDistance;
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
