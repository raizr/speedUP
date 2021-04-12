using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class PlayerCar : PathFollower {
		public int speedDownCoeff = 1;
		[Header("Seconds")]
		public float timeSpeedDown = 1.0f;
		public CinemachineVirtualCamera camera;
		public float greenZoneSpeedUpTime = 1.0f;

		// Start is called before the first frame update
		new void Start() {
			base.Start();
			StartCoroutine(SpeedTick());
		}

		IEnumerator SpeedTick() {
			if (state != RaceState.RACE) {
				yield return null;
			}

			yield return new WaitForSeconds(timeSpeedDown);
			while (true) // Or whatever condition you want
			{
				SpeedUp(-speedDownCoeff, speedUpTime);
				yield return new WaitForSeconds(timeSpeedDown);
			}
		}

		public void SpeedUpByGreenZone() {
			float zoneSpeed = ChooseSpeedByZone(ScrollZone.Green);
			SpeedUp((int) zoneSpeed, greenZoneSpeedUpTime);
		}

}
}
