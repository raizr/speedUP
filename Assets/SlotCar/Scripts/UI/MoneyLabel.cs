using TMPro;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class MoneyLabel : MonoBehaviour {
		private int money;
		public void OnGUI() {
			if (!money.Equals(PlayerProfile.Instance.profile.money)) {
				money = PlayerProfile.Instance.profile.money;
				TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
				if (text != null) {
					text.text = money.ToString();
				}
			}

		}
	}
}
