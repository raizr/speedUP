using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.SlotCar.Scripts.UI {

	public struct AnimStruct {
		public Transform animObj;
		public Vector2 showPos;
		public Vector2 hidePos;
	};

	public class UIBase : MonoBehaviour {
		public float showDuration = 0.3f;
		public float hideDuration = 0.1f;
		public UnityEvent onShowed;
		public UnityEvent onHided;
		protected List<AnimStruct> animated = new List<AnimStruct>();

		protected bool showed = true;
		public virtual void Show() {
			if (showed) {
				return;
			}
			gameObject.SetActive(true);
			if (animated.Count == 0) {
				showed = true;
				onShowed.Invoke();
				return;
			}
			foreach (var anim in animated) {
				var rect = anim.animObj.gameObject.GetComponent<RectTransform>();
				rect.DOAnchorPos(anim.showPos, showDuration)
					.SetEase(Ease.OutBack)
					.OnComplete(() => {
						showed = true;
						onShowed.Invoke();
					});
			}
		}

		public virtual void Hide() {
			if (!showed) {
				return;
			}
			if (animated.Count == 0) {
				showed = false;
				onHided.Invoke();
				gameObject.SetActive(false);
				return;
			}
			foreach (var anim in animated) {
				var rect = anim.animObj.gameObject.GetComponent<RectTransform>();
				rect.DOAnchorPos(anim.hidePos, hideDuration)
					.SetEase(Ease.InBack)
					.OnComplete(() => {
						showed = false;
						onHided.Invoke();
						gameObject.SetActive(false);
					});
			}
		}

		public virtual void SetShow() {
			gameObject.SetActive(true);
			showed = true;
			foreach (var anim in animated) {
				var rect = anim.animObj.gameObject.GetComponent<RectTransform>();
				rect.anchoredPosition = anim.showPos;
			}
		}

		public virtual void SetHide() {
			showed = false;
			foreach (var anim in animated) {
				var rect = anim.animObj.gameObject.GetComponent<RectTransform>();
				rect.anchoredPosition = anim.hidePos;
			}
			gameObject.SetActive(false);
		}

		public bool IsShowed() {
			return showed;
		}

		public AnimStruct AddAnimated(Transform obj, Vector2 show, Vector2 hide) {
			AnimStruct anim;
			anim.animObj = obj;
			anim.showPos = show;
			anim.hidePos = hide;
			return anim;
		}
	}
}
