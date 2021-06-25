using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.SlotCar.Scripts.Data;
using Assets.SlotCar.Scripts.Race;
using Assets.SlotCar.Scripts.UI;
using Assets.SlotCar.Scripts.Utils;
using Cinemachine;
using PathCreation;
using UnityEngine;

namespace Assets.SlotCar.Scripts {
	public class GameInstance : ASingleton<GameInstance> {
		public CinemachineVirtualCamera cameraTapView;
		public CinemachineVirtualCamera cameraSlotView;
		public UIController uiController;
		public GameObject roadHolder;
		public static event Action OnAppQuit;
		[HideInInspector]
		public RaceManager raceManager;
		public RaceTap raceTap;
		public RaceSlot raceSlot;
		public GameState gameState = GameState.Main;


		public CinemachineVirtualCamera cameraView;
		private PlayerProfile playerProfile;
		private GameDataInstance gameDataInstance;
		private GameObject levelHolder;
		private LevelData currentLevel;
		protected override void Init() {
			playerProfile = PlayerProfile.Instance;
			playerProfile.Initialize();
			LoadData();
		}

		private void LoadData() {
			gameDataInstance = GameDataInstance.Instance;
			gameDataInstance.Initialize();
		}

		// Start is called before the first frame update
		void Start() {
			SwitchTo(gameState);
		}

		void SpawnCurrentLevel() {
			if (levelHolder != null) {
				Destroy(levelHolder);
			}
			levelHolder = Instantiate(currentLevel.prefab, new Vector3(0, 0, 0), Quaternion.identity);
			levelHolder.transform.SetParent(roadHolder.transform);
			levelHolder.transform.position = Vector3.zero;
		}

		void SpawnCurrentCar() {
			var currentCar = gameDataInstance.GetCar(playerProfile.GetCurrentCarId());
			var car = Instantiate(currentCar.prefab, new Vector3(0, 0, 0), Quaternion.identity);
			car.transform.SetParent(roadHolder.transform);
		}

		void InitRace() {
			currentLevel = gameDataInstance.GetLevel(playerProfile.GetCurrentLevelId());
			if (raceManager != null) {
				if (raceManager.type != currentLevel.raceType) {
					raceManager.Clear();
					Destroy(raceManager);
					raceManager = null;
				}
			}
			if (raceManager == null) {
				if (currentLevel.raceType == RaceType.Tap) {
					raceManager = Instantiate(raceTap);
					if (cameraView != null) {
						if (cameraView.name != cameraTapView.name) {
							Destroy(cameraView);
							cameraView = Instantiate(cameraTapView);
						}
					}
					else {
						cameraView = Instantiate(cameraTapView);
					}
				}
				else if (currentLevel.raceType == RaceType.Slot) {
					raceManager = Instantiate(raceSlot);
					if (cameraView != null) {
						if (cameraView.name != cameraSlotView.name) {
							Destroy(cameraView);
							cameraView = Instantiate(cameraSlotView);
						}
					} else {
						cameraView = Instantiate(cameraSlotView);
					}
				}
			}
			SpawnCurrentLevel();
			raceManager.roadHolder = levelHolder.GetComponent<RoadMeshCreator>();
			raceManager.Init(currentLevel);
		}

		public void SwitchTo(GameState aGameState) {
			gameState = aGameState;
			
			switch (aGameState) {
				case GameState.Race:
					if (currentLevel.raceType == RaceType.Tap) {
						uiController.EnableUI(UIStates.RaceTap);
					}
					else if (currentLevel.raceType == RaceType.Slot) {
						uiController.EnableUI(UIStates.RaceSlot);
					}
					raceManager.StartRace();
					break;
				case GameState.Main:
					uiController.EnableUI(UIStates.Main);
					InitRace();
					break;
				case GameState.FinishRace:
					uiController.EnableUI(UIStates.FinishRace);
					//InitRace();
					break;
			}
		}

		public CinemachineVirtualCamera GetCamera() {
			return cameraView;
		}

		// Update is called once per frame
		void Update()
		{
		
		}

		void OnApplicationQuit() {
			Action onQuit = OnAppQuit;
			onQuit?.Invoke();
		}
	}
}
