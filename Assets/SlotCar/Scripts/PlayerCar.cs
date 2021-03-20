using System.Collections;
using PathCreation.Examples;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class PlayerCar : PathFollower {
		public float speedDownCoeff = 1.0f;
		[Header("Seconds")]
		public float timeSpeedDown = 1.0f;
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
				SpeedUp(-speedDownCoeff);
				yield return new WaitForSeconds(timeSpeedDown);
			}
		}
	}
}
