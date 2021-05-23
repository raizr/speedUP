using System.IO;
using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.Utils;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class PlayerProfile : Singleton<PlayerProfile> {
		public ProfileData profile;
		private string profilePath;
		public void Initialize() {
			profilePath = Application.persistentDataPath + "\\profile.json";
			LoadProfile();
		}

		private void LoadProfile() {
			if (!File.Exists(profilePath)) {
				profile.playerName = "Player";
				profile.currentCarId = 0;
				profile.currentLevel = 0;
				File.WriteAllText(profilePath, JsonUtility.ToJson(profile));
			}
			else {
				string jsonString = File.ReadAllText(profilePath);
				if (jsonString.Length == 0) {
					profile.playerName = "Player";
					profile.currentCarId = 0;
					profile.currentLevel = 0;
				}
				else {
					profile = JsonUtility.FromJson<ProfileData>(jsonString);
				}
			}
		}

		public void SaveProfile() {
			File.WriteAllText(profilePath, JsonUtility.ToJson(profile));
		}

		public int GetCurrentLevelId() {
			return profile.currentLevel;
		}

		public int GetCurrentCarId() {
			return profile.currentCarId;
		}
	}
}
