using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Data {

	[Serializable]
	public enum RaceType {
		Tap,
		Slot
	}

	[Serializable]
	public class LevelData {
		public int levelId;
		public int laps;
		public string prefabPath;
		public int[] enemyCarsIds;
	}

	[CreateAssetMenu(fileName = "LevelsData", menuName = "Data/Levels")]
	public class LevelsData : ScriptableObject {
		public List<LevelData> levels;
	}
}
