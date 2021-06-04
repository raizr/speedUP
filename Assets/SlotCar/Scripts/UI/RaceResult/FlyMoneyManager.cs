using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts.UI.RaceResult {
	public class FlyMoneyManager : MonoBehaviour {

		public GameObject moneyPrefab;
		public RectTransform startTarget;
		public float flyDuration = 1.0f;
		public float waiteStep = 0.1f;
		public UnityEvent endFly;
		private RectTransform endTarget;
		public Vector2 endPos;
		// Start is called before the first frame update
		void Start()
		{
			endTarget = GameInstance.Instance.uiController.moneyLabel.coinTarget;
        
		}

		public void StartFly(int counter) {
			StartCoroutine(StartFlyInternal(counter));
		}

		IEnumerator StartFlyInternal(int counter) {
			endTarget = GameInstance.Instance.uiController.moneyLabel.coinTarget;
			var flyRect = GetComponent<RectTransform>();
			endPos = Camera.main.WorldToViewportPoint(endTarget.position) * new Vector2(flyRect.rect.width, flyRect.rect.height);
			int internalCounter = counter / 100;
			for (int i = 0; i < internalCounter; i++) {
				var money = Instantiate(moneyPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
				money.transform.localScale = Vector3.one;

				var rect = money.GetComponent<RectTransform>();
				rect.localRotation = Quaternion.identity;
				rect.position = startTarget.position;
				var changePos = rect.DOAnchorPos(endPos , flyDuration);
				var scale = rect.DOScale(new Vector3(1.3f, 1.3f, 1.0f), flyDuration);

				TweenCallback onCompleteCallback;
				var i1 = i;
				if (i == internalCounter - 1) {
					onCompleteCallback = () => {
						RewardSystem.Instance.ApplyMoney((i1 + 1) * 100);
						PlayerProfile.Instance.SaveProfile();
						Destroy(money);
						endFly.Invoke();
					};
				}
				else {
					onCompleteCallback = () => {
						RewardSystem.Instance.ApplyMoney((i1 + 1) * 100);
						Destroy(money);
					};
				}
				changePos.OnComplete(onCompleteCallback);

				yield return new WaitForSeconds(waiteStep);
			}
		}
	}
}
