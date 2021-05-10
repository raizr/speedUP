using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using Assets.SlotCar.Scripts.Data;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Editor {
	[CustomEditor(typeof(CarData), true)]
	public class CarDataEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("carId"), new GUIContent("Id"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("rarity"), new GUIContent("rarity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"), new GUIContent("speed"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("prefab"), new GUIContent("prefab"));

			var texture = AssetPreview.GetAssetPreview(serializedObject.FindProperty("prefab").objectReferenceValue);
			if (texture) {
				GUILayout.Label(texture);
			}

			if (serializedObject.ApplyModifiedProperties()) {
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}
	}
}
