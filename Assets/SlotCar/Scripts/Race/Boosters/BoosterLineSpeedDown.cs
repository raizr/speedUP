using System;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Race.Boosters {
	[ExecuteInEditMode]
	public class BoosterLineSpeedDown : BoosterLine {

		public override void ApplyEffect(Car car) {
			car.SetupStaticSpeedDown();
		}

		public override void StopEffect(Car car) {
			car.DisableStaticSpeedDown();
		}
	}
}
