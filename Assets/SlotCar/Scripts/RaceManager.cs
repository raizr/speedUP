using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class RaceManager : MonoBehaviour {
		public List<PathFollower> cars;
		public PlayerCar playerCar;
		public TextAsset jsonFile;
		public EnemyDifficult difficultData;

		public int[] carDistances;
		private int playerDistance;
		public int playerPosition;
		int i;
		public int AllPositions => cars.Count;

		public int AllLaps = 3;

		// Start is called before the first frame update
		void Awake() {
			carDistances = new int[cars.Count];
			difficultData = EnemyDifficult.CreateFromJson(jsonFile.text);
		}

		// Update is called once per frame
		void FixedUpdate() {

			CheckPositions();
		}

		void CheckPositions() {
			i = 0;
			foreach (var car in cars) {
				carDistances[i] = (int)car.distanceTravelled;
				i++;
			}
			Array.Sort(carDistances);
			playerDistance = (int)playerCar.distanceTravelled;
			playerPosition = cars.Count - Array.IndexOf(carDistances, playerDistance);
		}
	}
}
