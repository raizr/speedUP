using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using Assets.SlotCar.Scripts.Data;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Editor {
	[CustomEditor(typeof(CarData), true)]
	public class CarDataEditor : UnityEditor.Editor {
		private static GameObject prefab;
		private static string carPrefabFolder = "\\SlotCar\\Prefabs\\Cars\\";
		UnityEditor.Editor gameObjectEditor;
		public override void OnInspectorGUI() {

		EditorGUILayout.PropertyField(serializedObject.FindProperty("carId"), new GUIContent("Id"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("rarity"), new GUIContent("rarity"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"), new GUIContent("speed"));
		var prefabSerializeObject = serializedObject.FindProperty("prefabPath");
		if (prefabSerializeObject.stringValue.Length > 0) {
			prefab = PrefabUtility.LoadPrefabContents(Application.dataPath + carPrefabFolder + prefabSerializeObject.stringValue);
		}
		prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false) as GameObject;
		if (prefab != null) {
			var texture = AssetPreview.GetAssetPreview(prefab);
			if (texture) {
				GUILayout.Label(prefabSerializeObject.stringValue);
				GUILayout.Label(texture);
			}
			var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
			if (path.Length > 0) {
				var prefabName = Path.GetFileName(path);
				if (prefabName.Length > 0) {
					prefabSerializeObject.stringValue = prefabName;
				}
			}
		}

		if (serializedObject.ApplyModifiedProperties()) {
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}

	public class CarsDataEditor : EditorWindow {
		private CarsData cars;
		private static readonly string configsFolderPath = "Assets/SlotCar/Configs/";
		private string carsDataPath;
		private Vector2 scrollPos;
		private List<int> removeIndeces = new List<int>();
		[MenuItem("Window/Cars Editor")]
		public static void ShowWindow() {
			GetWindow(typeof(CarsDataEditor));
		}

		private void Awake() {
			LoadData();

		}
		private void LoadData() {
			var assetCars = AssetDatabase.LoadAssetAtPath<CarsData>(configsFolderPath + "cars.asset");
			if (assetCars) {
				cars = assetCars;
			}
			else {
				cars = new CarsData { cars = new List<CarData>() };
			}
		}

		public void SaveProfile() {
			AssetDatabase.CreateAsset(cars, configsFolderPath + "cars.asset");
			AssetDatabase.SaveAssets();
		}

		void OnGUI() {
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("new car", GUILayout.ExpandWidth(false))) {
				var carData = new CarData();
				if (cars.cars.Count == 0) {
					carData.carId = 0;
				}
				else {
					carData.carId = cars.cars[cars.cars.Count - 1].carId + 1;
				}
				cars.cars.Add(carData);
			}
			if (GUILayout.Button("Save to json", GUILayout.ExpandWidth(false))) {
				SaveProfile();
			}
			GUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, true, 
				GUILayout.Width(position.width), GUILayout.Height(position.height));
			for (var i = 0; i < cars.cars.Count; i++) {
				var car = cars.cars[i];
				if (car != null) {
					
					var carEditor = UnityEditor.Editor.CreateEditor(car);
					carEditor.OnInspectorGUI();
					if (GUILayout.Button("x", GUILayout.ExpandWidth(false))) {
						removeIndeces.Add(i);
					}
				}
			}

			if (removeIndeces.Count > 0) {
				foreach (var index in removeIndeces) {
					cars.cars.RemoveAt(index);
				}
				removeIndeces.Clear();
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndHorizontal();
			if (GUI.changed) {
				if (cars) {
					EditorUtility.SetDirty(cars);
					var so = new SerializedObject(cars);
					so.ApplyModifiedProperties();
				}
			}
		}
	}
}
