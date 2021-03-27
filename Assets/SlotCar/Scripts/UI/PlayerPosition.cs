using TMPro;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class PlayerPosition : MonoBehaviour {
	
		public RaceManager raceManager;
		private TextMeshProUGUI text;

		void Awake() {
			text = GetComponent<TextMeshProUGUI>();
		}

		void OnGUI() {
			if (raceManager != null) {
				text.text = raceManager.playerPosition + "/" + raceManager.AllPositions;
			}
		}
	}
}
