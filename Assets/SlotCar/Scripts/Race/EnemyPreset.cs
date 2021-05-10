using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Race {
	public enum EnemyAiPreset {
		Easy,
		Normal,
		Hard
	}
	[System.Serializable]
	public class EnemyPreset {
		public int scrollFreqCounterMin = 2;
		public int scrollFreqCounterMax = 4;
		public int redChance = 65;
		public int yellowChance = 25;
		public int greenChance = 15;
	}
}