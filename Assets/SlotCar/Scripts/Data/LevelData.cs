using System;
using System.Collections.Generic;
using Assets.SlotCar.Scripts.UI;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Data {

	[Serializable]
	public enum RaceType {
		Tap,
		Slot
	}

	[Serializable, CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level")]
	public class LevelData : ScriptableObject {
		public int levelId;
		public RaceType raceType;
		public int laps;
		public int lines;
		public GameObject prefab;
		public EnemyData[] enemyCars;
		public int playerPosition;
	}
}
