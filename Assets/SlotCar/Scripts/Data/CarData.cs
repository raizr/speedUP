using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Data {
	[Serializable]
	public enum CarRarity {
		Common,
		Rare,
		Legendary
	}
	[CreateAssetMenu(fileName = "CarData", menuName = "Data/Car")]
	public class CarData : ScriptableObject {
		public int carId;
		public CarRarity rarity;
		public int speed;
		public string prefabPath;
	}
	[CreateAssetMenu(fileName = "CarsData", menuName = "Data/Cars")]
	public class CarsData : ScriptableObject {
		public List<CarData> cars;
	}
}
