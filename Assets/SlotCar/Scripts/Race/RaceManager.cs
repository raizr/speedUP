using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.Effects;
using Assets.SlotCar.Scripts.UI;
using DG.Tweening;
using PathCreation;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.SlotCar.Scripts.Race {
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
		public RaceType type = RaceType.Tap;
		public RoadMeshCreator roadHolder;
		public EnemyCar enemyCarPrefab;
		public PlayerCar playerCarPrefab;
		public List<PathFollower> cars;
		public List<EnemyCar> enemyCars;
		public LevelData levelData;
		public PlayerCar playerCar;

		public Slider raceProgressSlider;
		public Countdown countdown;

		public FinishBlast finishBlast;
		public int playerPosition;
		public int AllPositions => cars.Count;
		public int allLaps = 3;

		public float slowdownLength = 1.0f;
		public float slowDownFactor = 0.05f;

		public FinishResult finishResult = FinishResult.LOSE;
		
		protected RaceState state = RaceState.PAUSE;
		private bool slowMotionStarted = false;
		public virtual void Init(LevelData aLevelData) {
			levelData = aLevelData;
			allLaps = levelData.laps;
			int i = 0;
			foreach (var car in cars) {
				if (car != null) {
					Destroy(car.gameObject);
				}
			}
			cars.Clear();
			enemyCars.Clear();
			foreach (var enemyData in levelData.enemyCars) {
				var enemy = Instantiate(enemyCarPrefab, new Vector3(0, 0, 0), Quaternion.identity);
				enemy.Init(roadHolder, i, enemyData.carId, enemyData.lineNumber);
				enemyCars.Add(enemy);
				cars.Add(enemy);
			}

			playerCar = Instantiate(playerCarPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			playerCar.Init(roadHolder, PlayerProfile.Instance.profile.currentCarId, levelData.playerPosition);
			cars.Add(playerCar);
			GameInstance.Instance.camera.m_Follow = playerCar.transform;
			finishBlast = roadHolder.gameObject.GetComponent<FinishBlast>();
		}

		public virtual void StartRace() {

		}

		public virtual void ResetRace() {

		}

		public virtual void FinishRace() {

		}

		public virtual void RetryRace() {

		}

		public void DoSlowMotion() {
			if (slowMotionStarted) {
				return;
			}

			slowMotionStarted = true;
			GameInstance.Instance.uiController.HideUI();
			if (finishBlast != null) {
				finishBlast.StartFinishBlast();
			}
			DOTween.To(() => Time.timeScale, x => Time.timeScale = x,
					slowDownFactor, slowdownLength / 2.0f)
				.OnComplete(() => {
					slowMotionStarted = false;
					DOTween.To(() => Time.timeScale, x => Time.timeScale = x,
						1.0f, slowdownLength / 2.0f);
					Time.fixedDeltaTime = 0.02f;
					GameInstance.Instance.SwitchTo(GameState.FinishRace);
				});
		}
	}
}
