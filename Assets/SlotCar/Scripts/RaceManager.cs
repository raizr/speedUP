using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.SlotCar.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SlotCar.Scripts {

	public enum RaceState {
		PAUSE,
		RACE,
		FINISHED
	}

	public class RaceManager : MonoBehaviour {
		public List<PathFollower> cars;
		public PlayerCar playerCar;
		public TextAsset jsonFile;
		public EnemyDifficult difficultData;
		public Slider raceProgressSlider;
		public ScrollMiniGame scrollGame;
		public Countdown countdown;
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
		}

		// Update is called once per frame
		void FixedUpdate() {
			if (state == RaceState.RACE) {
				CheckPositions();
				SetCurrentRaceProgress();
			}
		}

		public void StartRace() {
			StartCountdown();
		}

		void StartRaceInternal() {
			state = RaceState.RACE;
			StartCoroutine("SetStartScrollGame");
			SetCarsState(state);
		}

		void StartCountdown() {
			countdown.countdownFinish.AddListener(StartRaceInternal);
			countdown.StartCountdown();
		}

		public void PauseRace() {
			state = RaceState.PAUSE;
			StopScrollGame();
			SetCarsState(state);
		}

		public void FinisRace() {
			PauseRace();
		}

		IEnumerator SetStartScrollGame() {
			yield return new WaitForSeconds(accelerationOnStartTime);
			StartScrollGame();
		}

		void StartScrollGame() {
			if (state == RaceState.RACE) {
				scrollGame.SetCurrentState(StateScrollGame.Start);
			}
		}

		void StopScrollGame() {
			scrollGame.SetCurrentState(StateScrollGame.Stop);
		}

		void CheckPositions() {
			cars = cars.OrderBy( o => o.distanceTravelled).ToList();
			playerPosition = cars.Count - cars.IndexOf(playerCar);
			foreach (var car in cars) {
				car.SetFirstPos(false);
			}
			PathFollower firstCar = cars.Last();
			firstCar.SetFirstPos(true);
			CheckEndRace(firstCar);
		}

		void SetCurrentRaceProgress() {
			raceProgressSlider.value = playerCar.currentTravelled;
		}

		void CheckEndRace(PathFollower firstCar) {
			if (firstCar.currentTravelled >= allLaps) {
				FinisRace();
			}
		}

		void SetCarsState(RaceState st) {
			foreach (var car in cars) {
				car.SetStateRace(st);
			}
		}
	}
}
