using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.SlotCar.Scripts.UI {
	public class MoneyLabel : MonoBehaviour {
		public float hideY = 200.0f;
		public float showY = -100.0f;
		public float showDuration = 0.3f;
		public float hideDuration = 0.1f;
		public UnityAction onShowed;
		public UnityAction onHided;
		public RectTransform coinTarget;
		private int money;
		private TextMeshProUGUI text;
		private bool showed = false;
		public void Awake() {
			text = GetComponentInChildren<TextMeshProUGUI>();
			if (text != null) {
				text.text = money.ToString();
			}
		}

		public void Show() {
			var rect = GetComponent<RectTransform>();
			rect.DOAnchorPosY(showY, showDuration)
				.SetEase(Ease.OutBack)
				.OnComplete(() => {
					showed = true;
					onShowed.Invoke();
				});
		}

		public void Hide() {
			var rect = GetComponent<RectTransform>();
			rect.DOAnchorPosY(hideY, hideDuration)
				.SetEase(Ease.InBack)
				.OnComplete(() => {
					showed = false;
					onHided.Invoke();
				});
		}

		public bool IsShowed() {
			return showed;
		}

		public void OnGUI() {
			if (!money.Equals(PlayerProfile.Instance.profile.money)) {
				money = PlayerProfile.Instance.profile.money;
				if (text != null) {
					text.text = money.ToString();
				}
			}
		}
	}
}
