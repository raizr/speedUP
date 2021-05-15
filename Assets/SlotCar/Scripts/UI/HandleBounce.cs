using DG.Tweening;
using UnityEngine;

namespace Assets.SlotCar.Scripts.UI {
	public class HandleBounce : MonoBehaviour
	{

		public float timeDown = 0.1f;
		public float timeUp = 0.1f;
		// Start is called before the first frame update
		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
        
		}

		public void BounceAnimation() {
			transform.DOScale(new Vector3(1.0f, 0.75f, 1.0f), timeDown)
				.OnComplete(() => {
					transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), timeUp);
				});
		}
	}
}
