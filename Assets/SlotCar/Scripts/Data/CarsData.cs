using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Data {
	[Serializable, CreateAssetMenu(fileName = "CarsData", menuName = "Data/Cars")]
	public class CarsData : ScriptableObject {
		public List<CarData> cars;
	}
}
