using System.Collections.Generic;
using System.IO;
using Assets.SlotCar.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Data {
	public class GameDataInstance : Singleton<GameDataInstance> {
		private static int FoldersPathSize = "Assets/Resources/".Length;
		public FileReference pathCars;
		public FileReference pathLevels;
		private CarsData cars;
		private LevelsData levels;
		public void Initialize() {
			LoadCars();
			LoadLevels();
		}
		
		private void LoadCars() {
			cars = Resources.Load<CarsData>(GetResourcePath(pathCars.Path));
		}

		private void LoadLevels() {
			levels = Resources.Load<LevelsData>(GetResourcePath(pathLevels.Path));
		}

		private string GetResourcePath(string fullPath) {
			var resourcePath = fullPath.Remove(0, FoldersPathSize);
			return Path.GetDirectoryName(resourcePath) + "/" + Path.GetFileNameWithoutExtension(resourcePath);
		}

		List<CarData> GetCars() {
			return cars.cars;
		}

		List<LevelData> GetLevels() {
			return levels.levels;
		}
	}
}
