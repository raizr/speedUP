using Assets.SlotCar.Scripts.Data;
using PathCreationEditor;
using UnityEditor;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Editor {
	[CustomEditor (typeof (BoostersCreator))]
	public class BoostersEditor : UnityEditor.Editor {
		private BoostersCreator creator;
		private BoosterType type;
		ScreenSpacePolyLine screenSpaceLine;
		ScreenSpacePolyLine.MouseInfo pathMouseInfo;

		void OnEnable() {
			creator = (BoostersCreator) target;
		}

		public override void OnInspectorGUI() { 
			type = (BoosterType)EditorGUILayout.EnumPopup("BoosterType", type); 
			EditorGUILayout.ObjectField(serializedObject.FindProperty("turboBoosterObject"), new GUIContent("Turbo prefab"));
			EditorGUILayout.ObjectField(serializedObject.FindProperty("stopBoosterObject"), new GUIContent("Stop prefab"));
			EditorGUILayout.ObjectField(serializedObject.FindProperty("moneyBoosterObject"), new GUIContent("Money prefab"));
			EditorGUILayout.FloatField(new GUIContent("timeOnPath"), pathMouseInfo.timeOnPath);
			serializedObject.ApplyModifiedProperties();
		}

		void UpdatePathMouseInfo () {
			screenSpaceLine = new ScreenSpacePolyLine (creator.pathCreator.bezierPath, creator.transform, 0.0f, 0.0f);

			if (screenSpaceLine != null) {
				pathMouseInfo = screenSpaceLine.CalculateMouseInfo();
			}
		}

		void OnSceneGUI () {
			EventType eventType = Event.current.type;
			
			DrawBooster();
			EditorApplication.QueuePlayerLoopUpdate ();
			Vector3 screenPosition = Handles.matrix.MultiplyPoint(pathMouseInfo.closestWorldPointToMouse);
			Handles.SphereHandleCap(0, screenPosition, Quaternion.identity, 0.2f * HandleUtility.GetHandleSize(screenPosition), EventType.Repaint);

			//if (eventType == EventType.MouseMove) {
				UpdatePathMouseInfo();
			//}

			// Don't allow clicking over empty space to deselect the object
			if (eventType == EventType.MouseDown && Event.current.button == 0) {
				creator.CreateBooster(type, pathMouseInfo.timeOnPath);
				HandleUtility.AddDefaultControl(0);
			}
		}

		public void DrawBooster() {

		}
	}
}
