using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class UIBase : MonoBehaviour
	{

		public void Show() {
			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}

		// Start is called before the first frame update
		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
        
		}
	}
}
