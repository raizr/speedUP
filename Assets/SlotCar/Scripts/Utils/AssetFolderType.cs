using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Utils {
	[System.Serializable]
	public class FileReference {
		public string GUID;

		public string Path => AssetDatabase.GUIDToAssetPath(GUID);
	}

	[CustomPropertyDrawer(typeof(FileReference))]
	public class FolderReferencePropertyDrawer : PropertyDrawer {
		private bool initialized;
		private SerializedProperty guid;
		private Object obj;

		private void Init(SerializedProperty property) {
			initialized = true;
			guid = property.FindPropertyRelative("GUID");
			obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid.stringValue));
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			if (!initialized)
				Init(property);

			GUIContent guiContent = EditorGUIUtility.ObjectContent(obj, typeof(DefaultAsset));

			Rect r = EditorGUI.PrefixLabel(position, label);

			Rect textFieldRect = r;
			textFieldRect.width -= 19f;

			GUIStyle textFieldStyle = new GUIStyle("TextField") {
				imagePosition = obj ? ImagePosition.ImageLeft : ImagePosition.TextOnly
			};

			if (GUI.Button(textFieldRect, guiContent, textFieldStyle) && obj) {
				EditorGUIUtility.PingObject(obj);
			}

			if (textFieldRect.Contains(Event.current.mousePosition)) {
				if (Event.current.type == EventType.DragUpdated) {
					string path = DragAndDrop.paths[0];
					DragAndDrop.visualMode = File.Exists(path) ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
					Event.current.Use();
				} else if (Event.current.type == EventType.DragPerform) {
					string path = DragAndDrop.paths[0];
					if (File.Exists(path)) {
						AssetDatabase.ImportAsset(path);
						obj = AssetDatabase.LoadAssetAtPath<Object>(path);
						guid.stringValue = AssetDatabase.AssetPathToGUID(path);
					}
					Event.current.Use();
				}
			}
			
			Rect objectFieldRect = r;
			objectFieldRect.x = textFieldRect.xMax + 1f;
			objectFieldRect.width = 19f;
			if (GUI.Button(objectFieldRect, "", GUI.skin.GetStyle("IN ObjectField"))) {
				string path = EditorUtility.OpenFolderPanel("Select a folder", "Assets", "");
				if (path.Contains(Application.dataPath)) {
					path = "Assets" + path.Substring(Application.dataPath.Length);
					obj = AssetDatabase.LoadAssetAtPath(path, typeof(DefaultAsset));
					guid.stringValue = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
				} else
					Debug.LogError("The path must be in the Assets folder");
			}
		}
	}
}