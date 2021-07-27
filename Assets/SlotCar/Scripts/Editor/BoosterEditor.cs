using Assets.SlotCar.Scripts.Race.Boosters;
using UnityEditor;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Editor {
	[CustomEditor(typeof(Booster))]
	public class BoosterEditor : UnityEditor.Editor {
		protected virtual void OnSceneGUI() {
			if (Event.current.type == EventType.Repaint) {
				Transform transform = ((Booster)target).transform;
				Handles.color = Handles.xAxisColor;
				Handles.ArrowHandleCap(
					0,
					transform.position + new Vector3(3f, 0f, 0f),
					transform.rotation * Quaternion.LookRotation(Vector3.right),
					1.0f,
					EventType.Repaint
				);
			}
		}
}
}
