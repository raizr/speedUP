using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.SlotCar.Scripts.UI.RaceResult {
	public class FlyMoneyManager : MonoBehaviour {

		public GameObject moneyPrefab;
		public RectTransform startTarget;
		public float flyDuration = 1.0f;
		public float waiteStep = 0.1f;
		public UnityEvent endFly;
		private RectTransform endTarget;

		// Start is called before the first frame update
		void Start()
		{
        
		}

		public void StartFly(int counter) {
			StartCoroutine(StartFlyInternal(counter));
		}

		IEnumerator StartFlyInternal(int counter) {
			endTarget = GameInstance.Instance.uiController.moneyLabel.coinTarget;
			Vector2 endPos = Camera.main.WorldToScreenPoint(endTarget.position);
			for (int i = 0; i < counter; i++) {
				var money = Instantiate(moneyPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
				money.transform.localScale = Vector3.one;

				var rect = money.GetComponent<RectTransform>();
				rect.localRotation = Quaternion.identity;
				rect.position = startTarget.position;
				rect.DOAnchorPos(endPos, flyDuration);
				var scale = rect.DOScale(new Vector2(1.3f, 1.3f), flyDuration);

				TweenCallback onCompleteCallback;
				if (i == counter - 1) {
					onCompleteCallback = () => {
						Destroy(money);
						endFly.Invoke();
					};
				}
				else {
					onCompleteCallback = () => {
						Destroy(money);
					};
				}
				scale.OnComplete(onCompleteCallback);

				yield return new WaitForSeconds(waiteStep);
			}
		}
	}
}
