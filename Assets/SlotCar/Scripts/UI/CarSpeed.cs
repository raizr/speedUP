using System.Globalization;
using PathCreation.Examples;
using TMPro;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class CarSpeed : MonoBehaviour {

		public PathFollower car;
		private TextMeshProUGUI text;

		void Awake() {
			text = GetComponent<TextMeshProUGUI>();
		}
		// Start is called before the first frame update
		void OnGUI() {
			if (car != null) {
				text.text = car.speed.ToString(CultureInfo.CurrentCulture);
			}
		}
	}
}
