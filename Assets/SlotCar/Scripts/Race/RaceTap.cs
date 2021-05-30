using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.UI;
using DG.Tweening;
using UnityEngine;

namespace Assets.SlotCar.Scripts.Race {
	public class RaceTap : RaceManager {
		public ScrollMiniGame scrollGame;
		public float accelerationOnStartTime = 2.0f;
		public float startCheckPosTime = 1.0f;
		private bool checkPos = false;

		public override void Init(LevelData aLevelData) {
			type = RaceType.Tap;
			base.Init(aLevelData);
			InitUI();
			if (raceProgressSlider != null) {
				raceProgressSlider.maxValue = allLaps;
				raceProgressSlider.Value = 0.0f;
			}
		}

		private void InitUI() {
			var raceTapUI = GameInstance.Instance.uiController.GetUI(GameState.RaceTap) as RaceTapUI;
			if (raceTapUI == null) {
				return;
			}

			raceTapUI.scrollMiniGame.car = playerCar;
			foreach (var enemy in enemyCars) {
				raceTapUI.scrollMiniGame.zoneCompleteEvent.AddListener(enemy.SpeedAi);
			}
			scrollGame = raceTapUI.scrollMiniGame;
			raceProgressSlider = raceTapUI.raceProgressSlider;
			countdown = raceTapUI.countdown;
			raceTapUI.carSpeed.car = playerCar;
		}

		void FixedUpdate() {
			if (state == RaceState.RACE) {
				CheckPositions();
				SetCurrentRaceProgress();
			}
		}

		public override void StartRace() {
			scrollGame.ResetProgress();
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
			if (result == FinishResult.WIN) {
				PlayerProfile.Instance.profile.currentLevel++;
				PlayerProfile.Instance.SaveProfile();
			}
			PauseRace();
			finishResult = result;
		}

		public override void ResetRace() {
			foreach (var car in cars) {
				car.ResetValues();
			}
			scrollGame.ResetProgress();
		}

		public override void RetryRace() {
			ResetRace();
		}

		public override void FinishRace() {
			ResetRace();
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
			raceProgressSlider.Value = playerCar.currentTravelled;
		}

		void CheckEndRace(PathFollower firstCar) {
			if (state == RaceState.PAUSE) {
				return;
			}
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
