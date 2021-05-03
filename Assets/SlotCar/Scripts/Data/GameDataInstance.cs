using System.Collections.Generic;
using System.IO;
using Assets.SlotCar.Scripts.Utils;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Data {
	public class GameDataInstance : Singleton<GameDataInstance> {
		public List<CarData> cars;
		public List<LevelData> levels;
		private static readonly string configsFolderPath = "\\SlotCar\\Configs";
		private string carsDataPath;
		private string levelsDataPath;
		private StreamWriter carsDataStream;
		private StreamWriter levelsDataStream;
		public void Initialize() {
			carsDataPath = Application.dataPath + configsFolderPath +"\\cars.json";
			levelsDataPath = Application.dataPath + configsFolderPath + "\\levels.json";
			LoadCars();
			LoadLevels();
		}
		private void LoadCars() {
			if (!File.Exists(carsDataPath)) {
				carsDataStream = File.CreateText(carsDataPath);
				carsDataStream.Write(JsonUtility.ToJson(cars));
				carsDataStream.Flush();
			}
			string jsonString = File.ReadAllText(carsDataPath);
			cars = JsonUtility.FromJson<List<CarData>>(jsonString);
		}

		private void LoadLevels() {
			if (!File.Exists(levelsDataPath)) {
				levelsDataStream = File.CreateText(levelsDataPath);
				levelsDataStream.Write(JsonUtility.ToJson(levels));
				levelsDataStream.Flush();
			}
			string jsonString = File.ReadAllText(levelsDataPath);
			levels = JsonUtility.FromJson<List<LevelData>>(jsonString);
		}

	}
}
