using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts {

	public enum RaceState {
		PAUSE,
		START,
		FINISHED
	}

	public class RaceManager : MonoBehaviour {
		public List<PathFollower> cars;
		public PlayerCar playerCar;
		public TextAsset jsonFile;
		public EnemyDifficult difficultData;
		public Slider raceProgressSlider;
		public ScrollMiniGame scrollGame;
		public int playerPosition;
		public int AllPositions => cars.Count;
		public int allLaps = 3;
		private RaceState state = RaceState.PAUSE;
		public float accelerationOnStartTime = 2.0f;
		// Start is called before the first frame update
		void Awake() {
			raceProgressSlider.maxValue = allLaps;
			raceProgressSlider.value = 0.0f;
			difficultData = EnemyDifficult.CreateFromJson(jsonFile.text);
		}

		void Start() {
			state = RaceState.START;
			StartCoroutine("SetStartScrollGame");
		}

		// Update is called once per frame
		void FixedUpdate() {

			CheckPositions();
			SetCurrentRaceProgreess();
		}

		IEnumerator SetStartScrollGame() {
			yield return new WaitForSeconds(accelerationOnStartTime);
			StartScrollGame();
		}

		void StartScrollGame() {
			scrollGame.SetCurrentState(StateScrollGame.Start);
		}

		void CheckPositions() {
			cars = cars.OrderBy( o => o.distanceTravelled).ToList();
			playerPosition = cars.Count - cars.IndexOf(playerCar);
			foreach (var car in cars) {
				car.SetFirstPos(false);
			}
			cars.Last().SetFirstPos(true);
		}

		void SetCurrentRaceProgreess() {
			raceProgressSlider.value = playerCar.currentTravelled;
		}
	}
}
