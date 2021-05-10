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
	[Serializable, CreateAssetMenu(fileName = "CarData", menuName = "Data/Car")]
	public class CarData : ScriptableObject {
		public int carId;
		public CarRarity rarity;
		public int speed;
		public GameObject prefab;
	}
}
