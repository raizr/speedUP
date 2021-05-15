using System.Text;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class UIBase : MonoBehaviour {
		protected bool isShowed;
		public virtual void Show() {
			isShowed = true;
			gameObject.SetActive(true);
		}

		public virtual void Hide() {
			isShowed = false;
			gameObject.SetActive(false);
		}
	}
}
