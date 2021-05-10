using System.Collections;
using System.Collections.Generic;
using Assets.SlotCar.Scripts.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Editor {
	[CustomEditor(typeof(LevelData), true)]
	public class LevelDataEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {

			EditorGUILayout.PropertyField(serializedObject.FindProperty("levelId"), new GUIContent("Id"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("raceType"), new GUIContent("raceType"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("laps"), new GUIContent("laps"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("lines"), new GUIContent("lines"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyCars"), new GUIContent("enemyCars"));
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