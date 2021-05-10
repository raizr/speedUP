using UnityEngine;

namespace Assets.SlotCar.Scripts.Race {
	[System.Serializable, CreateAssetMenu(fileName = "EnemyDifficult", menuName = "Data/EnemyDifficult")]
	public class EnemyDifficult : ScriptableObject {
		public EnemyPreset[] easy;
		public EnemyPreset[] normal;
		public EnemyPreset[] hard;
	}
}


