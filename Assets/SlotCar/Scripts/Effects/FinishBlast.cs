using UnityEngine;

namespace Assets.SlotCar.Scripts.Effects {
	public class FinishBlast : MonoBehaviour {

		public ParticleSystem[] particles;
		// Start is called before the first frame update
		void Start()
		{
        
		}

		public void StartFinishBlast() {
			foreach (var particle in particles) {
				particle.Play();
			}
		}

		// Update is called once per frame
		void Update()
		{
        
		}
	}
}
