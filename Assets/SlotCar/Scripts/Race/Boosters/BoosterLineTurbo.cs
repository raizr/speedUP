using System;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Race.Boosters {
	[ExecuteInEditMode]
	public class BoosterLineTurbo : BoosterLine {

		public override void ApplyEffect(Car car) {
			car.SetupStaticSpeedUp();
		}

		public override void StopEffect(Car car) {
			car.DisableStaticSpeedUp();
		}
	}
}
