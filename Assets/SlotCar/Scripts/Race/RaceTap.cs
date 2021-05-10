using System.Collections;
using System.Linq;
using Assets.SlotCar.Scripts.Data;
using DG.Tweening;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Race {
	public class RaceTap : RaceManager {
		public ScrollMiniGame scrollGame;
		public int allLaps = 3;
		public float accelerationOnStartTime = 2.0f;
		public float startCheckPosTime = 1.0f;
		private bool checkPos = false;

		void Awake() {
		}

		void Start() {
		}

		public override void Init(LevelData levelData) {
			base.Init(levelData);
			if (raceProgressSlider != null) {
				raceProgressSlider.maxValue = allLaps;
				raceProgressSlider.value = 0.0f;
			}

			if (finishUI != null) {
				finishUI.gameObject.SetActive(false);
			}

		}

		void FixedUpdate() {
			if (state == RaceState.RACE) {
				CheckPositions();
				SetCurrentRaceProgress();
			}
		}

		public override void StartRace() {
			StartCountdown();
		}

		void StartRaceInternal() {
			state = RaceState.RACE;
			StartScrollGame();
			StartCoroutine(SetStartChecPos());
			SetCarsState(state);
		}

		IEnumerator SetStartScrollGame() {
			yield return new WaitForSeconds(accelerationOnStartTime);
			StartScrollGame();
		}

		IEnumerator SetStartChecPos() {
			yield return new WaitForSeconds(startCheckPosTime);
			checkPos = true;
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
		}

		public void FinisRace(FinishResult result) {
			PauseRace();
			finishResult = result;
		}

		public override void RetryRace() {
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
			if (!checkPos) {
				return;
			}
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

			if (playerCar.currentTravelled >= allLaps - 0.03) {
				DoSlowMotion();
			}
		}

		void SetCarsState(RaceState st) {
			foreach (var car in cars) {
				car.SetStateRace(st);
			}
		}
	}
}
