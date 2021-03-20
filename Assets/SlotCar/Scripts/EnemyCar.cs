using System.Collections;
using PathCreation.Examples;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class EnemyCar : PathFollower {
		public PlayerCar playerCar;
		public float speedUpCoeff = 1.0f;
		[Header("Seconds")]
		public float timeSpeedUp = 1.0f;
		// Start is called before the first frame update
		void Start() {
			base.Start();
			StartCoroutine(SpeedTick());
		}

		IEnumerator SpeedTick() {
			yield return new WaitForSeconds(timeSpeedUp);
			while (true) // Or whatever condition you want
			{
				SpeedUp(speedUpCoeff);
				yield return new WaitForSeconds(timeSpeedUp);
			}
		}
	}
}
