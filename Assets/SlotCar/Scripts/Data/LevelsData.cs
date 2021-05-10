using System;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Data {
	[Serializable, CreateAssetMenu(fileName = "LevelsData", menuName = "Data/Levels")]
	public class LevelsData : ScriptableObject {
		public List<LevelData> levels;
	}
}
