using System.Collections.Generic;
using System.IO;
using Assets.SlotCar.Scripts.Race;
using Assets.SlotCar.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Data {
	public class GameDataInstance : Singleton<GameDataInstance> {
		private static int FoldersPathSize = "Assets/Resources/".Length;
		private CarsData cars;
		private LevelsData levels;
		private EnemyDifficult difficultData;
		public void Initialize() {
			LoadCars();
			LoadLevels();
			LoadEnemyDifficult();
		}
		
		private void LoadCars() {
			cars = Resources.Load<CarsData>("Data/Cars/CarsData");
		}

		private void LoadLevels() {
			levels = Resources.Load<LevelsData>("Data/Levels/LevelsData");
		}

		private void LoadEnemyDifficult() {
			difficultData = Resources.Load<EnemyDifficult>("Data/EnemyDifficult");
		}

		private string GetResourcePath(string fullPath) {
			var resourcePath = fullPath.Remove(0, FoldersPathSize);
			return Path.GetDirectoryName(resourcePath) + "/" + Path.GetFileNameWithoutExtension(resourcePath);
		}

		public List<CarData> GetCars() {
			return cars.cars;
		}

		public List<LevelData> GetLevels() {
			return levels.levels;
		}

		public LevelData GetLevel(int id) {
			if (id > levels.levels.Count - 1) {
				return levels.levels[0];
			}
			return levels.levels[id];
		}

		public CarData GetCar(int id) {
			if (id > cars.cars.Count - 1) {
				return cars.cars[0];
			}
			return cars.cars[id];
		}

		public EnemyDifficult GetEnemyDifficult() {
			return difficultData;
		}

	}
}
