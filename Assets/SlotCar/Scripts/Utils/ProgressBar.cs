using UnityEngine;

namespace Assets.SlotCar.Scripts.Utils {
	public class ProgressBar : MonoBehaviour {
		private UnityEngine.UI.Image fillImage;
		public int Value {
			get {
				fillImage = gameObject.GetComponent<UnityEngine.UI.Image>();
				if (fillImage != null)
					return (int)(fillImage.fillAmount * 100);
				else
					return 0;
			}
			set {
				fillImage = gameObject.GetComponent<UnityEngine.UI.Image>();
				if (fillImage != null)
					fillImage.fillAmount = value / 100f;
			}
		}

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
