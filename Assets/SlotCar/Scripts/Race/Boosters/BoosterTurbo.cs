using UnityEngine;

namespace Assets.SlotCar.Scripts.Race.Boosters {
	public class BoosterTurbo : Booster
	{
		
		public override void ApplyEffect(Car car) {
			if (car == null) {
				return;
			}
			car.StartTurbo();
			gameObject.SetActive(false);
		}

	}
}
