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
				profile.currentCarId = "0";
				profileStream = File.CreateText(profilePath);
				profileStream.Write(JsonUtility.ToJson(profile));
				profileStream.Flush();
			}
			string jsonString = File.ReadAllText(profilePath);
			profile = JsonUtility.FromJson<ProfileData>(jsonString);
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
	}
}
