using System;

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
}
