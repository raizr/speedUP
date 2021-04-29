using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SlotCar.Scripts.SlotRace {
	[System.Serializable]
	public class EnemyPreset {
		public int scrollFreqCounterMin = 2;
		public int scrollFreqCounterMax = 4;
		public int redChance = 65;
		public int yellowChance = 25;
		public int greenChance = 15;
	}
}