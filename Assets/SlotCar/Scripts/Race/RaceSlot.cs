using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.UI;
using DG.Tweening;
using UnityEngine;
using Assets.SlotCar.Scripts;

namespace Assets.SlotCar.Scripts.Race {
	public class RaceSlot : RaceManager {
		public FollowTarget followTarget;
		public float accelerationOnStartTime = 2.0f;
		public float startCheckPosTime = 1.0f;
		private bool checkPos = false;

		public override void Init(LevelData aLevelData) {
			type = RaceType.Slot;
			base.Init(aLevelData);
			InitUI();
			InitPlayerNames();
			if (raceProgressSlider != null) {
				raceProgressSlider.maxValue = allLaps;
				raceProgressSlider.Value = 0.0f;
			}
		}

		private void InitUI() {
			var raceUI = GameInstance.Instance.uiController.GetUI(UIStates.RaceSlot) as RaceSlotUI;
			if (raceUI == null) {
				return;
			}
			raceProgressSlider = raceUI.raceProgressSlider;
			countdown = raceUI.countdown;
			raceUI.carSpeed.car = playerCar;

			var swipeDetector = raceUI.GetComponent<SwipeDetector>();
			if (swipeDetector) {
				swipeDetector.car = playerCar;
				swipeDetector.maxLines = levelData.lines;
			}
		}

		private void InitPlayerNames() {
			foreach (var enemy in enemyCars) {
				var followUI = Instantiate(followTarget,
										new Vector3(0, 0.0f, 0),
												Quaternion.identity,
												enemy.transform);
				followUI.GetComponent<Canvas>().worldCamera = Camera.main;
				followUI.lookAt = enemy.linePosTransform;
				followUI.text.text = UtilsNameGen.GetRandomName();
				followUI.GenerateFlag();
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
			StartCoroutine(SetStartChecPos());
			SetCarsState(state);
		}

		IEnumerator SetStartChecPos() {
			yield return new WaitForSeconds(startCheckPosTime);
			checkPos = true;
		}

		void StartCountdown() {
			countdown.countdownFinish.AddListener(StartRaceInternal);
			countdown.StartCountdown();
		}

		public void PauseRace() {
			state = RaceState.PAUSE;
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
		}

		public override void RetryRace() {
			ResetRace();
		}

		public override void FinishRace() {
			ResetRace();
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
			var firstCar = cars.Last();
			firstCar.SetFirstPos(true);
			CheckEndRace(firstCar);
		}

		void SetCurrentRaceProgress() {
			raceProgressSlider.Value = playerCar.currentTravelled;
		}

		void CheckEndRace(Car firstCar) {
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
