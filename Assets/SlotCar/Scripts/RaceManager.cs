using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
			cars = cars.OrderBy( o => o.distanceTravelled).ToList();
			Array.Sort(carDistances);
			playerDistance = (int)playerCar.distanceTravelled;
			playerPosition = cars.Count - cars.IndexOf(playerCar);
			foreach (var car in cars) {
				car.SetFirstPos(false);
			}
			cars.Last().SetFirstPos(true);
		}
	}
}
