using UnityEngine;

namespace Assets.SlotCar.Scripts.Utils {
	public class ProgressBar : MonoBehaviour {
		private UnityEngine.UI.Image fillImage;
		public float Value {
			get {
				fillImage = gameObject.GetComponent<UnityEngine.UI.Image>();
				if (fillImage != null)
					return (fillImage.fillAmount * maxValue);
				else
					return 0;
			}
			set {
				fillImage = gameObject.GetComponent<UnityEngine.UI.Image>();
				if (fillImage != null)
					fillImage.fillAmount = value / maxValue;
			}
		}

		public float maxValue = 100.0f;

		void Awake() {
			fillImage = gameObject.GetComponent<UnityEngine.UI.Image>();
			Value = 0;
		}

		// Update is called once per frame
		void Update()
		{
        
		}
	}
}
