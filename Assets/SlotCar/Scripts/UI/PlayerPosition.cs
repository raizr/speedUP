using Assets.SlotCar.Scripts.Race;
using TMPro;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class PlayerPosition : MonoBehaviour {
		public RaceManager raceManager;
		private TextMeshProUGUI text;

		void Awake() {
			raceManager = GameInstance.Instance.raceManager;
			text = GetComponent<TextMeshProUGUI>();
		}

		void OnGUI() {
			if (raceManager == null) {
				raceManager = GameInstance.Instance.raceManager;
			}
			if (raceManager != null) {
				text.text = raceManager.playerPosition + "/" + raceManager.AllPositions;
			}
		}
	}
}
