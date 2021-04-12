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

	public enum FinishResult {
		WIN,
		LOSE
	}

	public class RaceManager : MonoBehaviour {
		public List<PathFollower> cars;
		public PlayerCar playerCar;
		public TextAsset jsonFile;
		public EnemyDifficult difficultData;
		public Slider raceProgressSlider;
		public ScrollMiniGame scrollGame;
		public Countdown countdown;
		public GameObject raceUI;
		public FinishRaceUI finishUI;
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
			finishUI.gameObject.SetActive(false);
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
			//StartCoroutine("SetStartScrollGame");
			StartScrollGame();
			SetCarsState(state);
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

		void StartCountdown() {
			countdown.countdownFinish.AddListener(StartRaceInternal);
			countdown.StartCountdown();
		}

		public void PauseRace() {
			state = RaceState.PAUSE;
			StopScrollGame();
			//SetCarsState(state);
		}

		public void FinisRace(FinishResult result) {
			PauseRace();

			finishUI.ShowUI(result);
			raceUI.gameObject.SetActive(false);
		}

		public void RetryRace() {
			foreach (var car in cars) {
				car.ResetValues();
			}
			raceUI.gameObject.SetActive(true);
			countdown.gameObject.SetActive(true);
			finishUI.gameObject.SetActive(false);
			scrollGame.ResetProgress();
			StartRace();
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
			if (playerCar.currentTravelled >= allLaps) {
				FinishResult result = (firstCar == playerCar) ? FinishResult.WIN : FinishResult.LOSE;
				FinisRace(result);
			}
		}

		void SetCarsState(RaceState st) {
			foreach (var car in cars) {
				car.SetStateRace(st);
			}
		}
	}
}
