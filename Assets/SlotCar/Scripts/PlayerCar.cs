using System.Collections;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class PlayerCar : PathFollower {
		public int speedDownCoeff = 1;
		[Header("Seconds")]
		public float timeSpeedDown = 1.0f;

		public float greenZoneSpeedUpTime = 1.0f;
		// Start is called before the first frame update
		new void Start()
		{
			base.Start();
			StartCoroutine(SpeedTick());
		}

		IEnumerator SpeedTick() {
			yield return new WaitForSeconds(timeSpeedDown);
			while (true) // Or whatever condition you want
			{
				SpeedUp(-speedDownCoeff, speedUpTime);
				yield return new WaitForSeconds(timeSpeedDown);
			}
		}

		public void SpeedUpByGreenZone() {
			float zoneSpeed = ChooseSpeedByZone(ScrollZone.Green);
			SpeedUp((int)zoneSpeed, greenZoneSpeedUpTime);
		}
	}
}
