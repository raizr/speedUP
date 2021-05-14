using System.IO;
using Assets.SlotCar.Scripts.Utils;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Data {
	public class PlayerProfile : Singleton<PlayerProfile> {
		public ProfileData profile;
		private string profilePath;
		private StreamWriter profileStream;
		public void Initialize() {
			profilePath = Application.persistentDataPath + "\\profile.json";
			LoadProfile();
		}

		private void LoadProfile() {
			if (!File.Exists(profilePath)) {
				profile.playerName = "Player";
				profile.currentCarId = 0;
				profile.currentLevel = 0;
				profileStream = File.CreateText(profilePath);
				profileStream.Write(JsonUtility.ToJson(profile));
				profileStream.Flush();
			}
			else {
				string jsonString = File.ReadAllText(profilePath);
				profile = JsonUtility.FromJson<ProfileData>(jsonString);
			}
		}

		public void SaveProfile() {
			if (!File.Exists(profilePath)) {
				profileStream = File.CreateText(profilePath);
			}
			else {
				profileStream = new StreamWriter(profilePath);
			}
			profileStream.Write(JsonUtility.ToJson(profile));
			profileStream.Flush();
		}

		public int GetCurrentLevelId() {
			return profile.currentLevel;
		}

		public int GetCurrentCarId() {
			return profile.currentCarId;
		}

		public override void OnDestroy() {
			if (profileStream != null) {
				profileStream.Flush();
				profileStream.Close();
			} 
			base.OnDestroy();
		}
	}
}
